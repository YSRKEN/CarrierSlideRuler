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
		public delegate void Action();
		// 艦名や装備についての情報
		public class Parts {
			public string Name { get; set; }
			public List<string> SelectList { get; set; }
			public bool Flg { get; set; }
		}
		public class Unit {
			Action act;

			string name;
			public string Name { get => name; set { name = value;  act(); } }
			public List<string> SelectList { get; set; }
			public List<Parts> PartsList;

			public List<string> USelectList { get => SelectList; }

			public string PName1 { get => PartsList[0].Name; set { PartsList[0].Name = value; } }
			public string PName2 { get => PartsList[1].Name; set { PartsList[1].Name = value; } }
			public string PName3 { get => PartsList[2].Name; set { PartsList[2].Name = value; } }
			public string PName4 { get => PartsList[3].Name; set { PartsList[3].Name = value; } }
			public List<string> PSelectList1 { get => PartsList[0].SelectList; }
			public List<string> PSelectList2 { get => PartsList[1].SelectList; }
			public List<string> PSelectList3 { get => PartsList[2].SelectList; }
			public List<string> PSelectList4 { get => PartsList[3].SelectList; }
			public bool PFlg1 { get => PartsList[0].Flg; }
			public bool PFlg2 { get => PartsList[1].Flg; }
			public bool PFlg3 { get => PartsList[2].Flg; }
			public bool PFlg4 { get => PartsList[3].Flg; }

			public Unit(Action act_) { act = act_; }
		}

		// タイトルバー
		public string Title { get; private set; }
		// 艦名一覧
		public List<string> KammusuNameList;
		// 艦名や装備についての情報リスト
		public List<Unit> UnitList { get; set; }

		// 敵制空値
		int enemyAirPower;
		public int EnemyAirPower {
			get => enemyAirPower;
			set {
				// 負の値を取らないようにする
				if (value < 0) return;
				// 代入操作
				enemyAirPower = value;
				// 反映操作
				NotifyPropertyChanged(nameof(EnemyAirPower));
				// タイトルバーを更新
				SetTitleBar();
			}
		}

		// 「装備...」ボタン
		public ICommand SetWeaponCommand { get; }
		private void SetWeaponAction() {
			//スタブ
		}
		// 「最適化」ボタン
		public ICommand OptimizeCommand { get; }
		private void OptimizeAction() {
			//スタブ
		}

		// タイトルバーを更新する
		public void SetTitleBar() {
			Title = "CarrierSlideRuler";
		}
		// コンストラクタ
		public MainViewModel() {
			// ボタンにCommandを設定する
			SetWeaponCommand = new CommandBase(SetWeaponAction);
			OptimizeCommand = new CommandBase(OptimizeAction);
			// 各オブジェクトの初期値を決定する
			#region 艦名や装備についての情報
			KammusuNameList = Database.KammusuNameList;
			UnitList = new List<Unit>();
			for(int k  = 0; k < Constant.MaxKammusuCount; ++k) {
				var unit = new Unit(SetTitleBar);
				unit.Name = "なし";
				unit.PartsList = new List<Parts>();
				unit.SelectList = KammusuNameList;
				for (int w = 0; w < Constant.MaxWeaponCount; ++w) {
					var parts = new Parts();
					parts.Name = "なし";
					parts.Flg = false;
					parts.SelectList = new List<string>{ "なし" };
					unit.PartsList.Add(parts);
				}
				UnitList.Add(unit);
			}
			#endregion
			EnemyAirPower = 0;
		}
	}
}
