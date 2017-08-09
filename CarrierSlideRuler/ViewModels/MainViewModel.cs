﻿using CarrierSlideRuler.Models;
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
		public class Unit : ViewModelBase {
			Action act;

			string name;
			public string Name {
				get => name;
				set {
					// 値を更新
					name = value;
					// 艦娘が変化したので、それに合わせてプロパティを変化させる
					var kammusu = Database.GetKammusuData(name);
					{
						// 装備コンボボックスのON/OFF
						for (int w = 0; w < kammusu.SlotCount; ++w) {
							PartsList[w].Flg = true;
						}
						for (int w = kammusu.SlotCount; w < Constant.MaxWeaponCount; ++w) {
							PartsList[w].Flg = false;
						}
						NotifyPropertyChanged(nameof(PFlg1));
						NotifyPropertyChanged(nameof(PFlg2));
						NotifyPropertyChanged(nameof(PFlg3));
						NotifyPropertyChanged(nameof(PFlg4));
						// 装備コンボボックスの中身
						for (int w = 0; w < kammusu.SlotCount; ++w) {
							// 一覧を初期化
							PartsList[w].SelectList = new List<string>();
							// 線形検索
							foreach(string name in Database.WeaponNameList) {
								// 装備の種類を取得
								var type = Database.GetWeaponData(name).Type;
								// 艦娘の状態に合わせ、その装備を装備できるかを判定する
								switch (type) {
								case WeaponType.PF:
									if (kammusu.HasPF) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.PA:
									if (kammusu.HasPA) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.PB:
									if (kammusu.HasPB) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.JPB:
									if (kammusu.HasJPB) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.WF:
									if (kammusu.HasWF) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.WB:
									if (kammusu.HasWB) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.PS:
								case WeaponType.PSS:
									if (kammusu.HasPS) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.PSK:
									if (kammusu.HasPSK) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.AS:
									if (kammusu.HasAS) PartsList[w].SelectList.Add(name);
									break;
								case WeaponType.Other:
									PartsList[w].SelectList.Add(name);
									break;
								}
							}
						}
						NotifyPropertyChanged(nameof(PSelectList1));
						NotifyPropertyChanged(nameof(PSelectList2));
						NotifyPropertyChanged(nameof(PSelectList3));
						NotifyPropertyChanged(nameof(PSelectList4));
						// 装備コンボボックスの表示内容
						for (int w = kammusu.SlotCount; w < Constant.MaxWeaponCount; ++w) {
							PartsList[w].Name = "なし";
						}
						NotifyPropertyChanged(nameof(PName1));
						NotifyPropertyChanged(nameof(PName2));
						NotifyPropertyChanged(nameof(PName3));
						NotifyPropertyChanged(nameof(PName4));
					}
					// SetTitleBarを走らせる
					act();
				}
			}
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
				unit.PartsList = new List<Parts>();
				unit.SelectList = KammusuNameList;
				for (int w = 0; w < Constant.MaxWeaponCount; ++w) {
					var parts = new Parts();
					parts.Name = "なし";
					parts.Flg = false;
					parts.SelectList = new List<string>{ "なし" };
					unit.PartsList.Add(parts);
				}
				unit.Name = "なし";
				UnitList.Add(unit);
			}
			#endregion
			EnemyAirPower = 0;
		}
	}
}
