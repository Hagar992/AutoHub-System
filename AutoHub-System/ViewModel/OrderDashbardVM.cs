public class OrderDashbardVM
{
    public int OrderID { get; set; }
    public string Client { get; set; }
    public string UserEmail { get; set; }
    public float CarPrice { get; set; }
    public string Status { get; set; }
    public DateTime OrderDate { get; set; }
    public float Deposit { get; set; }
    public float TotalPrice { get; set; }
    public string Car { get; set; }
    public double PolicyRate { get; set; }

    // Computed properties
    public bool CanConfirm => Status == OrderStatus.Pending;
    public bool CanCancel => Status == OrderStatus.Pending || Status == OrderStatus.Confirmed;
    public string StatusBadgeClass => GetStatusBadgeClass(Status);

    private static string GetStatusBadgeClass(string status)
    {
        return status switch
        {
            OrderStatus.Pending => "status-pending",
            OrderStatus.Confirmed => "status-confirmed",
            OrderStatus.Canceled => "status-canceled",
            _ => "status-unknown"
        };
    }
}