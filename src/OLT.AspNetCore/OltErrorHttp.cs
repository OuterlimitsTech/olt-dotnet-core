using System;
using System.Collections.Generic;
using System.Text.Json;

namespace OLT.Core
{
    public class OltErrorHttp : IOltErrorHttp
    { 
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();            
    }
}
