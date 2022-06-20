using EcAPI.Entities;
using EcAPI.Model;
using EcAPI.Services;
using EcAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EcAPI.Entity;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInmanager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _context;
        private readonly IMapper _mapper;
        private readonly AppIdentityDbContext _dbContext;


        public AccountController(AppIdentityDbContext dbContext, IMapper mapper, IHttpContextAccessor context, IConfiguration config, SignInManager<AppUser> signInmanager, UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _signInmanager = signInmanager;
            _userManager = userManager;
            _config = config;
            _context = context;
            _mapper = mapper;
        }

		//[Authorize]
		[HttpGet]
        [Route("GetCurrentUser")]
        public async Task<ActionResult<UsersDto>> GetCurrentUser()
		{    
            var email = _context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user =await _userManager.FindByEmailAsync(email);
            var response= new UsersDto
            {
                Email = email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
            return response;

        }
		[HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
        [Route("Register")]
        // GET: api/<AccountController>
        [HttpPost]
        public async Task<ActionResult<dynamic>> Register(UserDTO userDetails)
        {
            ResponseModel response = new ResponseModel();

            var user = new AppUser()
            {
                UserName = userDetails.Email,
                Email = userDetails.Email,
                Names = userDetails.UserName,
                DisplayName = userDetails.UserName,
                //Address = new Address()
                //{
                //	FirstName = userDetails.FirstName,
                //	LastName = userDetails.LastName,
                //	Street = userDetails.LastName,
                //	City = userDetails.City,
                //	State=userDetails.State,
                //	ZipCode = userDetails.ZipCode,
                //}
            };
            var userExist = await _userManager.FindByEmailAsync(userDetails.Email);
            if (userExist == null)
            {
                var result = await _userManager.CreateAsync(user, userDetails.Password);
                if (result.Succeeded)
                {

                    var userDetail = await _userManager.FindByEmailAsync(userDetails.Email);
                    var token = _tokenService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], user);
                    userDetails.Token = token;
                    response.StatusCode = 200;
                    response.Message = "Success";
                    userDetails.Id = userDetail.Id;
                    response.Response = userDetails;
                    //tokense
                    return response.Response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Register failed,try again";
                }
            }
            else
            {
                response.StatusCode = 400;
                response.Message = "User already exist";
            }
            return response;
        }
        //[Authorize]
        //[HttpGet]
        //public async Task<ActionResult<dynamic>> GetCurrentUser()
        //{
        //	var user = await _userManager.FindByEmailFromClaimsPrinciple(_context.HttpContext.User);

        //	return new UserDto
        //	{
        //		Email = user.Email,
        //		Token = _tokenService.CreateToken(user, _config),
        //		DisplayName = user.DisplayName
        //	};
        //}
        [Route("Login")]
        // GET: api/<AccountController>
        [HttpPost]
        public async Task<UserDTO> Login(UserModel userDetails)
        {
            ResponseModel response = new ResponseModel();
            var user = await _userManager.FindByEmailAsync(userDetails.UserName);
            if (user != null)
            {
                var result = await _signInmanager.CheckPasswordSignInAsync(user, userDetails.Password, false);
                if (result.Succeeded)
                {
                    UserDTO userDto = new UserDTO();
                    var token = _tokenService.BuildToken(_config["Jwt:Key"], _config["Jwt:Issuer"], user);
                    userDto.Token = token;
                    userDto.FirstName = user.DisplayName;
                    userDto.Email = user.Email;
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.Response = userDto;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Incorrect UserName,Password";
                };
            }
            else
            {
                response.StatusCode = 400;
                response.Message = "Login failed,try again";
            };
            return response.Response;
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var email = _context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user = await _userManager.FindByEmailAsync(email);
            var item = _userManager.Users.AsNoTracking().AsQueryable();
            try
            {
                var userAddress = await _dbContext.Address.Where(x => x.AppUserId == user.Id).FirstOrDefaultAsync();
                AddressDto addressDto = new AddressDto();
                if (userAddress != null)
				{
                    addressDto.FirstName = userAddress.FirstName;
                    addressDto.LastName = userAddress.LastName;
                    addressDto.State = userAddress.State;
                    addressDto.Street = userAddress.Street;
                    addressDto.City = userAddress.City;
                    addressDto.ZipCode = userAddress.ZipCode;
                }
                return addressDto;
            }
            catch (Exception ex)
            {

            }
            return _mapper.Map<Address, AddressDto>(user.Address);
        }
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var email = _context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            var user = await _userManager.FindByEmailAsync(email);
            user.Address = _mapper.Map<AddressDto, Address>(address);
            //user.Address.AppUserId=user.Id;
            try
            {
                var userAddressExist = _dbContext.Address.FirstOrDefault(item => item.AppUserId == user.Id);
                if (userAddressExist == null)
                {
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
                }
                else
                {
                    userAddressExist.FirstName=address.FirstName;
                    userAddressExist.LastName = address.LastName;
					userAddressExist.Street=address.Street;
					userAddressExist.State=address.State;
					userAddressExist.City=address.City;
					userAddressExist.ZipCode=address.ZipCode;
                    var result=_dbContext.SaveChanges();
					 return Ok(_mapper.Map<Address, AddressDto>(user.Address));
                }
                // var result =  await _userManager.UpdateAsync(user);
                // if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            }
            catch (Exception ex)
            {
            return BadRequest("Problem updating then user");
            }
            return BadRequest("Problem updating then user");
        }
        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
