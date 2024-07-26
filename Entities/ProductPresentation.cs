using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace WebApi.Entities
{
    public partial class ProductPresentation
    {
        public ProductPresentation()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            ProductPresentationGroupDetails = new HashSet<ProductPresentationGroupDetail>();
        }

        public int Code { get; set; }
        
        [JsonIgnore]
        public int ProductCode { get; set; }
        public string Imscode { get; set; }
        public string EanCode { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public DateTime? LaunchingDate { get; set; }
        [JsonIgnore]
        public int? TherapeuticalClassCode { get; set; }
        public int? Classcode { get; set; }
        public int? BusinessUnitCode { get; set; }
        public bool? IsModified { get; set; }

        public virtual BusinessUnit BusinessUnit { get; set; }
        public virtual Class Class { get; set; }
        public virtual Product Product { get; set; }
        public virtual TherapeuticalClass TherapeuticalClass { get; set; }

        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductPresentationGroupDetail> ProductPresentationGroupDetails { get; set; }
    }
}
