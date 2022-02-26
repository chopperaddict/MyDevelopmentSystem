using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;

//using static MyDev . ViewModels . Model;

namespace MyDev . ViewModels
{
	//class ItemProvider
	//{
	//	private readonly DirectoryItem _rootDirectoryItem;

	//	public ItemProvider ( )
	//	{
	//		_rootDirectoryItem = new DirectoryItem { Name = "X" };

	//		var childItem1 = new DirectoryItem {Name = "A"};

	//		var grandChildItem11 = new DirectoryItem {Name = "A1"};
	//		var grandChildItem12 = new DirectoryItem {Name = "A2"};

	//		var greatgrandChildItem2 = new DirectoryItem {Name = "A2_1"};
	//		grandChildItem11 . AddDirItem ( greatgrandChildItem2 );

	//		childItem1 . AddDirItem ( grandChildItem11 );
	//		childItem1 . AddDirItem ( grandChildItem12 );

	//		var childItem2 = new DirectoryItem {Name = "B"};
	//		var childItem3 = new DirectoryItem {Name = "C"};
	//		var childItem4 = new DirectoryItem {Name = "D"};

	//		var grandChildItem121 = new DirectoryItem {Name = "B1"};
	//		childItem2 . AddDirItem ( grandChildItem121 );

	//		var childList1 = new List<DirectoryItem>
	//		   {
	//			childItem1,
	//			childItem2,
	//			childItem3,
	//			childItem4
	//		   };

	//		_rootDirectoryItem . Items = childList1;
	//	}

	//	public List<Item> DirItems => _rootDirectoryItem . Traverse ( _rootDirectoryItem );
	//}
}
