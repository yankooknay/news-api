using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        readonly ILogger<HackerNewsController> _logger;

        public HackerNewsController(ILogger<HackerNewsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get(int? pageSize, int? page, string filter)
        {
            return null;
        }
    }
}
