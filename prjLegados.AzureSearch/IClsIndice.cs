using prjLegados.Data.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.AzureSearch
{
    public interface IClsIndice

    {
        void fntIndexarDocumentoPdf(List<BlobStructure>lstBlobs);
        string fntCrearIdStr(string strInput);
        void fntCrearIndice();
    }
}
