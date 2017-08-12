using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrierSlideRuler.ViewModels {
	class ViewModelBase : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string parameter) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parameter));
	}
}
