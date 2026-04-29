using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSystem.Data;
using CarSystem.Data.Models.Associative;
using CarSystem.Services.Contracts;

namespace CarSystem.Services
{
	public class PersonFinesService : IPersonFinesService
	{
		private readonly ICarSystemDbContext context;

		public PersonFinesService(ICarSystemDbContext context)
		{
			this.context = context;
		}

		public Task<List<PersonFines>> GetFilteredPersonFinesAsync(string violationName = "", string cardId = "", string egn = "", string carNumber = "", string fineNumber = "")
		{
			var personFines = this.context.PersonFines
				.Include(x => x.Person)
				.Include(x => x.Car)
				.Include(x => x.Violation)
				.Where(x => x.Violation.Name.Contains(violationName) && !x.IsDeleted)
				.AsQueryable();

			if (!string.IsNullOrEmpty(cardId))
			{
				personFines = personFines
					.Where(x => x.Person.CardId.Contains(cardId))
					.AsQueryable();
			}
			if (!string.IsNullOrEmpty(egn))
			{
				personFines = personFines
					.Where(x => x.Person.EGN.Contains(egn))
					.AsQueryable();
			}
			if (!string.IsNullOrEmpty(carNumber))
			{
				personFines = personFines
					.Where(x => x.Car.Number.Contains(carNumber))
					.AsQueryable();
			}
			if (!string.IsNullOrEmpty(fineNumber))
			{
				personFines = personFines.
					Where(x => x.FineNumber.Contains(fineNumber))
					.AsQueryable();
			}

			return personFines.ToListAsync();
		}

		public async Task DeletePersonFineAsync(int personFineId)
		{
			var dbRecord = await this.context.PersonFines
				.FirstOrDefaultAsync(x => x.Id == personFineId);
			if (dbRecord != null)
			{
				this.context.PersonFines.Remove(dbRecord);
			}

			await this.context.SaveChangesAsync();
		}

		public async Task CreatePersonFineAsync(int personId, int carId, int violationId, int fineId, decimal finePrice, string fineNumber, DateTime licenceBackOn)
		{
			var personFineRecord = new PersonFines()
			{
				PersonId = personId,
				CarId = carId,
				ViolationId = violationId,
				FineId = fineId,
				FineNumber = fineNumber,
				Price = finePrice,
				LicenceBackOn = licenceBackOn
			};

			this.context.PersonFines.Add(personFineRecord);
			await this.context.SaveChangesAsync();
		}

		public Task<List<PersonFines>> GetPersonFinesByPersonId(int personId)
		{
			return this.context.PersonFines
				.Include(x => x.Person)
				.Include("Person.PersonCars")
				.Include("Person.PersonFines")
				.Include(x => x.Car)
				.Include(x => x.Violation)
				.Include(x => x.Fine)
				.Where(x => x.PersonId == personId)
				.ToListAsync();
		}

		public Task<List<PersonFines>> GetCarFinesByCarId(int carId)
		{
			return this.context.PersonFines
				.Include(x => x.Person)
				.Include("Person.PersonCars")
				.Include("Person.PersonFines")
				.Include(x => x.Car)
				.Include(x => x.Violation)
				.Include(x => x.Fine)
				.Where(x => x.CarId == carId)
				.ToListAsync();
		}
	}
}
