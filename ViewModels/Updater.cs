
using System;
using System . Windows . Input;

namespace MyDev . ViewModels
{
    class Updater : ICommand
	{
		#region ICommand Members  

		public bool CanExecute ( object parameter )
		{
			return true;
		}
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute ( object parameter )
		{
			// This executes the command somehow
		}
		#endregion
	}
}