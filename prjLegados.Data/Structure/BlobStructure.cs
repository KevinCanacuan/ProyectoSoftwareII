using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
    public class BlobStructure
    {
        public int fntIdInt { get; set; }
        /// <summary>
        /// Full name of the file {company}-{application}-{filename}.{extension}
        /// </summary>
        public string fntNameStr { get; set; }
        public Uri fntUri { get; set; }
        public Container cntContainer { get; set; }
        public bool IsUploaded { get; set; }
        public bool IsIndexed { get; set; }
        public Dictionary<string, object> dctMetadata { get; set; } 

    }
}
