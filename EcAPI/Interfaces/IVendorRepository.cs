using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcAPI.Entity;

namespace EcAPI.Interfaces
{
    public interface IVendorRepository
    {
        public dynamic AddOrUpdateVendor(Vendor vendorDetails);
    }
}