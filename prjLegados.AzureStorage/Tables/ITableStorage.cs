using prjLegados.Data.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.AzureStorage.Tables
{
    public interface ITableStorage
    {
        Task<Container> fntCreateTableContainer(Container cntContainer);

        List<Container> fntListTableContainer(User usrUser);

        List<TableStructure> fntListTables(Container cntContainer, User usrUser);

        TableStructure fntCreateTable(User usrUser, TableStructure tblNewTable, FileUpload filDataUpload);

        List<TableStructure> fntListTable(User usrUser);

        Task fntGetData(TableStructure tblTable, TableSelectFrom tsfSelectStructure);

        Task fntDropTable(List<TableStructure> tblTable);
    }
}
