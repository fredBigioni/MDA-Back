using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class Period
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
