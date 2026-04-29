using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autofac;
using CarSystem.App.Infrastructure;
using CarSystem.App.Windows.Forms;
using MahApps.Metro.Controls;

namespace CarSystem.App.Windows
{
	public partial class Registrations : MetroWindow
	{
		IContainer container = ContainerConfiguration.GetContainer();

		public Registrations()
		{
			WindowTransitionsEnabled = false;
			InitializeComponent();
		}

		private void PreviousButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var myMenu = container.Resolve<MyMenu>();
			myMenu.Show();
			this.Close();
		}

		private void HomeScreenButton_Click(object sender, RoutedEventArgs e)
		{
			var startupWindow = container.Resolve<MainWindow>();
			startupWindow.Show();
			this.Close();
		}

		private void CameraRadarTile_Click(object sender, RoutedEventArgs e)
		{
			var startupWindow = container.Resolve<Violations>();
			startupWindow.Show();
			this.Close();
		}

		private void AddPersonTile_Click(object sender, RoutedEventArgs e)
		{
			var createPersonWindow = container.Resolve<CreatePerson>();

			createPersonWindow.ShowDialog();
		}

		private void AddCarTile_Click(object sender, RoutedEventArgs e)
		{
			var createCarWindow = container.Resolve<CreateCar>();

			createCarWindow.ShowDialog();
		}
	}
}
