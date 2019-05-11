using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Walterlv.BlogPartial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private IMemoryCache _cache;

        /// <inheritdoc />
        public ImageController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [Route("bulletin/csdn.png")]
        [HttpGet]
        public FileResult GetImageToCsdn()
        {
            return GetImage("CSDN");
        }

        [Route("bulletin/blog.png")]
        [HttpGet]
        public FileResult GetImageToBlog()
        {
            return GetImage("blog.walterlv.com");
        }

        private FileResult GetImage(string from)
        {
            Count++;

            StringBuilder str = new StringBuilder();
            str.Append(DateTime.Now);
            str.Append(" ");
            str.Append("用户访问 ");

            if (TryGetUserIpFromFrp(HttpContext.Request, out var ip))
            {
                str.Append("用户Ip=");
                str.Append(ip);
                str.Append(" ");
            }

            str.Append($"总共有{Count}访问");

            if (HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                str.Append("\r\n");
                str.Append("当前用户浏览器");
                str.Append(userAgent);
            }

            Console.WriteLine(str);

            var file = _cache.GetOrCreate("Image", entry => System.IO.File.ReadAllBytes("bulletin.png"));

            return File(file, "image/png");
        }

        private static bool TryGetUserIpFromFrp(HttpRequest httpContextRequest, out StringValues ip)
        {
            return httpContextRequest.Headers.TryGetValue("X-Forwarded-For", out ip);
        }

        private static int Count { set; get; }


        [Route("UrlMove"), HttpGet]
        public IActionResult UrlMove()
        {
            return Redirect("https://blog.walterlv.com");
        }
    }
}