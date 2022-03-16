using PropertyChanged;

using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Linq;
using System . Runtime . Remoting . Channels;
using System . Text;
using System . Threading . Tasks;

namespace MyDev . ViewModels
{
	//[ImplementPropertyChanged]
	public class Class1 : INotifyPropertyChanged
	{
		
		/// <summary>
		/// This is the VIEWMODEL that we binnd to
		/// It should provide handling for all changes to active properties
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
		private string test;

		public string Test
		{
			get { return test; }
			set { if ( test == value )
					return;
				 test = value; 
				PropertyChanged(this, new PropertyChangedEventArgs ( nameof(Test ))); 
			}
		}
		public Class1 ( )
		{
			// Just To prove this is subscribed to , button updates with value i;
			//Task . Run ( async ( ) =>
			//{
			//	int i = 0;
			//	while ( true )
			//	{
			//		await Task . Delay ( 200 );
			//		Test = ( i++ ) . ToString ( );
			//	}
			//} );
		}

	}
}
