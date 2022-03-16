using MyDev . Models;
using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Diagnostics;
using System . Runtime . InteropServices . WindowsRuntime;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;

namespace MyDev . Views
{
	/// <summary>
	/// Interaction logic for GenericMvvm.xaml
	/// This Hosts a number of different Classes, so it has complex  DataContexts
	///  that cause various problems in the view design., 
	///  especially Stack Overflow when using nested DataContexts
	/// </summary>
	public partial class GenericMvvmWindow : Window
	{
		// Host window for various MVVM tests using seperate Grids each
		// with seperate DataContextts that causes various problems
		public  static  ObservableCollection<Person>  People;
		public static PersonViewModel pvm;
		public static Person person;
		//public static MvvmGenericModel mgm;
		public static ListBox lb;
		public GenericMvvmWindow ( )
		{
			InitializeComponent ( );
			this . DataContext = this;
			RefreshListbox = new RelayCommand ( ExecuteRefreshListbox , CanRefreshListbox );
			Lbdata . SelectionChanged += Lbdata_SelectionChanged;
			PersonsList . SelectionChanged += PersonsList_SelectionChanged;
		}
		private void Genmvvm_Loaded ( object sender , RoutedEventArgs e )
		{
			// Allows other classes to access our listbox
			lb = Lbdata;
			People = PersonViewModel . People;
			// Get a  valid pointer to other classeswe may need to access
			pvm = Resources [ "PersonViewmodel" ] as PersonViewModel;
			//mgm = Resources [ "MvvmGenModel"] as MvvmGenericModel ;
			person = Resources [ "person" ] as Person;
		}

		#region ICommands	REFRESHLISTBOX
		public ICommand RefreshListbox { get; set; }
		private bool CanRefreshListbox ( object arg )
		{					    
			return true;
		}
		private void ExecuteRefreshListbox ( object obj )
		{
			Lbdata . Refresh ( );
			Lbdata . SelectedIndex = 1;
			Lbdata . SelectedItem = 1;
		}
		#endregion ICommands REFRESHLISTBOX

	
		public static ListBox GetGenericMvvm (  )
		{
			return lb;
		}
		#region Listbox handlers
		private void Lbdata_SelectionChanged ( object sender , System . Windows . Controls . SelectionChangedEventArgs e )
		{
			Lbdata . Refresh ( );
		}
		private void Lbdata_SourceUpdated ( object sender , System . Windows . Data . DataTransferEventArgs e )
		{
			Lbdata . Refresh ( );
		}
		#endregion Listbox handlers

		private void PersonsList_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			//Person pvm = sender as	Person;
			// Gets the Person record
			var person = e . AddedItems [ 0 ] as Person;
//			PersonViewModel pvm  = new PersonViewModel();
			foreach ( var item in People )
			{
				if ( item . Name == person . Name && item . Address == person . Address )
				{
					pvm. Selecteditem = item;
					person . SelectedPerson = item;
					break;
				}
			}
		}
	}
}
