using System.Windows;
using System.Windows.Controls;
using Autofac;
using CarSystem.App.Infrastructure;
using CarSystem.App.Windows;
using MahApps.Metro.Controls;

namespace CarSystem.App
{
	public partial class MainWindow : MetroWindow
	{
		IContainer container = ContainerConfiguration.GetContainer();

		public MainWindow()
		{
			WindowTransitionsEnabled = false;
			InitializeComponent();
		}

		private void StartupButton_Click(object sender, RoutedEventArgs e)
		{
			var myMenu = container.Resolve<MyMenu>();
			myMenu.Show();
			this.Close();
		}
	}
}
