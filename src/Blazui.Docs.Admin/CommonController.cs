using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class CommonController : ControllerBase
    {

        [Route("product/upload")]
        public async Task<IActionResult> UploadProductPackageAsync(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return Content(JsonConvert.SerializeObject(new
            {
                code = 0,
                id = filePath
            }), "application/json"); ;
        }
    }
}
