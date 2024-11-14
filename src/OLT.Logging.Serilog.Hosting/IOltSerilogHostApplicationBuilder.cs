using Serilog;

namespace OLT.Core
{
    public interface IOltSerilogHostApplicationBuilder : IOltApplicationBuilder
    {
        LoggerConfiguration LoggerConfiguration { get; }
    }

}

