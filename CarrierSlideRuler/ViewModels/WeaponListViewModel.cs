using CarrierSlideRuler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrierSlideRuler.ViewModels {
	class WeaponListViewModel : ViewModelBase {
		// 装備所持数についての情報
		public class HaveWeapon {
			Action act;
			public string Name { get; set; }
			int count;
			public int Count { get => count; set { count = value; Database.SetHaveWeaponCount(Name, count); act(); } }
			public List<string> CountList { get; set; }

			public HaveWeapon(Action act_) {
				act = act_;
			}
		}
		public List<HaveWeapon> HaveWeaponList { get; set; }

		// 装備所持数を保存
		public void SaveHaveWeaponData() {
			using (var sw = new System.IO.StreamWriter(@"have_weapon.csv")) {
				foreach (var data in HaveWeaponList) {
					int id = Database.GetWeaponData(data.Name).Id;
					int count = data.Count;
					sw.Write($"{id},{count}\n");
				}
			}
		}

		public WeaponListViewModel() {
			// 装備所持数を初期化
			// (hasWeaponTableも参照する)
			HaveWeaponList = new List<HaveWeapon>();
			foreach(string name in Database.WeaponNameList) {
				// IDと所持数を取得
				var weapon = Database.GetWeaponData(name);
				int id = weapon.Id;
				// 追加
				var hw = new HaveWeapon(SaveHaveWeaponData);
				hw.Name = name;
				hw.Count = Database.GetHaveWeaponCount(id);
				hw.CountList = Enumerable.Range(0, 100).Select(p => p.ToString()).ToList();
				HaveWeaponList.Add(hw);
			}
		}
	}
}
