using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjLegados.Models
{
    public class Enlace
    {
        public string alterName { get; set; }
        public int page { get; set; }
        public string uri { get; set; }
        public string container { get; set; }
        public DateTimeOffset? documentUploadAt { get; set; }
        public string application { get; set; }
        public string module { get; set; }
        public int? referencialYear { get; set; }
        public int? referencialMonth { get; set; }



    }
}