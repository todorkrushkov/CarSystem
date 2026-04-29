using System.Data.Entity;
using System.Threading.Tasks;
using CarSystem.Data.Models;
using CarSystem.Data.Models.Associative;

namespace CarSystem.Data
{
	public interface ICarSystemDbContext
	{
		DbSet<Person> People { get; set; }
		DbSet<Gender> Genders { get; set; }
		DbSet<Car> Cars { get; set; }
		DbSet<EmissionStandart> EmissionStandarts { get; set; }
		DbSet<Fine> Fines { get; set; }
		DbSet<Fuel> Fuels { get; set; }
		DbSet<PersonCars> PersonCars { get; set; }
		DbSet<PersonFines> PersonFines { get; set; }
		DbSet<Violation> Violations { get; set; }
		Task<int> SaveChangesAsync();
	}
}
