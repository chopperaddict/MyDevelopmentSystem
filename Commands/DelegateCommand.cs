﻿using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Input;

namespace MyDev . Commands
{
      public class DelegateCommand : ICommand
      {
            private readonly Predicate<object> _canExecute;
            private readonly Action<object> _execute;
        private Action convertText;

        public event EventHandler CanExecuteChanged;

            public DelegateCommand ( Action<object> execute ,
                           Predicate<object> canExecute )
            {
                  _execute = execute;
                  _canExecute = canExecute;
            }

        public DelegateCommand ( Action convertText )
        {
            this . convertText = convertText;
        }

        public bool CanExecute ( object parameter )
            {
                  if ( _canExecute == null )
                  {
                        return true;
                  }

                  return _canExecute ( parameter );
            }

            public void Execute ( object parameter )
            {
                  _execute ( parameter );
            }

            public void RaiseCanExecuteChanged ( )
            {
                  if ( CanExecuteChanged != null )
                  {
                        CanExecuteChanged ( this , EventArgs . Empty );
                  }
            }
      }
}