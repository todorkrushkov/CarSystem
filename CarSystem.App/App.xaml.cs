using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using AutoMapper;
using CarSystem.App.Infrastructure;
using CarSystem.App.Infrastructure.Helpers;
using CarSystem.ViewModels;
using CarSystem.Data.Models.Associative;

namespace CarSystem.App
{
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			var container = ContainerConfiguration.GetContainer();
			InitializeAutoMapper();
			var startupWindow = container.Resolve<MainWindow>();
			startupWindow.ShowDialog();
		}

		private void InitializeAutoMapper()
		{
			var config = new MapperConfiguration(cfg => {
				cfg.AddProfile<ProviderMappingProfile>();
			});

			config.AssertConfigurationIsValid();
			ModelHandler.Initialize(config.CreateMapper());
		}
	}
}
