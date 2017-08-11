using CarrierSlideRuler.Models;
using CarrierSlideRuler.Views;
using GlpkWrapperCS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarrierSlideRuler.ViewModels {
	class MainViewModel : ViewModelBase {
		public delegate void Action();
		WeaponListView wlv = null;
		// 艦名や装備についての情報
		public class Parts {
			public string Name { get; set; }
			public List<string> SelectList { get; set; }
			public bool Flg { get; set; }
			public bool FixedFlg { get; set; }
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
						// 装備コンボボックスの表示内容
						for (int w = 0; w < Constant.MaxWeaponCount; ++w) {
							PartsList[w].Name = "なし";
						}
						NotifyPropertyChanged(nameof(PName1));
						NotifyPropertyChanged(nameof(PName2));
						NotifyPropertyChanged(nameof(PName3));
						NotifyPropertyChanged(nameof(PName4));
						// 装備コンボボックスの中身
						for (int w = 0; w < kammusu.SlotCount; ++w) {
							// 一覧を初期化
							PartsList[w].SelectList = Database.GetCanHaveList(kammusu);
						}
						NotifyPropertyChanged(nameof(PSelectList1));
						NotifyPropertyChanged(nameof(PSelectList2));
						NotifyPropertyChanged(nameof(PSelectList3));
						NotifyPropertyChanged(nameof(PSelectList4));
					}
					// SetTitleBarを走らせる
					act();
				}
			}
			public List<string> SelectList { get; set; }
			public List<Parts> PartsList;

			public List<string> USelectList { get => SelectList; }

			public string PName1 { get => PartsList[0].Name; set { PartsList[0].Name = value; NotifyPropertyChanged(nameof(PName1)); act(); } }
			public string PName2 { get => PartsList[1].Name; set { PartsList[1].Name = value; NotifyPropertyChanged(nameof(PName2)); act(); } }
			public string PName3 { get => PartsList[2].Name; set { PartsList[2].Name = value; NotifyPropertyChanged(nameof(PName3)); act(); } }
			public string PName4 { get => PartsList[3].Name; set { PartsList[3].Name = value; NotifyPropertyChanged(nameof(PName4)); act(); } }
			public List<string> PSelectList1 { get => PartsList[0].SelectList; }
			public List<string> PSelectList2 { get => PartsList[1].SelectList; }
			public List<string> PSelectList3 { get => PartsList[2].SelectList; }
			public List<string> PSelectList4 { get => PartsList[3].SelectList; }
			public bool PFlg1 { get => PartsList[0].Flg; }
			public bool PFlg2 { get => PartsList[1].Flg; }
			public bool PFlg3 { get => PartsList[2].Flg; }
			public bool PFlg4 { get => PartsList[3].Flg; }
			public bool PCheck1 { get => PartsList[0].FixedFlg; set { PartsList[0].FixedFlg = value; } }
			public bool PCheck2 { get => PartsList[1].FixedFlg; set { PartsList[1].FixedFlg = value; }}
			public bool PCheck3 { get => PartsList[2].FixedFlg; set { PartsList[2].FixedFlg = value; }}
			public bool PCheck4 { get => PartsList[3].FixedFlg; set { PartsList[3].FixedFlg = value; }}

			public Unit(Action act_) { act = act_; }
		}

		// 制空状態を判定
		private string GetAirWarStatus(int myAirPower, int enemyAirPower) {
			if (myAirPower >= enemyAirPower * 3) return "制空権確保";
			if (myAirPower * 2 >= enemyAirPower * 3) return "航空優勢";
			if (myAirPower * 3 >= enemyAirPower * 2) return "航空均衡";
			if (myAirPower * 3 >= enemyAirPower) return "航空劣勢";
			return "制空権喪失";
		}

		// 艦名一覧
		public List<string> KammusuNameList;
		// 艦名や装備についての情報リスト
		public List<Unit> UnitList { get; set; }
		// 自制空値を計算
		private int GetMyAirPower() {
			if (UnitList.Count < Constant.MaxKammusuCount)
				return 0;
			int sum = 0;
			for (int k = 0; k < Constant.MaxKammusuCount; ++k) {
				var kammusu = Database.GetKammusuData(UnitList[k].Name);
				for (int w = 0; w < Constant.MaxWeaponCount; ++w) {
					if (kammusu.Airs[w] <= 0) continue;
					var weapon = Database.GetWeaponData(UnitList[k].PartsList[w].Name);
					if (!weapon.IsStage1) continue;
					double temp = Math.Sqrt(kammusu.Airs[w]) * weapon.AntiAir + (Math.Sqrt(100 / 10) + Constant.AntiAirBonus(weapon.Type));
					sum += (int)temp;
				}
			}
			return sum;
		}

		// タイトルバー
		string title;
		public string Title { get => title; private set { title = value; NotifyPropertyChanged(nameof(Title)); } }
		// タイトルバーを更新する
		public void SetTitleBar() {
			Title = "CarrierSlideRuler(計算中...)";
			int myAirPower = GetMyAirPower();
			Title = $"CarrierSlideRuler(自制空値{myAirPower}, 敵制空値{enemyAirPower}, {GetAirWarStatus(myAirPower, enemyAirPower)})";
		}

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

		// 対地攻撃について
		public int AntiFieldType { get; set; }
		// 最適化の方向について
		public int OptimizeType { get; set; }
		// 制空状態について
		public int AirStatusMode { get; set; }
		// 彩雲の有無
		public bool SaiunCheck { get; set; }
		// 最小スロ回避
		public bool MinSlotCheck { get; set; }
		// 噴式使用禁止
		public bool NoUseJPB { get; set; }

		// 「装備...」ボタン
		public ICommand SetWeaponCommand { get; }
		private void SetWeaponAction() {
			// 装備一覧画面のインスタンスを作成して表示する
			wlv = new WeaponListView();
			var wlvm = new WeaponListViewModel();
			wlv.DataContext = wlvm;
			wlv.Show();
		}

		// 「最適化」ボタン
		public ICommand OptimizeCommand { get; }
		bool optimizeButtonState;
		public bool OptimizeButtonState { get => optimizeButtonState; set { optimizeButtonState = value; NotifyPropertyChanged(nameof(OptimizeButtonState)); } }
		private async void OptimizeAction() {
			OptimizeButtonState = false;
			Title = "CarrierSlideRuler(最適化中...)";
			//
			int X = Constant.MaxKammusuCount;
			int Y = Constant.MaxWeaponCount;
			int Z = Database.WeaponNameList.Count;
			var weaponList = Database.WeaponNameList;
			using (var problem = new MipProblem()) {
				// 最適化の方向
				problem.ObjDir = ObjectDirection.Maximize;
				// 制約式の数・名前・範囲
				problem.AddRows(1+X*Y+X*Y*Z+Z+X*Y+1+1+1+1);
				{
					int p = 0;
					//制空値制約
					double wantAaPower = EnemyAirPower * 1.1;
					switch (AirStatusMode) {
					case 0:
						wantAaPower *= 3;
						break;
					case 1:
						wantAaPower *= 1.5;
						break;
					case 2:
						wantAaPower *= 2.0 / 3;
						break;
					case 3:
						wantAaPower *= 1.0 / 3;
						break;
					}
					problem.SetRowBounds(p, BoundsType.Lower, wantAaPower, 0.0);
					++p;
					//スロット制約
					for (int x = 0; x < X; ++x) {
						for (int y = 0; y < Y; ++y) {
							problem.SetRowBounds(p, BoundsType.Fixed, 1.0, 1.0);
							++p;
						}
					}
					//搭載制約
					for (int x = 0; x < X; ++x) {
						var kammusu = Database.GetKammusuData(UnitList[x].Name);
						for (int y = 0; y < Y; ++y) {
							var fixedFlg = new bool[] { UnitList[x].PCheck1, UnitList[x].PCheck2, UnitList[x].PCheck3, UnitList[x].PCheck4 };
							for (int z = 0; z < Z; ++z) {
								var weapon = Database.GetWeaponData(weaponList[z]);
								var wType = weapon.Type;
								// その装備が「固定」されている場合はそれに従う
								if (fixedFlg[y]) {
									if (weapon.Name != UnitList[x].PartsList[y].Name)
										problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
									else
										problem.SetRowBounds(p, BoundsType.Fixed, 1.0, 1.0);
								}
								// その装備が搭載不可能な場合は「＝0」
								else if (!Database.HasWeaponjudge(UnitList[x].Name, weaponList[z]))
									problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
								// その装備の位置が搭載スロット数より後ろの位置なら、「なし」しか載せられないようにする
								else if (kammusu.SlotCount <= y) {
									if (weapon.Name != "なし")
										problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
									else
										problem.SetRowBounds(p, BoundsType.Fixed, 1.0, 1.0);
								}
								// 噴式使用禁止なら、噴式は「＝0」
								else if(NoUseJPB && wType == WeaponType.JPB)
									problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
								// 対地攻撃ON状態なら、艦爆や噴式は「＝0」
								else if ((wType == WeaponType.PB || wType == WeaponType.JPB) && (AntiFieldType == 1))
									problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
								// それ以外の場合なら「0≦□≦1」
								else
									problem.SetRowBounds(p, BoundsType.Upper, 0.0, 1.0);
								++p;
							}
						}
					}
					// 所持数制約
					for (int z = 0; z < Z; ++z) {
						int wId = Database.GetWeaponData(weaponList[z]).Id;
						int count = Database.GetHaveWeaponCount(wId);
						problem.SetRowBounds(p, BoundsType.Upper, 0.0, 1.0 * count);
						++p;
					}
					// 対地攻撃OFF制約
					for (int x = 0; x < X; ++x) {
						var kammusu = Database.GetKammusuData(UnitList[x].Name);
						double temp = (kammusu.IsAirGunAttack && AntiFieldType == 2 ? 1.0 : 0.0);
						problem.SetRowBounds(p, BoundsType.Lower, temp, 0.0);
						++p;
					}
					// 航空戦火力
					problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
					++p;
					// 砲撃戦火力
					double temp2 = 0.0;
					for(int x = 0; x < X; ++x) {
						var kammusu = Database.GetKammusuData(UnitList[x].Name);
						if (kammusu.IsAirGunAttack) temp2 += 55.0 + 1.5 * kammusu.Attack * 1.5;
					}
					problem.SetRowBounds(p, BoundsType.Fixed, -temp2, -temp2);
					++p;
					// 彩雲の有無
					problem.SetRowBounds(p, BoundsType.Lower, (SaiunCheck ? 1.0 : 0.0), 0.0);
					++p;
					// 最小スロ回避制約
					problem.SetRowBounds(p, BoundsType.Upper, 0.0, (MinSlotCheck ? 0.0 : X * Y * Z + 1));
				}
				// 変数の数・名前・範囲
				problem.AddColumns(X * Y * Z + 1 + 1);
				{
					int p = 0;
					for (int x = 0; x < X; ++x) {
						for (int y = 0; y < Y; ++y) {
							for (int z = 0; z < Z; ++z) {
								problem.SetColumnBounds(p, BoundsType.Fixed, 0.0, 1.0);
								problem.ColumnKind[p] = VariableKind.Binary;
								problem.ColumnName[p] = $"s_{(x + 1)}_{(y + 1)}_{(z + 1)}";
								++p;
							}
						}
					}
					problem.SetColumnBounds(p, BoundsType.Free, 0.0, 0.0);
					problem.ColumnName[p] = "Attack_A";
					++p;
					problem.SetColumnBounds(p, BoundsType.Free, 0.0, 0.0);
					problem.ColumnName[p] = "Attack_G";
					++p;
				}
				// 目的関数の係数
				{
					int p = 0;
					for (int x = 0; x < X; ++x) {
						for (int y = 0; y < Y; ++y) {
							for (int z = 0; z < Z; ++z) {
								problem.ObjCoef[p] = 0.0;
								++p;
							}
						}
					}
					problem.ObjCoef[p] = (OptimizeType == 1 ? 2.0 : 1.0);
					++p;
					problem.ObjCoef[p] = (OptimizeType == 2 ? 2.0 : 1.0);
					++p;
				}
				// 制約式の係数
				{
					// 記録用のリストを用意
					var ia = new List<int>();
					var ja = new List<int>();
					var ar = new List<double>();
					// 係数を追加していく
					{
						// 制空値制約(ia=0)
						int p = 0;
						for (int x = 0; x < X; ++x) {
							var kammusu = Database.GetKammusuData(UnitList[x].Name);
							for (int y = 0; y < Y; ++y) {
								for (int z = 0; z < Z; ++z) {
									var weapon = Database.GetWeaponData(weaponList[z]);
									ia.Add(0);
									ja.Add(p);
									if (!weapon.IsStage1 || kammusu.Airs[y] == 0) {
										ar.Add(0.0);
									}
									else {
										double temp = Math.Sqrt(kammusu.Airs[y]) * weapon.AntiAir + (Math.Sqrt(100 / 10) + Constant.AntiAirBonus(weapon.Type));
										ar.Add(temp);
									}
									++p;
								}
							}
						}
					}
					{
						// スロット制約(ia=1～XY)
						int p = 1;
						for (int x = 0; x < X; ++x) {
							for (int y = 0; y < Y; ++y) {
								for (int z = 0; z < Z; ++z) {
									ia.Add(p);
									ja.Add((x * Y + y) * Z + z);
									ar.Add(1.0);
								}
								++p;
							}
						}
					}
					{
						// 搭載制約(ia=XY+1～XY+XYZ)
						int p = X * Y + 1;
						for (int x = 0; x < X; ++x) {
							for (int y = 0; y < Y; ++y) {
								for (int z = 0; z < Z; ++z) {
									ia.Add(p);
									ja.Add((x * Y + y) * Z + z);
									ar.Add(1.0);
									++p;
								}
							}
						}
					}
					{
						// 所持数制約(ia=XY+XYZ+1～XY+XYZ+Z)
						int p = X * Y + X * Y * Z + 1;
						for (int z = 0; z < Z; ++z) {
							for (int x = 0; x < X; ++x) {
								for (int y = 0; y < Y; ++y) {
									ia.Add(p);
									ja.Add((x * Y + y) * Z + z);
									ar.Add(1.0);
								}
							}
							++p;
						}
					}
					{
						// 対地攻撃OFF制約(ia=XY+XYZ+Z+1～XY+XYZ+Z+X)
						int p = X * Y + X * Y * Z + Z + 1;
						for (int x = 0; x < X; ++x) {
							for (int y = 0; y < Y; ++y) {
								for (int z = 0; z < Z; ++z) {
									ia.Add(p);
									ja.Add((x * Y + y) * Z + z);
									var weapon = Database.GetWeaponData(weaponList[z]);
									if (weapon.Type == WeaponType.PB || weapon.Type == WeaponType.JPB)
										ar.Add(1.0);
									else
										ar.Add(0.0);
								}
							}
							++p;
						}
					}
					{
						// 航空戦火力(ia=XY+XYZ+Z+X+1)
						int p = X * Y + X * Y * Z + Z + X + 1;
						for (int x = 0; x < X; ++x) {
							var kammusu = Database.GetKammusuData(UnitList[x].Name);
							for (int y = 0; y < Y; ++y) {
								for (int z = 0; z < Z; ++z) {
									ia.Add(p);
									ja.Add((x * Y + y) * Z + z);
									var weapon = Database.GetWeaponData(weaponList[z]);
									if (!weapon.IsStage3 || kammusu.Airs[y] == 0)
										ar.Add(0.0);
									else {
										double temp = Math.Sqrt(kammusu.Airs[y]) * (weapon.Bomb + weapon.Torpedo) + (Math.Sqrt(100 / 10) + 25);
										switch (weapon.Type) {
										case WeaponType.PA:
											temp *= 1.15;
											break;
										case WeaponType.JPB:
											temp *= 1.0 / Math.Sqrt(2);
											break;
										}
										ar.Add(temp);
									}
								}
							}
						}
						ia.Add(p);
						ja.Add(X*Y*Z);
						ar.Add(-1.0);
						++p;
						// 砲撃戦火力(ia=XY+XYZ+Z+X+2)
						for (int x = 0; x < X; ++x) {
							var kammusu = Database.GetKammusuData(UnitList[x].Name);
							for (int y = 0; y < Y; ++y) {
								for (int z = 0; z < Z; ++z) {
									ia.Add(p);
									ja.Add((x * Y + y) * Z + z);
									var weapon = Database.GetWeaponData(weaponList[z]);
									if (!kammusu.IsAirGunAttack || !weapon.IsStage3)
										ar.Add(0.0);
									else {
										switch (weapon.Type) {
										case WeaponType.PA:
											ar.Add(1.5 * weapon.Torpedo);
											break;
										case WeaponType.PB:
										case WeaponType.JPB:
											ar.Add(1.95 * weapon.Bomb);
											break;
										default:
											ar.Add(0.0);
											break;
										}
									}
								}
							}
						}
						ia.Add(p);
						ja.Add(X * Y * Z + 1);
						ar.Add(-1.0);
						++p;
						// 彩雲の有無(ia=XY+XYZ+Z+X+3)
						for (int x = 0; x < X; ++x) {
							for (int y = 0; y < Y; ++y) {
								for (int z = 0; z < Z; ++z) {
									var weapon = Database.GetWeaponData(weaponList[z]);
									if(weapon.Type == WeaponType.PSS) {
										ia.Add(p);
										ja.Add((x * Y + y) * Z + z);
										ar.Add(1.0);
									}
								}
							}
						}
						++p;
						// 最小スロ回避制約(ia=XY+XYZ+Z+X+4)
						for (int x = 0; x < X; ++x) {
							var kammusu = Database.GetKammusuData(UnitList[x].Name);
							if (!kammusu.IsAirGunAttack) continue;
							int y = kammusu.SlotCount - 1;
							for (int z = 0; z < Z; ++z) {
								var weapon = Database.GetWeaponData(weaponList[z]);
								if (weapon.IsStage3) {
									ia.Add(p);
									ja.Add((x * Y + y) * Z + z);
									ar.Add(1.0);
								}
							}
						}
					}
					problem.LoadMatrix(ia.ToArray(), ja.ToArray(), ar.ToArray());
				}
				string hoge = problem.ToLpString();
				// 最適化を実行
				var result = await Task.Run(() => problem.BranchAndCut(false));
				// 結果を読み取る
				if(result == SolverResult.OK) {
					// スコア
					int offset = X * Y * Z;
					string message = $"総攻撃スコア：{problem.MipObjValue}\n航空攻撃スコア：{problem.MipColumnValue[offset]}\n航空砲撃戦スコア：{problem.MipColumnValue[offset + 1]}\n";
					// 正解となる装備配置
					var answer = new List<string>();
					for (int x = 0; x < X; ++x) {
						for (int y = 0; y < Y; ++y) {
							for (int z = 0; z < Z; ++z) {
								int index = (x * Y + y) * Z + z;
								// 値が1なら、それが選択されたとみなす
								if (Math.Abs(problem.MipColumnValue[index]) >= 0.0000001) {
									answer.Add(weaponList[z]);
								}
							}
						}
					}
					for (int x = 0; x < X; ++x) {
						message += $"{UnitList[x].Name}→";
						for (int y = 0; y < Y; ++y) {
							message += $"{answer[x * Y + y]},";
						}
						message += "\n";
					}
					// ダイアログで結果を表示
					MessageBox.Show(message, "CarrierSlideRuler", MessageBoxButton.OK, MessageBoxImage.Information);
					// 画面に反映する
					int p = 0;
					for(int x = 0; x < X; ++x) {
						UnitList[x].PName1 = answer[p];
						++p;
						UnitList[x].PName2 = answer[p];
						++p;
						UnitList[x].PName3 = answer[p];
						++p;
						UnitList[x].PName4 = answer[p];
						++p;
					}
				}
				else {
					MessageBox.Show("実行可能解が出せませんでした。", "CarrierSlideRuler", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			}
			OptimizeButtonState = true;
			SetTitleBar();
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
			AntiFieldType = 0;
			OptimizeButtonState = true;
			AirStatusMode = 1;
			SaiunCheck = true;
			MinSlotCheck = true;
		}
	}
}
