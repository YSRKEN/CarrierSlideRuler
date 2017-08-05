using CarrierSlideRuler.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarrierSlideRuler.ViewModels {
	class MainViewModel : ViewModelBase {
		// 艦娘名選択
		public ObservableCollection<string> KammusuNameList { get; }
		int kammusuSelectedIndex1;
		public int KammusuSelectedIndex1 {
			get => kammusuSelectedIndex1;
			set {
				kammusuSelectedIndex1 = value;
				NotifyPropertyChanged(nameof(KammusuSelectedIndex1));
			}
		}
		int kammusuSelectedIndex2;
		public int KammusuSelectedIndex2 {
			get => kammusuSelectedIndex2;
			set {
				kammusuSelectedIndex2 = value;
				NotifyPropertyChanged(nameof(KammusuSelectedIndex2));
			}
		}
		int kammusuSelectedIndex3;
		public int KammusuSelectedIndex3 {
			get => kammusuSelectedIndex3;
			set {
				kammusuSelectedIndex3 = value;
				NotifyPropertyChanged(nameof(KammusuSelectedIndex3));
			}
		}
		int kammusuSelectedIndex4;
		public int KammusuSelectedIndex4 {
			get => kammusuSelectedIndex4;
			set {
				kammusuSelectedIndex4 = value;
				NotifyPropertyChanged(nameof(KammusuSelectedIndex4));
			}
		}
		int kammusuSelectedIndex5;
		public int KammusuSelectedIndex5 {
			get => kammusuSelectedIndex5;
			set {
				kammusuSelectedIndex5 = value;
				NotifyPropertyChanged(nameof(KammusuSelectedIndex5));
			}
		}
		int kammusuSelectedIndex6;
		public int KammusuSelectedIndex6 {
			get => kammusuSelectedIndex6;
			set {
				kammusuSelectedIndex6 = value;
				NotifyPropertyChanged(nameof(KammusuSelectedIndex6));
			}
		}
		// 「最適化」ボタン
		public ICommand ButtonCommand { get; }
		private void ButtonAction() {
			//
		}
		// コンストラクタ
		public MainViewModel() {
			// ボタン設定
			ButtonCommand = new CommandBase(ButtonAction);
			// 艦娘名選択
			{
				KammusuNameList = new ObservableCollection<string>();
				foreach(string name in Database.KammusuNameList) {
					KammusuNameList.Add(name);
				}
			}
			KammusuSelectedIndex1 = -1;
			KammusuSelectedIndex2 = -1;
			KammusuSelectedIndex3 = -1;
			KammusuSelectedIndex4 = -1;
			KammusuSelectedIndex5 = -1;
			KammusuSelectedIndex6 = -1;
		}
	}
}
