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
    public class WaliKelasController : Controller
    {
        private readonly ILogger<WaliKelasController> _logger;
        public AdminRoleProcess _adminProcess { get; set; }

        public WaliKelasController(ILogger<WaliKelasController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _adminProcess = new AdminRoleProcess(context);
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

        public IActionResult Index()
        {
            if (TokenConst.TokenValue == string.Empty)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["TokenValue"] = TokenConst.TokenValue;
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDaftarWaliKelas()
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetWaliKelas(int id)
        {
            if (this.AuthorizeRequest().Role == 1)
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
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetKelasTanpaWaliKelas()
        {
            if (this.AuthorizeRequest().Role == 1)
            {
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.GetKelasTanpaWaliKelas()));
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(SetWaliKelas setWaliKelas)
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Edit(SetWaliKelas setWaliKelas)
        {
            if (this.AuthorizeRequest().Role == 1)
            {
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.EditWaliKelas(setWaliKelas)));
            }
            return BadRequest("Akun Anda Tidak Diizinkan");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.AuthorizeRequest().Role == 1)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}