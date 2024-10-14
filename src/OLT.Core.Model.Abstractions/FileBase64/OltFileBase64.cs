namespace OLT.Core
{
    /// <summary>
    /// Represents a file encoded as a base64 string
    /// </summary>
    public class OltFileBase64 : IOltFileBase64
    {
        public virtual string? FileBase64 { get; set; }
        public virtual string? FileName { get; set; }
        public virtual string? ContentType { get; set; }
        public virtual bool Success => !string.IsNullOrWhiteSpace(FileBase64);
    }
}