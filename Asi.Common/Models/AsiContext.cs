using Microsoft.EntityFrameworkCore;

namespace Asi.Common.Models
{
    public class AsiContext : DbContext
    {
        public AsiContext(DbContextOptions<AsiContext> options) : base(options)
		{
			ChangeTracker.LazyLoadingEnabled = false;
			//ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			optionsBuilder.UseInMemoryDatabase(databaseName: "AsiDb")
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Contact>().HasData(
				new Contact { Id = 1, BirthDate = new DateTime(2000, 4, 6), Name = "やまだ たろう" },
				new Contact { Id = 2, BirthDate = new DateTime(1964, 2, 12), Name = "John Doe" },
				new Contact { Id = 3, BirthDate = new DateTime(1988, 12, 4), Name = "José Perez" },
				new Contact { Id = 4, BirthDate = new DateTime(1976, 11, 8), Name = "Иван Иванович" },
				new Contact { Id = 5, BirthDate = new DateTime(1997, 3, 22), Name = "陳大文" });

			modelBuilder.Entity<Email>(entity =>
			{
				entity.HasOne(d => d.Contact)
					.WithMany(p => p.Emails)
					.HasForeignKey(d => d.ContactId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Email>().HasData(
				new Email { Id = 1, IsPrimary = 1, Address = "yamada-tanako@yahoo.co.jp", ContactId = 1 },
				new Email { Id = 2, IsPrimary = 0, Address = "81351448866@docomo.ne.jp", ContactId = 1 },
				new Email { Id = 3, IsPrimary = 0, Address = "tanako@outlook.com", ContactId = 1 },
				new Email { Id = 4, IsPrimary = 1, Address = "johndoe@aol.com", ContactId = 2 },
				new Email { Id = 5, IsPrimary = 1, Address = "joseperez@latinmail.com", ContactId = 3 },
				new Email { Id = 6, IsPrimary = 0, Address = "jose999@hotmail.com", ContactId = 3 },
				new Email { Id = 7, IsPrimary = 1, Address = "chen@qq.com", ContactId = 5 });
		}

		public DbSet<Contact> Contacts { get; set; }
		public DbSet<Email> Emails { get; set; }
    }
}