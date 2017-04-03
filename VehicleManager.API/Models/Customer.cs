using System;
using System.Collections.Generic;

namespace VehicleManager.API.Models
{
    public class Customer
    {
        // Scalar
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Navigation
        public virtual ICollection<Sale> Sales { get; set; }
    }
}