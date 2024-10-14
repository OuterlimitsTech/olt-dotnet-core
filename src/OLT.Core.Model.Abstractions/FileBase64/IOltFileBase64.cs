using System;

namespace OLT.Core
{
    /// <summary>
    /// Represents a file encoded as a base64 string
    /// </summary>
    public interface IOltFileBase64
    {
        string? ContentType { get; set; }
        string? FileBase64 { get; set; }
        string? FileName { get; set; }
        bool Success { get; }
    }
}