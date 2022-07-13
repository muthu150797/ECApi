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
                    Name = vendorDetails.Name,
                    Email = vendorDetails.Email,
                    IFSC = vendorDetails.IFSC,
                    Mobile = vendorDetails.Mobile,
                    Active = 1,
                    AccountNumber = vendorDetails.AccountNumber,
                    Branch = vendorDetails.Branch,
                    VendorAddress = new VendorAddress
                    {
                        FirstName = vendorDetails.VendorAddress.FirstName,
                        LastName = vendorDetails.VendorAddress.LastName,
                        City = vendorDetails.VendorAddress.City,
                        Street = vendorDetails.VendorAddress.Street,
                        State = vendorDetails.VendorAddress.State,
                        ZipCode = vendorDetails.VendorAddress.ZipCode

                    }
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