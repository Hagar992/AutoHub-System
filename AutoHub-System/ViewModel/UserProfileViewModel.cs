using System;
using System.Collections.Generic;

namespace AutoHub_System.Models
{
    /// <summary>
    /// ViewModel for User Profile Page
    /// Contains user information, statistics, and list of orders
    /// </summary>
    public class UserProfileViewModel
    {
        // User Basic Information
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime DateRegistered { get; set; }

        // User Statistics
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ConfirmedOrders { get; set; }
        public int CanceledOrders { get; set; }
        public float TotalSpent { get; set; }

        // List of all user orders
        public List<UserOrderViewModel> Orders { get; set; } = new List<UserOrderViewModel>();
    }

    /// <summary>
    /// ViewModel for individual orders displayed in user profile
    /// Contains order details, car information, and payment details
    /// </summary>
    public class UserOrderViewModel
    {
        // Order Identification
        public int OrderID { get; set; }

        // Car Information
        public int CarID { get; set; }
        public string CarName { get; set; }
        public string CarImage { get; set; }
        public float CarPrice { get; set; }

        // Payment Information
        public float Deposit { get; set; }
        public float TotalPaid { get; set; }
        public double DepositRate { get; set; }

        // Order Status and Dates
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? BuyingDate { get; set; }
    }
}