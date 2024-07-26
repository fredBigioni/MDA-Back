using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace WebApi.Entities
{
    public partial class ProductType
    {

       public ProductType()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
        }
        public int Code { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }
    }
}
