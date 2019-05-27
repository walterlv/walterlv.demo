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
            return GetImage("CSDN", "csdn-column");
        }

        [HttpGet, Route("banner/csdn.png")]
        public FileResult GetBannerToCsdn()
        {
            _counter.AddCsdnPv();
            return GetImage("CSDN", "bulletin");
        }

        [HttpGet, Route("bulletin/blog.png")]
        public FileResult GetImageToBlog()
        {
            _counter.AddBlogPv();
            return GetImage("blog.walterlv.com", "bulletin");
        }

        [HttpGet, Route("banner/blog.png")]
        public FileResult GetBannerToBlog()
        {
            _counter.AddBlogPv();
            return GetImage("blog.walterlv.com", "banner");
        }

        private FileResult GetImage(string from, string name)
        {
            // 获取用户的真实 IP（此字段记录了出发点 IP 和代理服务器经过的 IP）。
            HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var ip);

            // 获取用户浏览器信息。
            HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent);

            // 输出摘要。
            Console.WriteLine($@"==== [{DateTime.Now}] ====
IP: {ip}
UserAgent: {userAgent}");
            _counter.PrintSummary();

            // 返回图片。
            var file = _cache.GetOrCreate(name, entry => System.IO.File.ReadAllBytes($"{name}.png"));
            return File(file, "image/png");
        }

        [HttpGet, Route("UrlMove")]
        public IActionResult UrlMove() => BulletinUrlMove();

        [HttpGet, Route("bulletin/UrlMove")]
        public IActionResult BulletinUrlMove()
        {
            return Redirect("https://blog.walterlv.com");
        }

        [HttpGet, Route("banner/UrlMove")]
        public IActionResult BannerUrlMove()
        {
            return Redirect("https://www.zhipin.com/job_detail/26bf2e69fc65103d1HN539S_FFQ~.html");
        }
    }
}