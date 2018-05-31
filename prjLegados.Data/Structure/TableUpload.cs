using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
    public class TableUpload
    {
        public string fntNameStr { get; set; }
        public string fntLocalPathStr { get; set; }
        public string fntCompanyStr { get; set; }
        public string fntApplicationStr { get; set; }
        public string fntFullNameStr { get { return fntApplicationStr + "-" + fntNameStr; } }
        public Dictionary<string,Type> fntFieldsDct { get; set; }
    }
}
