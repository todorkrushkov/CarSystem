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
using CarSystem.Services.Contracts;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CarSystem.App.Windows.Forms
{
	public partial class CreateViolation : MetroWindow
	{
		Autofac.IContainer container = ContainerConfiguration.GetContainer();

		private readonly IPeopleService _peopleService;
		private readonly ICarService _carService;
		private readonly IViolationService _violationService;
		private readonly IFineService _fineService;
		private readonly IPersonFinesService _personFinesService;

		public BulkObservableCollection<PersonViewModel> PersonViewModels { get; set; }
		public BulkObservableCollection<CarViewModel> CarViewModels { get; set; }
		public BulkObservableCollection<FineViewModel> FineViewModels { get; set; }
		public BulkObservableCollection<ViolationViewModel> ViolationViewModels { get; set; }

		public CreateViolation()
		{
			WindowTransitionsEnabled = false;
			_peopleService = container.Resolve<IPeopleService>();
			_carService = container.Resolve<ICarService>();
			_violationService = container.Resolve<IViolationService>();
			_fineService = container.Resolve<IFineService>();
			_personFinesService = container.Resolve<IPersonFinesService>();

			PersonViewModels = new BulkObservableCollection<PersonViewModel>();
			CarViewModels = new BulkObservableCollection<CarViewModel>();
			FineViewModels = new BulkObservableCollection<FineViewModel>();
			ViolationViewModels = new BulkObservableCollection<ViolationViewModel>();
			InitializeComponent();
			PersonPickerButton.ItemsSource = PersonViewModels;
			CarPickerButton.ItemsSource = CarViewModels;
			ViolationPickerButton.ItemsSource = ViolationViewModels;
			FinePickerButton.ItemsSource = FineViewModels;
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			LoadPersonViewModels();
		}

		private async void LoadPersonViewModels()
		{
			var dbRecords = await _peopleService.GetAllPersonsAsync();
			var observableDtoModels = ModelHandler.PersonToObservableDto(dbRecords);
			ModelHandler.ProcessObservableDtoModels(PersonViewModels, observableDtoModels);
		}

		private async void LoadCarViewModels(int personId)
		{
			var dbRecords = await _carService.GetPersonCarsAsync(personId);
			var observableDtoModels = ModelHandler.CarToObservableDto(dbRecords);
			ModelHandler.ProcessObservableDtoModels(CarViewModels, observableDtoModels);
		}

		private async void LoadViolationsViewModels()
		{
			var dbRecords = await _violationService.GetAllViolationsAsync();
			var observableDtoModels = ModelHandler.ViolationsToObservableDto(dbRecords);
			ModelHandler.ProcessObservableDtoModels(ViolationViewModels, observableDtoModels);
		}

		private async void LoadFineViewModels()
		{
			var dbRecords = await _fineService.GetAllFinesAsync();
			var observableDtoModels = ModelHandler.FinesToObservableDto(dbRecords);
			ModelHandler.ProcessObservableDtoModels(FineViewModels, observableDtoModels);
		}

		private async void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			this.Cursor = Cursors.Hand;

			var person = PersonPickerButton.SelectedItem as PersonViewModel;
			var car = CarPickerButton.SelectedItem as CarViewModel;
			var violation = ViolationPickerButton.SelectedItem as ViolationViewModel;
			var fine = FinePickerButton.SelectedItem as FineViewModel;

			if (person == null || car == null || violation == null || fine == null
				|| FinePriceNumericUpDown.Value == null || FinePriceNumericUpDown.Value <= 0
				|| FineNumberNumericUpDown.Value == null
				|| string.IsNullOrEmpty(LicenceBackOnDatePicker.Text))
			{
				await this.ShowMessageAsync("Непълни данни", "Моля, попълнете всички полета преди да запишете. Сумата трябва да е по-голяма от 0.", MessageDialogStyle.Affirmative);
				this.Cursor = Cursors.Arrow;
				return;
			}

			var finePrice = decimal.Parse(FinePriceNumericUpDown.Value.Value.ToString());
			var fineNumber = FineNumberNumericUpDown.Value.Value.ToString();
			var licenceBackOn = DateTime.Parse(LicenceBackOnDatePicker.Text);

			await _personFinesService.CreatePersonFineAsync(person.Id, car.Id, violation.Id, fine.Id, finePrice, fineNumber, licenceBackOn);

			var result = await this.ShowMessageAsync("Записът бе добавен", "Нарушението бе успешно добавено в системата.", MessageDialogStyle.AffirmativeAndNegative, MetroDialogOptions = new MetroDialogSettings()
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
			PersonPickerButton.SelectedItem = null;
			ClearButton.IsEnabled = false;
			SaveButton.IsEnabled = false;

			CarPickerPanel.Visibility = Visibility.Hidden;
			CarPickerButton.SelectedItem = null;

			ViolationPickerPanel.Visibility = Visibility.Hidden;
			ViolationPickerButton.SelectedItem = null;

			FinePickerPanel.Visibility = Visibility.Hidden;
			FinePickerButton.SelectedItem = null;

			LicenceBackOnDatePickerPanel.Visibility = Visibility.Hidden;
			LicenceBackOnDatePicker.Text = "";

			FinePriceFineNumberPanel.Visibility = Visibility.Hidden;
			FinePriceNumericUpDown.Value = 0;
			FineNumberNumericUpDown.Value = 0;
		}

		private void PersonPickerButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			HideFrom(CarPickerPanel);

			var personViewModel = PersonPickerButton.SelectedItem as PersonViewModel;
			if (personViewModel == null) return;

			ClearButton.IsEnabled = true;
			CarPickerPanel.Visibility = Visibility.Visible;
			CarViewModels.Clear();
			LoadCarViewModels(personViewModel.Id);
		}

		private void CarPickerButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			HideFrom(ViolationPickerPanel);

			var carViewModel = CarPickerButton.SelectedItem as CarViewModel;
			if (carViewModel == null) return;

			ViolationPickerPanel.Visibility = Visibility.Visible;
			ViolationViewModels.Clear();
			LoadViolationsViewModels();
		}

		private void ViolationPickerButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			HideFrom(FinePickerPanel);

			var violationViewModel = ViolationPickerButton.SelectedItem as ViolationViewModel;
			if (violationViewModel == null) return;

			FinePickerPanel.Visibility = Visibility.Visible;
			FineViewModels.Clear();
			LoadFineViewModels();
		}

		private void FinePickerButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			HideFrom(LicenceBackOnDatePickerPanel);

			var fineViewModel = FinePickerButton.SelectedItem as FineViewModel;
			if (fineViewModel == null) return;

			LicenceBackOnDatePickerPanel.Visibility = Visibility.Visible;
		}

		private void LicenceBackOnDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			HideFrom(FinePriceFineNumberPanel);

			if (string.IsNullOrEmpty(LicenceBackOnDatePicker.Text)) return;

			FinePriceFineNumberPanel.Visibility = Visibility.Visible;
			FinePriceNumericUpDown.Value = 0;
			FineNumberNumericUpDown.Value = 0;
			SaveButton.IsEnabled = true;
		}

		private void HideFrom(UIElement startPanel)
		{
			var panels = new UIElement[]
			{
				CarPickerPanel, ViolationPickerPanel, FinePickerPanel,
				LicenceBackOnDatePickerPanel, FinePriceFineNumberPanel
			};

			bool hiding = false;
			foreach (var panel in panels)
			{
				if (panel == startPanel) hiding = true;
				if (hiding)
				{
					panel.Visibility = Visibility.Hidden;
				}
			}

			SaveButton.IsEnabled = false;
			ClearButton.IsEnabled = PersonPickerButton.SelectedItem != null;
		}
	}
}
