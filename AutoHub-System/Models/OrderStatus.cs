
namespace AutoHub_System.Models
{
    public static class OrderStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Canceled = "Canceled";

        public static readonly string[] AllStatuses = { Pending, Confirmed, Canceled };

        public static bool IsValidStatus(string status)
        {
            return AllStatuses.Contains(status);
        }
    }
}