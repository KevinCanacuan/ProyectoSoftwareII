//using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using prjLegados.Data.AzureEntities;
using prjLegados.Data.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.AzureStorage.Common
{
    public class Catalogs
    {
        public clsIndexSearch GetIndexDocumentByCompany(User usrUser)
        {
            var idxSearch = new clsIndexSearch();

            var tblIndexSearch = fntGetTableByName(cnsTableCatalog.IndexSearch);

            var queryFilter = new TableQuery<clsIndexSearch>().Where(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, usrUser.fntCompanyStr.ToLower())
                ).Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cnsIndexTypes.Documents)
                );

            idxSearch = tblIndexSearch.ExecuteQuery(queryFilter).FirstOrDefault();

            return idxSearch;
        }

        public void DeleteIndexDocumentByCompany(User usrUser)
        {
            var idxSearch = GetIndexDocumentByCompany(usrUser);

            var tblOperation = TableOperation.Delete(idxSearch);

            fntGetTableByName(cnsTableCatalog.IndexSearch).Execute(tblOperation);
        }

        public clsIndexSearch InsertIndexDocumentByCompany(User usrUser, clsIndexSearch idxNew)
        {

            var tblIndexSearch = fntGetTableByName(cnsTableCatalog.IndexSearch);

            var insertOperation = TableOperation.Insert(idxNew);
            tblIndexSearch.Execute(insertOperation);

            return idxNew;
        }

        public clsBlobUpload GetUploadDocument(string strContainerName, string strBlobFullName)
        {
            var blbUpload = new clsBlobUpload();

            var tblIndexSearch = fntGetTableByName(cnsTableCatalog.BlobUpload);

            var queryFilter = new TableQuery<clsBlobUpload>().Where(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, strContainerName + strBlobFullName)
                ).Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, strContainerName)
                );

            blbUpload = tblIndexSearch.ExecuteQuery(queryFilter).FirstOrDefault();

            return blbUpload;
        }

        public clsBlobUpload InsertUploadBlob(clsBlobUpload blbNew)
        {

            var tblIndexSearch = fntGetTableByName(cnsTableCatalog.BlobUpload);

            var insertOperation = TableOperation.InsertOrMerge(blbNew);
            tblIndexSearch.Execute(insertOperation);

            return blbNew;
        }

        public void DeleteUploadBlob(string strContainerName, string strBlobFullName)
        {
            var blbUpload = GetUploadDocument(strContainerName, strBlobFullName);

            var tblOperation = TableOperation.Delete(blbUpload);

            fntGetTableByName(cnsTableCatalog.BlobUpload).Execute(tblOperation);
        }

        private CloudTable fntGetTableByName(string tableName)
        {
            CloudStorageAccount cldStorageAccount = Common.fntCreateStorageAccountForTables();

            //Get the list of the blob from the above container
            CloudTableClient cldTable = cldStorageAccount.CreateCloudTableClient();

            CloudTable table = cldTable.GetTableReference(tableName);

            table.CreateIfNotExists();

            return table;
        }
    }
}
