using System;
using System . Collections . Generic;
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
	public class DirectoryItem
	{
		// Type this item
		public DirectoryItemType  Type  {get ; set;}
		public string FullPath { get; set; }
		public string Name { get { return 
					this.Type == DirectoryItemType.Drive  
					? this.FullPath 
					: DirectoryStructure . GetFileFolderName ( this . FullPath); } }
	}
}
