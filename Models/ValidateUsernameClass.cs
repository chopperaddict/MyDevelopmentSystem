﻿using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Input;
using System . Windows . Media;
using System . Xml . Linq;

using MyDev . ViewModels;

namespace MyDev . Models
{
    /// <summary>
    /// Class to support my TextBox UserControl with suport for IDataErrorInfo
    /// </summary>
    [DebuggerDisplay ( "{" + nameof ( GetDebuggerDisplay ) + "(),nq}" )]
    public class ValidateUsernameClass : IDataErrorInfo, INotifyPropertyChanged, ICommand
    {
        private string ErrorInfo1;
        private string invalidchars = "0123456789!=+!£$%^&*()}{@\"~#?//>\"\"<,.\\¬";
//        public ICommand CloseBtn;
  
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CanExecuteChanged;

        protected void OnPropertyChanged ( string PropertyName )
        {
            if ( this . PropertyChanged != null )
            {
                var e = new PropertyChangedEventArgs ( PropertyName );
                this . PropertyChanged ( this , e );
            }
        }
        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional ( "DEBUG" )]
        [DebuggerStepThrough]
        public virtual void VerifyPropertyName ( string propertyName )
        {
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if ( TypeDescriptor . GetProperties ( this ) [ propertyName ] == null )
            {
                string msg = "Invalid property name: " + propertyName;

                if ( this . ThrowOnInvalidPropertyName )
                    throw new Exception ( msg );
                else
                    Debug . Fail ( msg );
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName
        {
            get; private set;
        }

        #endregion OnPropertyChanged
        public ValidateUsernameClass ( )
        {
//            ICommand CloseBtn = new RelayCommand ( ExecuteClosewin , CanExecuteCloseWin );
        }
        //private bool CanExecuteCloseWin ( object arg )
        //{return true;}
        //private void ExecuteClosewin ( object obj )
        //{
        //    Window win = obj as Window;
        //    win . Close ( );
        //}


        #region Full Properties
 
        private string userName;
        public string UserName
        {
            get
            { return userName; }
            set
            {
                userName = value; OnPropertyChanged ( "UserName" );
            }
        }
        private string promptText;
        public string PromptText
        {
            get { return promptText; }
            set { promptText = value; OnPropertyChanged ( "PromptText" ); }
        }
        private string dataItem { get; set; }
        public string DataItem
        {
            get { return dataItem; }
            set
            {
                dataItem = value;
                UserName = value;
                OnPropertyChanged ( "DataItem" );
            }
        }
        private Visibility isPromptVisible;
        public Visibility IsPromptVisible
        {
            get { return isPromptVisible; }
            set { isPromptVisible = value; OnPropertyChanged ( "IsPromptVisible" ); }
        }
        private double promptFontsize;
        public double  PromptFontsize
        {
            get { return promptFontsize; }
            set { promptFontsize = value; OnPropertyChanged ( "PromptFontsize" ); }
        }
        private SolidColorBrush promptBkground;
        public SolidColorBrush PromptBkground
        {
            get { return promptBkground; }
            set { promptBkground = value; OnPropertyChanged ( "PromptBkground" ); }
        }
        private SolidColorBrush textboxBkground;
        public SolidColorBrush TextboxBkground
        {
            get { return textboxBkground; }
            set { textboxBkground = value; OnPropertyChanged ( "TextboxBkground" ); }
        }        
        private SolidColorBrush textboxFground;
        public SolidColorBrush TextboxFground
        {
            get { return textboxFground; }
            set { textboxFground = value; OnPropertyChanged ( "TextboxFground" ); }
        }
        private double dataFontSize;
        public double DataFontSize
        {
            get { return dataFontSize; }
            set { dataFontSize = value; OnPropertyChanged ( "DataFontSize" ); }
        }
        // Data flag to check if data in field is valid, that can be checked by the process before using it
        private bool isValid;
        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; OnPropertyChanged ( "IsValid" ); }
        }

        #endregion Full Properties

        #region DataErrorInfo
        public string Error { get { return "An Error has occured in the data entered.."; } }
        public string this [ string Propertyname ]
          {
            // called by IDataErrorInfo system
            get
            {
                if ( Propertyname == "DataItem" )
                    return ValidateName ( Propertyname );
                else if ( Propertyname == "Age" )
                {
                    return "";
                }
                return "";
            }
        }

        // This validation for above can live anywhere we wish !!!!!  
        //Even a different file or class.
        private string ValidateName ( string PropertyName )
        {
            // Validate a name to be exactly 2 words in length
            // with only Alpha characters
            // Reports back if any other values are identified
            // Responses are targetted at a Full Property (ErrorInfo x ) Text field in the caller module
            // This does NOT provide ToolTip response
            string result = null;
            int reslt = -1;
            string ch = "";
            if ( PropertyName == "DataItem" )
            {
                IsValid = false;
                if ( DataItem == null )
                    return null;
                reslt = CheckforValidChars ( DataItem , out ch );
                if ( reslt != -1 )
                {
                    this . ErrorInfo1 = $"Entry contains an invalid character of '{ch}' at position {reslt + 1}...";
                    return ErrorInfo1;
                }
                string [ ] clauses = DataItem . Split ( ' ' );
                if ( clauses . Length > 3 )
                {
                    int spacescount = 0;
                    int wordcount = 0;
                    foreach ( var item in clauses )
                    {
                        if ( item.Trim()== ""  )
                            spacescount ++;
                        else 
                            wordcount++;
                    }
                    if ( wordcount > 2 )
                        this . ErrorInfo1 = "User Name can ONLY be 2 words with a space between them";
                    else
                    {
                        this . ErrorInfo1 = "";
                        this . IsValid = true;
                    }
                    return ErrorInfo1;
                }
                // Works well
                if ( DataItem == null || string . IsNullOrWhiteSpace ( DataItem ) )
                {
                    return "";
                }
                else if ( DataItem . Contains ( " " ) == false )
                {
                    this . ErrorInfo1 = "User Name must be 2 seperate words totalling 6 or more characters....";
                    return ErrorInfo1;
                }
                else if ( DataItem . Length > 6 )
                {
                    string [ ] str = this . DataItem . Split ( ' ' );
                    if ( str . Length >= 2 )
                        if ( str [ 1 ] != "" && str [ 1 ] . Length < 1 )
                            this . ErrorInfo1 = "User Name must be two seperate words totalling 6 or more characters....";
                        else
                        {
                            IsValid = true;
                            this . ErrorInfo1 = "";
                        }
                    return ErrorInfo1;
                }
                else if ( DataItem . Length > 1 )
                {
                    this . ErrorInfo1 = "User Name must be 2 words totalling 6 or more characters with space between them";
//                    this . ErrorInfo2 = "Please enter a valid Age between 16 & 109 ....";
                    return ErrorInfo1;
                }
                else
                {
                    this . ErrorInfo1 = "The User Name must be 2 words totalling at least 6 characters in length....";
                    return ErrorInfo1;
                }
            }
            return "";
        }
        private int CheckforValidChars ( string entry , out string ch )
        {
            ch = "";
            bool success = true;
            //int sourcecounter = 0;
            int result = -1;
            if ( entry == null )
                return -1;
            try
            {
                foreach ( var item in invalidchars )
                {
                    if ( entry . Contains ( item ) )
                    {
     //                   ch = entry [ sourcecounter ] . ToString ( );
                        success = false;
                        for ( int y = 0 ; y < entry . Length ; y++ )
                        {
                            if ( entry [ y ] == item )
                            {
                                result = y;
                                ch = item . ToString ( );
                                break;
                            }
                        }
                        break;
                    }
                    result++;
                }
                //sourcecounter++;
            }
            catch ( Exception ex ) { }
            if ( !success )
                return result;
            else return -1;
        }

        private string GetDebuggerDisplay ( )
        {
            return ToString ( );
        }

        public bool CanExecute ( object parameter )
        {
            throw new NotImplementedException ( );
        }

        public void Execute ( object parameter )
        {
            throw new NotImplementedException ( );
        }
        #endregion DataErrorInfo
    }
}
