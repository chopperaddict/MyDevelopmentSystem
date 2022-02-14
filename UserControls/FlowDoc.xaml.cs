using MyDev . ViewModels;
using MyDev . Views;

using System;
using System . Collections . Generic;
using System . Linq;
using System . Runtime . Remoting . Messaging;
using System . Security . Cryptography;
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
	/// Interaction logic for FlowDoc.xaml
	/// </summary>
	public partial class FlowDoc : UserControl
	{
		//private bool buttonDown;
		//public bool ButtonDown
		//{
		//	get { return buttonDown; }
		//	set { buttonDown = value; }
		//}
		//private Point _startpoint;
		//public Point startpoint  
		//{
		//	get { return _startpoint; }
		//	set { _startpoint = value; }
		//}
		private bool mouseCaptured  ;
		public bool MouseCaptured
		{
			get { return mouseCaptured; }
			set { mouseCaptured = value; }
		}
		private double docHeight;
		public double DocHeight
		{
			get { return docHeight; }
			set { docHeight = value; }
		}
		private double docWidth;
		public double DocWidth
		{
			get { return docWidth; }
			set { docWidth = value; }
		}

		public FlowDoc ( )
		{
			InitializeComponent ( );
			this . DataContext = this;
		}
		private void flowdoc_Loaded ( object sender , RoutedEventArgs e )
		{
			DocHeight = this . ActualHeight;
			if ( DocHeight == 0 )
				DocHeight = 250;
			DocWidth = this . ActualWidth;
			if ( DocWidth == 0 )
				DocWidth = 450;
			if ( Flags . UseScrollView )
			{
				fdviewer . Visibility = Visibility . Visible;
				doc . Visibility = Visibility . Hidden;
			} else
			{
				fdviewer . Visibility = Visibility . Hidden;
				doc . Visibility = Visibility . Visible;
			}
		}
		public void ShowInfo (
			string line1 = "" ,
			string clr1 = "Black0" ,
			string line2 = "" ,
			string clr2 = "Blue0" ,
			string line3 = "" ,
			string clr3 = "Green2" ,
			string header = "" ,
			string clr4 = "Red3" ,
			bool beep = false )
		{
			TextRange textRange;
			FlowDocumentScrollViewer myFlowDocumentScrollViewer = new FlowDocumentScrollViewer();
			FlowDocument myFlowDocument = new FlowDocument();
			FlowDocument myFlowDocument2= new FlowDocument();


			if ( Flags . UseScrollView )
			{
				fdviewer . Visibility = Visibility . Visible;
				doc . Visibility = Visibility . Hidden;
				myFlowDocument2 = CreateFlowDocumentScroll ( line1 , clr1 , line2 , clr2 , line3 , clr3 , header , clr4 );
				fdviewer . Document = myFlowDocument2;
				textRange = new TextRange ( fdviewer . Document . ContentStart , fdviewer . Document . ContentEnd );
			} else
			{
				fdviewer . Visibility = Visibility . Hidden;
				doc . Visibility = Visibility . Visible;
				myFlowDocument = CreateFlowDocument ( line1 , clr1 , line2 , clr2 , line3 , clr3 , header , clr4 );
				doc . Document = myFlowDocument;
				textRange = new TextRange ( doc . Document . ContentStart , doc . Document . ContentEnd );
			}
			// Get length of the controls content so we can resize as needed			
			int retcount = 0;
			for ( int x = 0 ; x < textRange . Text . Length - 1 ; x++ )
			{
				if ( textRange . Text [ x ] == '\n' )
					retcount++;
			}

			if ( flowdoc . Width == 0 )
				flowdoc . Width = 520;
			if ( flowdoc . Height == 0 )
				flowdoc . Height = 300;
			var v1 = Convert . ToDouble ( flowdoc . GetValue ( HeightProperty ) );
			var v2 = Convert . ToDouble ( flowdoc . GetValue ( WidthProperty ) );
			flowdoc . SetValue ( HeightProperty , DocHeight );
			flowdoc . SetValue ( WidthProperty , DocWidth );
			if ( textRange . Text . Length < 100 )
				flowdoc . SetValue ( HeightProperty , ( double ) 180 + retcount * Flags . FlowdocCrMultplier );
			else if ( textRange . Text . Length < 150 )
				flowdoc . SetValue ( HeightProperty , ( double ) 210 + retcount * Flags . FlowdocCrMultplier );
			else if ( textRange . Text . Length < 200 )
				flowdoc . SetValue ( HeightProperty , ( double ) 235 + retcount * Flags . FlowdocCrMultplier );
			else if ( textRange . Text . Length < 250 )
				flowdoc . SetValue ( HeightProperty , ( double ) 255 + retcount * Flags . FlowdocCrMultplier );
			else if ( textRange . Text . Length < 300 )
				flowdoc . SetValue ( HeightProperty , ( double ) 275 + retcount * Flags . FlowdocCrMultplier );
			else if ( textRange . Text . Length < 350 )
				flowdoc . SetValue ( HeightProperty , ( double ) 285 + retcount * Flags . FlowdocCrMultplier );
			else if ( textRange . Text . Length < 400 )
				flowdoc . SetValue ( HeightProperty , ( double ) 320 + retcount * Flags . FlowdocCrMultplier );
			else if ( textRange . Text . Length < 450 )
				flowdoc . SetValue ( HeightProperty , ( double ) 340 + retcount * Flags . FlowdocCrMultplier );
			else if ( textRange . Text . Length < 500 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 360 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 20 );
			} else if ( textRange . Text . Length < 600 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 450 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 30 );
			} else if ( textRange . Text . Length < 700 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 500 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 40 );
			} else if ( textRange . Text . Length < 800 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 600 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 50 );
			} else if ( textRange . Text . Length < 900 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 700 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 60 );
			} else
			{
				Flags . UseFlowScrollbar = true;
				flowdoc . SetValue ( HeightProperty , ( double ) 500 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 100 );
			}
			flowdoc . Height = Convert . ToDouble ( flowdoc . GetValue ( HeightProperty ) );
			flowdoc . Width = Convert . ToDouble ( flowdoc . GetValue ( WidthProperty ) );
			if ( flowdoc . Height == 0 )
				flowdoc . Height = v1;
			flowdoc . SetValue ( HeightProperty , ( double ) flowdoc . Height );
			if ( flowdoc . Width == 0 )
				flowdoc . Width = v2;
			flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width );

			if ( this . Visibility == Visibility . Hidden )
			{
				if ( Flags . UseFlowScrollbar )
				{
					// text is too long for window, so handle scrollbar
					if ( Flags . UseScrollView )
					{
						fdviewer . VerticalScrollBarVisibility = ScrollBarVisibility . Visible;
						fdviewer . IsEnabled = true;
					} else
					{
						doc . VerticalScrollBarVisibility = ScrollBarVisibility . Visible;
						doc . IsEnabled = true;
					}
				} else
				{
					if ( Flags . UseScrollView )
						fdviewer . IsEnabled = false;
					else
						doc . IsEnabled = false;
				}
				this . Visibility = Visibility . Visible;
				this . BringIntoView ( );
				if ( beep )
					Utils . DoErrorBeep ( 300 , 50 , 1 );
			} else
			{
				this . BringIntoView ( );
			}
		}


		private FlowDocument CreateFlowDocument ( string line1 , string clr1 , string line2 , string clr2 , string line3 , string clr3 , string header , string clr4 )
		{
			// Create new FlowDocument to be used by our RichTextBox
			FlowDocument myFlowDocument = new FlowDocument();

			if ( header != "" )
			{
				// BOLD + UNDERLINED
				Paragraph para2= new Paragraph();
				// how to concatenate attributes on a paragraph
				para2 . FontSize = 16;
				para2 . FontFamily = new FontFamily ( "Arial" );
				if ( clr4 != "" )
					para2 . Foreground = FindResource ( clr4 . Trim ( ) ) as SolidColorBrush;
				else
					para2 . Foreground = FindResource ( "Black0" ) as SolidColorBrush;
				// Add some Bold text to the paragraph
				para2 . Inlines . Add ( new Underline ( new Bold ( new Run ( header . Trim ( ) ) ) ) );
				//Add paragraph to flowdocument
				myFlowDocument . Blocks . Add ( para2 );
			}

			if ( line1 != "" )
			{
				//NORMAL
				Paragraph para1= new Paragraph();
				para1 . FontFamily = new FontFamily ( "Arial" );
				if ( clr1 != "" )
					para1 . Foreground = FindResource ( clr1 . Trim ( ) ) as SolidColorBrush;
				else
					para1 . Foreground = FindResource ( "Black0" ) as SolidColorBrush;
				para1 . Inlines . Add ( new Run ( line1 ) );
				//Add paragraph to flowdocument
				myFlowDocument . Blocks . Add ( para1 );
			}
			if ( line2 != "" )
			{
				//\NORMAL
				Paragraph para2= new Paragraph();
				para2 . FontFamily = new FontFamily ( "Arial" );
				para2 . FontSize = 14;
				if ( clr2 != "" )
					para2 . Foreground = FindResource ( clr2 . Trim ( ) ) as SolidColorBrush;
				else
					para2 . Foreground = FindResource ( "Black2" ) as SolidColorBrush;
				para2 . Inlines . Add ( new Run ( line2 ) );
				//Add paragraph to flowdocument
				myFlowDocument . Blocks . Add ( para2 );
			}
			if ( line3 != "" )
			{
				//ITALIC
				Paragraph para3= new Paragraph();
				para3 . FontFamily = new FontFamily ( "Arial" );
				if ( clr3 != "" )
					para3 . Foreground = FindResource ( clr3 . Trim ( ) ) as SolidColorBrush;
				else
					para3 . Foreground = FindResource ( "Black3" ) as SolidColorBrush;
				para3 . Inlines . Add ( new Italic ( new Run ( line3 ) ) );
				//Add paragraph to flowdocument
				myFlowDocument . Blocks . Add ( para3 );
			}
			return myFlowDocument;
		}
		private FlowDocument CreateFlowDocumentScroll ( string line1 , string clr1 , string line2 , string clr2 , string line3 , string clr3 , string header , string clr4 )
		{
			// Create new FlowDocument to be used by our RichTextBox
			FlowDocument myFlowDocument = new FlowDocument();

			if ( header != "" )
			{
				// BOLD + UNDERLINED
				Paragraph para2= new Paragraph();
				// how to concatenate attributes on a paragraph
				para2 . FontSize = 16;
				para2 . FontFamily = new FontFamily ( "Arial" );
				if ( clr4 != "" )
					para2 . Foreground = FindResource ( clr4 . Trim ( ) ) as SolidColorBrush;
				else
					para2 . Foreground = FindResource ( "Black0" ) as SolidColorBrush;
				// Add some Bold text to the paragraph
				para2 . Inlines . Add ( new Underline ( new Bold ( new Run ( header . Trim ( ) ) ) ) );
				//Add paragraph to flowdocument
				myFlowDocument . Blocks . Add ( para2 );
			}

			if ( line1 != "" )
			{
				//NORMAL
				Paragraph para1= new Paragraph();
				para1 . FontSize = 12;
				para1 . FontFamily = new FontFamily ( "Arial" );
				if ( clr1 != "" )
					para1 . Foreground = FindResource ( clr1 . Trim ( ) ) as SolidColorBrush;
				else
					para1 . Foreground = FindResource ( "Black0" ) as SolidColorBrush;
				para1 . Inlines . Add ( new Run ( line1 ) );
				//Add paragraph to flowdocument
				myFlowDocument . Blocks . Add ( para1 );
			}
			if ( line2 != "" )
			{
				// BOLD
				Paragraph para2= new Paragraph();
				para2 . FontFamily = new FontFamily ( "Arial" );
				para2 . FontSize = 14;
				if ( clr2 != "" )
					para2 . Foreground = FindResource ( clr2 . Trim ( ) ) as SolidColorBrush;
				else
					para2 . Foreground = FindResource ( "Black2" ) as SolidColorBrush;
				para2 . Inlines . Add ( new Run ( line2 ) );
				//Add paragraph to flowdocument
				myFlowDocument . Blocks . Add ( para2 );
			}
			if ( line3 != "" )
			{
				//ITALIC
				Paragraph para3= new Paragraph();
				para3 . FontFamily = new FontFamily ( "Arial" );
				para3 . FontSize = 14;
				if ( clr3 != "" )
					para3 . Foreground = FindResource ( clr3 . Trim ( ) ) as SolidColorBrush;
				else
					para3 . Foreground = FindResource ( "Black3" ) as SolidColorBrush;
				para3 . Inlines . Add ( new Italic ( new Run ( line3 ) ) );
				//Add paragraph to flowdocument
				myFlowDocument . Blocks . Add ( para3 );
			}
			//myFlowDocumentScrollViewer = myFlowDocument;
			return myFlowDocument;
		}
		private void Button_Click ( object sender , RoutedEventArgs e )
		{
			this . Visibility = Visibility . Hidden;
		}


		#region Dependency properties
		public Brush borderColor
		{
			get { return ( Brush ) GetValue ( borderColorProperty ); }
			set { SetValue ( borderColorProperty , value ); }
		}
		public static readonly DependencyProperty borderColorProperty =
			DependencyProperty.Register("borderColor", typeof(Brush), typeof(FlowDoc), new PropertyMetadata(Brushes.Red));

		public Brush backGround
		{
			get { return ( Brush ) GetValue ( backGroundProperty ); }
			set { SetValue ( backGroundProperty , value ); }
		}
		public static readonly DependencyProperty backGroundProperty =
			DependencyProperty.Register("backGround", typeof(Brush), typeof(FlowDoc), new PropertyMetadata(Brushes.LightGray));

		public Brush btnBkGround
		{
			get { return ( Brush ) GetValue ( btnBkGroundProperty ); }
			set { SetValue ( btnBkGroundProperty , value ); }
		}
		public static readonly DependencyProperty btnBkGroundProperty =
			DependencyProperty.Register("btnBkGround", typeof(Brush), typeof(FlowDoc), new PropertyMetadata(Brushes.Red));

		public Brush btnForeGround
		{
			get { return ( Brush ) GetValue ( btnForeGroundProperty ); }
			set { SetValue ( btnForeGroundProperty , value ); }
		}
		public static readonly DependencyProperty btnForeGroundProperty=
			DependencyProperty.Register("btnForeGround", typeof(Brush), typeof(FlowDoc), new PropertyMetadata(Brushes.White));
		#endregion Dependency properties

		#region keyboard handlers
		private void flowdoc_PreviewKeyDown ( object sender , KeyEventArgs e )
		{
			if ( e . Key == Key . Escape )
			{
				e . Handled = true;
				this . Visibility = Visibility . Hidden;
			} else if ( e . Key == Key . F8 )
			{
				fdviewer . ReleaseMouseCapture ( );
				flowdoc . ReleaseMouseCapture ( );
				Console . WriteLine ( "Mouse RELEASED... flowdoc_PreviewKeyDown()" );
			}
		}
		#endregion keyboard handlers

		#region Mouse handlers
		private void flowdoc_PreviewLMBDn ( object sender , MouseButtonEventArgs e )
		{
			if ( Utils . HitTestScrollBar ( sender , e ) )
			{
				fdviewer . IsEnabled = true;
				return;
			} else
				fdviewer . IsEnabled = false;
		}

		Point _point;
		private void doc_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			if ( Utils . HitTestScrollBar ( sender , e ) )
			{
				fdviewer . IsEnabled = true;
				return;
			} else
				fdviewer . IsEnabled = false;
			MouseCaptured = flowdoc . CaptureMouse ( );
			Console . WriteLine ( "Mouse CAPTURED...flowdoc_PreviewMouseLeftButtonDown()" );
		}
		private void flowdoc_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			if ( Utils . HitTestScrollBar ( sender , e ) )
			{
				// Over the Scrollbar so let user scroll contents
				if ( e . OriginalSource . ToString ( ) . Contains ( ".Run" ) )
				{
					if ( Flags . UseScrollView )
						fdviewer . IsEnabled = true;
					else
						doc . IsEnabled = true;
				} else
				{
					if ( Flags . UseScrollView )
					{
						// We get here when clicking on scrollbar
						fdviewer . IsEnabled = false;
						if ( fdviewer . VerticalScrollBarVisibility == ScrollBarVisibility . Visible )
						{
							fdviewer . IsEnabled = true;
							fdviewer . ReleaseMouseCapture ( );
							flowdoc . ReleaseMouseCapture ( );
							return;
						}
					} else
					{
						doc . IsEnabled = false;
						if ( doc . VerticalScrollBarVisibility == ScrollBarVisibility . Visible )
						{
							doc . IsEnabled = true;
							fdviewer . ReleaseMouseCapture ( );
							flowdoc . ReleaseMouseCapture ( );
							return;
						}
					}
				}
			} else
			{
				// NOT over scrollbar, so only allow drag
				if ( Flags . UseScrollView )
				{
					fdviewer . IsEnabled = true;
					//	doc . IsEnabled = false;
					//	if ( fdviewer . VerticalScrollBarVisibility == ScrollBarVisibility . Visible )
					//	{
					//		fdviewer . IsEnabled = true;
					//		return;
					//	}
				} else
				{
					doc . IsEnabled = true;
					//if ( doc . VerticalScrollBarVisibility == ScrollBarVisibility . Visible )
					//{
					////	doc . IsEnabled = true;
					//	//return;
					//}
				}
			}
			Button btn = sender as Button;
			string str = e.OriginalSource .ToString().ToUpper();
			if ( str . Contains ( "BORDER" ) == true )
			{
				Button_Click ( null , null );
				fdviewer . ReleaseMouseCapture ( );
				flowdoc . ReleaseMouseCapture ( );
				return;
			}
			if ( Flags . UseScrollView )
				MouseCaptured = fdviewer . CaptureMouse ( );
			else
				MouseCaptured = flowdoc . CaptureMouse ( );
			Console . WriteLine ( "Mouse CAPTURED...flowdoc_PreviewMouseLeftButtonDown()" );

			if ( Flags . UseScrollView )
				fdviewer . IsEnabled = true;
			else
				doc . IsEnabled = true;
			//e . Handled = true;
		}
		private void doc_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			fdviewer . ReleaseMouseCapture ( );
			flowdoc . ReleaseMouseCapture ( );
			Console . WriteLine ( "Mouse RELEASED...(doc_PreviewMouseLeftButtonUp" );
			//e . Handled = true;
		}
		private void scrollviewer_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			fdviewer . ReleaseMouseCapture ( );
			flowdoc . ReleaseMouseCapture ( );
			Console . WriteLine ( "Mouse RELEASED...(scrollviewerdoc_PreviewMouseLeftButtonUp" );
			//			e . Handled = true;
		}

		private void scrollviewer_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			if ( Utils . HitTestScrollBar ( sender , e ) )
			{
				fdviewer . IsEnabled = true;
				return;
			} else
				fdviewer . IsEnabled = false;

			MouseCaptured = flowdoc . CaptureMouse ( );
			Console . WriteLine ( "Mouse CAPTURED...scrollviewer_PreviewMouseLeftButtonDown()" );
			e . Handled = true;
		}
		private void scrollviewer_MouseDown ( object sender , MouseButtonEventArgs e )
		{
			if ( Utils . HitTestScrollBar ( sender , e ) )
			{
				fdviewer . IsEnabled = true;
				return;
			} else
				fdviewer . IsEnabled = false;
			MouseCaptured = flowdoc . CaptureMouse ( );
			Console . WriteLine ( "Mouse CAPTURED...scrollviewer_PreviewMouseLeftButtonDown()" );
			e . Handled = true;
		}

		#endregion Mouse handlers
		private void doc_GotFocus ( object sender , RoutedEventArgs e )
		{
			e . Handled = true;
		}

		private void Closebtn_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			this . Visibility = Visibility . Hidden;
		}

		private void dummy_Click ( object sender , RoutedEventArgs e )
		{
			;
		}

		private void fdviewer_MouseDown ( object sender , MouseButtonEventArgs e )
		{

		}

		private void checkBox_Click ( object sender , RoutedEventArgs e )
		{
			Flags . PinToBorder = !Flags . PinToBorder;
		}

		private void Exit_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			this . Visibility = Visibility . Hidden;
		}

		#region External Hook
		//code to allow this action in flowdocto be andled by an external window
		// Clever stuff really

		// Allows any other (External) window to control this via  the control 
		public event EventHandler ExecuteFlowDocSizeMethod;
		protected virtual void OnExecuteMethod ( )
		{
			if ( ExecuteFlowDocSizeMethod != null )
				ExecuteFlowDocSizeMethod ( this , EventArgs . Empty );
		}
		private void Image_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			//allows remote window to maximize /resize  this control ?
			OnExecuteMethod ( );
		}
		#endregion External Hook


	}
}
