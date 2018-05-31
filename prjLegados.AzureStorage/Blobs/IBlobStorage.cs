using Microsoft.WindowsAzure.Storage.Blob;
using prjLegados.Data.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.AzureStorage.Blobs
{
    public interface IBlobStorage
    {
        Container fntCreateBlobContainer(Container cntContainer);

        List<Container> fntListBlobContainerLst(User usrUser);

        Task fntDeleteBlob(Container cntContainer, List<BlobStructure> lstFiles);

        MemoryStream fntGetStreamBlob(BlobStructure blbBlob);

        List<BlobStructure> fntListBlobsLst(Container cntContainer, User usrUser, List<string> lstApp);

        List<BlobStructure> fntUploadBlobs(Container cntContainer, IEnumerable<FileUpload> lstFiles, User usrUser);

        void fntRegisterUploadBlobsAsync(User usrUser, List<BlobStructure> lstFiles);

        CloudBlobContainer fntGetContainer(Container cntContainer);

        bool fntDeleteContainer(Container cntContainer, User usrUser);

        string fntGetBlobSasUri(string strContainerName, string strBlobFullName, int intPageNumber);
    }
}
