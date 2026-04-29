using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CarSystem.Data.Models;
using CarSystem.Data.Models.Associative;
using CarSystem.Data.Models.Contracts;

namespace CarSystem.Data
{
	public class CarSystemSQLiteContext : DbContext, ICarSystemDbContext
	{
		public CarSystemSQLiteContext() : base("CarSystemSQLiteDb")
		{
			Database.SetInitializer<CarSystemSQLiteContext>(null);
		}

		public DbSet<Person> People { get; set; }
		public DbSet<Gender> Genders { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<EmissionStandart> EmissionStandarts { get; set; }
		public DbSet<Fine> Fines { get; set; }
		public DbSet<Fuel> Fuels { get; set; }
		public DbSet<PersonCars> PersonCars { get; set; }
		public DbSet<PersonFines> PersonFines { get; set; }
		public DbSet<Violation> Violations { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public override Task<int> SaveChangesAsync()
		{
			ApplyDeletionRules();
			return base.SaveChangesAsync();
		}

		private void ApplyDeletionRules()
		{
			var entitiesForDeletion = ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Deleted && e.Entity is IDeletable);

			foreach (var entry in entitiesForDeletion)
			{
				var entity = (IDeletable)entry.Entity;
				entity.IsDeleted = true;
				entry.State = EntityState.Modified;
			}
		}
	}
}
