using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
    public class Container
    {
        public string fntNameStr { get; set; }
        public Uri fntUri { get; set; }
        public string fntCompanyStr { get; set; }

        public string fntFullNameStr { get { return fntCompanyStr + "-" + fntNameStr.Replace(fntCompanyStr+"-",""); } }
    }
}
