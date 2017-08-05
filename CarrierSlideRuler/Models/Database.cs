using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CarrierSlideRuler.Models {
	// データベース
	using KammusuTable = Dictionary<string, KammusuData>;
	using WeaponTable = Dictionary<string, WeaponData>;
	static class Database {
		static KammusuTable kammusuDictionary;
		public static void Initialize() {
			kammusuDictionary = new KammusuTable();
			using (var sr = new System.IO.StreamReader(@"kammusu.csv")) {
				while (!sr.EndOfStream) {
					// 1行を読み込む
					string line = sr.ReadLine();
					// マッチさせてから各数値を取り出す
					string pattern = @"(?<Number>\d+),(?<Name>[^,]+),(?<Type>[^,]+),(?<Attack>\d+),(?<SlotCount>\d+),(?<Airs1>\d+),(?<Airs2>\d+),(?<Airs3>\d+),(?<Airs4>\d+),(?<HasPF>○|×),(?<HasPA>○|×),(?<HasPB>○|×),(?<HasJPB>○|×),(?<HasWF>○|×),(?<HasWB>○|×),(?<HasPS>○|×),(?<HasPSK>○|×),(?<HasAS>○|×)";
					var match = Regex.Match(line, pattern);
					if (!match.Success) {
						continue;
					}
					// 取り出した数値をKammusuDataに変換する
					{
						try {
							int id = int.Parse(match.Groups["Number"].Value);
							string name = match.Groups["Name"].Value;
							FleetType type = Constant.ParseFleetType(match.Groups["Type"].Value);
							int attack = int.Parse(match.Groups["Attack"].Value);
							int slotCount = int.Parse(match.Groups["SlotCount"].Value);
							int airs1 = int.Parse(match.Groups["Airs1"].Value);
							int airs2 = int.Parse(match.Groups["Airs2"].Value);
							int airs3 = int.Parse(match.Groups["Airs3"].Value);
							int airs4 = int.Parse(match.Groups["Airs4"].Value);
							bool hasPF = (match.Groups["HasPF"].Value == "○");
							bool hasPA = (match.Groups["HasPA"].Value == "○");
							bool hasPB = (match.Groups["HasPB"].Value == "○");
							bool hasJPB = (match.Groups["HasJPB"].Value == "○");
							bool hasWF = (match.Groups["HasWF"].Value == "○");
							bool hasWB = (match.Groups["HasWB"].Value == "○");
							bool hasPS = (match.Groups["HasPS"].Value == "○");
							bool hasPSK = (match.Groups["HasPSK"].Value == "○");
							bool hasAS = (match.Groups["HasAS"].Value == "○");
							var kammusu = new KammusuData(id, name, type, attack, slotCount, new int[] { airs1, airs2, airs3, airs4 },
								hasPF, hasPA, hasPB, hasJPB, hasWF, hasWB, hasPS, hasPSK, hasAS);
							kammusuDictionary[name] = kammusu;
						}
						catch {
							continue;
						}
					}
				}
				return;
			}
		}
		public static List<string> KammusuNameList {
			get {
				return kammusuDictionary.Keys.ToList();
			}
		}
	}
	// 艦娘データの内部表現
	class KammusuData {
		// フィールド
		// ID,艦名,艦種,火力,スロ数,搭載数,
		// 艦戦？,艦攻？,艦爆？,噴式？,水戦？,水爆？,艦偵？,景雲？,整備員？
		public int Id { get; }
		public string Name { get; }
		public FleetType Type { get; }
		public int Attack { get; }
		public int SlotCount { get; }
		public int[] Airs { get; }
		public bool HasPF { get; }
		public bool HasPA { get; }
		public bool HasPB { get; }
		public bool HasJPB { get; }
		public bool HasWF { get; }
		public bool HasWB { get; }
		public bool HasPS { get; }
		public bool HasPSK { get; }
		public bool HasAS { get; }
		// コンストラクタ
		public KammusuData() {
			Id = 0;
			Name = "なし";
			Type = FleetType.None;
			Attack = 0;
			SlotCount = 0;
			Airs = new int[] { 0, 0, 0, 0 };
			HasPF = false;
			HasPA = false;
			HasPB = false;
			HasJPB = false;
			HasWF = false;
			HasWB = false;
			HasPS = false;
			HasPSK = false;
			HasAS = false;
		}
		public KammusuData(int id, string name, FleetType type, int attack, int slotCount, int[] airs,
			bool hasPF, bool hasPA, bool hasPB, bool hasJPB, bool hasWF, bool hasWB, bool hasPS, bool hasPSK, bool hasAS) {
			Id = id;
			Name = name;
			Type = type;
			Attack = attack;
			SlotCount = slotCount;
			Airs = airs;
			HasPF = hasPF;
			HasPA = hasPA;
			HasPB = hasPB;
			HasJPB = hasJPB;
			HasWF = hasWF;
			HasWB = hasWB;
			HasPS = hasPS;
			HasPSK = hasPSK;
			HasAS = hasAS;
		}
	}
	// 装備データの内部表現
	class WeaponData {
		// フィールド
		// ID,装備名,装備種,火力,雷装,爆装,対空,命中,回避
		public int Id { get; }
		public string Name { get; }
		public WeaponType Type { get; }
		public int Attack { get; }
		public int Torpedo { get; }
		public int Bomb { get; }
		public int AntiAir { get; }
		public int Hit { get; }
		public int Evade { get; }
		// コンストラクタ
		public WeaponData() {
			Id = 0;
			Name = "なし";
			Type = WeaponType.None;
			Attack = 0;
			Torpedo = 0;
			Bomb = 0;
			AntiAir = 0;
			Hit = 0;
			Evade = 0;
		}
		public WeaponData(int id, string name, WeaponType type,
			int attack, int torpedo, int bomb, int antiAir, int hit, int evade) {
			Id = id;
			Name = name;
			Type = type;
			Attack = attack;
			Torpedo = torpedo;
			Bomb = bomb;
			AntiAir = antiAir;
			Hit = hit;
			Evade = evade;
		}
	}
}
