using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarrierSlideRuler.ViewModels {
	class MainViewModel : ViewModelBase {
		public ICommand ButtonCommand { get; private set; }
		private void ButtonAction() {
			//
		}
		// コンストラクタ
		public MainViewModel() {
			ButtonCommand = new CommandBase(ButtonAction);
		}
	}
}
