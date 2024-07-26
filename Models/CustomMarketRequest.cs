using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class CustomMarketRequest
    {
        [Required]
        public bool ControlPanel { get; set; }
        public string Description { get; set; }
        public bool DrugReport { get; set; }
        public string Footer { get; set; }
        public string Header { get; set; }
        public int IsOtc { get; set; }
        public bool? LabReport { get; set; }
        public int? LineCode { get; set; }
        public int? MarketClass { get; set; }
        public int? MarketFilter { get; set; }
        public int? MarketReference { get; set; }
        public int Order { get; set; }
        public bool? ProductReport { get; set; }
        public bool? Tcreport { get; set; }
        public bool TestMarket { get; set; }
        public bool TravelCrm { get; set; }

        public virtual ICollection<CustomMarketDetailRequest> CustomMarketDetail { get; set; }
    }
}