using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace WebApi.Entities
{
    public partial class Line
    {
        public Line()
        {
            CustomMarkets = new HashSet<CustomMarket>();
        }

        public int Code { get; set; }
        [JsonIgnore]
        public int LineGroupCode { get; set; }
        public string Description { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string LaboratoryReportHeader { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string LaboratoryReportFooter { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string DrugReportHeader { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public string DrugReportFooter { get; set; }
        [JsonIgnore]
        public string Status { get; set; }
        [JsonIgnore]
        public byte[] Stamp { get; set; }

        public virtual LineGroup LineGroup { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomMarket> CustomMarkets { get; set; }
    }
}
