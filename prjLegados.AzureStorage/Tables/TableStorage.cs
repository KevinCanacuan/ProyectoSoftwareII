using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjLegados.Data.Structure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.CosmosDB.Table;

namespace prjLegados.AzureStorage.Tables
{
    public class TableStorage : ITableStorage
    {
        CloudStorageAccount storageAccount;

        public TableStructure fntCreateTable(User usrUser, TableStructure tblNewTable, FileUpload filDataUpload)
        {
            throw new NotImplementedException();
        }

        public Task<Container> fntCreateTableContainer(Container cntContainer)
        {
            throw new NotImplementedException();
        }

        public Task fntDropTable(List<TableStructure> tblTable)
        {
            throw new NotImplementedException();
        }

        public Task fntGetData(TableStructure tblTable, TableSelectFrom tsfSelectStructure)
        {
            throw new NotImplementedException();
        }

        public List<TableStructure> fntListTable(User usrUser)
        {
            throw new NotImplementedException();
        }

        public List<Container> fntListTableContainer(User usrUser)
        {
            throw new NotImplementedException();
        }

        public List<TableStructure> fntListTables(Container cntContainer, User usrUser)
        {
            throw new NotImplementedException();
        }
    }
}
