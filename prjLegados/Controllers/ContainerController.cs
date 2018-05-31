using prjLegados.AzureStorage.Blobs;
using prjLegados.Data.Structure;
using prjLegados.AzureStorage.Helper;
using prjLegados.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjLegados.Data.AzureEntities;
using prjLegados.AzureStorage.Search;

namespace prjLegados.Controllers
{
    public class ContainerController : Controller
    {
        BlobStorage blobStorage = new BlobStorage();
        List<string> lstArchivos = new List<string>();
        ClsContainer guardar = new ClsContainer();

        User usrUser = new User();

        // GET: Container
        public ActionResult Container()
        {
            //TODO: 

            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            cntContenedores.usrUser = usrUser;
            return View(cntContenedores);
        }

        [HttpPost]
        public JsonResult sveContainer(string name)
        {
            
            //var blobStorage = new BlobStorage();
            var cntContainer = blobStorage.fntCreateBlobContainer(
                            new Container()
                            {
                                fntNameStr = name,
                                fntCompanyStr = usrUser.fntCompanyStr
                            });
            Mensaje mns = new Mensaje();
            mns.mensaje = "Contenedor Guardado";


            return Json(mns);
        }

        [HttpPost]
        public JsonResult dltContainer(string nombre)
        {
            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);

            var cntContainer = blobStorage.fntDeleteContainer(cntContenedores.lstContainers.FirstOrDefault(x => x.fntFullNameStr == nombre), usrUser);

            return Json(new
            {
                Result = "ERROR",
                Message = "Formato no válido"
            });
        }


        public PartialViewResult bscBlobs(string nombreContainer)
        {

            Container cnEncontrado = new Container();
            ClsContainer mdBlobs = new ClsContainer();

            mdBlobs.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            foreach (var item in mdBlobs.lstContainers)
            {
                if (nombreContainer.Equals(item.fntFullNameStr))
                {
                    cnEncontrado = item;
                }

            }
            mdBlobs.lstBlobs = blobStorage.fntListBlobsLst(cnEncontrado, usrUser, new List<string>());

            return PartialView("_Blobs", mdBlobs.lstBlobs);
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {

            //string path = Server.MapPath("~/Uploads/");
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                var direccion = Path.Combine(Server.MapPath("~/Uploads/"), file.FileName);
                //file.SaveAs(direccion);  
                lstArchivos.Add(direccion);

            }
            guardar.lstArchivos = lstArchivos;
            return Json(lstArchivos);
        }

        [HttpPost]
        public ActionResult fntsubirArchivos(string strDatosMetadata, string strNombreAplicacion, string strArchivos, string strNombreContainer)
        {
            string strNuevoArchivos = strArchivos.Substring(9);
            Metadata mtdArchivoMetadata = new Metadata();
            Container cntEncontrado = new Container();
            List<FileUpload> lstFileUpload = new List<FileUpload>();
            string[] strDatoMetadata = strDatosMetadata.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] strPath = strNuevoArchivos.Split(',');
            cntEncontrado = buscarContainer(strNombreContainer);

            //Guarda cada atributo en la clase metadata en la clase metadata
            mtdArchivoMetadata.strStorageContentType = strDatoMetadata[0];
            mtdArchivoMetadata.strStorageName = strDatoMetadata[1];
            mtdArchivoMetadata.strContentType = strDatoMetadata[2];
            mtdArchivoMetadata.strLanguage = strDatoMetadata[3];
            mtdArchivoMetadata.strAutor = strDatoMetadata[4];

            //guardar cada atributo en la clase fileupload
            foreach (string strDireccionArchivo in strPath)
            {
                lstFileUpload.Add(new FileUpload
                {
                    fntApplicationStr = strNombreAplicacion,
                    fntCompanyStr = "Maresa",
                    fntNameFileStr = Path.GetFileNameWithoutExtension(strDireccionArchivo).Replace(' ', '-'),
                    fntLocalPathStr = strDireccionArchivo
                });
            }


            blobStorage.fntUploadBlobs(cntEncontrado, lstFileUpload, usrUser);


            return Json("Sus Archivos se estan subiendo con satisfaccion");
        }

        public Container buscarContainer(string strNombreContainer)
        {
            ClsContainer cntContenedores = new ClsContainer();
            Container cntTemp = new Container();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            foreach (var item in cntContenedores.lstContainers)
            {
                if (strNombreContainer.Equals(item.fntFullNameStr))
                {
                    cntTemp = item;
                }

            }
            return cntTemp;
        }

        public ActionResult DeleteContainer()
        {
            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            cntContenedores.usrUser = usrUser;
            return PartialView("_DeleteContainer", cntContenedores);
        }

        public ActionResult Upload()
        {

            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            cntContenedores.usrUser = usrUser;
            return PartialView("_Upload", cntContenedores);
        }

        public ActionResult NewUpload()
        {
            ClsContainer cntContenedores = new ClsContainer();

            cntContenedores.lstContainers = blobStorage.fntListBlobContainerLst(usrUser);
            cntContenedores.usrUser = usrUser;
            return PartialView("_NewUpload", cntContenedores);
        }

        public ActionResult FileView() {
            return PartialView("_FileView");
        }

        [HttpPost]
        public ActionResult fntUploadFiles(List<DocumentUpload> lstDocuments)
        {
            // Upload files for each container
            foreach (var strContainer in lstDocuments.Select(x => x.Container).Distinct())
            {
                var cntEncontrado = buscarContainer(strContainer);

                //guardar cada atributo en la clase fileupload
                var lstFileUpload = from doc in lstDocuments.Where(l => l.Container.Equals(strContainer))
                                    select (new FileUpload()
                                    {
                                        fntNameFileStr = doc.AlterName+"."+doc.Extension,
                                        fntApplicationStr = doc.Application,
                                        fntCompanyStr = doc.Company,
                                        fntLocalPathStr = doc.Path,
                                        fntMetadataDct = doc.AsDictionary()
                                    });

                //Upload documents to azure storage
                var lstBlobsUploads = blobStorage.fntUploadBlobs(cntEncontrado, lstFileUpload, usrUser);

                //Index pdf documents in list lstBlobs
                lstBlobsUploads = new AzureStorage.Search.IndexFiles().AddDocumentsToIndex(lstBlobsUploads, usrUser);
            }
            return Json("Tengo los archivos");
        }
        [HttpPost]
        public JsonResult fntAzureSearch(clsDocumentPdfSearch parameters) {
            
            IndexFiles indexFiles = new IndexFiles();
            List<Enlace>lstTempEnlaces= new List<Enlace>();
            ClsContainer cntContenedores = new ClsContainer();
            try
            {
                var lstResult = indexFiles.SearchDocuments(parameters, usrUser);
                foreach (var item in lstResult)
                {
                    lstTempEnlaces.Add(new Enlace
                    {
                        alterName = item.AlterName,
                        referencialMonth = item.ReferencialMonth,
                        referencialYear = item.ReferencialYear,
                        container = item.Container,
                        application = item.Application,
                        module = item.Module,
                        documentUploadAt = item.DocumentUploadAt,
                        page = item.Page,
                        uri = fntReturnUri(item.Container, item.FullName, item.Page)
                    });
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                cntContenedores.lstEnlaces = lstTempEnlaces;
            }

            return Json(cntContenedores.lstEnlaces);
        }
        
        public string fntReturnUri(string strContainerName, string strBlobFullName, int intPageNumber) {
            var uri = blobStorage.fntGetBlobSasUri(strContainerName, strBlobFullName, intPageNumber);
            return uri;
        }
    }

}