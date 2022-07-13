using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entity;
using EcAPI.Interfaces;

namespace EcAPI.Repository
{
    public class VendorRepository : IVendorRepository
    {
        public readonly AppIdentityDbContext _context;

        public VendorRepository(AppIdentityDbContext context)
        {
            _context = context;
        }

        public dynamic AddOrUpdateVendor(Vendor vendorDetails)
        {
            try
            {
                var res = _context.Vendors.Add(new Vendor
                {
                    Email = "nm",
                    IFSC = "",
                    Mobile = "",
                    AccountNumber = "",
                    Branch = "",
                    Name = "jsks"
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
               return 400;
            }
            return 200;
        }
    }
}