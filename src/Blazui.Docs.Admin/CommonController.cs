using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Docs.Admin
{
    [Route("api/")]
    [Authorize(Roles = "管理员")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [Route("product/upload")]
        [HttpPost]
        public async Task<IActionResult> UploadProductPackageAsync(IFormFile fileContent)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                await fileContent.CopyToAsync(stream);
            }
            return Content(JsonConvert.SerializeObject(new
            {
                code = 0,
                id = filePath
            }), "application/json");
        }
    }
}
