using Microsoft.Azure.CosmosDB.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.AzureEntities
{
    public class clsIndexSearch : TableEntity
    {
        public clsIndexSearch(string strCompanyName, string strIndexType)
        {
            PartitionKey = strIndexType;
            RowKey = strCompanyName;
        }

        public string IndexName { get; set; }

        public clsIndexSearch() { }
    }
}
