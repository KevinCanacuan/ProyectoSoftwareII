using prjLegados.Data.AzureEntities;
using prjLegados.Data.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjLegados.Models
{
    public class ClsContainer
    {
        public Container cntCurrentContainer{ get; set;}

        public List<Container> lstContainers { get; set; }
        public User usrUser { get; set; }

        public List<BlobStructure> lstBlobs { get; set; }
        public BlobStructure blbBlob { get; set; }

        public Metadata mtdMetadata { get; set; }
        public List<string>lstArchivos { get; set; }
        public List<Enlace> lstEnlaces { get; set; }
    }
}