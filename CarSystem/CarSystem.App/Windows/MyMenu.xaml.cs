using Autofac;
using CarSystem.App.Infrastructure;
using MahApps.Metro.Controls;

namespace CarSystem.App.Windows
{
	public partial class MyMenu : MetroWindow
	{
		IContainer container = ContainerConfiguration.GetContainer();

		public MyMenu()
		{
			WindowTransitionsEnabled = false;
			InitializeComponent();
		}

		private void ViolationsTile_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var violationsWindow = container.Resolve<Violations>();
			violationsWindow.Show();
			this.Close();
		}

		private void PreviousButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var startupWindow = container.Resolve<MainWindow>();
			startupWindow.Show();
			this.Close();
		}

		private void RegistrationsTile_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var registrationsWindow = container.Resolve<Registrations>();
			registrationsWindow.Show();
			this.Close();
		}

		private void ReferencesTile_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var referencesWindow = container.Resolve<References>();
			referencesWindow.Show();
			this.Close();
		}
	}
}
