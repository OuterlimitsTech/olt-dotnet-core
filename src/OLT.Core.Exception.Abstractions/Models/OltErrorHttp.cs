using System.Collections.Generic;

namespace OLT.Core
{
    /// <summary>
    /// General class for Http Errors (used for frontend standardization)
    /// </summary>
    public class OltErrorHttp : IOltErrorHttp
    { 
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();            
    }
}
