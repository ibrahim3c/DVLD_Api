namespace DVLD.Core.Helpers
{
    public static class AppStatuses
    {
        public const string Pending = "Pending";    // Submitted but not yet processed
        public const string Approved = "Approved";  // Reviewed and accepted
        public const string Rejected = "Rejected";  // Not approved due to missing documents

        // ✅ Validate if a given status is valid
        public static bool IsValidStatus(string status)
        {
            return status == Pending || status == Approved || status == Rejected;
        }
    }
}
