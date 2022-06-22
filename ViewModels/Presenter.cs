using MyDev . Commands;
using MyDev . Views;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Windows . Controls;
using System . Windows . Input;



namespace MyDev . ViewModels
{
	public class Presenter
	{
		#region NotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged ( string propertyName )
		{
			if ( PropertyChanged != null )
			{
				PropertyChanged ( this , new PropertyChangedEventArgs ( propertyName ) );
			}
		}
		#endregion NotifyPropertyChanged
		private readonly TextConverter _textConverter    = new TextConverter(s => s.ToUpper());
		private readonly ObservableCollection<string> _history  = new ObservableCollection<string>();

		private string _someText = "This is just a test of TextConvert ";
		public string SomeText
		{
			get { return _someText; }
			set
			{
				_someText = value;
				NotifyPropertyChanged ( SomeText );
			}
		}
		public IEnumerable<string> History
		{
			get { return _history; }
		}

	#region  ICommand ConvertText
		public ICommand ConvertTextCommand
		{
			get { return new DelegateCommand ( ConvertText ); }
		}
		private void ConvertText ( )
		{
			if ( string . IsNullOrWhiteSpace ( SomeText ) )
				return;
			AddToHistory ( _textConverter . ConvertText ( SomeText ) );
			SomeText = string . Empty;
		}
		#endregion  ICommand ConvertText


		private void AddToHistory ( string item )
		{
			if ( !_history . Contains ( item ) )
				_history . Add ( item );
		}
	}

}
