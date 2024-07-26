using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class LineGroup
    {
        public LineGroup()
        {
            Lines = new HashSet<Line>();
        }

        public int Code { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Line> Lines { get; set; }
    }
}
