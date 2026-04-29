using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSystem.Data;
using CarSystem.Data.Models;
using CarSystem.Services.Contracts;

namespace CarSystem.Services
{
	public class GendersService : IGendersService
	{
		private readonly ICarSystemDbContext context;

		public GendersService(ICarSystemDbContext context)
		{
			this.context = context;
		}

		public Task<List<Gender>> GetAllGendersAsync()
		{
			return this.context.Genders
				.ToListAsync();
		}
	}
}
