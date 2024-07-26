using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class ProductPresentationGroup
    {
        public ProductPresentationGroup()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            ProductPresentationGroupDetails = new HashSet<ProductPresentationGroupDetail>();
        }

        public int Code { get; set; }
        public string Description { get; set; }
        public bool ExpandGroup { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }

        public virtual ICollection<ProductPresentationGroupDetail> ProductPresentationGroupDetails { get; set; }        
    }
}
