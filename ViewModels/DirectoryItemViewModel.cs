using MyDev . Views;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Linq;
using System . Runtime . CompilerServices;
using System . Security . Policy;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Input;

namespace MyDev . ViewModels
{
	//
	public class DirectoryItemViewModel 
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

		//		public event PropertyChangedEventHandler PropertyChanged;
		//private void OnPropertyChanged ( string propertyName )
		//{
		//	if ( Flags . SqlBankActive == false )
		//		//				this . VerifyPropertyName ( propertyName );

		//		if ( this . PropertyChanged != null )
		//		{
		//			var e = new PropertyChangedEventArgs ( propertyName );
		//			this . PropertyChanged ( this , e );
		//		}
		//}


		#region Public properties
		//The type of this item
		private DirectoryItemType type;
		public DirectoryItemType Type
		{
			get { return type; }
			set { type = value; 
				//PropertyChanged( this,new PropertyChangedEventArgs(  Type . ToString ( )));
			}
		}
		// Full path to the item
		private string fullpath;
		public string FullPath
		{
			get { return fullpath; }
			set { fullpath = value; } //OnPropertyChanged (FullPath); }
		}
		// Name of this directory item
		private string name;
		public string Name
		{
			get { return this . Type == DirectoryItemType . Drive ? this . FullPath : DirectoryStructure . GetFileFolderName ( this . FullPath ); }
			set { name = value; }//OnPropertyChanged ( Name . ToString ( )); }
		}

		// List of all children inside this current item
		public ObservableCollection<DirectoryItemViewModel> Children { get; set; }
		// Can it expand or not ?
		// Not if it is a file item
		private bool canExpand;
		public bool CanExpand
		{
			get { return this . Type != DirectoryItemType . File; }
			set { canExpand = value; } //OnPropertyChanged ( CanExpand . ToString (  ) ); }
		}
		public bool IsExpanded
		{
			get
			{
				return this . Children?.Count ( f => f != null ) > 0;
			}
			set
			{
				if ( value == false)
				{
					this . ClearChildren ( );       // Clear list, add dummy item as required
					//OnPropertyChanged ( IsExpanded . ToString ( ));
					//IsExpanded = true;
				}
				else
				{
					Expand ( );        // Find all children
					//OnPropertyChanged ( IsExpanded . ToString ( ));
					//IsExpanded = false;
				};
			}
		}

		#endregion properties

		#region public commands
		// command to expannd current item
		public ICommand ExpandCommand { get; set; }

		#endregion public commands


		public DirectoryItemViewModel ( string FullPath , DirectoryItemType type )
		{
			//this . ExpandCommand = new RelayCommand ( Expand );
			//this . FullPath = FullPath;
			//this . Type = type;
		}

		#region Helper Methods
		private void ClearChildren ( )
		{
			this . Children = new ObservableCollection<DirectoryItemViewModel> ( );
			// Show expand arrow if not a file
			if ( this . Type != DirectoryItemType . File )
				this . Children . Add ( null );
		}
		#endregion Helper Methods

		// Expand current directory and find all children
		private void Expand ( )
		{
			if ( this . Type == DirectoryItemType . File )
				return;

			// 2 ways to do this !!
			//var children = DirectoryStructure . GetDirectoryContents ( this . FullPath );
			//this . Children = new ObservableCollection<DirectoryItemViewModel>
			//	( children . Select ( content => new DirectoryItemViewModel ( content . FullPath , content . Type ) ) );

			// Simple (original) way to do the same thing
			this . Children = new ObservableCollection<DirectoryItemViewModel> ( );
			List<DirectoryItem> AllDirs= DirectoryStructure . GetDirectoryContents ( this . FullPath );
			foreach ( var item in AllDirs )
			{
				var content = new DirectoryItemViewModel ( item . FullPath , item . Type );
				this . Children . Add ( content);
			}
		}
	}
}
