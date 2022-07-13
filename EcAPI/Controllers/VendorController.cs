using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcAPI.Interfaces;
using EcAPI.Entity;

namespace EcAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepository _vendorRepos;

        private readonly ILogger<VendorController> _logger;

        public VendorController(IVendorRepository vendorRepos, ILogger<VendorController> logger)
        {
            _vendorRepos = vendorRepos;
            _logger = logger;
        }
        [HttpPost]
        [Route("AddOrUpdateVendor")]
        public dynamic AddOrUpdateVendor(Vendor vendorDetails)
        {
            return _vendorRepos.AddOrUpdateVendor(vendorDetails);
        }
       
    }
}