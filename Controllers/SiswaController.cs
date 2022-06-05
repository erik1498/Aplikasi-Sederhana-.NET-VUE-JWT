using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASPVUE.Models;
using ASPVUE.Data;
using ASPVUE.Rules.Input;
using Microsoft.AspNetCore.Authorization;
using ASPVUE.Process.RoleProcess;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

namespace ASPVUE.Controllers
{
    [RequestFormLimits(ValueCountLimit = 5000)]
    public class SiswaController : Controller
    {
        private readonly ILogger<SiswaController> _logger;
        public AdminRoleProcess _adminProcess { get; set; }
        public WaliKelasRoleProcess _waliKelasProcess { get; set; }
        private static IWebHostEnvironment _webHostEnvironment { get; set; }
        
        public SiswaController(ILogger<SiswaController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _adminProcess = new AdminRoleProcess(context);
            _waliKelasProcess = new WaliKelasRoleProcess(context);
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload()
        {
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1 && Request.Form.Files.Count() > 0)
                {
                    int SiswaId = int.Parse(Request.Form["SiswaID"][0]);
                    var formFile = Request.Form.Files[0];
                    if (formFile.Length > 0)
                    {
                        if (!Directory.Exists(_webHostEnvironment.WebRootPath + "\\ImagesSiswa\\"))
                        {
                            Directory.CreateDirectory(_webHostEnvironment.WebRootPath + "\\ImageSiswa\\");
                        }
                        
                        using (FileStream fileStream = System.IO.File.Create(_webHostEnvironment.WebRootPath + "\\ImageSiswa\\" + formFile.FileName))
                        {
                            await formFile.CopyToAsync(fileStream);
                            await fileStream.FlushAsync();
                        }
                    }
                    return Ok(await _adminProcess.UploadImgSiswa(formFile.FileName, SiswaId));

                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ImageSource(GetSource getSource)
        {
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\ImageSiswa\\";
                    var filePath = path +""+ getSource.sourcename;
                    if (System.IO.File.Exists(filePath))
                    {
                        byte[] b = System.IO.File.ReadAllBytes(filePath);
                        return File(b, "image/jpg");
                    }
                    return NotFound();
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDaftarSiswa(){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.GetAllSiswa()));   
                }
                else if(int.Parse(HttpContext.Session.GetString("Role")) == 2){
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _waliKelasProcess.GetAllSiswa(int.Parse(HttpContext.Session.GetString("UserID")))));   
                }
                return BadRequest("Akun Anda Tidak Diizinkan.");
            }
            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Siswa siswa){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    if (ModelState.IsValid){
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(await _adminProcess.AddSiswa(siswa)));
                    }
                    return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));   
                }
                return BadRequest("Akun Anda Tidak Diizinkan.");
            }
            return Unauthorized();

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Search(Siswa siswa){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1 || int.Parse(HttpContext.Session.GetString("Role")) == 2)
                {
                    if (siswa.NamaSiswa != string.Empty)
                    {
                        var exist = await _adminProcess.SearchSiswa(siswa);
                        if (exist != null)
                        {
                            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(exist));
                        }
                        else{
                            return NotFound();
                        }
                    }else{
                        return BadRequest();
                    }
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Edit(Siswa siswa){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (ModelState.IsValid)
                {
                    var exist = await _adminProcess.EditSiswa(siswa);
                    if (exist != null)
                    {
                        return Ok(exist);
                    }
                }
                return BadRequest(Newtonsoft.Json.JsonConvert.SerializeObject(ModelState));
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSiswa(int id){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.GetIdSiswa(id);
                    if (exist != null)
                    {
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(exist));
                    }else{
                        return NotFound();
                    }
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var exist = await _adminProcess.DeleteSiswa(id);
                    if (exist)
                    {
                        return NoContent();
                    }else{
                        return NotFound();
                    }
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSiswaTanpaKelas(){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    var siswa = await _adminProcess.GetSiswaTanpaKelas();
                    if (siswa != null)
                    {
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(siswa));
                    }else{
                        return NotFound();
                    }
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> SetKelas(SetKelas setKelas){
            var auth = this.AuthorizeRequest();
            if (auth)
            {
                if (int.Parse(HttpContext.Session.GetString("Role")) == 1)
                {
                    await _adminProcess.SetKelasSiswa(setKelas);
                    return NoContent();
                }
                return BadRequest("Akun Anda Tidak Diizinkan");
            }
            return Unauthorized();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string[] strings)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
