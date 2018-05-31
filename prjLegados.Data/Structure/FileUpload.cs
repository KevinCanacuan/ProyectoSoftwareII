using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
    public class FileUpload
    {
        public string fntNameFileStr { get; set; }
        public string fntLocalPathStr { get; set; }
        public string fntCompanyStr { get; set; }
        public string fntApplicationStr { get; set; }
        public string fntFullNameStr { get { return fntCompanyStr + "-" + fntApplicationStr + "-" + fntNameFileStr; } }
        public FileStream fntStreamFileFil { get; set; }
        public Dictionary<string, object> fntMetadataDct { get; set; }
    }

    public class DocumentUpload
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string AlterName { get; set; }
        public string Extension { get; set; }
        public string Company { get; set; }
        public string Container { get; set; }
        public string Application { get; set; }
        public string Module { get; set; }
        public DateTimeOffset DocumentUploadAt { get; set; }
        public int ReferencialYear { get; set; }
        public int ReferencialMonth { get; set; }
        public string Path { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
    }
}
