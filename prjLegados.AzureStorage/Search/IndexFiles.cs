using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using prjLegados.AzureStorage.Common;
using prjLegados.AzureStorage.Helper;
using prjLegados.Data.AzureEntities;
using prjLegados.Data.Structure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace prjLegados.AzureStorage.Search
{
    public class IndexFiles
    {
        //Process batch each 1k registers
        private const int intRegistersSkip = 1000;
        private const string strExtensions = "pdf";
        //private readonly string[] lstExtensions = { "pdf", "doc", "docx" };

        /// <summary>
        /// Add documents to index document by company's user
        /// </summary>
        /// <param name="lstBlobs">list of blobs</param>
        /// <param name="usrUser">user data</param>
        /// <returns></returns>
        public List<BlobStructure> AddDocumentsToIndex(List<BlobStructure> lstBlobs, User usrUser)
        {
            var idxSearch = CreateIfNotExistIndex(usrUser, cnsIndexTypes.Documents);
            // Initialize client search
            var client = new SearchIndexClient(ConfigurationManager.AppSettings["SearchServiceName"],
                                               idxSearch.IndexName,
                                               new SearchCredentials(ConfigurationManager.AppSettings["SearchApiKey"]));
            // Only uploaded files
            foreach (var blbBlob in lstBlobs.Where(x => x.IsUploaded))
            {
                // remove path metadata (local path is not stored)
                blbBlob.dctMetadata.Remove(cnsBlobMetadata.Path);
                // transform tags to collection string
                var strTags = blbBlob.dctMetadata[cnsBlobMetadata.Tags];
                if (strTags != null)
                    blbBlob.dctMetadata[cnsBlobMetadata.Tags] = strTags.ToString().Split(',');
                // get full blob name with out extension
                var strFileNameWithOutExt = blbBlob.fntNameStr.Split('.')[0];
                
                // If files not are valid documents
                if (!strExtensions.Equals(blbBlob.dctMetadata[cnsBlobMetadata.Extension].ToString()))
                {
                    continue;
                }

                try
                {
                    var lstActions = new List<IndexAction<clsDocumentPdfIndex>>();

                    // download blob like stream var
                    var streamFile = new Blobs.BlobStorage().fntGetStreamBlob(blbBlob);
                    streamFile.Position = 0;

                    // Create a index action by each page
                    using (PdfReader reader = new PdfReader(streamFile))
                    {
                        for (int indexPage = 1; indexPage <= reader.NumberOfPages; indexPage++)
                        {
                            // transform dictionary to object
                            var dcmMetadata = blbBlob.dctMetadata.ToObject<clsDocumentPdfIndex>();
                            dcmMetadata.Container = blbBlob.cntContainer.fntFullNameStr;
                            dcmMetadata.Uri = blbBlob.fntUri.AbsoluteUri;
                            dcmMetadata.FullName = blbBlob.fntNameStr;
                            // add index page before extension file
                            dcmMetadata.Id = MakeSafeId(strFileNameWithOutExt + "_" + indexPage + "." + dcmMetadata.Extension);
                            dcmMetadata.Content = PdfTextExtractor.GetTextFromPage(reader, indexPage);
                            dcmMetadata.Page = indexPage;

                            lstActions.Add(IndexAction.MergeOrUpload(dcmMetadata));
                        }
                    }

                    // Index batch process
                    for (int i = 0; i < (int)Math.Ceiling(lstActions.Count / (double)intRegistersSkip); i++)
                    {
                        client.Documents.Index(new IndexBatch<clsDocumentPdfIndex>(lstActions.Skip(i * intRegistersSkip).Take(intRegistersSkip)));
                    }

                    lstBlobs.First(l => l.fntNameStr == blbBlob.fntNameStr).IsIndexed = true;
                }
                catch (Exception ex)
                {

                    lstBlobs.First(l => l.fntNameStr == blbBlob.fntNameStr).IsIndexed = false;
                }
                
            }

            return lstBlobs;
        }


        /// <summary>
        /// Delete documents from index document by company's user 
        /// </summary>
        /// <param name="lstBlobs"></param>
        /// <param name="usrUser"></param>
        /// <returns></returns>
        public List<BlobStructure> DeleteDocumentsFromIndex(List<BlobStructure> lstBlobs, User usrUser)
        {
            var idxSearch = CreateIfNotExistIndex(usrUser, cnsIndexTypes.Documents);
            // Initialize client search
            var client = new SearchIndexClient(ConfigurationManager.AppSettings["SearchServiceName"],
                                               idxSearch.IndexName,
                                               new SearchCredentials(ConfigurationManager.AppSettings["SearchApiKey"]));
            // Only uploaded files
            foreach (var blbBlob in lstBlobs.Where(x => x.IsUploaded))
            {
                var dcmMetadata = blbBlob.dctMetadata.ToObject<clsDocumentPdfIndex>();
                var strFileNameWithOutExt = blbBlob.fntNameStr.Split('.')[0];
                // If files not are valid documents
                if (!strExtensions.Equals(dcmMetadata.Extension))
                {
                    continue;
                }

                try
                {
                    var lstActions = new List<IndexAction<clsDocumentPdfIndex>>();

                    // download blob like stream var
                    var streamFile = new Blobs.BlobStorage().fntGetStreamBlob(blbBlob);
                    streamFile.Position = 0;

                    // Create a index action by each page
                    using (PdfReader reader = new PdfReader(streamFile))
                    {
                        for (int indexPage = 1; indexPage <= reader.NumberOfPages; indexPage++)
                        {
                            // add index page before extension file
                            dcmMetadata.Id = MakeSafeId(strFileNameWithOutExt + "_" + indexPage + "." + dcmMetadata.Extension);

                            lstActions.Add(IndexAction.Delete(dcmMetadata));
                        }
                    }

                    // Index batch process
                    for (int i = 0; i < (int)Math.Ceiling(lstActions.Count / (double)intRegistersSkip); i++)
                    {
                        client.Documents.Index(new IndexBatch<clsDocumentPdfIndex>(lstActions.Skip(i * intRegistersSkip).Take(intRegistersSkip)));
                    }

                    lstBlobs.First(l => l.fntNameStr == blbBlob.fntNameStr).IsIndexed = true;
                }
                catch (Exception)
                {

                    lstBlobs.First(l => l.fntNameStr == blbBlob.fntNameStr).IsIndexed = false;
                }

            }

            return lstBlobs;
        }


        /// <summary>
        /// Search a term in the index document by company's user
        /// </summary>
        /// <param name="searchParameters">parameters for search</param>
        /// <param name="usrUser">user data</param>
        /// <returns></returns>
        public IEnumerable<clsDocumentPdfIndex> SearchDocuments(clsDocumentPdfSearch searchParameters, User usrUser)
        {
            var idxSearch = CreateIfNotExistIndex(usrUser, cnsIndexTypes.Documents);
            // Initialize client search
            var client = new SearchIndexClient(ConfigurationManager.AppSettings["SearchServiceName"],
                                               idxSearch.IndexName,
                                               new SearchCredentials(ConfigurationManager.AppSettings["SearchApiKey"]));

            var strFilter = String.Join(" and ", searchParameters.FilterMetadata.Select(x => x.FilterExpression).ToArray());
            ;
            var parameters = new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                Filter = strFilter,
                Top = searchParameters.TopResults,
                Skip = searchParameters.SkipResults
            };

            var searchResults = client.Documents.Search<clsDocumentPdfIndex>(searchParameters.SearchWords, parameters).Results;

            var enumResult = from list in searchResults
                             select list.Document;

            return enumResult;
        }

        /// <summary>
        /// Create Index by company
        /// </summary>
        /// <param name="usrUser">user data</param>
        /// <param name="strIndexType">type of index like document or image, use clsIndexType</param>
        /// <returns></returns>
        private clsIndexSearch CreateIfNotExistIndex(User usrUser, string strIndexType)
        {
            // only documents for now
            if (strIndexType != cnsIndexTypes.Documents)
                return null;

            // Get the index for company only for documents
            var ctlOperations = new Catalogs().GetIndexDocumentByCompany(usrUser);
            
            // if is not null then return the class, else construct the index
            if (!(ctlOperations is null))
            {
                return ctlOperations;
            }
            else
            {
                var searchClient = new SearchServiceClient(ConfigurationManager.AppSettings["SearchServiceName"],
                                                       new SearchCredentials(ConfigurationManager.AppSettings["SearchApiKey"]));

                var strIndexName = "index" + usrUser.fntCompanyStr + strIndexType.ToLower();

                var idxDefinition = new Index()
                {
                    Name = strIndexName,
                    Fields = FieldBuilder.BuildForType<clsDocumentPdfIndex>()
                };

                searchClient.Indexes.Create(idxDefinition);

                var idxNew = new clsIndexSearch(usrUser.fntCompanyStr, strIndexType) { IndexName = strIndexName };

                return new Catalogs().InsertIndexDocumentByCompany(usrUser, idxNew);
            }
        }

        public void DropExistIndexDocuments(User usrUser)
        {
            var ctlOperations = new Catalogs().GetIndexDocumentByCompany(usrUser);

            // if is not null then return the class, else construct the index
            if (ctlOperations is null)
            {
                return;
            }
            else
            {
                var searchClient = new SearchServiceClient(ConfigurationManager.AppSettings["SearchServiceName"],
                                                       new SearchCredentials(ConfigurationManager.AppSettings["SearchApiKey"]));

                searchClient.Indexes.Delete(ctlOperations.IndexName);

                new Catalogs().DeleteIndexDocumentByCompany(usrUser);
            }
        }

        private string MakeSafeId(string input)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(input, "");

        }

    }
}
