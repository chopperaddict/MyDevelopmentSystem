using MyDev . Views;

using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Windows . Controls;
using System . Windows . Input;

namespace MyDev . ViewModels
{
	public class GenListboxClass : BaseViewModel
	{
//		private GenericMvvmWindow gvm;
		private bool Toggle = true;

		public GenListboxClass ( )
		{
			ReloadCommand = new RelayCommand ( ExecuteReload , CanExecuteReload );
			LbDataCollection = new ObservableCollection<string> ( );
			LoadListbox ( );
		}

		#region Properties
		private string  entry;
		public string Entry
		{
			get { return entry; }
			set { entry = value; }
		}
		private List<string> lbData;
		private ObservableCollection<string> lbDataCollection { get; set; }

		public List<string> LbData
		{
			get { return lbData; }
			set { lbData = value; OnPropertyChanged ( LbData . ToString ( ) ); }
		}
		public ObservableCollection<string> LbDataCollection
		{
			get { return lbDataCollection; }
			set { lbDataCollection = value; OnPropertyChanged ( LbDataCollection . ToString ( ) ); }
		}
		#endregion Properties

		#region ICommand RELOADCOMMAND
		public ICommand ReloadCommand { get; set; }
		private bool CanExecuteReload ( object arg )
		{
			return true;
		}
		public void ExecuteReload ( object obj )
		{
			//LbData . Clear ( );
			if ( Toggle )
				LbDataCollection = GetBankData2 ( );
			else
				LbDataCollection = GetBankData ( );
			Toggle = !Toggle;
			ListBox lb = GenericMvvmWindow.GetGenericMvvm (  );
			if ( lb != null )
			{
				lb . SelectedIndex = 0;
				lb . SelectedItem = 0;
				lb . Refresh ( );
				lb . ScrollIntoView ( lb . SelectedItem );
			}

		}
		#endregion ICommands RELOADCOMMAND


		#region Class Helper Methods
		public ObservableCollection<string> LoadListbox ( )
		{
			return LbDataCollection = GetBankData ( );
		}
		private ObservableCollection<string> GetBankData ( )
		{
			LbDataCollection . Clear ( );
			List<string>  data = new List<string>();
			data . Add ( "aaaaaaaaaaaaa" );
			data . Add ( "bbbbbbbbbbb" );
			data . Add ( "ccccccccccccccc" );
			data . Add ( "dddddddddd" );
			data . Add ( "eeeeeeeeeeee" );
			data . Add ( "ffffffffffffffffffffffffff" );
			foreach ( var item in data )
			{
				LbDataCollection . Add ( item );
			}
			return LbDataCollection;
		}
		private ObservableCollection<string> GetBankData2 ( )
		{
			LbDataCollection . Clear ( );
			List<string>  data = new List<string>();
			data . Add ( "1111111111111" );
			data . Add ( "222222222" );
			data . Add ( "333333333333333" );
			data . Add ( "4444444444" );
			data . Add ( "5555555555" );
			data . Add ( "66666666666666666666666" );
			foreach ( var item in data )
			{
				LbDataCollection . Add ( item );
			}
			return LbDataCollection;
		}
		#endregion Class Helper Methods

	}
}
