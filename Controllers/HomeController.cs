using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASPVUE.Constants;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Rules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ASPVUE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ApplicationDbContext _context;
        private IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (TokenConst.TokenValue != string.Empty)
            {
                return RedirectToAction("Index", "Siswa");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user){
            if (ModelState.IsValid)
            {
                var exist = await _context.Users.Where(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password)).FirstOrDefaultAsync();
                if (exist != null)
                {
                    return Ok();
                }else{
                    return NotFound();
                }
            }
            return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
        }

        [HttpPost]
        public async Task<IActionResult> AsyncLogin(User user){
            if (ModelState.IsValid)
            {
                var exist = await _context.Users.Where(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password)).FirstOrDefaultAsync();
                if (exist != null)
                {
                    var token = this.GenerateToken(exist);
                    TokenConst.TokenValue = token;
                    UserAuth.UserID = exist.UserID;
                    UserAuth.Username = exist.Username;
                    UserAuth.Role = exist.Role;
                    if (exist.Role == 1)
                    {
                        return RedirectToAction("Index", "Siswa");
                    }
                    else if (exist.Role == 2)
                    {
                        return RedirectToAction("Index", "Siswa");
                    }
                }else{
                    return NotFound();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            TokenConst.TokenValue = string.Empty;
            return RedirectToAction("Index");
        }

        private string GenerateToken(User user){            
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(
                    new []{
                        new Claim(ClaimTypes.PrimaryGroupSid, user.UserID.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Username),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(JwtRegisteredClaimNames.Aud, _configuration["JWT:Audience"]),
                        new Claim(JwtRegisteredClaimNames.Iss, _configuration["JWT:Issuer"]),
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}