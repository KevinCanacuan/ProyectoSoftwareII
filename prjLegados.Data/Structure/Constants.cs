using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
    public class cnsBlobMetadata
    {
        public const string Company = "Company";
        public const string Aplication = "Application";
        public const string Description = "Description";
        public const string AlterName = "AlterName";
        public const string FileName = "FileName";
        public const string Extension = "Extension";
        public const string Module = "Module";
        public const string DocumentUploadAt = "DocumentUploadAt";
        public const string ReferencialYear = "ReferencialYear";
        public const string ReferencialMonth = "ReferencialMonth";
        public const string Path = "Path";
        public const string Tags = "Tags";
        // Index of Name's blobs in Uri
        public const int IndexOfNameUri = 2;
    }

    public class cnsContainerMetadata
    {
        public const string Company = "Company";
        // Index of Name's blobs in Uri
        public const int IndexOfNameUri = 1;
    }

    public class cnsTableMetadata
    {
        public const string Company = "Company";
        public const string Aplication = "Application";
        public const string Description = "Description";
        public const string AlterName = "AlterName";
        // Index of Name's blobs in Uri
        public const int IndexOfNameUri = 2;
    }

    public class cnsTableCatalog
    {
        public const string IndexSearch = "IndexSearch";
        public const string BlobUpload = "BlobUpload";
    }

    public class cnsIndexTypes
    {
        public const string Documents = "documents";
        public const string Images = "images";
    }
}
