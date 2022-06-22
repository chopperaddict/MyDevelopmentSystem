// ALL #defines should  be in here....
// extensions.cs
#define USECW

#define USETASK
#undef USETASK

#define SHOWALLFLAGS
#undef SHOWALLFLAGS

#define USEDETAILEDEXCEPTIONHANDLER
#undef USEDETAILEDEXCEPTIONHANDLER

#define DEBUGEXPAND
#undef DEBUGEXPAND

#define SHOWWINDOWDATA
// DetailsViewModel.cs
#define TASK1

#define BINDINGDBUG
//#undef BINDINGDBUG

// if set, Datatable is cleared and reloaded, otherwise it is not reloaded in bankaccountviewmodel.cs & CustomerViewModel.cs
//#define PERSISTENTDATA

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
//#if BINDINGDBUG
//            new BindingErrorListener ( msg => Console . WriteLine (msg));
            //new BindingErrorListener ( msg => Debugger . Break ( ) );
//#endif
        }

        protected override void OnStartup ( StartupEventArgs e )
        {
#if BINDINGDBUG
            //PresentationTraceSources . Refresh ( );
            //PresentationTraceSources . DataBindingSource . Listeners . Add ( new ConsoleTraceListener ( ) );
            //PresentationTraceSources . DataBindingSource . Listeners . Add ( new DebugTraceListener ( ) );
            //PresentationTraceSources . DataBindingSource . Switch . Level = SourceLevels . Warning | SourceLevels . Error;
#endif       
            //base . OnStartup ( e );
        }
    }
    public class DebugTraceListener : TraceListener
    {
        public override void Write ( string message )
        {
            //if ( message . Contains ( "got raw value" ) == false
            //    && message . Contains ( "converter produced" ) == false
            //    && message . Contains ( "using final" ) == false)
            //    Console . WriteLine ($"DebugTrace:  {message}");
        }

        public override void WriteLine ( string message )
        {
            //if(message.Contains ("Error:" ) == true)
            //Debugger . Break ( );
        }
    }
}
