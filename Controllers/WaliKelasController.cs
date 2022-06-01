using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Process.RoleProcess;
using ASPVUE.Rules.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASPVUE.Controllers
{
    public class WaliKelasController : Controller
    {
        private readonly ILogger<WaliKelasController> _logger;
        public AdminRoleProcess _adminProcess { get; set; }

        public WaliKelasController(ILogger<WaliKelasController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _adminProcess = new AdminRoleProcess(context);
        }

        private bool AuthorizeRequest(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                string UserID = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.PrimaryGroupSid).Value;
                string Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value;
                int Role = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value);
                if (HttpContext.Session.GetString("UserID") != UserID && HttpContext.Session.GetString("Username") != Username && int.Parse(HttpContext.Session.GetString("Role")) != Role)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public IActionResult Index()
        {
            string tokenValue = HttpContext.Session.GetString("TokenValue");
            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["TokenValue"] = tokenValue;
            ViewData["Role"] = HttpContext.Session.GetString("Role");
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDaftarWaliKelas()
        {
            var auth = this.AuthorizeRequest();
            if (auth){
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.GetAllWaliKelas();
                    if (exist != null)
                    {
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(exist));
                    }
                    return NotFound();
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWaliKelas(int id)
        {
            var auth = this.AuthorizeRequest();
            if (auth){
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.GetIdWaliKelas(id);
                    if (exist != null)
                    {
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(exist));
                    }
                    return NotFound();
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetKelasTanpaWaliKelas()
        {
            var auth = this.AuthorizeRequest();
            if (auth){
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.GetKelasTanpaWaliKelas()));
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(SetWaliKelas setWaliKelas)
        {
            var auth = this.AuthorizeRequest();
            if (auth){
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    if (ModelState.IsValid)
                    {
                        var walikelas = await _adminProcess.CreateWaliKelas(setWaliKelas);
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(walikelas));
                    }
                    return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Edit(SetWaliKelas setWaliKelas)
        {
            var auth = this.AuthorizeRequest();
            if (auth){
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.EditWaliKelas(setWaliKelas)));
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var auth = this.AuthorizeRequest();
            if (auth){
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.DeleteWaliKelas(id);
                    if (exist)
                    {
                        return NoContent();
                    }
                    return BadRequest();
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}