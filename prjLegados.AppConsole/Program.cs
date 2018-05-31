using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjLegados.AzureStorage.Blobs;
using prjLegados.AzureStorage.Common;
using prjLegados.Data.Structure;

namespace prjLegados.AppConsole
{
    class Program
    {
        static User usrUser = new User();
        static void Main(string[] args)
        {
            //string functionName = "DocumentUploadAt";
            //Console.WriteLine(Char.ToLowerInvariant(functionName[0]) + functionName.Substring(1));
            //System.Globalization.TextInfo txtInfo = new System.Globalization.CultureInfo("es-us", false).TextInfo;
            //functionName = txtInfo.ToTitleCase(functionName).Replace("_", string.Empty).Replace(" ", string.Empty);
            //functionName = $"{functionName.First().ToString().ToLowerInvariant()}{functionName.Substring(1)}";
            //Console.WriteLine(functionName);
            
            //Console.ReadLine();

            var cltOperations = new AzureStorage.Search.IndexFiles();
            cltOperations.DropExistIndexDocuments(usrUser);

            var filters = new Data.AzureEntities.clsDocumentPdfSearch();
            filters.SearchWords = "mazda";
            filters.FilterMetadata.Add(new Data.AzureEntities.clsOperationFilterAzure() {
                ComparisonExpresion = "endswith",
                FieldName = "module",
                FieldType = "string",
                FieldValue = "zas"
            });
            var idxSearch = cltOperations.SearchDocuments(filters, usrUser);

            var blobStorage = new BlobStorage();
            
            do
            {
                Console.Clear();
                Console.WriteLine("***CONTAINERS***");
                Console.WriteLine("***1. Listar Containers");
                Console.WriteLine("***2. Crear Containers");
                Console.WriteLine("***3. Salir");
                Console.Write("Escoja una opcion: ");
                var strOp = Console.ReadLine();
                switch (Convert.ToInt32(strOp))
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("***Containers***");
                        Console.WriteLine("***Escoja un numero para listar los blobs***");
                        var lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
                        foreach (var item in lstContainers)
                        {
                            Console.WriteLine(String.Format("{0}.- {1}", lstContainers.IndexOf(item), item.fntNameStr));
                        }
                        Console.Write("Escoja una opcion: ");
                        var strContainer = lstContainers[Convert.ToInt32(Console.ReadLine())];
                        fntContainersMenu(strContainer);
                        break;
                    case 2:
                        Console.WriteLine("***Containers***");
                        Console.WriteLine("***Digite el nombre del contenedor***");
                        Console.Write("Nombre del Container: ");
                        var strContainerName = Console.ReadLine();
                        var cntContainer = blobStorage.fntCreateBlobContainer(
                            new Container()
                            {
                                fntNameStr = strContainerName,
                                fntCompanyStr = usrUser.fntCompanyStr
                            });
                        Console.WriteLine(String.Format("Container creado: {0}", cntContainer.fntFullNameStr));
                        Console.ReadLine();
                        break;
                    case 3:
                        return;
                    default:
                        break;
                }
            } while (true);
            
        }

        static void fntContainersMenu(Container cntContainer)
        {
            var blobStorage = new BlobStorage();
            do
            {
                Console.Clear();
                Console.WriteLine(String.Format("***CONTAINER {0}***", cntContainer.fntFullNameStr));
                Console.WriteLine("***1. Listar Blobs");
                Console.WriteLine("***2. Subir Blobs");
                Console.WriteLine("***3. Salir");
                Console.Write("Escoja una opcion: ");
                var strOp = Console.ReadLine();
                switch (Convert.ToInt32(strOp))
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("***Blobs***");
                        Console.WriteLine("***Escoja un numero para listar los blobs***");
                        var lstBlobs = blobStorage.fntListBlobsLst(cntContainer, usrUser, new List<string>());
                        foreach (var item in lstBlobs)
                        {
                            Console.WriteLine(String.Format("{0}.- {1}", lstBlobs.IndexOf(item), item.fntNameStr));
                        }
                        Console.Write("Escoja una opcion: ");
                        var blob = lstBlobs[Convert.ToInt32(Console.ReadLine())];
                        Console.WriteLine(blob.fntUri.ToString());
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("***Blobs***");
                        Console.WriteLine("***Digite el path del archivo a subir***");
                        Console.Write("Path: ");
                        var blobUp = new Data.Structure.FileUpload();
                        blobUp.fntLocalPathStr = Console.ReadLine();
                        Console.Write("Name: ");
                        blobUp.fntNameFileStr = Console.ReadLine();
                        Console.Write("App: ");
                        blobUp.fntApplicationStr = Console.ReadLine();
                        blobUp.fntCompanyStr = usrUser.fntCompanyStr;
                        blobStorage.fntUploadBlobs(cntContainer, new List<FileUpload>() { blobUp }, usrUser);
                        Console.WriteLine(String.Format("Arhivo subido"));
                        Console.ReadLine();
                        break;
                    case 3:
                        return;
                    default:
                        break;
                }
            } while (true);
        }
    }
}
