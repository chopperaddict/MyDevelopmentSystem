using MyDev . Models;
using MyDev . Views;

using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Linq;
using System . Net;
using System . Net . Http . Headers;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;
using System . Xml . Linq;

namespace MyDev . ViewModels
{
    public class PersonViewModel
	{
		#region OnPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged ( string propertyName )
		{
			if ( PropertyChanged != null )
			{
				PropertyChanged ( this , new PropertyChangedEventArgs ( propertyName ) );
			}
		}
		#endregion OnPropertyChanged
		//..Viewmodel for just one (of four) grid sections in the GENERICMVVM.window
		private static Person selectedItem;
//		private IList<Person> _personList;
		private static ObservableCollection<Person> people = new ObservableCollection<Person>();

		#region ICommands UPDATENAME, UPDATEADDRESS
		private ICommand mUpdater { get; set; }
		public ICommand UpdateName { get; set; }
		public ICommand UpdateAddress { get; set; }
		private bool CanUpdateAddress ( object arg )
		{
			return true;
		}
		private bool CanUpdateName ( object arg )
		{
			return true;
		}
		private void ExecuteUpdateName ( object obj )
		{
			var  str = obj .ToString ( );

			var f = this. Selecteditem;
			if ( str != null )
			{
				var pcollection = GenericMvvmWindow.People;
				Person person = this.Selecteditem;
				if ( person != null )
					person . Name = str;
				else
					MessageBox . Show ( "Please select an item in the listbox above before trying to modify the Name !!!");
			}
		}
		private void ExecuteUpdateAddress ( object obj )
		{
			if ( obj . ToString ( ) != "" )
			{
				if( Selecteditem  != null)
					Selecteditem . Address = obj . ToString ( );
				else
					MessageBox . Show ( "Please select an item in the listbox above before trying to modify the Address !!!" );
			}
		}
		#endregion ICommands UPDATENAME, UPDATEADDRESS
		public PersonViewModel ( )
		{

			//_personList = new List<Person> ( )
			//  {
			//	new Person(){Name="Prabhat", Address="Bangalore"},
			//	ne
			//	if(People.w Person(){Name="John",Address="Delhi"}
			//  };
			if ( People . Count == 0 )
			{
				People . Add ( new Person { Name = "Prabhat" , Address = "Bangalore" } );
				People . Add ( new Person { Name = "John" , Address = "Delhi" } );
			}
			UpdateName = new RelayCommand ( ExecuteUpdateName , CanUpdateName );
			UpdateAddress = new RelayCommand ( ExecuteUpdateAddress , CanUpdateAddress );
			// Get main windows pointer to PersonViewModel (the active structure)
			var pvm = GenericMvvmWindow.pvm;   		
		}
		public static ObservableCollection<Person> GetPeopleList()
		{
			return People;
		}
		#region Properties
		//public IList<Person> Persons
		//{
		//	get { return _personList; }
		//	set { _personList = value; }
		//}
		public static ObservableCollection<Person> People 
		{
			get { return people; }
			set { people = value; }// NotifyPropertyChanged ( People . ToString ( ) ); }
		}
		
		public Person Selecteditem
		{
			get { return selectedItem; }
			set { selectedItem = value; NotifyPropertyChanged ( Selecteditem . ToString ( )); }
		}		
		public ObservableCollection<Person> MyProperty
		{
			get { return people; }
			set { people = value; }
		}

		#endregion Properties

		public ICommand UpdateCommand
		{
			get
			{
				if ( mUpdater == null )
					mUpdater = new Updater ( );
				return mUpdater;
			}
			set
			{
				mUpdater = value;
			}
		}
	}
}