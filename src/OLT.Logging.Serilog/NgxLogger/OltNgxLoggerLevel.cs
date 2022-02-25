namespace OLT.Logging.Serilog
{
    /// <summary>
    /// ngx-logger <see href="https://www.npmjs.com/package/ngx-logger"/> Log Levels
    /// </summary>
    public enum OltNgxLoggerLevel
    {
        Trace = 0,
        Debug,
        Information,
        Log,
        Warning,
        Error,
        Fatal,
        Off
    }
}