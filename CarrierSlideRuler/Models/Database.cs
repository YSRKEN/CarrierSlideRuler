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
	using HaveWeaponTable = Dictionary<int, int>;
	static class Database {
		static KammusuTable kammusuDictionary;
		static WeaponTable weaponDictionary;
		static HaveWeaponTable haveWeaponDictionary;
		// 初期化
		public static void Initialize() {
			#region 艦娘データを読み込む
			kammusuDictionary = new KammusuTable();
			using (var sr = new System.IO.StreamReader(@"kammusu.csv")) {
				while (!sr.EndOfStream) {
					// 1行を読み込む
					string line = sr.ReadLine();
					// マッチさせてから各数値を取り出す
					string pattern = @"^(?<Number>\d+),(?<Name>[^,]+),(?<Type>[^,]+),(?<Attack>\d+),(?<SlotCount>\d+),(?<Airs1>\d+),(?<Airs2>\d+),(?<Airs3>\d+),(?<Airs4>\d+),(?<HasPF>○|×),(?<HasPA>○|×),(?<HasPB>○|×),(?<HasJPB>○|×),(?<HasWF>○|×),(?<HasWB>○|×),(?<HasPS>○|×),(?<HasPSK>○|×),(?<HasAS>○|×)";
					var match = Regex.Match(line, pattern);
					if (!match.Success) {
						continue;
					}
					// 取り出した数値をKammusuDataに変換し、kammusuDictionaryに代入する
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
			}
			#endregion
			#region 装備データを読み込む
			weaponDictionary = new WeaponTable();
			using (var sr = new System.IO.StreamReader(@"weapon.csv")) {
				while (!sr.EndOfStream) {
					// 1行を読み込む
					string line = sr.ReadLine();
					// マッチさせてから各数値を取り出す
					string pattern = @"^(?<Number>\d+),(?<Name>[^,]+),(?<Type>[^,]+),(?<Attack>\d+),(?<Torpedo>\d+),(?<Bomb>\d+),(?<AntiAir>\d+),(?<Hit>\d+),(?<Evade>\d+)";
					var match = Regex.Match(line, pattern);
					if (!match.Success) {
						continue;
					}
					// 取り出した数値をWeaponDataに変換し、weaponDictionaryに代入する
					{
						try {
							int id = int.Parse(match.Groups["Number"].Value);
							string name = match.Groups["Name"].Value;
							WeaponType type = Constant.ParseWeaponType(match.Groups["Type"].Value);
							int attack = int.Parse(match.Groups["Attack"].Value);
							int torpedo = int.Parse(match.Groups["Torpedo"].Value);
							int bomb = int.Parse(match.Groups["Bomb"].Value);
							int antiair = int.Parse(match.Groups["AntiAir"].Value);
							int hit = int.Parse(match.Groups["Hit"].Value);
							int evade = int.Parse(match.Groups["Evade"].Value);
							var weapon = new WeaponData(id, name, type, attack, torpedo, bomb, antiair, hit, evade);
							weaponDictionary[name] = weapon;
						}
						catch {
							continue;
						}
					}
				}
			}
			#endregion
			#region 所持装備データを読み込む
			haveWeaponDictionary = new HaveWeaponTable();
			// ファイル(has_weapon.csv)が存在する場合はそちらから読み取る
			if (System.IO.File.Exists(@"have_weapon.csv")) {
				using (var sr = new System.IO.StreamReader(@"have_weapon.csv")) {
					while (!sr.EndOfStream) {
						// 1行を読み込む
						string line = sr.ReadLine();
						// マッチさせてから各数値を取り出す
						string pattern = @"^(?<Number>\d+),(?<Count>\d+)";
						var match = Regex.Match(line, pattern);
						if (!match.Success) {
							continue;
						}
						// 取り出した数値をhasWeaponDictionaryに代入する
						{
							try {
								int id = int.Parse(match.Groups["Number"].Value);
								int count = int.Parse(match.Groups["Count"].Value);
								count = (count >= 100 ? 99 : count < 0 ? 0 : count);
								haveWeaponDictionary[id] = count;
							}
							catch {
								continue;
							}
						}
					}
				}
			}
			// 読み取ってない部分の装備について情報を補完する
			foreach(var pair in weaponDictionary) {
				int id = pair.Value.Id;
				if (!haveWeaponDictionary.ContainsKey(id)) {
					haveWeaponDictionary[id] = 0;
				}
			}
			haveWeaponDictionary[weaponDictionary["なし"].Id] = 99;
			#endregion
		}
		// 艦娘名のリスト
		public static List<string> KammusuNameList {
			get {
				return kammusuDictionary.Keys.ToList();
			}
		}
		// 装備名のリスト
		public static List<string> WeaponNameList {
			get {
				return weaponDictionary.Keys.ToList();
			}
		}
		// 艦娘データ
		public static KammusuData GetKammusuData(string name) {
			return kammusuDictionary[name];
		}
		// 装備データ
		public static WeaponData GetWeaponData(string name) {
			return weaponDictionary[name];
		}
		// 所持可能な装備名リスト
		public static List<string> GetCanHaveList(KammusuData kammusu) {
			// 一覧を初期化
			var list = new List<string>();
			// 線形検索
			foreach (var pair in weaponDictionary) {
				// 装備の種類を取得
				var name = pair.Value.Name;
				var type = pair.Value.Type;
				// 艦娘の状態に合わせ、その装備を装備できるかを判定する
				switch (type) {
				case WeaponType.PF:
					if (kammusu.HasPF) list.Add(name);
					break;
				case WeaponType.PA:
					if (kammusu.HasPA) list.Add(name);
					break;
				case WeaponType.PB:
					if (kammusu.HasPB) list.Add(name);
					break;
				case WeaponType.JPB:
					if (kammusu.HasJPB) list.Add(name);
					break;
				case WeaponType.WF:
					if (kammusu.HasWF) list.Add(name);
					break;
				case WeaponType.WB:
					if (kammusu.HasWB) list.Add(name);
					break;
				case WeaponType.PS:
				case WeaponType.PSS:
					if (kammusu.HasPS) list.Add(name);
					break;
				case WeaponType.PSK:
					if (kammusu.HasPSK) list.Add(name);
					break;
				case WeaponType.AS:
					if (kammusu.HasAS) list.Add(name);
					break;
				case WeaponType.Other:
					list.Add(name);
					break;
				}
			}
			return list;
		}
		// 所持装備データ
		public static int GetHaveWeaponCount(int id) {
			// 所持数データを参照
			if (haveWeaponDictionary.ContainsKey(id)) {
				return haveWeaponDictionary[id];
			}
			else {
				return 0;
			}
		}
		// 艦娘Xが装備Yを持てるか？
		public static bool HasWeaponjudge(string kName, string wName) {
			var kammusu = GetKammusuData(kName);
			var weapon = GetWeaponData(wName);
			switch (weapon.Type) {
			case WeaponType.PF:
				if (kammusu.HasPF) return true;
				break;
			case WeaponType.PA:
				if (kammusu.HasPA) return true;
				break;
			case WeaponType.PB:
				if (kammusu.HasPB) return true;
				break;
			case WeaponType.JPB:
				if (kammusu.HasJPB) return true;
				break;
			case WeaponType.WF:
				if (kammusu.HasWF) return true;
				break;
			case WeaponType.WB:
				if (kammusu.HasWB) return true;
				break;
			case WeaponType.PS:
			case WeaponType.PSS:
				if (kammusu.HasPS) return true;
				break;
			case WeaponType.PSK:
				if (kammusu.HasPSK) return true;
				break;
			case WeaponType.AS:
				if (kammusu.HasAS) return true;
				break;
			case WeaponType.Other:
				return true;
			default:
				return false;
			}
			return false;
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
		// 砲撃戦で空撃するか？
		public bool IsAirGunAttack {
			get => (Type == FleetType.CV || Type == FleetType.CVL || Type == FleetType.ACV || Name == "速吸改");
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
		// Stage1(航空戦)に参加するか？
		public bool IsStage1 {
			get => (Type == WeaponType.PF || Type == WeaponType.PA || Type == WeaponType.PB
				|| Type == WeaponType.JPB || Type == WeaponType.WF || Type == WeaponType.WB);
		}
		// Stage3(航空攻撃)に参加するか？
		public bool IsStage3 {
			get => (Type == WeaponType.PA || Type == WeaponType.PB || Type == WeaponType.JPB || Type == WeaponType.WB);
		}
	}
}
