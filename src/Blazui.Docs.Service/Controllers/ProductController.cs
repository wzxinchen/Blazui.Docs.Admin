using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Docs.Admin.Model;
using Blazui.Docs.Admin.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blazui.Docs.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductService productService;

        public ProductController(ILogger<ProductController> logger, ProductService productService)
        {
            _logger = logger;
            this.productService = productService;
        }

        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            return productService.GetProducts();
        }

        [HttpGet]
        [Route("{productId}/versions")]
        public IEnumerable<VersionModel> GetVersions(int productId)
        {
            return productService.GetProductVersions(productId).Where(x => x.IsPublish).Select(x => new VersionModel()
            {
                Id = x.Id,
                Version = x.Version
            });
        }

        [HttpGet]
        [Route("{productId}/versions/{versionId}/quickstarts")]
        public IActionResult GetVersions(int productId, int versionId)
        {
            return Ok(productService.GetProductQuickStartSteps(versionId).Select(x => new QuickStartStepModel()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }));
        }

        [HttpGet]
        [Route("{productId}/versions/{versionId}/components")]
        public IActionResult GetComponents(int productId, int versionId)
        {
            return Ok(productService.GetComponents(versionId).Select(x => new ComponentModel()
            {
                Name = x.Name,
                TagName = x.TagName
            }));
        }
    }
}
