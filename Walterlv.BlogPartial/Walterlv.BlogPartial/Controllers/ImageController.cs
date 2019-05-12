using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Walterlv.BlogPartial.Services;

namespace Walterlv.BlogPartial.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IVisitingCounter _counter;

        public ImageController(IMemoryCache memoryCache, IVisitingCounter counter)
        {
            _cache = memoryCache;
            _counter = counter;
        }

        [HttpGet, Route("bulletin/csdn.png")]
        public FileResult GetImageToCsdn()
        {
            _counter.AddCsdnPv();
            return GetImage("CSDN");
        }

        [HttpGet, Route("bulletin/blog.png")]
        public FileResult GetImageToBlog()
        {
            _counter.AddBlogPv();
            return GetImage("blog.walterlv.com");
        }

        private FileResult GetImage(string from)
        {
            // 获取用户的真实 IP（此字段记录了出发点 IP 和代理服务器经过的 IP）。
            HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var ip);

            // 获取用户浏览器信息。
            HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent);

            // 输出摘要。
            Console.WriteLine($@"==== [{DateTime.Now}] ====
IP={ip}, UserAgent={userAgent}");
            _counter.PrintSummary();

            // 返回图片。
            var file = _cache.GetOrCreate("Image", entry => System.IO.File.ReadAllBytes("bulletin.png"));
            return File(file, "image/png");
        }

        [HttpGet, Route("UrlMove")]
        public IActionResult UrlMove()
        {
            return Redirect("https://blog.walterlv.com");
        }
    }
}