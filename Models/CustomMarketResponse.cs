using System.Text.Json.Serialization;
using WebApi.Entities;
using System.Collections.Generic;
using System.Text.Json;

namespace WebApi.Models
{
    public class CustomMarketResponse
    {        
        public int Code { get; set; }
        public bool? ControlPanel { get; set; }
        public string Description { get; set; }
        public bool DrugReport { get; set; }
        public string Footer { get; set; }
        public string Header { get; set; }
        public int IsOtc { get; set; }
        public bool? LabReport { get; set; }
        public int? LineCode { get; set; }
        public string LineDescription { get; set; }
        public int? LineGroupCode { get; set; }
        public string LineGroupDescription { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public int? MarketClass { get; set; }
        public int? MarketFilter { get; set; }
        public int? MarketReference { get; set; }
        public int Order { get; set; }
        public bool? ProductReport { get; set; }
        public bool? TcReport { get; set; }
        public bool TestMarket { get; set; }
        public bool? TravelCrm { get; set; }

        [JsonPropertyName("customMarketDetail")]
        public virtual ICollection<CustomMarketDetailResponse> CustomMarketDetailResponses { get; set; }

        public CustomMarketResponse(
            int code, 
            string description, 
            bool drugReport,
            string footer,
            string header,
            bool? labReport,
            Line line, 
            int? marketClass,
            int? marketFilter,
            int? marketReference,
            int order,
            bool? porductReport,
            bool testMarket,
            bool? controlPanel,
            bool? tcReport,   
            bool? travelCrm,
            List<CustomMarketDetailResponse> customMarketDetailResponses)
        {
            Code = code;
            Description = description;
            DrugReport = drugReport;
            Footer = footer;
            Header = header;
            LabReport = labReport;
            LineCode =  line?.Code;
            LineDescription = line?.Description;
            LineGroupCode =  line?.LineGroup?.Code;
            LineGroupDescription = line?.LineGroup?.Description;
            MarketClass = marketClass;
            MarketFilter = marketFilter;
            MarketReference = marketReference;
            Order = order;
            ProductReport = porductReport;
            TestMarket = testMarket;
            ControlPanel = controlPanel;
            TcReport = tcReport;
            TravelCrm = travelCrm;
            CustomMarketDetailResponses = customMarketDetailResponses;
        }        
    }
}