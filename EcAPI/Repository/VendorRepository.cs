using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entity;
using EcAPI.Interfaces;
using EcAPI.Model;

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
            ResponseModel response = new ResponseModel();
            try
            {
                if (vendorDetails.Id == 0)
                {
                    _context.Vendors.Add(vendorDetails);
                    _context.SaveChanges();
                    response.StatusCode = 200;
                    response.Message = "The Vendor added successfully";
                }
                else
                {
                    var entity = _context.Vendors.FirstOrDefault(x => x.Id == vendorDetails.Id);
                    entity.Name = vendorDetails.Name;
                    entity.Email = vendorDetails.Email;
                    entity.Mobile = vendorDetails.Mobile;
                    entity.AccountNumber = vendorDetails.AccountNumber;
                    entity.IFSC = vendorDetails.IFSC;
                    entity.Branch = vendorDetails.Branch;
                    var entityAddress = _context.VendorAddress.FirstOrDefault(x => x.VendorId == entity.Id);
                    entityAddress.FirstName = vendorDetails.VendorAddress.FirstName;
                    entityAddress.LastName = vendorDetails.VendorAddress.LastName;
                    entityAddress.City = vendorDetails.VendorAddress.City;
                    entityAddress.State = vendorDetails.VendorAddress.State;
                    entityAddress.Street = vendorDetails.VendorAddress.Street;
                    entityAddress.ZipCode = vendorDetails.VendorAddress.ZipCode;
                    entityAddress.LastName = vendorDetails.VendorAddress.LastName;
                    _context.SaveChanges();
                    response.StatusCode = 200;
                    response.Message = "The Vendor updated successfully";
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 400;
                response.Message = ex.ToString();
            }
            return response;
        }

        public dynamic GetAllVendors()
        {
            var venderAddress = _context.VendorAddress.ToList();
           var result= from v in _context.Vendors
            join va in _context.VendorAddress on v.Id equals va.VendorId select v;
            return result.ToList();
        }
    }
}