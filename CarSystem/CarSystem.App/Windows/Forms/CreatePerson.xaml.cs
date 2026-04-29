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
using CarSystem.Services.Contracts;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CarSystem.App.Windows.Forms
{
	public partial class CreatePerson : MetroWindow
	{
		Autofac.IContainer container = ContainerConfiguration.GetContainer();

		private readonly IGendersService _gendersService;
		private readonly ICarService _carService;
		private readonly IPersonCarsService _personCarsService;

		public BulkObservableCollection<GenderViewModel> GenderViewModels { get; set; }
		public BulkObservableCollection<CarViewModel> CarViewModels { get; set; }

		public CreatePerson()
		{
			WindowTransitionsEnabled = false;
			_gendersService = container.Resolve<IGendersService>();
			_carService = container.Resolve<ICarService>();
			_personCarsService = container.Resolve<IPersonCarsService>();

			GenderViewModels = new BulkObservableCollection<GenderViewModel>();
			CarViewModels = new BulkObservableCollection<CarViewModel>();
			InitializeComponent();
			GenderPickerButton.ItemsSource = GenderViewModels;
			CarPickerButton.ItemsSource = CarViewModels;
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			LoadGenderViewModels();
			LoadCarViewModels();
		}

		private async void LoadGenderViewModels()
		{
			var dbRecords = await _gendersService.GetAllGendersAsync();
			var observableDtoModels = ModelHandler.GendersToObservableDto(dbRecords);
			ModelHandler.ProcessObservableDtoModels(GenderViewModels, observableDtoModels);
		}

		private async void LoadCarViewModels()
		{
			var dbRecords = await _carService.GetAllCarsAsync();
			var observableDtoModels = ModelHandler.CarToObservableDto(dbRecords);
			ModelHandler.ProcessObservableDtoModels(CarViewModels, observableDtoModels);
		}

		private void GenderPickerButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ResolveClearButtonStatus();
			ResolveSaveButtonStatus();
		}

		private void CarPickerButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ResolveClearButtonStatus();
			ResolveSaveButtonStatus();
		}

		private async void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			this.Cursor = Cursors.Hand;

			var gender = GenderPickerButton.SelectedItem as GenderViewModel;
			var person = new Person()
			{
				FirstName = FirstNameTextBox.Text,
				LastName = LastNameTextBox.Text,
				EGN = EGNTextBox.Text,
				CardId = CardIdTextBox.Text,
				GenderId = gender.Id
			};
			var car = CarPickerButton.SelectedItem as CarViewModel;

			await _personCarsService.CreatePersonCarAsync(person, car.Id);

			var result = await this.ShowMessageAsync("Записът бе добавен", "Потребителят бе успешно добавен в системата.", MessageDialogStyle.AffirmativeAndNegative, MetroDialogOptions = new MetroDialogSettings()
			{
				AffirmativeButtonText = "Върни се обратно",
				NegativeButtonText = "Добави още",
				AnimateHide = true,
				AnimateShow = true,
				DefaultButtonFocus = MessageDialogResult.Affirmative,
				DialogResultOnCancel = MessageDialogResult.Canceled,
			});

			if (result == MessageDialogResult.Affirmative)
			{
				this.Close();
			}
			else
			{
				ClearFields();
			}

			this.Cursor = Cursors.Arrow;
		}

		private async void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.Cursor = Cursors.Hand;
			var result = await this.ShowMessageAsync("Сигурни ли сте?", "Ако излезете сега, записът няма да бъде добавен в системата!", MessageDialogStyle.AffirmativeAndNegative, MetroDialogOptions = new MetroDialogSettings()
			{
				AffirmativeButtonText = "Да",
				AnimateHide = true,
				AnimateShow = true,
				NegativeButtonText = "Не",
				DefaultButtonFocus = MessageDialogResult.Affirmative,
				DialogResultOnCancel = MessageDialogResult.Canceled,
			});

			if (result == MessageDialogResult.Affirmative)
			{
				this.Close();
			}

			this.Cursor = Cursors.Arrow;
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			ClearFields();
		}

		private void ClearFields()
		{
			FirstNameTextBox.Text = "";
			LastNameTextBox.Text = "";
			EGNTextBox.Text = "";
			CardIdTextBox.Text = "";

			ClearButton.IsEnabled = false;
			SaveButton.IsEnabled = false;
			GenderPickerButton.SelectedItem = null;
			CarPickerButton.SelectedItem = null;
		}

		private void TextBoxChange(object sender, TextChangedEventArgs e)
		{
			ResolveClearButtonStatus();
			ResolveSaveButtonStatus();
		}

		private void ResolveClearButtonStatus()
		{
			bool buttonEnabled = false;
			var gender = GenderPickerButton.SelectedItem as GenderViewModel;
			var car = CarPickerButton.SelectedItem as CarViewModel;

			if (!string.IsNullOrEmpty(FirstNameTextBox.Text) || !string.IsNullOrEmpty(LastNameTextBox.Text) || !string.IsNullOrEmpty(EGNTextBox.Text) || !string.IsNullOrEmpty(CardIdTextBox.Text))
			{
				buttonEnabled = true;
			}

			if (gender != null || car != null)
			{
				buttonEnabled = true;
			}

			ClearButton.IsEnabled = buttonEnabled ? true : false;
		}

		private void ResolveSaveButtonStatus()
		{
			bool buttonEnabled = true;
			var gender = GenderPickerButton.SelectedItem as GenderViewModel;
			var car = CarPickerButton.SelectedItem as CarViewModel;

			if (string.IsNullOrEmpty(FirstNameTextBox.Text) || string.IsNullOrEmpty(LastNameTextBox.Text) || string.IsNullOrEmpty(EGNTextBox.Text) || string.IsNullOrEmpty(CardIdTextBox.Text))
			{
				buttonEnabled = false;
			}
			
			if (gender == null || car == null)
			{
				buttonEnabled = false;
			}

			SaveButton.IsEnabled = buttonEnabled ? true : false;
		}
	}
}
