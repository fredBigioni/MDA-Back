using System;

namespace WebApi.Entities
{
    public partial class Log
    {
        public int Code { get; set; }
        public string? Type { get; set; }
        public string Description { get; set; }
        public string UserLog { get; set; }
        public DateTime Date { get; set; }
        public int? CustomMarketCode { get; set; }
    }
}
