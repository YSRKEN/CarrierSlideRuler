﻿using CarrierSlideRuler.Models;
using CarrierSlideRuler.Views;
using GlpkWrapperCS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarrierSlideRuler.ViewModels {
	class MainViewModel : ViewModelBase {
		public delegate void Action();
		WeaponListView wlv;
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
                        NotifyPropertyChanged(nameof(PFlg5));
                        // 装備コンボボックスの表示内容
                        for (int w = 0; w < Constant.MaxWeaponCount; ++w) {
							PartsList[w].Name = "なし";
						}
						NotifyPropertyChanged(nameof(PName1));
						NotifyPropertyChanged(nameof(PName2));
						NotifyPropertyChanged(nameof(PName3));
						NotifyPropertyChanged(nameof(PName4));
                        NotifyPropertyChanged(nameof(PName5));
                        // 装備コンボボックスの中身
                        for (int w = 0; w < kammusu.SlotCount; ++w) {
							// 一覧を初期化
							PartsList[w].SelectList = Database.GetCanHaveList(kammusu);
						}
						NotifyPropertyChanged(nameof(PSelectList1));
						NotifyPropertyChanged(nameof(PSelectList2));
						NotifyPropertyChanged(nameof(PSelectList3));
						NotifyPropertyChanged(nameof(PSelectList4));
                        NotifyPropertyChanged(nameof(PSelectList5));
                        // 搭載数
                        pSlotSize = Enumerable.Repeat(0, Constant.MaxWeaponCount).ToList();
						for(int w = 0; w < Constant.MaxWeaponCount; ++w) {
							pSlotSize[w] = kammusu.Airs[w];
						}
						NotifyPropertyChanged(nameof(PSlotSize1));
						NotifyPropertyChanged(nameof(PSlotSize2));
						NotifyPropertyChanged(nameof(PSlotSize3));
						NotifyPropertyChanged(nameof(PSlotSize4));
                        NotifyPropertyChanged(nameof(PSlotSize5));
                    }
					// SetTitleBarを走らせる
					act();
				}
			}
			public List<string> SelectList { get; set; }
			public List<Parts> PartsList;
			public List<int> pSlotSize;

			public List<string> USelectList { get => SelectList; }

			public string PName1 { get => PartsList[0].Name; set { PartsList[0].Name = value; NotifyPropertyChanged(nameof(PName1)); act(); } }
			public string PName2 { get => PartsList[1].Name; set { PartsList[1].Name = value; NotifyPropertyChanged(nameof(PName2)); act(); } }
			public string PName3 { get => PartsList[2].Name; set { PartsList[2].Name = value; NotifyPropertyChanged(nameof(PName3)); act(); } }
			public string PName4 { get => PartsList[3].Name; set { PartsList[3].Name = value; NotifyPropertyChanged(nameof(PName4)); act(); } }
            public string PName5 { get => PartsList[4].Name; set { PartsList[4].Name = value; NotifyPropertyChanged(nameof(PName5)); act(); } }
            public List<string> PSelectList1 { get => PartsList[0].SelectList; }
			public List<string> PSelectList2 { get => PartsList[1].SelectList; }
			public List<string> PSelectList3 { get => PartsList[2].SelectList; }
			public List<string> PSelectList4 { get => PartsList[3].SelectList; }
            public List<string> PSelectList5 { get => PartsList[4].SelectList; }
            public bool PFlg1 { get => PartsList[0].Flg; }
			public bool PFlg2 { get => PartsList[1].Flg; }
			public bool PFlg3 { get => PartsList[2].Flg; }
			public bool PFlg4 { get => PartsList[3].Flg; }
            public bool PFlg5 { get => PartsList[4].Flg; }
            public int PSlotSize1 { get => pSlotSize[0]; }
			public int PSlotSize2 { get => pSlotSize[1]; }
			public int PSlotSize3 { get => pSlotSize[2]; }
			public int PSlotSize4 { get => pSlotSize[3]; }
            public int PSlotSize5 { get => pSlotSize[4]; }
            public bool PCheck1 { get => PartsList[0].FixedFlg; set { PartsList[0].FixedFlg = value; } }
			public bool PCheck2 { get => PartsList[1].FixedFlg; set { PartsList[1].FixedFlg = value; }}
			public bool PCheck3 { get => PartsList[2].FixedFlg; set { PartsList[2].FixedFlg = value; }}
			public bool PCheck4 { get => PartsList[3].FixedFlg; set { PartsList[3].FixedFlg = value; }}
            public bool PCheck5 { get => PartsList[4].FixedFlg; set { PartsList[4].FixedFlg = value; } }
            public bool CiFlg { get; set; }
			public bool NightFlg { get; set; }

			public Unit(Action act_) { act = act_; }
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
			Title = $"CarrierSlideRuler(自制空値{myAirPower}, 目標制空値{GetWantAaPower()}, {(myAirPower >= GetWantAaPower() ? "OK" : "NG")})";
		}

		// 敵制空値
		public int EnemyAirPowerTemp { get; set; }
		public int AirStatusModeTemp { get; set; }

		// 対地攻撃について
		public int AntiFieldType { get; set; }
		// 最適化の方向について
		public int OptimizeType { get; set; }
		// 彩雲の有無
		public bool SaiunCheck { get; set; }
		// 最小スロ回避
		public bool MinSlotCheck { get; set; }
		// 噴式使用禁止
		public bool NoUseJPB { get; set; }
		// 時間制限
		public int TimeLimitType { get; set; }
		// 制空値を更に盛る設定
		public int AddAirPowerPerIndex { get; set; }

		// 敵艦隊一覧のインデックス
		public int EnemyParamIndex { get; set; }
		// 敵艦隊一覧の表示リスト
		public ObservableCollection<string> EnemyParamViewList { get; set; } = new ObservableCollection<string>();
		// 敵艦隊一覧のリスト
		private List<KeyValuePair<int, int>> enemyParamList = new List<KeyValuePair<int, int>>();
		// 敵艦隊＆制空状況の条件をすべて満たす制空値を求める
		private int GetWantAaPower()
		{
			if (enemyParamList.Count == 0)
				return 0;
			int wantAaPower = 0;
			foreach(var param in enemyParamList)
			{
				int enemyAaPower = param.Key;
				int airStatusMode = param.Value;
				switch (airStatusMode)
				{
					case 0:
						wantAaPower = Math.Max(wantAaPower, enemyAaPower * 3);
						break;
					case 1:
						wantAaPower = Math.Max(wantAaPower, (int)Math.Ceiling(enemyAaPower * 3.0 / 2));
						break;
					case 2:
						if (enemyAaPower % 3 == 0)
						{
							wantAaPower = Math.Max(wantAaPower, (int)Math.Ceiling(enemyAaPower * 2.0 / 3) + 1);
						}
						else
						{
							wantAaPower = Math.Max(wantAaPower, (int)Math.Ceiling(enemyAaPower * 2.0 / 3));
						}
						break;
					case 3:
						if (enemyAaPower % 3 == 0)
						{
							wantAaPower = Math.Max(wantAaPower, (int)Math.Ceiling(enemyAaPower * 1.0 / 3) + 1);
						}
						else
						{
							wantAaPower = Math.Max(wantAaPower, (int)Math.Ceiling(enemyAaPower * 1.0 / 3));
						}
						break;
					case 4:
						wantAaPower = Math.Max(wantAaPower, 0);
						break;
				}
			}
			return wantAaPower;
		}

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
			// 敵艦隊が登録されていない場合は無視する
			if (enemyParamList.Count == 0)
				return;
			// 最適化中なので、「最適化」ボタンを無効にする
			OptimizeButtonState = false;
			Title = "CarrierSlideRuler(最適化中...)";
			// 目指す制空値の初期目標として、敵艦隊＆制空状況の条件をすべて満たすものがある
			double wantAaPower = GetWantAaPower();
			var addAirPowerPer = new List<double> { 1.0, 1.01, 1.03, 1.05, 1.07, 1.10 };
			wantAaPower *= addAirPowerPer[AddAirPowerPerIndex];
			//
			while (true)
			{
				int X = Constant.MaxKammusuCount;
				int Y = Constant.MaxWeaponCount;
				int Z = Database.WeaponNameList.Count;
				var weaponList = Database.WeaponNameList;
				using (var problem = new MipProblem())
				{
					// 最適化の方向
					problem.ObjDir = ObjectDirection.Maximize;
					#region 制約式の数・名前・範囲
					problem.AddRows(1 + X * Y + X * Y * Z + Z + X * Y + 1 + 1 + 1 + 1 + X * 2 + X * 2);
					{
						int p = 0;
						//制空値制約
						problem.SetRowBounds(p, BoundsType.Lower, wantAaPower, 0.0);
						++p;
						//スロット制約
						for (int x = 0; x < X; ++x)
						{
							for (int y = 0; y < Y; ++y)
							{
								problem.SetRowBounds(p, BoundsType.Fixed, 1.0, 1.0);
								++p;
							}
						}
						//搭載制約
						for (int x = 0; x < X; ++x)
						{
							var kammusu = Database.GetKammusuData(UnitList[x].Name);
							for (int y = 0; y < Y; ++y)
							{
								var fixedFlg = new bool[] { UnitList[x].PCheck1, UnitList[x].PCheck2, UnitList[x].PCheck3, UnitList[x].PCheck4, UnitList[x].PCheck5 };
								for (int z = 0; z < Z; ++z)
								{
									var weapon = Database.GetWeaponData(weaponList[z]);
									var wType = weapon.Type;
									// その装備が「固定」されている場合はそれに従う
									if (fixedFlg[y])
									{
										if (weapon.Name != UnitList[x].PartsList[y].Name)
											problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
										else
											problem.SetRowBounds(p, BoundsType.Fixed, 1.0, 1.0);
									}
									// その装備が搭載不可能な場合は「＝0」
									else if (!Database.HasWeaponjudge(UnitList[x].Name, weaponList[z]))
										problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
									// その装備の位置が搭載スロット数より後ろの位置なら、「なし」しか載せられないようにする
									else if (kammusu.SlotCount <= y)
									{
										if (weapon.Name != "なし")
											problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
										else
											problem.SetRowBounds(p, BoundsType.Fixed, 1.0, 1.0);
									}
									// 噴式使用禁止なら、噴式は「＝0」
									else if (NoUseJPB && wType == WeaponType.JPB)
										problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
									// 対地攻撃ON状態なら、艦爆や噴式は「＝0」
									// (ただし当該艦が昼戦CIをONにしていた場合は除く)
									else if ((wType == WeaponType.PB || wType == WeaponType.JPB) && (AntiFieldType == 1) && !UnitList[x].CiFlg)
										problem.SetRowBounds(p, BoundsType.Fixed, 0.0, 0.0);
									// それ以外の場合なら「0≦□≦1」
									else
										problem.SetRowBounds(p, BoundsType.Upper, 0.0, 1.0);
									++p;
								}
							}
						}
						// 所持数制約
						for (int z = 0; z < Z; ++z)
						{
							int wId = Database.GetWeaponData(weaponList[z]).Id;
							int count = Database.GetHaveWeaponCount(wId);
							problem.SetRowBounds(p, BoundsType.Upper, 0.0, 1.0 * count);
							++p;
						}
						// 対地攻撃OFF制約
						for (int x = 0; x < X; ++x)
						{
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
						for (int x = 0; x < X; ++x)
						{
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
						++p;
						// 昼戦CI用の制約
						for (int x = 0; x < X; ++x)
						{
							var kammusu = Database.GetKammusuData(UnitList[x].Name);
							double temp = (kammusu.IsAirGunAttack && UnitList[x].CiFlg ? 1.0 : 0.0);
							problem.SetRowBounds(p, BoundsType.Lower, temp, 0.0);
							++p;
							problem.SetRowBounds(p, BoundsType.Lower, temp, 0.0);
							++p;
						}
						// 夜戦用の制約
						for (int x = 0; x < X; ++x)
						{
							var kammusu = Database.GetKammusuData(UnitList[x].Name);
							double temp = (kammusu.IsAirGunAttack && UnitList[x].NightFlg ? 1.0 : 0.0);
							// 夜戦特性を読み取り、それによって条件を変える
							switch (kammusu.NightAttackType)
							{
								case NightAttackType.First:
									problem.SetRowBounds(p, BoundsType.Lower, 0.0, 0.0);
									++p;
									problem.SetRowBounds(p, BoundsType.Lower, 0.0, 0.0);
									++p;
									break;
								case NightAttackType.ArkRoyal:
									problem.SetRowBounds(p, BoundsType.Lower, temp, 0.0);
									++p;
									problem.SetRowBounds(p, BoundsType.Lower, temp, 0.0);
									++p;
									break;
								case NightAttackType.SaratogaMkII:
									problem.SetRowBounds(p, BoundsType.Lower, temp, 0.0);
									++p;
									problem.SetRowBounds(p, BoundsType.Lower, 0.0, 0.0);
									++p;
									break;
								case NightAttackType.Other:
									problem.SetRowBounds(p, BoundsType.Lower, temp, 0.0);
									++p;
									problem.SetRowBounds(p, BoundsType.Lower, temp, 0.0);
									++p;
									break;
							}
						}
					}
					#endregion
					#region 変数の数・名前・範囲
					problem.AddColumns(X * Y * Z + 1 + 1);
					{
						int p = 0;
						for (int x = 0; x < X; ++x)
						{
							for (int y = 0; y < Y; ++y)
							{
								for (int z = 0; z < Z; ++z)
								{
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
					#endregion
					#region 目的関数の係数
					{
						int p = 0;
						for (int x = 0; x < X; ++x)
						{
							for (int y = 0; y < Y; ++y)
							{
								for (int z = 0; z < Z; ++z)
								{
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
					#endregion
					#region 制約式の係数
					{
						// 記録用のリストを用意
						var ia = new List<int>();
						var ja = new List<int>();
						var ar = new List<double>();
						// 係数を追加していく
						{
							// 制空値制約(ia=0)
							int p = 0;
							for (int x = 0; x < X; ++x)
							{
								var kammusu = Database.GetKammusuData(UnitList[x].Name);
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
										var weapon = Database.GetWeaponData(weaponList[z]);
										ia.Add(0);
										ja.Add(p);
										if (!weapon.IsStage1 || kammusu.Airs[y] == 0)
										{
											ar.Add(0.0);
										}
										else
										{
											double temp = (int)(Math.Sqrt(kammusu.Airs[y]) * weapon.AntiAir + (Math.Sqrt(100 / 10) + Constant.AntiAirBonus(weapon.Type)));
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
							for (int x = 0; x < X; ++x)
							{
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
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
							for (int x = 0; x < X; ++x)
							{
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
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
							for (int z = 0; z < Z; ++z)
							{
								for (int x = 0; x < X; ++x)
								{
									for (int y = 0; y < Y; ++y)
									{
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
							for (int x = 0; x < X; ++x)
							{
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
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
							// 空母カットイン時は熟練クリティカル補正が適用されないので注意
							int p = X * Y + X * Y * Z + Z + X + 1;
							for (int x = 0; x < X; ++x)
							{
								var kammusu = Database.GetKammusuData(UnitList[x].Name);
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
										ia.Add(p);
										ja.Add((x * Y + y) * Z + z);
										var weapon = Database.GetWeaponData(weaponList[z]);
										double coeff = 1.0;
										if (UnitList[x].CiFlg)
											coeff = 1.15;   //本当は複数の係数があるが決め打ち
										else
											coeff = (1.0 + 1.0 * (weapon.Hit + weapon.Evade) / 100) * (y == 0 ? 1.5 : 1.1); //1スロ励行のため、ちょっと極端な設定にした
										if (!weapon.IsStage3 || kammusu.Airs[y] == 0)
											ar.Add(0.0);
										else
										{
											double temp = Math.Sqrt(kammusu.Airs[y]) * (weapon.Bomb + weapon.Torpedo) + (Math.Sqrt(100 / 10) + 25);
											switch (weapon.Type)
											{
												case WeaponType.PA:
												case WeaponType.PAN:
													temp *= 1.15;
													break;
												case WeaponType.JPB:
													temp *= 1.0 / Math.Sqrt(2);
													break;
											}
											ar.Add(temp * coeff);
										}
									}
								}
							}
							ia.Add(p);
							ja.Add(X * Y * Z);
							ar.Add(-1.0);
							++p;
							// 砲撃戦火力(ia=XY+XYZ+Z+X+2)
							// 空母カットイン時は熟練クリティカル補正が適用されないので注意
							for (int x = 0; x < X; ++x)
							{
								var kammusu = Database.GetKammusuData(UnitList[x].Name);
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
										ia.Add(p);
										ja.Add((x * Y + y) * Z + z);
										var weapon = Database.GetWeaponData(weaponList[z]);
										double coeff = 1.0;
										if (UnitList[x].CiFlg)
											coeff = 1.15;   //本当は複数の係数があるが決め打ち
										else
											coeff = (1.0 + 1.0 * (weapon.Hit + weapon.Evade) / 100) * (y == 0 ? 1.5 : 1.1); //1スロ励行のため、ちょっと極端な設定にした
										if (!kammusu.IsAirGunAttack || !weapon.IsStage3)
											ar.Add(0.0);
										else
										{
											switch (weapon.Type)
											{
												case WeaponType.PA:
												case WeaponType.PAN:
													ar.Add(1.5 * weapon.Torpedo * coeff + weapon.Attack);
													break;
												case WeaponType.PB:
												case WeaponType.JPB:
													ar.Add(1.95 * weapon.Bomb * coeff + weapon.Attack);
													break;
												default:
													ar.Add(weapon.Attack);
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
							for (int x = 0; x < X; ++x)
							{
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
										var weapon = Database.GetWeaponData(weaponList[z]);
										if (weapon.Type == WeaponType.PSS)
										{
											ia.Add(p);
											ja.Add((x * Y + y) * Z + z);
											ar.Add(1.0);
										}
									}
								}
							}
							++p;
							// 最小スロ回避制約(ia=XY+XYZ+Z+X+4)
							for (int x = 0; x < X; ++x)
							{
								var kammusu = Database.GetKammusuData(UnitList[x].Name);
								if (!kammusu.IsAirGunAttack) continue;
								int y = kammusu.SlotCount - 1;
								for (int z = 0; z < Z; ++z)
								{
									var weapon = Database.GetWeaponData(weaponList[z]);
									if (weapon.IsStage3)
									{
										ia.Add(p);
										ja.Add((x * Y + y) * Z + z);
										ar.Add(1.0);
									}
								}
							}
						}
						{
							// 昼戦CI用の制約(ia=XY+XYZ+Z+X+4+1～ia=XY+XYZ+Z+X+4+2X)
							int p = X * Y + X * Y * Z + Z + X + 5;
							for (int x = 0; x < X; ++x)
							{
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
										var weapon = Database.GetWeaponData(weaponList[z]);
										if (weapon.Type == WeaponType.PB)
										{
											ia.Add(p);
											ja.Add((x * Y + y) * Z + z);
											ar.Add(1.0);
										}
									}
								}
								++p;
								for (int y = 0; y < Y; ++y)
								{
									for (int z = 0; z < Z; ++z)
									{
										var weapon = Database.GetWeaponData(weaponList[z]);
										if (weapon.Type == WeaponType.PA || weapon.Type == WeaponType.PAN)
										{
											ia.Add(p);
											ja.Add((x * Y + y) * Z + z);
											ar.Add(1.0);
										}
									}
								}
								++p;
							}
						}
						{
							// 夜戦用の制約(ia=XY+XYZ+Z+X+4+2X+1～ia=XY+XYZ+Z+X+4+2X+2X)
							int p = X * Y + X * Y * Z + Z + X + 4 + 2 * X + 1;
							for (int x = 0; x < X; ++x)
							{
								var kammusu = Database.GetKammusuData(UnitList[x].Name);
								// 夜戦特性を読み取り、それによって条件を変える
								switch (kammusu.NightAttackType)
								{
									case NightAttackType.First:
										// First→無条件に夜戦可能
										++p;
										++p;
										break;
									case NightAttackType.ArkRoyal:
										// ArkRoyal→(Swordfish＋その派生型)≧1またはOtherの条件
										// 実装上は、(夜航＋夜航甲＋カジキ系)≧1かつ(夜攻＋夜戦＋カジキ系)≧1と書く
										for (int y = 0; y < Y; ++y)
										{
											for (int z = 0; z < Z; ++z)
											{
												var weapon = Database.GetWeaponData(weaponList[z]);
												if (weapon.Name.IndexOf("夜間作戦航空要員") >= 0
													|| weapon.Name.IndexOf("Swordfish") >= 0)
												{
													ia.Add(p);
													ja.Add((x * Y + y) * Z + z);
													ar.Add(1.0);
												}
											}
										}
										++p;
										for (int y = 0; y < Y; ++y)
										{
											for (int z = 0; z < Z; ++z)
											{
												var weapon = Database.GetWeaponData(weaponList[z]);
												if (weapon.Type == WeaponType.PAN || weapon.Type == WeaponType.PFN
													|| weapon.Name.IndexOf("Swordfish") >= 0)
												{
													ia.Add(p);
													ja.Add((x * Y + y) * Z + z);
													ar.Add(1.0);
												}
											}
										}
										++p;
										break;
									case NightAttackType.SaratogaMkII:
										// SaratogaMkII→(夜間攻撃機+夜間戦闘機)≧1
										for (int y = 0; y < Y; ++y)
										{
											for (int z = 0; z < Z; ++z)
											{
												var weapon = Database.GetWeaponData(weaponList[z]);
												if (weapon.Type == WeaponType.PAN || weapon.Type == WeaponType.PFN)
												{
													ia.Add(p);
													ja.Add((x * Y + y) * Z + z);
													ar.Add(1.0);
												}
											}
										}
										++p;
										++p;
										break;
									case NightAttackType.Other:
										// OTher→(夜間作戦航空要員＋{夜間作戦航空要員＋熟練甲板員})≧1かつ(夜間攻撃機＋夜間戦闘機)≧1
										for (int y = 0; y < Y; ++y)
										{
											for (int z = 0; z < Z; ++z)
											{
												var weapon = Database.GetWeaponData(weaponList[z]);
												if (weapon.Name.IndexOf("夜間作戦航空要員") >= 0)
												{
													ia.Add(p);
													ja.Add((x * Y + y) * Z + z);
													ar.Add(1.0);
												}
											}
										}
										++p;
										for (int y = 0; y < Y; ++y)
										{
											for (int z = 0; z < Z; ++z)
											{
												var weapon = Database.GetWeaponData(weaponList[z]);
												if (weapon.Type == WeaponType.PAN || weapon.Type == WeaponType.PFN)
												{
													ia.Add(p);
													ja.Add((x * Y + y) * Z + z);
													ar.Add(1.0);
												}
											}
										}
										++p;
										break;
								}
							}
						}
						problem.LoadMatrix(ia.ToArray(), ja.ToArray(), ar.ToArray());
					}
					#endregion
					//string hoge = problem.ToLpString();
					// 最適化を実行
					int[] timeLimit = new int[] { 10, 30, 60, 600, 3600, 86400, 86400 * 21 };
					var result = await Task.Run(() => problem.BranchAndCut(false, timeLimit[TimeLimitType]));
					// 結果を読み取る
					if (result == SolverResult.OK || result == SolverResult.ErrorTimeLimit)
					{
						// スコア
						int offset = X * Y * Z;
						string message = $"総攻撃スコア：{problem.MipObjValue}\n航空攻撃スコア：{problem.MipColumnValue[offset]}\n航空砲撃戦スコア：{problem.MipColumnValue[offset + 1]}\n";
						// 正解となる装備配置
						var answer = new List<string>();
						for (int x = 0; x < X; ++x)
						{
							for (int y = 0; y < Y; ++y)
							{
								for (int z = 0; z < Z; ++z)
								{
									int index = (x * Y + y) * Z + z;
									// 値が1なら、それが選択されたとみなす
									if (Math.Abs(problem.MipColumnValue[index]) >= 0.0000001)
									{
										answer.Add(weaponList[z]);
									}
								}
							}
						}
						for (int x = 0; x < X; ++x)
						{
                            if (UnitList[x].Name != "なし")
                            {
                                message += $"{UnitList[x].Name}→";
                                for (int y = 0; y < Y; ++y)
                                {
                                    message += $"{answer[x * Y + y]},";
                                }
                                message += "\n";
                            }
						}
						// 通知
						if (result == SolverResult.ErrorTimeLimit)
							message += "\n※時間制限が来たので計算を打ち切りました。\n";
						// ダイアログで結果を表示
						SetTitleBar();
						MessageBox.Show(message, "CarrierSlideRuler", MessageBoxButton.OK, MessageBoxImage.Information);
						// 画面に反映する
						int p = 0;
						for (int x = 0; x < X; ++x)
						{
							UnitList[x].PName1 = answer[p];
							++p;
							UnitList[x].PName2 = answer[p];
							++p;
							UnitList[x].PName3 = answer[p];
							++p;
							UnitList[x].PName4 = answer[p];
							++p;
                            UnitList[x].PName5 = answer[p];
                            ++p;
                        }
						// その状態でSt1撃墜をテストし、問題ないかを判断する
						//制空に参加するスロットの一覧表を出す(制空値・搭載数)
						var slotList = new List<KeyValuePair<WeaponData, int>>();
						for (int x = 0; x < X; ++x)
						{
							var kammusu = Database.GetKammusuData(UnitList[x].Name);
							for(int si = 0; si < kammusu.SlotCount; ++si){
								var weapon = Database.GetWeaponData(UnitList[x].PartsList[si].Name);
								if(weapon.IsStage1 && kammusu.Airs[si] > 0)
								{
									slotList.Add(new KeyValuePair<WeaponData, int>(weapon, kammusu.Airs[si]));
								}
							}
						}
						var resultSt1 = Simulator.MonteCarloTestWithSt1(enemyParamList, slotList, 10000);
						string message2 = "テスト結果：\n";
						var list = new List<string> { "確保", "優勢", "均衡", "劣勢", "喪失" };
						for (int ei = 0; ei < resultSt1.Key.Count; ++ei)
						{
							message2 += $"　{ei + 1}回目→制空{enemyParamList[ei].Key}・{list[enemyParamList[ei].Value]}　{Math.Round(100.0 * resultSt1.Key[ei], 1)}％\n";
						}
						message2 += "\n最適化を終了しますか？";
						if (MessageBox.Show(message2, "CarrierSlideRuler", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
						{
							break;
						}
						else
						{
							Title = "CarrierSlideRuler(最適化中...)";
							wantAaPower = Simulator.CalcAAV(slotList) * Math.Max(resultSt1.Value, 1.01);
							wantAaPower *= addAirPowerPer[AddAirPowerPerIndex];
						}
					}
					else
					{
						MessageBox.Show("実行可能解が出せませんでした。", "CarrierSlideRuler", MessageBoxButton.OK, MessageBoxImage.Warning);
						break;
					}
				}
			}
			OptimizeButtonState = true;
		}

		// 「追加」「削除」ボタン
		public ICommand AddEnemyParamCommand { get; }
		public ICommand DeleteEnemyParamCommand { get; }
		private void AddEnemyParamAction(){
			enemyParamList.Add(new KeyValuePair<int, int>(EnemyAirPowerTemp, AirStatusModeTemp));
			var list = new List<string> { "確保","優勢","均衡","劣勢","喪失" };
			EnemyParamViewList.Add($"{EnemyAirPowerTemp}-{list[AirStatusModeTemp]}");
			SetTitleBar();
		}
		private void DeleteEnemyParamAction(){
			if (EnemyParamIndex < 0)
				return;
			enemyParamList.RemoveAt(EnemyParamIndex);
			EnemyParamViewList.RemoveAt(EnemyParamIndex);
			SetTitleBar();
		}

		// 「制空状況をテスト」ボタン
		public ICommand AirStatusTestCommand { get; }
		private void AirStatusTest() {
			if (enemyParamList.Count == 0)
				return;
			//制空に参加するスロットの一覧表を出す(制空値・搭載数)
			int X = Constant.MaxKammusuCount;
			var slotList = new List<KeyValuePair<WeaponData, int>>();
			for (int x = 0; x < X; ++x) {
				var kammusu = Database.GetKammusuData(UnitList[x].Name);
				for (int si = 0; si < kammusu.SlotCount; ++si) {
					var weapon = Database.GetWeaponData(UnitList[x].PartsList[si].Name);
					if (weapon.IsStage1 && kammusu.Airs[si] > 0) {
						slotList.Add(new KeyValuePair<WeaponData, int>(weapon, kammusu.Airs[si]));
					}
				}
			}
			var resultSt1 = Simulator.MonteCarloTestWithSt1(enemyParamList, slotList, 10000);
			string message = "テスト結果：\n";
			var list = new List<string> { "確保", "優勢", "均衡", "劣勢", "喪失" };
			for (int ei = 0; ei < resultSt1.Key.Count; ++ei) {
				message += $"　{ei + 1}回目→制空{enemyParamList[ei].Key}・{list[enemyParamList[ei].Value]}　{Math.Round(100.0 * resultSt1.Key[ei], 1)}％\n";
			}
			MessageBox.Show(message, "CarrierSlideRuler",MessageBoxButton.OK, MessageBoxImage.Information);
		}

		// コンストラクタ
		public MainViewModel() {
			// ボタンにCommandを設定する
			SetWeaponCommand = new CommandBase(SetWeaponAction);
			OptimizeCommand = new CommandBase(OptimizeAction);
			AddEnemyParamCommand = new CommandBase(AddEnemyParamAction);
			DeleteEnemyParamCommand = new CommandBase(DeleteEnemyParamAction);
			AirStatusTestCommand = new CommandBase(AirStatusTest);
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
			AntiFieldType = 0;
			OptimizeButtonState = true;
			SaiunCheck = true;
			MinSlotCheck = true;
			TimeLimitType = 2;
		}
	}
}
