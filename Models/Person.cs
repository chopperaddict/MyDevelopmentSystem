using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Linq;
using System . Net;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Input;
using System . Xml . Linq;

namespace MyDev . Models
{
	public class Person : BaseViewModel
	{
		#region Properties
		private string name;
		private string address;
		private Person selectedperson;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				OnPropertyChanged ( "Name" );
			}
		}
		public string Address
		{
			get
			{
				return address;
			}
			set
			{
				address = value;
				OnPropertyChanged ( "Address" );
			}
		}

		public Person SelectedPerson
		{
			get { return selectedperson; }
			set { selectedperson = value; }
		}

		#endregion Properties
		public Person ( )
		{
		}

	}

}