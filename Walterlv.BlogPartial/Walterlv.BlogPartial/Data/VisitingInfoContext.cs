using Microsoft.EntityFrameworkCore;

namespace Walterlv.BlogPartial.Data
{
    public class VisitingInfoContext : DbContext
    {
        public DbSet<VisitingInfo> VisitingInfoSet { get; set; }

        public VisitingInfoContext(DbContextOptions<VisitingInfoContext> options)
            : base(options)
        {
        }
    }
}
