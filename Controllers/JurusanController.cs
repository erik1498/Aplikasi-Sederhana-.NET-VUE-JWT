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
    public class JurusanController : Controller
    {
        private readonly ILogger<JurusanController> _logger;
        public AdminRoleProcess _adminProcess { get; set; }

        public JurusanController(ILogger<JurusanController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _adminProcess = new AdminRoleProcess(context);
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

        private bool AuthorizeRequest()
        {
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDaftarJurusan()
        {
            var auth = this.AuthorizeRequest();
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    return Ok(await _adminProcess.GetAllJurusan());
                }
                return BadRequest("Akun Anda Tidak Diizinkan.");
            }
            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Jurusan jurusan)
        {
            var auth = this.AuthorizeRequest();
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    if (ModelState.IsValid)
                    {
                        var exist = await _adminProcess.AddJurusan(jurusan);
                        if (exist != null)
                        {
                            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(exist));
                        }
                    }
                    return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetJurusan(int id)
        {
            var auth = this.AuthorizeRequest();
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.GetIdJurusan(id);
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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Edit(Jurusan jurusan)
        {
            var auth = this.AuthorizeRequest();
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    if (ModelState.IsValid)
                    {
                        var exist = await _adminProcess.EditJurusan(jurusan);
                        if (exist != null)
                        {
                            return Ok(exist);
                        }
                        return NotFound();
                    }
                    return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
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
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.DeleteJurusan(id);
                    if (exist)
                    {
                        return NoContent();
                    }
                    return NotFound();
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetListKelas(int id)
        {
            var auth = this.AuthorizeRequest();
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.GetListKelas(id);
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
        public async Task<IActionResult> GetKelasTanpaJurusan()
        {
            var auth = this.AuthorizeRequest();
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.GetKelasTanpaJurusan();
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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> SetJurusan(SetJurusan setJurusan)
        {
            var auth = this.AuthorizeRequest();
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    if (ModelState.IsValid)
                    {
                        await _adminProcess.SetJurusan(setJurusan);
                        return NoContent();
                    }
                    return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> HapusKelas(int id)
        {
            var auth = this.AuthorizeRequest();
            if (auth) {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    await _adminProcess.HapusKelasInJurusan(id);
                    return NoContent();
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