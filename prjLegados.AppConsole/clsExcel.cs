using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace prjLegados.AppConsole
{
   public class clsExcel
    {
        public DataTable fntGetExcels()
        {
            string strvalue = ConfigurationManager.AppSettings["Direccion"];
            string strconnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" +strvalue +";Extended Properties='Excel 12.0 Macro;HDR=YES'";

            OleDbConnection OleDbconn = new OleDbConnection(strconnectionString);
            OleDbCommand OleDbcmd = new OleDbCommand();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter();

           DataTable dtlDataTable = new DataTable();
           OleDbcmd.Connection = OleDbconn;

            OleDbconn.Open();
            OleDbcmd.CommandText = "SELECT * From [Hoja1$]";
            dataAdapter.SelectCommand = OleDbcmd;
            dataAdapter.Fill(dtlDataTable);
            OleDbconn.Close();
            return dtlDataTable;

    

        }
    }
}
