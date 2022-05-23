using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Navigation;
using System . Windows . Shapes;

namespace MyDev . UserControls
{
    /// <summary>
    /// Interaction logic for TextBoxwithDataError.xaml
    /// </summary>
    public partial class TextBoxwithDataError : UserControl, INotifyPropertyChanged
    {
        // define the delegate handler signature and the event that will be raised
        // to send the message
//        public delegate void customMessageHandler ( object  sender , EventArgs e );
        public event RoutedEventHandler OnSend;

        // define the delegate handler signature and the event that will be raised
        // to send the message using my own specific Arguments
        public delegate void SendUserHandler ( object sender , MessageEventArgs args );
        public event SendUserHandler SendUser;


        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

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

        public TextBoxwithDataError ()
        {
            InitializeComponent ( );
        }
        public  void SetDc(object dc)
        {
            this.DataContext = dc;

        }

        private void UserControl_LostFocus ( object sender , RoutedEventArgs e )
        {
            
            
            if ( this . SendUser != null )
            {
                // This works fine, but have to parse out the arguments format
                MessageEventArgs mea = new MessageEventArgs ( );
                string str = e . OriginalSource.ToString();
                if ( str == "" )
                    return;
                string [ ] data = str . Split ( ':' );
                str = data [ 1 ] . Trim ( );
                mea . message = $"{str}";
                this . SendUser ( sender , mea );
            }
        }

        private void UserControl_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e . Key == Key . Enter )
                UserControl_LostFocus ( sender , e );
        }
    }
    public class MessageEventArgs : System . EventArgs
    {
        private String mMessage;
        public String message
        {
            get
            {
                return ( mMessage );
            }
            set
            {
                mMessage = value;
            }
        }  // message
    }  // MessageEventArgs
}
