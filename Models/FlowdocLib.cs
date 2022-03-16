using MyDev . UserControls;
using MyDev . ViewModels;
using MyDev . Views;

using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;

namespace MyDev . Models
{
	public class FlowdocLib : BaseViewModel
	{
		#region Flowdoc Full Properties
		private FlowDoc flowDoc;
		public FlowDoc Flowdoc
		{
			get { return flowDoc; }
			set { flowDoc = value; OnPropertyChanged ( Flowdoc . ToString ( ) ); }
		}

		private bool flowdocResizing;
		public bool FlowdocResizing
		{
			get { return flowdocResizing; }
			set { flowdocResizing = value; OnPropertyChanged ( FlowdocResizing . ToString ( ) ); }
		}
		private double flowdocFloatingTop;
		public double FlowdocFloatingTop
		{
			get { return flowdocFloatingTop; }
			set { flowdocFloatingTop = value; OnPropertyChanged ( FlowdocFloatingTop . ToString ( ) ); }
		}
		private double flowdocFloatingLeft;
		public double FlowdocFloatingLeft
		{
			get { return flowdocFloatingLeft; }
			set { flowdocFloatingLeft = value; OnPropertyChanged ( FlowdocFloatingLeft . ToString ( ) ); }
		}
		private double flowdocFloatingHeight;
		public double FlowdocFloatingHeight
		{
			get { return flowdocFloatingHeight; }
			set { flowdocFloatingHeight = value; OnPropertyChanged ( FlowdocFloatingHeight . ToString ( ) ); }
		}
		private double flowdocFloatingWidth;
		public double FlowdocFloatingWidth
		{
			get { return flowdocFloatingWidth; }
			set { flowdocFloatingWidth = value; OnPropertyChanged ( FlowdocFloatingWidth . ToString ( ) ); }
		}

		#endregion Flowdoc Full Properties

		#region Flowdoc Variables
		// These ARE USED
		public double flowdocHeight=0;
		public   double flowdocWidth=0;
		public   double flowdocTop=0;
		public   double flowdocLeft=0;
		//		private double XLeft=0;
		//		private double YTop=0;
		// This IS USED
		public   bool TvMouseCaptured = false;
		public  double newWidth=0;
		public  double newHeight =0;
		public  double YDiff = 0;
		public  double XDiff = 0;
		public  double FdLeft = 0;
		public  double FdTop =0;
		public  double FdHeight=0;
		public  double FdWidth=0;
		public  double MLeft=0;
		public  double MTop=0;
		public  bool CornerDrag = false;
		public  double FdBorderWidth=0;
		public  double FdBottom =0;
		public  double ValidTop = 0;
		public  double ValidBottom =0;
		public Thickness th = new Thickness(0,0,0,0);
		public  double CpFirstXPos=0;
		public  double CpFirstYPos=0;

		#endregion Flowdoc Variables

		#region Methods
		public  void ShowInfo (FlowDoc Flowdoc,  Canvas canvas , string line1 = "" , string clr1 = "" , string line2 = "" , string clr2 = "" , string line3 = "" , string clr3 = "" , string header = "" , string clr4 = "" , bool beep = false )
		{
			Flowdoc . ShowInfo (Flowdoc, canvas,  line1 , clr1 , line2 , clr2 , line3 , clr3 , header , clr4 , beep );
			canvas . Visibility = Visibility . Visible;
			canvas . BringIntoView ( );
			Flowdoc . Visibility = Visibility . Visible;
			Flowdoc . BringIntoView ( );
			if ( Flags . PinToBorder == true )
			{
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
			}
		}

		public void FdMsg( FlowDoc Flowdoc , Canvas canvas , string line1 = "" , string line2 = "", string line3="", bool beep = false )
		{
			string clr1 = "Black0";
			string clr2 = "Blue0";
			string clr3 = "Green2";
			string clr4 = "Red3";

			Flowdoc . ShowInfo ( Flowdoc , canvas , line1 , clr1 , line2 , clr2 , "", clr3 , "", clr4 , beep );
			canvas . Visibility = Visibility . Visible;
			canvas . BringIntoView ( );
			Flowdoc . Visibility = Visibility . Visible;
			Flowdoc . BringIntoView ( );
			if ( Flags . PinToBorder == true )
			{
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
			}
		}
		public void Flowdoc_MouseMove ( FlowDoc Flowdoc , Canvas canvas , object MovingObject , MouseEventArgs e )
		{
			// We are Resizing the Flowdoc using the mouse on the border  (Border.Name=FdBorder)
			if ( Flowdoc . BorderClicked )
			{
				// Get current sizes and position of Flowdoc windowo intilize our calculations
				if ( FdLeft == 0 )
					FdLeft = Canvas . GetLeft ( Flowdoc );
				if ( FdTop == 0 )
					FdTop = Canvas . GetTop ( Flowdoc );
				FdHeight = Flowdoc . ActualHeight;
				FdWidth = Flowdoc . ActualWidth;
				//Get mouse cursor position
				Point pt = Mouse . GetPosition (canvas );
				MLeft = pt . X;
				MTop = pt . Y;
				//				if ( th . Left == 0 )
				th = Flowdoc . FdBorder . BorderThickness;
				FdBorderWidth = th . Left * 2;
				FdBottom = FdTop + FdHeight;
				ValidTop = FdBottom - ( FdBorderWidth / 2 );
				ValidBottom = FdBottom + ( FdBorderWidth / 2 );

				if ( Flowdoc . BorderSelected == 1 )  // Top
				{
					// Top border - WORKING CORRECTLY
					Canvas . SetTop ( Flowdoc , MTop );
					YDiff = MTop - FdTop;
					FdTop = MTop;

					newHeight = FdHeight - YDiff;
					if ( newHeight < 200 )
						newHeight = 200;
					Flowdoc . Height = newHeight;
					return;
				}
				else if ( Flowdoc . BorderSelected == 2 )
				{     // Bottom border
					newHeight = MTop - FdTop;
					Flowdoc . Height = newHeight;
					return;
				}
				else if ( Flowdoc . BorderSelected == 3 )
				{
					// Left hand side border  - WORKING CORRECTLY
					XDiff = MLeft - FdLeft;
					newWidth = FdWidth - XDiff;
					if ( newWidth < 350 )
						newWidth = 350;
					Flowdoc . Width = newWidth;
					Canvas . SetLeft ( Flowdoc , MLeft );
					FdLeft = MLeft;
					return;
				}
				// Right  border or right lower corner
				else if ( Flowdoc . BorderSelected == 4 )
				{
					// Right hand side border  OR Top Right Corner 
					if ( CornerDrag || MTop - FdTop <= FdBorderWidth || FdTop - MTop <= -FdBorderWidth )
					{
						//if ( MTop >= ValidTop && MTop <= ValidBottom)
						if ( FdTop - MTop >= -FdBorderWidth )
						{
							// Top Right corner clicked	- working very well - resizes in BOTH directions
							CornerDrag = true;

							YDiff = FdTop - MTop;
							FdTop = MTop;
							Canvas . SetTop ( Flowdoc , MTop );
							// Handle Height
							newHeight = FdHeight + YDiff;
							if ( newHeight < 200 )
								newHeight = 200;
							Flowdoc . Height = FdHeight;
							// handle width
							newWidth = MLeft - FdLeft;
							FdWidth = newWidth;
							Flowdoc . Width = FdWidth;
							Flowdoc . SetValue ( FlowDoc . HeightProperty , newHeight );
							Flowdoc . SetValue ( FlowDoc . WidthProperty , newWidth );
							return;
						}
						else if (
							( ( FdTop + FdHeight - MTop >= FdBorderWidth )
							|| ( FdTop + FdHeight - MTop <= FdBorderWidth ) )
							&& ( MTop > FdBorderWidth + FdBorderWidth + 5 )
							)
						{
							// Right Border or Lower Right corner
							if ( MTop >= ValidTop - th . Left && MTop <= ValidBottom + th . Left )
							{     // WORKING 23/2//2022
								// Pointer is in lower right corner, so drag both ways
								CornerDrag = true;
								// Reset height.  Mouse is at bottom right, so pinpoint where iti s in real terms
								double mouseposY =FdTop + FdHeight;
								YDiff = MTop - FdTop - FdHeight;
								newHeight = FdHeight + YDiff;// - FdLeft;
								Flowdoc . Height = newHeight;
								if ( newHeight < 0 )
									return;
								//Reset width - WORKING
								newWidth = MLeft - FdLeft;
								Flowdoc . Width = newWidth;
								Flowdoc . SetValue ( FlowDoc . HeightProperty , newHeight );
								Flowdoc . SetValue ( FlowDoc . WidthProperty , newWidth );
								return;
							}
							else
							{     // WORKING 23/2//2022
								// Just dragging right border	    
								newWidth = MLeft - FdLeft;
								Flowdoc . Width = newWidth;
								Flowdoc . SetValue ( FlowDoc . WidthProperty , newWidth );
								CornerDrag = true;
								return;
							}
						}
						else
						{
							// just dragging right border WORKING CORRECTLY  for right border
							newWidth = MLeft - FdLeft;
							Flowdoc . Width = newWidth;
							Flowdoc . SetValue ( FlowDoc . WidthProperty , newWidth );
							CornerDrag = true;
							return;
						}
					}
					else if ( CornerDrag || MTop - FdTop + FdHeight <= FdBorderWidth )
					{
						if ( ( FdTop + FdHeight - MTop ) >= -FdBorderWidth )
						{
							if ( FdTop - MTop >= -FdBorderWidth )
							{
								// Bottom Right corner clicked
								CornerDrag = true;
								Canvas . SetTop ( Flowdoc , FdTop );
								newHeight = FdHeight - YDiff;
								if ( newHeight < 0 )
									return;
								XDiff = ( MLeft - FdWidth ) - FdLeft;
								Flowdoc . Width += XDiff;
								Flowdoc . Refresh ( );
								return;
							}
						}
					}
					else
					{
						// if we get here, we are only draggig the RIGHT Border of the window to change it's with
						newWidth = MLeft - FdLeft;// - FdLeft;
						Flowdoc . Width = newWidth;
					}
				}
				return;
			}
			else
			{
				var obj = MovingObject as FlowDoc;
				if ( MovingObject != null && e . LeftButton == MouseButtonState . Pressed && Flowdoc . BorderClicked == false )
				{
					// MOVING WINDOW around the Parent window (MDI ?)
					// Get mouse position IN FlowDoc !!
					double left = e . GetPosition ( ( MovingObject as FrameworkElement ) . Parent as FrameworkElement ) . X - CpFirstXPos ;
					double top = e . GetPosition ( ( MovingObject as FrameworkElement ) . Parent as FrameworkElement ) . Y - CpFirstYPos ;
					double trueleft = left - CpFirstXPos;
					double truetop = left - CpFirstYPos;
					if ( left >= 0 ) // && left <= canvas.ActualWidth - Flowdoc.ActualWidth)
						( MovingObject as FrameworkElement ) . SetValue ( Canvas . LeftProperty , left );
					if ( top >= 0 ) //&& top <= canvas . ActualHeight- Flowdoc. ActualHeight)
						( MovingObject as FrameworkElement ) . SetValue ( Canvas . TopProperty , top );
				}
				else if ( FlowdocResizing == false )
				{
					int x = 0;
				}
			}
		}

		public void MaximizeFlowDoc ( FlowDoc Flowdoc , Canvas canvas , EventArgs e )
		{
			//Canvas CanVas = canvas;
			// Clever "Hook" method that Allows the flowdoc to be resized to fill window
			// or return to its original size and position courtesy of the Event declard in FlowDoc
			if ( Flowdoc . BorderClicked )
			{
				return;
			}
			double height = canvas . Height;
			double width = canvas . Width;
			if ( Flowdoc . Height < canvas . Height && Flowdoc . Width < canvas . Width )
			{
				// it is in NORMAL mode right now
				// Set flowdoc size into variables for later use
				FlowdocFloatingTop = Convert . ToDouble ( Flowdoc . GetValue ( Canvas . TopProperty ) );
				FlowdocFloatingLeft = Convert . ToDouble ( Flowdoc . GetValue ( Canvas . LeftProperty ) );
				FlowdocFloatingHeight = Flowdoc . ActualHeight;
				FlowdocFloatingWidth = Flowdoc . ActualWidth;
				flowdocHeight = Flowdoc . Height;
				flowdocWidth = Flowdoc . Width;
				flowdocTop = Convert . ToDouble ( Flowdoc . GetValue ( Canvas . LeftProperty ) );
				flowdocLeft = Convert . ToDouble ( Flowdoc . GetValue ( Canvas . TopProperty ) );
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
				Flowdoc . Height = height;
				Flowdoc . Width = width;
				// save current size/position
			}
			else
			{
				// it is MAXIMIZED right now
				// We re returning it to normal position/Size
				Flowdoc . Height = FlowdocFloatingHeight;
				Flowdoc . Width = FlowdocFloatingWidth;
				if ( Flags . PinToBorder )
				{
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
				}
				else
				{
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) FlowdocFloatingLeft );
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) FlowdocFloatingTop );
				}
			}
		}
		public object Flowdoc_PreviewMouseLeftButtonDown ( object sender , FlowDoc Flowdoc , MouseButtonEventArgs e )
		{
			//In this event, we get current mouse position on the control to use it in the MouseMove event.
			Border border = e . OriginalSource as Border;
			if ( border != null )
			{
				FlowdocResizing = true;
				flowdocLeft = e . GetPosition ( Flowdoc ) . X;
				flowdocTop = e . GetPosition ( Flowdoc ) . Y;
				flowdocHeight = Flowdoc . ActualHeight;
				flowdocWidth = Flowdoc . ActualWidth;
				Flowdoc . CaptureMouse ( );
				return null;
			}
			else
			{
				Button btn = sender as Button;
				if ( btn != null )
				{
					return null;
				}
				var  str = sender as FlowDoc;
			}
			flowdocLeft = e . GetPosition ( Flowdoc ) . X;
			flowdocTop = e . GetPosition ( Flowdoc ) . Y;
			flowdocHeight = Flowdoc . ActualHeight;
			flowdocWidth = Flowdoc . ActualWidth;
			double currcursorH = e . GetPosition ( Flowdoc) . Y;
			double currcursorW = e . GetPosition ( Flowdoc) . X;
			CpFirstXPos = e . GetPosition ( sender as Control ) . X;
			CpFirstYPos = e . GetPosition ( sender as Control ) . Y;
			double FirstArrowXPos = e . GetPosition ( ( sender as Control ) . Parent as Control ) . X - CpFirstXPos;
			double FirstArrowYPos = e . GetPosition ( ( sender as Control ) . Parent as Control ) . Y - CpFirstYPos;
			Flowdoc . BorderClicked = false;
			return sender;
		}
		public object Flowdoc_MouseLeftButtonUp ( object sender , FlowDoc Flowdoc , object MovingObject , MouseButtonEventArgs e )
		{
			// Window wide  !!
			// Called  when a Flowdoc MOVE has ended
			FlowdocResizing = false;
			Flowdoc . BorderClicked = false;
			Flowdoc . BorderSelected = -1;
			CornerDrag = false;
			TvMouseCaptured = false;
			FdLeft = FdTop = th . Left = 0;
			return MovingObject = null;
		}

		#endregion Methods
	}
}
