using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . IO;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Controls;
using System . Windows . Shapes;

using static System . Net . WebRequestMethods;

namespace MyDev . ViewModels
{
	/// <summary>
	/// General Helper class for Directory handling
	/// loads  drives, folders or just files, or combinations of these as  required
	/// </summary>
	public static class DirectoryStructure	
	{

		/// <summary>
		/// Load all logical drives on this PC
		/// </summary>
		/// <returns></returns>
		public static List<DirectoryItem> GetLogicalDrives ( )
		{
			// Return a new list of logical drives
			return Directory . GetLogicalDrives ( ) . Select (
				    drive => new DirectoryItem { FullPath = drive , Type = DirectoryItemType . Drive } ) . ToList ( );
		}
		/// <summary>
		/// get directories top level contents
		/// </summary>
		/// <param name="FullPath"></param>
		/// <returns></returns>
		public static List<DirectoryItem> GetDirectoryContents ( string FullPath )
		{
			var items = new List<DirectoryItem>();
			try
			{
				string[] dirs = Directory . GetDirectories( FullPath);
				if ( dirs . Length > 0 )
					items . AddRange ( dirs .Select(
						   dir => new DirectoryItem { FullPath = dir , Type= DirectoryItemType . Folder }) );
			} catch { }


			//Get a list of all items in the currently selected  folder
			try
			{
				var fs= Directory . GetFiles( FullPath);
				if ( fs . Length > 0 )
					items . AddRange ( fs.Select( file =>new DirectoryItem { FullPath = file, Type = DirectoryItemType . File }) );
			} catch { }
			return items;
		}
		#region Helper Methods
		public static string GetFileFolderName ( string path )
		{
			if ( string . IsNullOrEmpty ( path ) )
				return String . Empty;
			var normalizedPath = path.Replace('/', '\\');
			var lastindex = normalizedPath.LastIndexOf('\\');
			if ( lastindex <= 0 )
				return path;
			return path . Substring ( lastindex + 1 );
		}
		#endregion Helper Methods


		//public static List<DirectoryItem> GetFiles( string FullPath )
		//{
		//	var files= new List<string>();
		//	// Get a list of all items in the current folder
		//	try
		//	{
		//	files = Directory . GetFiles( FullPath);
		//		//if ( fs. Length > 0 )
		//		//	items. AddRange ( fs );
		//	} catch { }
		//	return fs;
		//}
	}
}
