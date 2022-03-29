using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Linq;
using System . Runtime . InteropServices . WindowsRuntime;
using System . Text;
using System . Threading . Tasks;

using MyDev . Models;

namespace MyDev . ViewModels
{
	public static class SqlAccess   
	{
		// Run any Stored procedure  to get a generic Db data set
		// do NOT use with noral "Select" statements
		public static ObservableCollection<GenericClass> LoadDbAsGenericData (
			ref ObservableCollection<GenericClass> GenClass , 
			ref List<string> list , 
			string SqlCommand , 
			string Arguments , 
			string DbDomain )
		{
			if ( SqlCommand. ToUpper ( ). Contains ( "SELECT" ) )
				return null;
			if ( DbDomain == "" )
				DbDomain = "IAN1";
			List< int> VarCharLength= new List<int>();
			// Ensure we hve access t cnnection strings
			Utils. LoadConnectionStrings ( );			
			GenClass = GenericDbHandlers . LoadDbAsGenericData ( ref list , SqlCommand , Arguments , DbDomain , ref VarCharLength );
			return GenClass;
		}


	}
}
