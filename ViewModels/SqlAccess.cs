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
		public static ObservableCollection<GenericClass> LoadDbAsGenericData (
			ref ObservableCollection<GenericClass> GenClass , 
			ref List<string> list , 
			string SqlCommand , 
			string Arguments , 
			string DbDomain )
		{
			if ( DbDomain == "" )
				DbDomain = "IAN1";
			List< int> VarCharLength= new List<int>();
			GenClass = GenericDbHandlers . LoadDbAsGenericData ( ref list , SqlCommand , Arguments , DbDomain , ref VarCharLength );
			return GenClass;
		}


	}
}
