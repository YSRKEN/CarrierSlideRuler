using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrierSlideRuler.Models {
	using FleetTypeTable = Dictionary<string, FleetType>;
	using WeaponTypeTable = Dictionary<string, WeaponType>;
	// 艦種データ
	enum FleetType { None, CV, ACV, CVL, CA, CAV, CC, BB, BBV, AV, SSV, Other }
	// 装備種データ
	enum WeaponType { None, PF, PA, PB, JPB, WF, WB, PS, PSK, PSS, AS, Other }
	// その他定数
	static class Constant {
		// 艦種データと文字列との対応ハッシュ
		public static FleetTypeTable fleetTypeTable = new FleetTypeTable() {
			{"正規空母", FleetType.CV},
			{"装甲空母", FleetType.ACV},
			{"軽空母", FleetType.CVL},
			{"重巡洋艦", FleetType.CA},
			{"航空巡洋艦", FleetType.CAV},
			{"高速戦艦", FleetType.CC},
			{"低速戦艦", FleetType.BB},
			{"航空戦艦", FleetType.BBV},
			{"水上機母艦", FleetType.AV},
			{"潜水空母", FleetType.SSV},
			{"その他", FleetType.Other},
		};
		// 文字列を艦種データに変換
		public static FleetType ParseFleetType(string str) {
			if (fleetTypeTable.ContainsKey(str))
				return fleetTypeTable[str];
			throw new FormatException();
		}
		// 装備種データと文字列との対応ハッシュ
		public static WeaponTypeTable weaponTypeTable = new WeaponTypeTable() {
			{"艦上戦闘機", WeaponType.PF}, //Plane Fighter
			{"艦上攻撃機", WeaponType.PA}, //Plane Attacker
			{"艦上爆撃機", WeaponType.PB}, //Plane Bomber
			{"噴式戦闘爆撃機", WeaponType.JPB}, //Jet Plane Bomber
			{"水上戦闘機", WeaponType.WF}, //Water Fighter
			{"水上爆撃機", WeaponType.WB}, //Water Bomber
			{"艦上偵察機", WeaponType.PS}, //Plane Searcher
			{"艦上偵察機(景雲)", WeaponType.PSK}, //Plane Searcher(Keiun)
			{"艦上偵察機(彩雲)", WeaponType.PSS}, //Plane Searcher(Saiun)
			{"航空要員", WeaponType.AS}, //Air Staff
			{"その他", WeaponType.Other},
		};
		// 文字列を 装備種データに変換
		public static WeaponType ParseWeaponType(string str) {
			if (weaponTypeTable.ContainsKey(str))
				return weaponTypeTable[str];
			throw new FormatException();
		}
		// 艦載機熟練度による制空ボーナス
		public static int AntiAirBonus(WeaponType wt) {
			switch (wt) {
			case WeaponType.PF:
			case WeaponType.WF: return 22;
			case WeaponType.WB: return 6;
			case WeaponType.PA:
			case WeaponType.PB:
			case WeaponType.JPB:
			default: return 0;
			}
		}
	}
}
