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
			public string Name { get; set; }
			public int Count { get; set; }
			public List<string> CountList { get; set; }
		}
		public List<HaveWeapon> HaveWeaponList { get; set; }

		public WeaponListViewModel() {
			HaveWeaponList = new List<HaveWeapon>();
			foreach(string name in Database.WeaponNameList) {
				var hw = new HaveWeapon();
				hw.Name = name;
				hw.Count = 0;
				hw.CountList = Enumerable.Range(0, 100).Select(p => p.ToString()).ToList();
				HaveWeaponList.Add(hw);
			}
		}
	}
}
