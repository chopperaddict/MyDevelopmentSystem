using Microsoft . Win32;

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
		private bool mouseCaptured  ;
		public bool MouseCaptured
		{
			get { return mouseCaptured; }
			set { mouseCaptured = value; }
		}
		private static double docHeight;
		public static double DocHeight
		{
			get { return docHeight; }
			set { docHeight = value; }
		}
		private static double docWidth;
		public static double DocWidth
		{
			get { return docWidth; }
			set { docWidth = value; }
		}
		private bool borderClicked ;
		public bool BorderClicked
		{
			get { return borderClicked; }
			set { borderClicked = value; }
		}
		private int borderSelected;
		public int BorderSelected
		{
			get { return borderSelected; }
			set { borderSelected = value; }
		}
		//private double xPos;
		//public double XPos
		//{
		//	get { return xPos; }
		//	set { xPos = value; }
		//}
		//private double yPos;
		//public double YPos
		//{
		//	get { return yPos; }
		//	set { yPos = value; }
		//}
		private bool keepSize;
		public bool KeepSize
		{
			get { return keepSize; }
			set { keepSize = value; }
		}
		private string keepSizeIcon1;
		public string KeepSizeIcon1
		{
			get { return keepSizeIcon1; }
			set { keepSizeIcon1 = value; }
		}
		private string keepSizeIcon2;
		public string KeepSizeIcon2
		{
			get { return keepSizeIcon2; }
			set { keepSizeIcon2 = value; }
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
				BorderSelected = -1;
			}
			else
			{
				fdviewer . Visibility = Visibility . Hidden;
				doc . Visibility = Visibility . Visible;
				BorderSelected = -1;
			}
			KeepSizeIcon1 = "/Icons/down arroiw red.png";
			KeepSizeIcon2 = "/Icons/up arroiw red.png";
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
			}
			else
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
			Console . WriteLine ( $"{textRange . Text }\n" );
			Console . WriteLine ( $"Text Length in Flowdoc = {textRange . Text . Length }" );
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
			}
			else if ( textRange . Text . Length < 600 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 450 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 30 );
			}
			else if ( textRange . Text . Length < 700 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 500 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 40 );
			}
			else if ( textRange . Text . Length < 800 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 600 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 50 );
			}
			else if ( textRange . Text . Length < 900 )
			{
				flowdoc . SetValue ( HeightProperty , ( double ) 700 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 60 );
			}
			else
			{
				Flags . UseFlowScrollbar = true;
				flowdoc . SetValue ( HeightProperty , ( double ) 500 + retcount * Flags . FlowdocCrMultplier );
				flowdoc . SetValue ( WidthProperty , ( double ) flowdoc . Width + 100 );
			}
			flowdoc . Height = Convert . ToDouble ( flowdoc . GetValue ( HeightProperty ) );
			flowdoc . Width = Convert . ToDouble ( flowdoc . GetValue ( WidthProperty ) );
			DocHeight = flowdoc . Height;
			if ( flowdoc . Height == 0 )
				flowdoc . Height = v1;

			flowdoc . SetValue ( HeightProperty , ( double ) flowdoc . Height );
			//FlowDoc.DocHeight = 
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
					}
					else
					{
						doc . VerticalScrollBarVisibility = ScrollBarVisibility . Visible;
						doc . IsEnabled = true;
					}
				}
				else
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
			}
			else
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
			BorderSelected = -1;

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
				BorderSelected = -1;
			}
			else if ( e . Key == Key . F8 )
			{
				fdviewer . ReleaseMouseCapture ( );
				flowdoc . ReleaseMouseCapture ( );
				//				Console . WriteLine ( "Mouse RELEASED... flowdoc_PreviewKeyDown()" );
			}
		}
		#endregion keyboard handlers

		#region Mouse handlers
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
				}
				else
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
					}
					else
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
			}
			else
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
				}
				else
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
			//			Console . WriteLine ( "Mouse CAPTURED...flowdoc_PreviewMouseLeftButtonDown()" );

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
			//			Console . WriteLine ( "Mouse RELEASED...(doc_PreviewMouseLeftButtonUp" );
			BorderClicked = false;
			//e . Handled = true;
		}
		private void scrollviewer_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			fdviewer . ReleaseMouseCapture ( );
			flowdoc . ReleaseMouseCapture ( );
			//Console . WriteLine ( "Mouse RELEASED...(scrollviewerdoc_PreviewMouseLeftButtonUp" );
			BorderClicked = false;
			//			e . Handled = true;
		}

		private void scrollviewer_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			if ( Utils . HitTestScrollBar ( sender , e ) )
			{
				fdviewer . IsEnabled = true;
				return;
			}
			else
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
			}
			else
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
			BorderSelected = -1;
		}

		private void dummy_Click ( object sender , RoutedEventArgs e )
		{
			;     // context menu click
		}

		private void Exit_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			this . Visibility = Visibility . Hidden;
			BorderSelected = -1;
		}

		#region External Hook
		//code to allow this action in flowdoc to allow smart resizing
		// Clever stuff really
		public event EventHandler ExecuteFlowDocBorderMethod;
		protected virtual void OnExecuteFlowDocBorderMethod ( EventArgs e )
		{
			if ( ExecuteFlowDocBorderMethod != null )
				ExecuteFlowDocBorderMethod ( this , e );
		}
		//code to allow this action in flowdocto be andled by an external window
		// Clever stuff really
		public event EventHandler<FlowArgs> ExecuteFlowDocResizeMethod;
		protected virtual void OnExecuteResizeMethod ( FlowArgs e )
		{
			if ( ExecuteFlowDocResizeMethod != null )
				ExecuteFlowDocResizeMethod ( this , e );
		}

		// Allows any other (External) window to control this via  the control 
		public event EventHandler ExecuteFlowDocMaxmizeMethod;
		protected virtual void OnExecuteMethod ( )
		{
			if ( ExecuteFlowDocMaxmizeMethod != null )
				ExecuteFlowDocMaxmizeMethod ( this , EventArgs . Empty );
		}
		private void Image_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			//allows remote window to maximize /resize  this control ?
			OnExecuteMethod ( );
		}

		#endregion External Hook

		private void Border_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			//	BorderClicked = false;
			Border bd = sender as Border;
			if ( Utils . HitTestBorder ( bd, e ) )
			{
				// Over the Border, so let user resize contents
				BorderClicked = true;

				// Mouse Horizontal (X) position
				double left = e . GetPosition ( (FdBorder  as FrameworkElement ) . Parent as FrameworkElement ) . X ;
				double height = this.ActualHeight;
				// Mouse Vertical (Y) position
				double MTop = e . GetPosition ( (FdBorder as FrameworkElement ) . Parent as FrameworkElement ) . Y;
				double MBottom = MTop + this.ActualHeight;

				//Console . WriteLine ( $"Border Hit : Left {left}, Top {MTop}\nWidth {this . ActualWidth}, Height {this . ActualHeight}" );
				double ValidTopT = FdBorder.BorderThickness.Left ;
				double ValidBottomT = this.ActualHeight + FdBorder.BorderThickness.Left ;
				double ValidTopB = MBottom -  (FdBorder.BorderThickness.Left  * 2);
				double ValidBottomB = MBottom + ( FdBorder.BorderThickness.Left  * 2);

				if ( MTop <= ValidTopT && MTop >=  0)
				{
					// Top
					BorderSelected = 1;
					if  ( this . ActualWidth - left < 10 )
						BorderSelected = 4;
				}
				else if ( MBottom >= ValidTopB && MBottom <= ValidBottomB && MTop > height - 20 )
				{
					// Bottom
					BorderSelected = 2;
					if ( this . ActualWidth - left < 10 )
						BorderSelected = 4;
				}
				else if ( left < 10 )
				{
					// Left
					BorderSelected = 3;
				}
				else if ( this . ActualWidth - left < 10 )
				{
					//Right
					BorderSelected = 4;
				}
				Console . WriteLine ( $"BorderSelected = {BorderSelected}" );
				ExecuteFlowDocBorderMethod ( this , EventArgs . Empty );
			}
		}

		private void KeepSize_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			KeepSize = !KeepSize;
			if ( KeepSize == true )
			{
				KeepIcon . Source = new BitmapImage ( new Uri ( KeepSizeIcon1 , UriKind . Relative ) );
				SaveLabel . Content = "Using Saved Height =";
			}
			else
			{
				KeepIcon . Source = new BitmapImage ( new Uri ( KeepSizeIcon2 , UriKind . Relative ) );
				SaveLabel . Content = "Using Auto Height =";
			}
		}

		private void Border_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			BorderClicked = false;
		}

		private void FdBorder_MouseMove ( object sender , MouseEventArgs e )
		{
			// Flowdoc is being resized
			if ( BorderClicked )
			{
			}
		}

		private void FlowdocBorder_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			Border border = sender as Border;
			if ( border . Name == "FdBorder" )
				BorderClicked = true;
		}
	}
	public class FlowArgs : EventArgs
	{
		public double Height
		{
			get; set;
		}
		public double Width
		{
			get; set;
		}
		public double CTop
		{
			get; set;
		}
		public double CLeft
		{
			get; set;
		}
		public double Xpos
		{
			get; set;
		}
		public double Ypos
		{
			get; set;
		}
		public bool BorderClicked
		{
			get; set;
		}
	}
}
