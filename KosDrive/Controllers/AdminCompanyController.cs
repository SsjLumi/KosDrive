//using KosDrive.Data;
//using KosDrive.DTOs;
//using KosDrive.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace KosDrive.Controllers
//{
//    [ApiController]
//    [Route("Api/taxicompanies/{companyId}/drivers")]
//    public class AdminCompanyController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public AdminCompanyController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddDriver(int companyId, [FromBody]AddDriverDto dto)
//        {
//            var company = await _context.Companies.FindAsync(companyId);
//            if(company == null) 
//                return NotFound("Taxi Company not found");

//            var driver = new User
//            {
//                FirstName = dto.FirstName,
//                LastName = dto.LastName,
//                PhoneNumber = dto.Phone,
//                DriversLicensePicture = dto.LicenseNumber,
//                Rating = 5.0,
//                IsAvailable = true,
//            };

//            _context.Users.Add(driver);
//            await _context.SaveChangesAsync();

//            return Ok(driver);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetCompanyDrivers(Guid companyId)
//        {
//            var drivers = await _context.Users
//                .Where(d => d.CompanyId == companyId).ToListAsync();

//            return Ok(drivers);
//        }
//    }
//}
