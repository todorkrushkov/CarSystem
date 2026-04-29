using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarSystem.ViewModels
{
	public class PersonViewModel : INotifyPropertyChanged
	{
		private int id;
		private string name;
		private string egn;
		private string displayName;

		[DisplayName("№")]
		public int Id
		{
			get { return this.id; }
			set { this.id = value; NotifyPropertyChanged(); }
		}

		[DisplayName("Имена")]
		public string Name
		{
			get { return this.name; }
			set { this.name = value; NotifyPropertyChanged(); }
		}

		[DisplayName("ЕГН")]
		public string EGN
		{
			get { return this.egn; }
			set { this.egn = value; NotifyPropertyChanged(); }
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
