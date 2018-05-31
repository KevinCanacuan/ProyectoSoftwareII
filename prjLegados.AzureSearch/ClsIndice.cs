using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Configuration;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using prjLegados.Data.Structure;

namespace prjLegados.AzureSearch
{
    public class ClsIndice
    {
        
    void fntIndexarDocumentoPdf(List<BlobStructure> blbItems)
        {
              

            foreach (var item in blbItems)
            {

                if (item.dctMetadata["tipo_archivo"].ToLower() != "pdf")
                {
                    return;
                }

                List<IndexAction> actions = new List<IndexAction>();


                using (PdfReader reader = new PdfReader(item.fntUri))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var dcmMetadata = new Document();
                        /*
                        foreach (var meta in item.dctMetadata)
                        {
                            dcmMetadata.Add("filename", item.fntNameStr + "." + item.dctMetadata["tipo_archivo"]);
                           // dcmMetadata.Add("id", MakeSafeId(name + "_" + i + "." + ext));
                            dcmMetadata.Add("content", PdfTextExtractor.GetTextFromPage(reader, i));
                            dcmMetadata.Add("page", i);
                            dcmMetadata.Add("descripcion", item.dctMetadata["descripcion"]);
                            dcmMetadata.Add("empresa", item.dctMetadata["empresa"]);
                            dcmMetadata.Add("aplicativo", item.dctMetadata["aplicativo"]);
                            dcmMetadata.Add("modulo", item.dctMetadata["modulo"]);
                            dcmMetadata.Add("creacion_documento", item.dctMetadata["creacion_documento"]);
                            dcmMetadata.Add("fecha_carga", item.dctMetadata["fecha_carga"]);
                            dcmMetadata.Add("anio", item.dctMetadata["anio"]);
                            dcmMetadata.Add("mes", item.dctMetadata["mes"]);
                            actions.Add(new IndexAction(IndexActionType.MergeOrUpload, dcmMetadata));
                        }
                        */
                        

                        

                    }
                }

                var client = new SearchIndexClient(ConfigurationManager.AppSettings["SearchServiceName"],
                                                   ConfigurationManager.AppSettings["IndexName"],
                                                   new SearchCredentials(ConfigurationManager.AppSettings["SearchApiKey"]));


                for (int i = 0; i < (int)Math.Ceiling(actions.Count / 1000.0); i++)
                {
                    client.Documents.Index(new IndexBatch(actions.Skip(i * 1000).Take(actions.Count - (i * 1000))));
                }
            }
        }

    }

}
    

