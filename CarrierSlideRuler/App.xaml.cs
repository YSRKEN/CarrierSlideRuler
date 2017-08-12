using CarrierSlideRuler.Models;
using CarrierSlideRuler.ViewModels;
using CarrierSlideRuler.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CarrierSlideRuler {
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application {
		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);
			// データベースを初期化する
			Database.Initialize();
			// メイン画面を作成して表示する
			var mv = new MainView();
			var mvm = new MainViewModel();
			mv.DataContext = mvm;
			mv.Show();
		}
	}
}
