using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class LaboratoryGroup
    {
        public LaboratoryGroup()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            LaboratoryGroupDetails = new HashSet<LaboratoryGroupDetail>();
        }

        public int Code { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }

        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }

        public virtual ICollection<LaboratoryGroupDetail> LaboratoryGroupDetails { get; set; }
    }
}
