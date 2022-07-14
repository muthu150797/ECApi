using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcAPI.Entity
{
    public class Vendor : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Active { get; set; }
        public string AccountNumber { get; set; }
        public string IFSC { get; set; }
        public string Branch { get; set; }
        public string Mobile { get; set; }
        public VendorAddress VendorAddress { get; set; }
    }
    public class VendorAddress:BaseEntity {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int VendorId { get; set; }
    }
}