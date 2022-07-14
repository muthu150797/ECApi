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
    }
}