using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
    public class User
    {
        public User()
        {
            fntUserNameStr = "UsuarioPrueba";
            fntFirstNameStr = "Usuario";
            fntLastNameStr = "Prueba";
            fntCompanyStr = "maresa";
            fntAplicationsLst = new List<string>() { "baaniv", "dms", "zeuz" };
            fntIsAdminBln = true;
            fntIsContributorBln = false;
            fntIsContributorBln = false;

        }
        public string fntUserNameStr { get; set; }
        public string fntFirstNameStr { get; set; }
        public string fntLastNameStr { get; set; }
        public string fntCompanyStr { get; set; }
        public List<string> fntAplicationsLst { get; set; }
        public bool fntIsAdminBln { get; set; }
        public bool fntIsContributorBln { get; set; }
        public bool fntIsReaderBln { get; set; }
    }
}
