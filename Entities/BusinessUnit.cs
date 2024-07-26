using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace WebApi.Entities
{
    public partial class BusinessUnit
    {
        public BusinessUnit()
        {
            ProductPresentations = new HashSet<ProductPresentation>();
        }        
        public int Code { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductPresentation> ProductPresentations { get; set; }
    }
}
