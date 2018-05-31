using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
    public class TableStructure
    {
        public int fntIdInt { get; set; }
        public string fntNameStr { get; set; }
        public string fntCompanyStr { get; set; }
        public string fntApplicationStr { get; set; }
        private string fntPrefixStr { get { return fntCompanyStr + "-" + fntApplicationStr + "-"; } }
        public string fntFullNameStr { get { return fntPrefixStr + fntNameStr.Replace(fntPrefixStr, ""); } }
        public Uri fntUri { get; set; }
        public List<TableFieldStructure> fntFieldsLst { get; set; }
    }

    public class TableFieldStructure
    {
        public string fntFieldStr { get; set; }
        public string fntTypeStr { get; set; }
        public bool fntIsMandatoryBln { get; set; }
        public bool fntIsNullableBln { get; set; }
    }
}
