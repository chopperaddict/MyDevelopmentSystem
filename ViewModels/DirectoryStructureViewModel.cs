using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Linq;
using System . Text;
using System . Threading . Tasks;

namespace MyDev . ViewModels
{
	/// <summary>
	/// view model for the 	apps main directory view
	/// </summary>
	public class DirectoryStructureViewModel//	  : BaseViewModel
	{
		// List of all directories on the machine 
		public ObservableCollection<DirectoryItemViewModel> Items { get; set; }
		public DirectoryStructureViewModel ( )
		{
			//Get logical drives
			var children = DirectoryStructure.GetLogicalDrives();
			// Create view models from the data above
			this .Items	     = new ObservableCollection<DirectoryItemViewModel>
				(children . Select ( drive => 
					new DirectoryItemViewModel ( drive . FullPath , DirectoryItemType .Drive )));
		}
	}
}
