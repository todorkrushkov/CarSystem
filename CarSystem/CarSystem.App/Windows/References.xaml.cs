using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CarSystem.App.Infrastructure.Helpers;
using CarSystem.ViewModels;
using CarSystem.Data.Models;
using CarSystem.Data.Models.Associative;
using CarSystem.Services.Contracts;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace CarSystem.App.Windows
{
	public partial class References : MetroWindow
	{
		IContainer container = ContainerConfiguration.GetContainer();

		private readonly IPeopleService _peopleService;
		private readonly ICarService _carService;
		private readonly IPersonFinesService _personFinesService;
		private readonly IExportService _exportService;

		public BulkObservableCollection<PersonViewModel> PersonViewModels { get; set; }
		public BulkObservableCollection<CarViewModel> CarViewModels { get; set; }

		public References()
		{
			WindowTransitionsEnabled = false;
			_peopleService = container.Resolve<IPeopleService>();
			_carService = container.Resolve<ICarService>();
			_personFinesService = container.Resolve<IPersonFinesService>();
			_exportService = container.Resolve<IExportService>();

			PersonViewModels = new BulkObservableCollection<PersonViewModel>();
			CarViewModels = new BulkObservableCollection<CarViewModel>();
			InitializeComponent();
			PersonPickerButton.ItemsSource = PersonViewModels;
			CarPickerButton.ItemsSource = CarViewModels;
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			LoadPersonViewModels();
			LoadCarViewModels();
		}

		private async void LoadPersonViewModels()
		{
			var dbRecords = await _peopleService.GetAllPersonsAsync();
			var observableDtoModels = ModelHandler.PersonToObservableDto(dbRecords);
			ModelHandler.ProcessObservableDtoModels(PersonViewModels, observableDtoModels);
		}

		private async void LoadCarViewModels()
		{
			var dbRecords = await _carService.GetAllCarsAsync();
			var observableDtoModels = ModelHandler.CarToObservableDto(dbRecords);
			ModelHandler.ProcessObservableDtoModels(CarViewModels, observableDtoModels);
		}

		private void PreviousButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			ReturnToPreviousScreen();
		}

		private void ReturnToPreviousScreen()
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

		private void CarsTabItem_Selected(object sender, RoutedEventArgs e)
		{
			CarPickerButton.SelectedItem = null;
			CarPdfDownloadImage.Visibility = Visibility.Hidden;
		}

		private void PeopleTabItem_Selected(object sender, RoutedEventArgs e)
		{
			PersonPickerButton.SelectedItem = null;
			PersonPdfDownloadImage.Visibility = Visibility.Hidden;
		}

		private async void PersonPdfDownloadImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var personFinesService = _personFinesService;
			var exportService = _exportService;
		tryAgain:
			string fileName = SaveFileHelper();

			bool shouldRepeat = await RepeatHelperDialog(fileName);
			if (shouldRepeat)
			{
				goto tryAgain;
			}

			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}

			var person = PersonPickerButton.SelectedItem as PersonViewModel;

			var personFines = await personFinesService.GetPersonFinesByPersonId(person.Id);

			exportService.ExportPersonInformation(fileName, person.Name, personFines);

			SuccessfullHelperDialog(PersonPdfDownloadImage, PersonPickerButton);
		}

		private async void CarPdfDownloadImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var personFinesService = _personFinesService;
			var exportService = _exportService;
		tryAgain:
			string fileName = SaveFileHelper();

			bool shouldRepeat = await RepeatHelperDialog(fileName);
			if (shouldRepeat)
			{
				goto tryAgain;
			}

			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}

			var car = CarPickerButton.SelectedItem as CarViewModel;

			var carFines = await personFinesService.GetCarFinesByCarId(car.Id);

			exportService.ExportCarInformation(fileName, car.DisplayName, carFines);
			SuccessfullHelperDialog(CarPdfDownloadImage, CarPickerButton);
		}

		private void PersonPickerButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PersonPdfDownloadImage.Visibility = Visibility.Visible;
		}

		private void CarPickerButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CarPdfDownloadImage.Visibility = Visibility.Visible;
		}

		private void BrowsePeopleButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new GenericListWindow("Лица", PersonViewModels.ToList());
			window.ShowDialog();
		}

		private void BrowseCarsButton_Click(object sender, RoutedEventArgs e)
		{
			var window = new GenericListWindow("Автомобили", CarViewModels.ToList());
			window.ShowDialog();
		}

		private string SaveFileHelper()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog()
			{
				Filter = "Pdf files (*.pdf)|*.pdf",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
			};
			saveFileDialog.ShowDialog();

			if (string.IsNullOrEmpty(saveFileDialog.FileName))
			{
				return string.Empty;
			}

			string fileName =
					($"{saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 4)}-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}-" +
					$"{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}-{DateTime.Now.Millisecond}.pdf");

			return fileName;
		}

		private async Task<bool> RepeatHelperDialog(string fileName)
		{
			this.Cursor = Cursors.Hand;
			bool shouldRepeat = false;
			if (string.IsNullOrEmpty(fileName))
			{
				var result = await this.ShowMessageAsync("Не сте избрали име на файл", "Необходимо е да изберете име. Желаете ли да продължите?", MessageDialogStyle.AffirmativeAndNegative, MetroDialogOptions = new MetroDialogSettings()
				{
					AffirmativeButtonText = "Да, ще добавя име",
					NegativeButtonText = "Не, искам да изляза",
					AnimateHide = true,
					AnimateShow = true,
					DefaultButtonFocus = MessageDialogResult.Affirmative,
					DialogResultOnCancel = MessageDialogResult.Canceled,
				});

				if (result == MessageDialogResult.Affirmative)
				{
					shouldRepeat = true;
				}
				else
				{
					ReturnToPreviousScreen();
				}

				this.Cursor = Cursors.Arrow;
			}
			return shouldRepeat;
		}

		private async void SuccessfullHelperDialog(Image image, SplitButton splitButton)
		{
			this.Cursor = Cursors.Hand;
			var result = await this.ShowMessageAsync("Файлът бе записан успешно", "Желаете ли да експортирате още справки?", MessageDialogStyle.AffirmativeAndNegative, MetroDialogOptions = new MetroDialogSettings()
			{
				AffirmativeButtonText = "Да",
				NegativeButtonText = "Не",
				AnimateHide = true,
				AnimateShow = true,
				DefaultButtonFocus = MessageDialogResult.Affirmative,
				DialogResultOnCancel = MessageDialogResult.Canceled,
			});

			if (result == MessageDialogResult.Affirmative)
			{
				splitButton.SelectedItem = null;
				image.Visibility = Visibility.Hidden;
			}
			else
			{
				ReturnToPreviousScreen();
			}

			this.Cursor = Cursors.Arrow;
		}
	}
}
