using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Linq;
using System . Security . Policy;
using System . Text;
using System . Threading . Tasks;

namespace MyDev . ViewModels
{

	/// <summary>
	/// 
	/// info about a directory item
	/// </summary>
	public class DirectoryItem// :INotifyPropertyChanged
	{
		//public event PropertyChangedEventHandler PropertyChanged;
		// Type of this item
		private DirectoryItemType type;
		public DirectoryItemType Type
		{
			get { return type; }
			set { type = value; }
			//PropertyChanged ( this, new PropertyChangedEventArgs( type.ToString()) ); }
		}

		private string fullpath;
		public string FullPath
		{
			get { return fullpath; }
			set
			{
				fullpath = value;
				//var e = new PropertyChangedEventArgs ( FullPath );
				//this . PropertyChanged ( this , e );
			}
		}
#pragma warning disable CS0169 // The field 'DirectoryItem.name' is never used
		private string name;
#pragma warning restore CS0169 // The field 'DirectoryItem.name' is never used
		public string Name
		{
			get
			{
				return this . Type == DirectoryItemType . Drive
						? this . FullPath
						: DirectoryStructure . GetFileFolderName ( this . FullPath );
			}
		}
	}
}
