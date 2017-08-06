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
		#region 艦娘名選択
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
		#endregion
		#region 装備選択
		public ObservableCollection<string> WeaponNameList { get; }
		int weaponSelectedIndex11;
		public int WeaponSelectedIndex11 {
			get => weaponSelectedIndex11;
			set {
				weaponSelectedIndex11 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex11));
			}
		}
		int weaponSelectedIndex21;
		public int WeaponSelectedIndex21 {
			get => weaponSelectedIndex21;
			set {
				weaponSelectedIndex21 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex21));
			}
		}
		int weaponSelectedIndex31;
		public int WeaponSelectedIndex31 {
			get => weaponSelectedIndex31;
			set {
				weaponSelectedIndex31 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex31));
			}
		}
		int weaponSelectedIndex41;
		public int WeaponSelectedIndex41 {
			get => weaponSelectedIndex41;
			set {
				weaponSelectedIndex41 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex41));
			}
		}
		int weaponSelectedIndex51;
		public int WeaponSelectedIndex51 {
			get => weaponSelectedIndex51;
			set {
				weaponSelectedIndex51 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex51));
			}
		}
		int weaponSelectedIndex61;
		public int WeaponSelectedIndex61 {
			get => weaponSelectedIndex61;
			set {
				weaponSelectedIndex61 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex61));
			}
		}
		int weaponSelectedIndex12;
		public int WeaponSelectedIndex12 {
			get => weaponSelectedIndex12;
			set {
				weaponSelectedIndex12 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex12));
			}
		}
		int weaponSelectedIndex22;
		public int WeaponSelectedIndex22 {
			get => weaponSelectedIndex22;
			set {
				weaponSelectedIndex22 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex22));
			}
		}
		int weaponSelectedIndex32;
		public int WeaponSelectedIndex32 {
			get => weaponSelectedIndex32;
			set {
				weaponSelectedIndex32 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex32));
			}
		}
		int weaponSelectedIndex42;
		public int WeaponSelectedIndex42 {
			get => weaponSelectedIndex42;
			set {
				weaponSelectedIndex42 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex42));
			}
		}
		int weaponSelectedIndex52;
		public int WeaponSelectedIndex52 {
			get => weaponSelectedIndex52;
			set {
				weaponSelectedIndex52 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex52));
			}
		}
		int weaponSelectedIndex62;
		public int WeaponSelectedIndex62 {
			get => weaponSelectedIndex62;
			set {
				weaponSelectedIndex62 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex62));
			}
		}
		int weaponSelectedIndex13;
		public int WeaponSelectedIndex13 {
			get => weaponSelectedIndex13;
			set {
				weaponSelectedIndex13 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex13));
			}
		}
		int weaponSelectedIndex23;
		public int WeaponSelectedIndex23 {
			get => weaponSelectedIndex23;
			set {
				weaponSelectedIndex23 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex23));
			}
		}
		int weaponSelectedIndex33;
		public int WeaponSelectedIndex33 {
			get => weaponSelectedIndex33;
			set {
				weaponSelectedIndex33 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex33));
			}
		}
		int weaponSelectedIndex43;
		public int WeaponSelectedIndex43 {
			get => weaponSelectedIndex43;
			set {
				weaponSelectedIndex43 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex43));
			}
		}
		int weaponSelectedIndex53;
		public int WeaponSelectedIndex53 {
			get => weaponSelectedIndex53;
			set {
				weaponSelectedIndex53 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex53));
			}
		}
		int weaponSelectedIndex63;
		public int WeaponSelectedIndex63 {
			get => weaponSelectedIndex63;
			set {
				weaponSelectedIndex63 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex63));
			}
		}
		int weaponSelectedIndex14;
		public int WeaponSelectedIndex14 {
			get => weaponSelectedIndex14;
			set {
				weaponSelectedIndex14 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex14));
			}
		}
		int weaponSelectedIndex24;
		public int WeaponSelectedIndex24 {
			get => weaponSelectedIndex24;
			set {
				weaponSelectedIndex24 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex24));
			}
		}
		int weaponSelectedIndex34;
		public int WeaponSelectedIndex34 {
			get => weaponSelectedIndex34;
			set {
				weaponSelectedIndex34 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex34));
			}
		}
		int weaponSelectedIndex44;
		public int WeaponSelectedIndex44 {
			get => weaponSelectedIndex44;
			set {
				weaponSelectedIndex44 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex44));
			}
		}
		int weaponSelectedIndex54;
		public int WeaponSelectedIndex54 {
			get => weaponSelectedIndex54;
			set {
				weaponSelectedIndex54 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex54));
			}
		}
		int weaponSelectedIndex64;
		public int WeaponSelectedIndex64 {
			get => weaponSelectedIndex64;
			set {
				weaponSelectedIndex64 = value;
				NotifyPropertyChanged(nameof(WeaponSelectedIndex64));
			}
		}
		#endregion
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
				int defaultSelectedIndex = KammusuNameList.Count - 1;
				KammusuSelectedIndex1 = defaultSelectedIndex;
				KammusuSelectedIndex2 = defaultSelectedIndex;
				KammusuSelectedIndex3 = defaultSelectedIndex;
				KammusuSelectedIndex4 = defaultSelectedIndex;
				KammusuSelectedIndex5 = defaultSelectedIndex;
				KammusuSelectedIndex6 = defaultSelectedIndex;
			}
			// 装備名選択
			{
				WeaponNameList = new ObservableCollection<string>();
				foreach (string name in Database.WeaponNameList) {
					WeaponNameList.Add(name);
				}
				int defaultSelectedIndex = WeaponNameList.Count - 1;
				WeaponSelectedIndex11 = defaultSelectedIndex;
				WeaponSelectedIndex21 = defaultSelectedIndex;
				WeaponSelectedIndex31 = defaultSelectedIndex;
				WeaponSelectedIndex41 = defaultSelectedIndex;
				WeaponSelectedIndex51 = defaultSelectedIndex;
				WeaponSelectedIndex61 = defaultSelectedIndex;
				WeaponSelectedIndex12 = defaultSelectedIndex;
				WeaponSelectedIndex22 = defaultSelectedIndex;
				WeaponSelectedIndex32 = defaultSelectedIndex;
				WeaponSelectedIndex42 = defaultSelectedIndex;
				WeaponSelectedIndex52 = defaultSelectedIndex;
				WeaponSelectedIndex62 = defaultSelectedIndex;
				WeaponSelectedIndex13 = defaultSelectedIndex;
				WeaponSelectedIndex23 = defaultSelectedIndex;
				WeaponSelectedIndex33 = defaultSelectedIndex;
				WeaponSelectedIndex43 = defaultSelectedIndex;
				WeaponSelectedIndex53 = defaultSelectedIndex;
				WeaponSelectedIndex63 = defaultSelectedIndex;
				WeaponSelectedIndex14 = defaultSelectedIndex;
				WeaponSelectedIndex24 = defaultSelectedIndex;
				WeaponSelectedIndex34 = defaultSelectedIndex;
				WeaponSelectedIndex44 = defaultSelectedIndex;
				WeaponSelectedIndex54 = defaultSelectedIndex;
				WeaponSelectedIndex64 = defaultSelectedIndex;
			}
		}
	}
}
