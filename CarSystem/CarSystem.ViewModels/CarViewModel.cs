using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarSystem.ViewModels
{
	public class CarViewModel : INotifyPropertyChanged
	{
		private int id;
		private string brand;
		private string model;
		private string number;
		private string displayName;

		[DisplayName("№")]
		public int Id
		{
			get { return this.id; }
			set { this.id = value; NotifyPropertyChanged(); }
		}

		[DisplayName("Марка")]
		public string Brand
		{
			get { return this.brand; }
			set { this.brand = value; NotifyPropertyChanged(); }
		}

		[DisplayName("Модел")]
		public string Model
		{
			get { return this.model; }
			set { this.model = value; NotifyPropertyChanged(); }
		}

		[DisplayName("Рег. №")]
		public string Number
		{
			get { return this.number; }
			set { this.number = value; NotifyPropertyChanged(); }
		}

		[Browsable(false)]
		public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; NotifyPropertyChanged(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
