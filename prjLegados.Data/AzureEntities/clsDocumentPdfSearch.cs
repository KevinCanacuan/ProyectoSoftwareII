using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.AzureEntities
{
    public class clsDocumentPdfSearch
    {
        public clsDocumentPdfSearch()
        {
            TopResults = 10;
            SkipResults = 0;
            FilterMetadata = new List<clsOperationFilterAzure>();
        }
        public string SearchWords { get; set; }
        public int TopResults { get; set; }
        public int SkipResults { get; set; }
        public List<clsOperationFilterAzure> FilterMetadata { get; set; }
    }
}
