using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarSystem.ViewModels
{
	public class EmissionStandartViewModel : INotifyPropertyChanged
	{
		private int id;
		private string name;

		[DisplayName("№")]
		public int Id
		{
			get { return this.id; }
			set { this.id = value; NotifyPropertyChanged(); }
		}

		[DisplayName("Стандарт")]
		public string Name
		{
			get { return this.name; }
			set { this.name = value; NotifyPropertyChanged(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
