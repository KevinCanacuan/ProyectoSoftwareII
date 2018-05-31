using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjLegados.Data.Structure;
using System.IO;

namespace prjLegados.AzureStorage.Blobs
{
    public class BlobStorage: IBlobStorage
    {
        CloudStorageAccount cldStorageAccount;

        public Container fntCreateBlobContainer(Container cntContainer)
        {
            cldStorageAccount = Common.Common.fntCreateStorageAccountForBlobs();
            // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
            CloudBlobClient cldBlobClient = cldStorageAccount.CreateCloudBlobClient();
            //hola 

            // Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
            CloudBlobContainer cldBlobContainer = cldBlobClient.GetContainerReference(cntContainer.fntFullNameStr);
            cldBlobContainer.Metadata.Add(cnsContainerMetadata.Company, cntContainer.fntCompanyStr);
            cldBlobContainer.CreateIfNotExists();

            // Set the permissions so the blobs are public. 
            BlobContainerPermissions blbpermissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            };
            cldBlobContainer.SetPermissions(blbpermissions);

            cntContainer.fntUri = cldBlobContainer.Uri;

            return cntContainer;
        }

        public List<Container> fntListBlobContainerLst(User usrUser)
        {
            cldStorageAccount = Common.Common.fntCreateStorageAccountForBlobs();

            // Create the blob client.
            CloudBlobClient clcBlobClient = cldStorageAccount.CreateCloudBlobClient();

            //Get the list of the blob from the above container
            var containers = clcBlobClient.ListContainers(usrUser.fntCompanyStr).Select(c => 
                new Container {
                    fntCompanyStr = usrUser.fntCompanyStr,
                    fntNameStr = string.Format("{0}", c.Uri.Segments[cnsContainerMetadata.IndexOfNameUri]),
                    fntUri = c.Uri
                }).ToList();

            return containers;
        }

        public async Task fntDeleteBlob(Container cntContainer, List<BlobStructure> lstFiles)
        {
            var container = fntGetContainer(cntContainer);

            foreach (var item in lstFiles)
            {
                var blob = new CloudBlob(item.fntUri, container.ServiceClient);
                await blob.DeleteAsync();
            }
        }

        public MemoryStream fntGetStreamBlob(BlobStructure blbBlob)
        {
            MemoryStream strStream = new MemoryStream();
            var container = fntGetContainer(blbBlob.cntContainer);

            CloudBlockBlob cldBlockBlob = container.GetBlockBlobReference(blbBlob.fntNameStr);

            if (cldBlockBlob.Exists())
                cldBlockBlob.DownloadToStream(strStream);
            return strStream;
        }

        public List<BlobStructure> fntListBlobsLst(Container cntContainer, User usrUser, List<string> lstApp)
        {
            var blobs = new List<BlobStructure>();
            var container = fntGetContainer(cntContainer);

            var colApp = lstApp.Count() == 0 ? usrUser.fntAplicationsLst : lstApp;
            
            foreach (var item in colApp)
            {
                var lstBlobs = container.ListBlobs(usrUser.fntCompanyStr + "-" + item, true);
                var blobReader = from l in lstBlobs
                                 select (new BlobStructure
                                            {
                                                fntUri = l.Uri,
                                                fntNameStr = l.Uri.Segments[cnsBlobMetadata.IndexOfNameUri],
                                                cntContainer = cntContainer,
                                                IsUploaded = true,
                                                dctMetadata = new Dictionary<string, object> { { cnsBlobMetadata.Extension, l.Uri.Segments[cnsBlobMetadata.IndexOfNameUri].Split('.')[1] } }
                                            });

                blobs.AddRange(blobReader);
            }

            return blobs;
        }

        public List<BlobStructure> fntUploadBlobs(Container cntContainer, IEnumerable<FileUpload> lstFiles, User usrUser)
        {
            var container = fntGetContainer(cntContainer);
            var lstBlobs = new List<BlobStructure>();
            bool blnIsUpload;
            // Use the value of localFileName for the blob name.
            foreach (var item in lstFiles)
            {
                // Get a reference to the blob address, then upload the file to the blob.
                var cldBlockBlob = container.GetBlockBlobReference(item.fntFullNameStr);

                // Add metadata
                if (item.fntMetadataDct != null)
                {
                    foreach (var meta in item.fntMetadataDct.Where(x => x.Value != null))
                    {
                        cldBlockBlob.Metadata.Add(meta.Key, meta.Value.ToString());
                    }
                }

                blnIsUpload = false;

                try
                {
                    // Upload
                    cldBlockBlob.UploadFromFile(item.fntLocalPathStr);
                    blnIsUpload = true;
                }
                catch (Exception)
                {
                    // Upload
                    blnIsUpload = false;
                }

                // Add to lstBlobs for index
                lstBlobs.Add(new BlobStructure()
                {
                    cntContainer = cntContainer,
                    dctMetadata = item.fntMetadataDct,
                    fntNameStr = item.fntFullNameStr,
                    fntUri = cldBlockBlob.Uri,
                    IsUploaded = blnIsUpload
                });

            }

            return lstBlobs;
        }

        public void fntRegisterUploadBlobsAsync(User usrUser, List<BlobStructure> lstFiles)
        {
            var clsCatalog = new Common.Catalogs();
            foreach (var item in lstFiles)
            {
                clsCatalog.InsertUploadBlob(new Data.AzureEntities.clsBlobUpload(item.cntContainer.fntFullNameStr, item.fntNameStr) {
                    AlterFileName = item.dctMetadata[cnsBlobMetadata.AlterName].ToString(),
                    Container = item.cntContainer.fntFullNameStr,
                    OriginalFileName = item.dctMetadata[cnsBlobMetadata.FileName].ToString(),
                    User = usrUser.fntUserNameStr
                });
            }
        }

        public CloudBlobContainer fntGetContainer(Container cntContainer)
        {
            cldStorageAccount = Common.Common.fntCreateStorageAccountForBlobs();

            //Get the list of the blob from the above container
            CloudBlobContainer container = new CloudBlobContainer(cntContainer.fntUri, cldStorageAccount.Credentials);

            return container;
        }

        public bool fntDeleteContainer(Container cntContainer, User usrUser)
        {
            var container = fntGetContainer(cntContainer);

            bool result = false;

            if (container != null)
            {
                try
                {
                    // get list of blobs in the container
                    var lstBlobs = fntListBlobsLst(cntContainer, usrUser, new List<string>());
                    // delete all 
                    lstBlobs = new Search.IndexFiles().DeleteDocumentsFromIndex(lstBlobs, usrUser);
                    result = container.DeleteIfExists();
                }
                catch (Exception ex)
                {

                    return false;
                }
                
            }

            return result;
        }

        /// <summary>
        /// Return shared access only reader for a blob
        /// </summary>
        /// <param name="strContainerName">container name</param>
        /// <param name="strBlobFullName">full name of the blob {company}-{application}{name}.{extension}</param>
        /// <param name="intPageNumber">page number of the pdf</param>
        /// <returns>Uri access</returns>
        public string fntGetBlobSasUri(string strContainerName, string strBlobFullName, int intPageNumber)
        {
            cldStorageAccount = Common.Common.fntCreateStorageAccountForBlobs();

            // Create the blob client.
            CloudBlobClient clcBlobClient = cldStorageAccount.CreateCloudBlobClient();

            //Get the list of the blob from the above container
            var container = clcBlobClient.GetContainerReference(strContainerName);

            //Get a reference to a blob within the container.
            CloudBlockBlob cldBlockBlob = container.GetBlockBlobReference(strBlobFullName);

            //Set the expiry time and permissions for the blob.
            //In this case, the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(5);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = cldBlockBlob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return cldBlockBlob.Uri + sasBlobToken + (intPageNumber>0 ? "#page=" +intPageNumber.ToString():"");
        }
    }
}
