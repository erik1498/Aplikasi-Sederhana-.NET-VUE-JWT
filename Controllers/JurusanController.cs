using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASPVUE.Constants;
using ASPVUE.Data;
using ASPVUE.Models;
using ASPVUE.Process.RoleProcess;
using ASPVUE.Rules.Input;
using Microsoft.AspNetCore.Authorization;
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
             if (TokenConst.TokenValue == string.Empty)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["TokenValue"] = TokenConst.TokenValue;
            return View();
        }

        private User AuthorizeRequest(){
            User user = new User();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                user.UserID = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.PrimaryGroupSid).Value);
                user.Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value.ToString();
                user.Role = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value);
                return user;
            }
            return user;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDaftarJurusan()
        {
            if (this.AuthorizeRequest().Role == 1)
            {
                return Ok(await _adminProcess.GetAllJurusan());
            }
            return BadRequest("Akun Anda Tidak Diizinkan.");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Jurusan jurusan)
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetJurusan(int id)
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Edit(Jurusan jurusan)
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetListKelas(int id)
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetKelasTanpaJurusan()
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> SetJurusan(SetJurusan setJurusan)
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> HapusKelas(int id)
        {
            if (this.AuthorizeRequest().Role == 1)
            {
                await _adminProcess.HapusKelasInJurusan(id);
                return NoContent();
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}