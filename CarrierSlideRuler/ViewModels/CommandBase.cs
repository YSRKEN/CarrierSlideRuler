using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarrierSlideRuler.ViewModels {
	public class CommandBase : ICommand {
		// デリゲートを保持するためのフィールド
		Action action;
		// ICommandを継承したことで生じるプロパティ
		public bool CanExecute(object parameter) => true;
		public event EventHandler CanExecuteChanged;
		// デリゲートを実行するメソッド
		public void Execute(object parameter) { action(); }
		// コンストラクタ
		public CommandBase(Action action) { this.action = action; }
	}
}
