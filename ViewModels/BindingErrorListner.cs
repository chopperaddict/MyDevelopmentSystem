using System;
using System . Collections . Generic;
using System . Diagnostics;
using System . Linq;
using System . Text;
using System . Threading . Tasks;

namespace MyDev . ViewModels
{
    public class BindingErrorListener : TraceListener
    {
#pragma warning disable CS0649 // Field 'BindingErrorListener._errorHandler' is never assigned to, and will always have its default value null
        private readonly Action<string> _errorHandler;
#pragma warning restore CS0649 // Field 'BindingErrorListener._errorHandler' is never assigned to, and will always have its default value null

        public BindingErrorListener ( Action<string> errorHandler )
        {
            //_errorHandler = errorHandler;
            //TraceSource bindingTrace = PresentationTraceSources
            //    . DataBindingSource;

            //bindingTrace . Listeners . Add ( this );
            //bindingTrace . Switch . Level = SourceLevels . Error;
        }

        public override void WriteLine ( string message )
        {
            _errorHandler?.Invoke ( message );
        }

        public override void Write ( string message )
        {
        }
    }
}
