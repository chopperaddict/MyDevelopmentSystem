using System;
using System . Collections . Generic;
using System . Configuration;
using System . Data;
using System . Diagnostics;
using System . Linq;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Interop;

using MyDev . ViewModels;

namespace MyDev
{
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
	public partial class App : Application
	{
		public App ( )
		{
            //new BindingErrorListener ( msg => Console . WriteLine (msg));
			//new BindingErrorListener ( msg => Debugger . Break ( ) );
		}
	}
}
