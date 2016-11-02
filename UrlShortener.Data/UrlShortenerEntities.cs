using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
namespace UrlShortener.Data
{
    public class UrlShortenerEntities : DbContext
    {
        public UrlShortenerEntities()
            : base("name=ShortenerDb")
        {
        }

        public virtual DbSet<Url> Urls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }


}