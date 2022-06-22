using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Linq;
using System . Runtime . CompilerServices;
using System . Runtime . Remoting . Channels;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Input;

namespace MyDev . ViewModels
{
	public abstract class xBaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
            //public void OnPropertyChanged ( string propertyName ) => PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) );
            protected void OnPropertyChanged( string propertyName )
            {
                  var handler = PropertyChanged;
                  if ( handler != null )
                        handler ( this , new PropertyChangedEventArgs ( propertyName ) );
            }
            public class DelegateCommand : ICommand
            {
                  private readonly Action _action;

                  public DelegateCommand ( Action action )
                  {
                        _action = action;
                  }

                  public void Execute ( object parameter )
                  {
                        _action ( );
                  }

                  public bool CanExecute ( object parameter )
                  {
                        return true;
                  }

#pragma warning disable 67
                  public event EventHandler CanExecuteChanged;
#pragma warning restore 67
            }
            //protected bool SetProperty<T> ( ref T field , T newValue , [CallerMemberName] string propertyName = null )
            //{
            //      if ( !EqualityComparer<T> . Default . Equals ( field , newValue ) )
            //      {
            //            field = newValue;
            //            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) );
            //            return true;
            //      }
            //      return false;
            //}

      }
}
