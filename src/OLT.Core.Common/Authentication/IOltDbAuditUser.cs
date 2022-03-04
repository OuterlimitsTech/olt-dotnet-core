namespace OLT.Core
{
    public interface IOltDbAuditUser
    {
        /// <summary>
        /// Gets value to put in the DB Create/Update Audit Field
        /// </summary>
        string GetDbUsername();
    }
}