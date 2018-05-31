using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
    public class TableSelectFrom
    {
        public List<FieldsForSelect> fntFieldsSelectLst { get; set; }
        public List<string> fntTablesFromLst { get; set; }
        public string fntWhereClauseStr { get; set; }
    }

    public class FieldsForSelect
    {
        public string fntTableFromStr { get; set; }
        public string fntFieldStr { get; set; }
    }
}
