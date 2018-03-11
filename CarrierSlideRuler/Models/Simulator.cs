using MersenneTwister;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarrierSlideRuler.Models
{
	static class Simulator
	{
		// 各敵編成について、指定した制空状況を達成できた割合を取得する
		public static KeyValuePair<List<double>, double> MonteCarloTestWithSt1(
			List<KeyValuePair<int,int>> enemyParamList,
			List<KeyValuePair<WeaponData, int>> slotList,
			int loopCount)
		{
			// 初期化
			var output = new List<double>();
			foreach(var pair in enemyParamList)
			{
				output.Add(0.0);
			}
			var averageAavCount = new List<double>();
			foreach (var pair in enemyParamList)
			{
				averageAavCount.Add(0.0);
			}
			// モンテカルロ・シミュレーションを行う
			for (int li = 0; li < loopCount; ++li) {
				// 初期搭載数を初期化する
				var slotListNow = new List<KeyValuePair<WeaponData, int>>(slotList);
				// 各制空値によってSt1撃墜を行う
				for(int ei = 0; ei < enemyParamList.Count; ++ei)
				{
					// 自制空値を計算する
					int aav = CalcAAV(slotListNow);
					averageAavCount[ei] += aav;
					// 制空状況を判断する
					int status = GetAirStatus(aav, enemyParamList[ei].Key);
					// 制空条件を満たしていれば加算する
					if(enemyParamList[ei].Value>= status)
					{
						output[ei] += 1.0;
					}
					// St1撃墜を行う
					CalcSt1(ref slotListNow, status);
				}
			}
			// シミュレーション回数で割る
			for (int ei = 0; ei < output.Count; ++ei)
			{
				output[ei] /= loopCount;
				averageAavCount[ei] /= loopCount;
			}
			// もしシミュレーションをやり直す際、次にどれぐらい制空値を上げればいいか
			// ＝一番成績が悪かった敵編成について、目標とする制空条件を満たすために
			// 　どれほど制空値を引き上げればいいかの目安を算出する
			int minOutputIndex = output.IndexOf(output.Min());
			double averageAavCountWhenminOutputIndex = averageAavCount[minOutputIndex];
			var statusCoeff = new List<double> { 3.0, 1.5, 2.0 / 3.0, 1.0 / 3.0, 0.0 };
			double rate = enemyParamList[minOutputIndex].Key * statusCoeff[enemyParamList[minOutputIndex].Value] / averageAavCountWhenminOutputIndex;
			return new KeyValuePair<List<double>, double>(output, rate);
		}

		// 搭載数から制空値を計算する
		public static int CalcAAV(List<KeyValuePair<WeaponData, int>> slotList)
		{
			int aav = 0;
			foreach(var pair in slotList)
			{
				var weapon = pair.Key;
				int slot = pair.Value;
				if(slot > 0)
				{
					aav += (int)(Math.Sqrt(slot) * weapon.AntiAir + (Math.Sqrt(100 / 10) + Constant.AntiAirBonus(weapon.Type)));
				}
			}
			return aav;
		}

		// 制空値から制空状況を判断する
		private static int GetAirStatus(int friendAAV, int enemyAAV)
		{
			if (friendAAV >= enemyAAV * 3) return 0;
			if (friendAAV * 2 >= enemyAAV * 3) return 1;
			if (friendAAV * 3 > enemyAAV * 2) return 2;
			if (friendAAV * 3 > enemyAAV) return 3;
			return 4;
		}

		// St1撃墜を行う
		private static List<int> MinSt1Per = new List<int> { 7, 20, 30, 45, 65 };
		private static List<int> MaxSt1Per = new List<int> { 15, 45, 75, 105, 150 };
		private static Random random = DsfmtRandom.Create();
		private static void CalcSt1(ref List<KeyValuePair<WeaponData, int>> slotList, int status)
		{
			for(int si = 0; si < slotList.Count; ++si)
			{
				int newSlot = slotList[si].Value - random.Next(MinSt1Per[status], MaxSt1Per[status]) * slotList[si].Value / 256;
				slotList[si] = new KeyValuePair<WeaponData, int>(slotList[si].Key, newSlot);
			}
		}
	}
}
