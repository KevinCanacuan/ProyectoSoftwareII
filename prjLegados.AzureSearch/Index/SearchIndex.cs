using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using prjLegados.Data.Structure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace prjLegados.AzureSearch.Index
{
    public class SearchIndex
    {
        //Process batch each 1k registers
        private const int intRegistersSkip = 1000;
        private const string strExtensions = "pdf" ;
        //private readonly string[] lstExtensions = { "pdf", "doc", "docx" };
        public void IndexDocuments(List<BlobStructure> lstBlobs)
        {
            object strExtension;
            object strName;
            var blbOp = new AzureStorage.Blobs.BlobStorage();
            foreach (var blbBlob in lstBlobs)
            {

                blbBlob.dctMetadata.TryGetValue(cnsBlobMetadata.Extension,out strExtension);
                blbBlob.dctMetadata.TryGetValue(cnsBlobMetadata.AlterName, out strName);
                
                // If files not are valid documents
                if (!strExtensions.Equals(strExtension))
                {
                    return;
                }
                List<IndexAction> lstActions = new List<IndexAction>();

                // Create a index action by each page
                // download blob like stream var
                using (PdfReader reader = new PdfReader(blbOp.fntGetStreamBlob(blbBlob)))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var dcmMetadata = new Document();

                        // Get all metadata from blob
                        foreach (var meta in blbBlob.dctMetadata)
                        {
                            dcmMetadata.Add(meta.Key, meta.Value);
                        }

                        dcmMetadata.Add("container", blbBlob.cntContainer.fntFullNameStr);
                        dcmMetadata.Add("uri", blbBlob.fntUri);
                        dcmMetadata.Add("fullname", blbBlob.fntNameStr + "." + strExtension);
                        dcmMetadata.Add("id", MakeSafeId(strName + "_" + i + "." + strExtension));
                        dcmMetadata.Add("content", PdfTextExtractor.GetTextFromPage(reader, i));
                        dcmMetadata.Add("page", i);

                        lstActions.Add(IndexAction.MergeOrUpload(dcmMetadata));
                    }
                }

                // Initialize client search
                var client = new SearchIndexClient(ConfigurationManager.AppSettings["SearchServiceName"],
                                                   ConfigurationManager.AppSettings["IndexName"],
                                                   new SearchCredentials(ConfigurationManager.AppSettings["SearchApiKey"]));

                // Index batch process
                for (int i = 0; i < (int)Math.Ceiling(lstActions.Count / (double)intRegistersSkip); i++)
                {
                    client.Documents.Index(new IndexBatch(lstActions.Skip(i * intRegistersSkip).Take(lstActions.Count - (i * intRegistersSkip))));
                }
            }
        }

        private static string MakeSafeId(string input)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(input, "");

        }
    }
}
