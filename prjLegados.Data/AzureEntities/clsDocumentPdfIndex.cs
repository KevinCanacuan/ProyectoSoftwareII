using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;

namespace prjLegados.Data.AzureEntities
{
    [SerializePropertyNamesAsCamelCase]
    public partial class clsDocumentPdfIndex
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string Id { get; set; }
        [IsFilterable, IsSortable, IsFacetable]
        public string Company { get; set; }
        [IsFilterable, IsSortable, IsFacetable]
        public string Container { get; set; }
        [IsFilterable]
        public string Uri { get; set; }
        [IsSearchable]
        public string FullName { get; set; }
        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EsLucene)]
        public string Content { get; set; }
        [IsFilterable, IsSortable]
        public int Page { get; set; }
        [IsFilterable, IsSortable, IsFacetable]
        public string Application { get; set; }
        [IsSearchable,IsFilterable, IsSortable, IsFacetable]
        public string Description { get; set; }
        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string AlterName { get; set; }
        [IsSearchable, IsFilterable, IsSortable]
        public string FileName { get; set; }
        [IsFilterable, IsSortable]
        public string Extension { get; set; }
        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string Module { get; set; }
        [IsFilterable, IsSortable, IsFacetable]
        public DateTimeOffset? DocumentUploadAt { get; set; }
        [IsFilterable, IsSortable, IsFacetable]
        public int? ReferencialYear { get; set; }
        [IsFilterable, IsSortable, IsFacetable]
        public int? ReferencialMonth { get; set; }
        [IsSearchable, IsFilterable, IsFacetable]
        public string[] Tags { get; set; }
    }
}
