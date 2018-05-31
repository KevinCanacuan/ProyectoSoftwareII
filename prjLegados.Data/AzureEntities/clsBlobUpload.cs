using Microsoft.Azure.CosmosDB.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.AzureEntities
{
    public class clsBlobUpload : TableEntity
    {
        public clsBlobUpload(string strConatinerName, string strFullNameBlob)
        {
            PartitionKey = strConatinerName;
            RowKey = strConatinerName+strFullNameBlob;
            BlobFullName = strFullNameBlob;
            Container = strConatinerName;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public clsBlobUpload() { }

        public string BlobFullName { get; set; }
        public string Container { get; set; }
        public string OriginalFileName { get; set; }
        public string AlterFileName { get; set; }
        public string Application { get; set; }
        public string User { get; set; }

        
    }
}
