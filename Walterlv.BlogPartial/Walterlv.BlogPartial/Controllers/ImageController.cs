using System;
using System.Linq;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Walterlv.BlogPartial.Data;
using static Walterlv.BlogPartial.Data.Site;

namespace Walterlv.BlogPartial.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemoryCache _cache;
        private readonly VisitingInfoContext _context;

        public ImageController(IHttpContextAccessor accessor, IMemoryCache memoryCache, VisitingInfoContext context)
        {
            _accessor = accessor;
            _cache = memoryCache;
            _context = context;
        }

        [HttpGet, Route("bulletin/csdn.png")]
        public FileResult GetImageToCsdn()
        {
            RecordVisitingInfo(Csdn, "/bulletin/csdn.png");
            return GetImage("CSDN", "csdn-column");
        }

        [HttpGet, Route("banner/csdn.png")]
        public FileResult GetBannerToCsdn()
        {
            RecordVisitingInfo(Csdn, "/banner/csdn.png");
            return GetImage("CSDN", "bulletin");
        }

        [HttpGet, Route("bulletin/blog.png")]
        public FileResult GetImageToBlog()
        {
            RecordVisitingInfo(Blog, "/bulletin/blog.png");
            return GetImage("blog.walterlv.com", "bulletin");
        }

        [HttpGet, Route("banner/blog.png")]
        public FileResult GetBannerToBlog()
        {
            RecordVisitingInfo(Blog, "/banner/blog.png");
            return GetImage("blog.walterlv.com", "banner");
        }

        private FileResult GetImage(string from, string name)
        {
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

        private void RecordVisitingInfo(string site, string url)
        {
            // 获取用户的真实 IP（此字段记录了出发点 IP 和代理服务器经过的 IP）。
            _accessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var ip);

            // 获取用户浏览器信息。
            _accessor.HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent);

            _context.VisitingInfoSet.Add(new VisitingInfo
            {
                Time = DateTimeOffset.Now,
                Ip = ip,
                UserAgent = userAgent,
                Site = site,
                Url = url,
            });
            _context.SaveChangesAsync();

            // 输出摘要。
            var csdnCount = _context.VisitingInfoSet.Where(x => x.Site == Csdn).Count();
            var blogCount = _context.VisitingInfoSet.Where(x => x.Site == Blog).Count();
            Console.WriteLine($@"[{DateTime.Now}] [{site}{url}] {ip}                ");
            Console.WriteLine($@"CSDN = {csdnCount} | Blog = {blogCount}");
            Console.CursorLeft = 0;
        }
    }
}
