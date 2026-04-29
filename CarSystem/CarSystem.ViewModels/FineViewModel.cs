using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarSystem.ViewModels
{
	public class FineViewModel : INotifyPropertyChanged
	{
		private int id;
		private string name;
		private string violation;

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

		[DisplayName("Нарушение")]
		public string Violation
		{
			get { return this.violation; }
			set { this.violation = value; NotifyPropertyChanged(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
