using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CarSystem.App.Windows;
using CarSystem.App.Windows.Forms;
using CarSystem.Data;
using CarSystem.Services;
using CarSystem.Services.Contracts;

namespace CarSystem.App.Infrastructure
{
	public class ContainerConfiguration
	{
		private static IContainer container = null;

		public static IContainer GetContainer()
		{
			if (container == null)
			{
				container = InitializeContainer();
			}
			return container;
		}

		public static IContainer InitializeContainer()
		{
			var builder = new ContainerBuilder();

			// Register windows
			builder.RegisterType<MyMenu>().AsSelf();
			builder.RegisterType<MainWindow>().AsSelf();
			builder.RegisterType<Registrations>().AsSelf();
			builder.RegisterType<Violations>().AsSelf();
			builder.RegisterType<References>().AsSelf();

			// Register form windows
			builder.RegisterType<CreateViolation>().AsSelf();
			builder.RegisterType<CreatePerson>().AsSelf();
			builder.RegisterType<CreateCar>().AsSelf();

			// InstancePerDependency: each singleton service gets its own DbContext at construction time
			builder.RegisterType<CarSystemDbContext>().As<ICarSystemDbContext>().AsSelf();

			// Services share the single DbContext — no re-creation on every window open
			builder.RegisterType<PeopleService>().As<IPeopleService>().SingleInstance();
			builder.RegisterType<PersonFinesService>().As<IPersonFinesService>().SingleInstance();
			builder.RegisterType<CarService>().As<ICarService>().SingleInstance();
			builder.RegisterType<ViolationService>().As<IViolationService>().SingleInstance();
			builder.RegisterType<FineService>().As<IFineService>().SingleInstance();
			builder.RegisterType<GendersService>().As<IGendersService>().SingleInstance();
			builder.RegisterType<PersonCarsService>().As<IPersonCarsService>().SingleInstance();
			builder.RegisterType<EmissionStandartService>().As<IEmissionStandartService>().SingleInstance();
			builder.RegisterType<FuelService>().As<IFuelService>().SingleInstance();
			builder.RegisterType<ExportService>().As<IExportService>().SingleInstance();

			return builder.Build();
		}
	}
}
