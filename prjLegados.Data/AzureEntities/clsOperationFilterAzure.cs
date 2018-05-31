using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.AzureEntities
{
    public class clsOperationFilterAzure
    {
        private string strStartsWithPattern = "/{0}.*/";
        private string strEndsWithPattern = "/.*{0}/";
        private string strContainsPattern = "/.*{0}.*/";
        private string getRegPattern(string pattern, string value)
        {
            string strPattern;

            switch (pattern.ToLower())
            {
                case "startswith":
                    strPattern = String.Format(strStartsWithPattern, value);
                    break;
                case "endswith":
                    strPattern = String.Format(strEndsWithPattern, value);
                    break;
                default:
                    strPattern = String.Format(strContainsPattern, value);
                    break;
            }
            return strPattern;
        }
        public string FieldName { get; set; }
        public string ComparisonExpresion { get; set; }
        public string FieldValue { get; set; }
        public string FieldType { get; set; }

        public string FilterExpression
        {
            get
            {
                string filter;
                // return for tag
                if (FieldName.ToLower().Equals("tags"))
                    return "search.in(tags,'" + FieldValue + "')";

                if (ComparisonExpresion.Length > 2)
                    filter = "search.ismatch" + "('" +
                            getRegPattern(ComparisonExpresion, FieldValue) +
                            "','" + FieldName + "','full','any')";
                else
                    filter = FieldName + " " + ComparisonExpresion + " " +
                            (FieldType.ToLower() == "string" ? "'" + FieldValue + "'" : FieldValue);

                return filter;
            }
        }
    }
}
