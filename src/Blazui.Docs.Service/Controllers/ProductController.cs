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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

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
    }
}
