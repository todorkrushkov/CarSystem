using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarSystem.ViewModels
{
	public class ViolationViewModel : INotifyPropertyChanged
	{
		private int id;
		private string name;
		private string message;

		[DisplayName("№")]
		public int Id
		{
			get { return this.id; }
			set { this.id = value; NotifyPropertyChanged(); }
		}

		[DisplayName("Наименование")]
		public string Name
		{
			get { return this.name; }
			set { this.name = value; NotifyPropertyChanged(); }
		}

		[DisplayName("Описание")]
		public string Message
		{
			get { return this.message; }
			set { this.message = value; NotifyPropertyChanged(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
