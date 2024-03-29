﻿using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;

namespace MyDev . Views
{
	
	public class EventControl
	{
		#region ALL NEW EVENTS

		// THIS IS  HOW  TO HANDLE EVENTS RIGHT NOW  - WORKING WELL 4/5/21//
		// We no longer need to delcare a Delegate  to do this
		//Event CallBack for when Asynchronous data loading has been completed in the Various ViewModel classes

		// Main Event handlersfor when DB's load data from disk
		
		//<SelectionChangedArgs> 
		//public UIElement element;
		//public string sendername;
		//public string sendertype;
		//public int index;
		//public object data;
		//public string Empty;
		public static event EventHandler<SelectionChangedArgs> ListSelectionChanged;
		
		public static event EventHandler<LoadedEventArgs> BankDataLoaded;
		public static event EventHandler<LoadedEventArgs> CustDataLoaded;
		public static event EventHandler<LoadedEventArgs> DetDataLoaded;
		public static event EventHandler<LoadedEventArgs> GenDataLoaded;
		public static event EventHandler<LoadedEventArgs> BankListDataLoaded;
		public static event EventHandler<IndexChangedArgs> ViewerIndexChanged;
		public static event EventHandler<IndexChangedArgs> EditIndexChanged;
		public static event EventHandler<IndexChangedArgs> MultiViewerIndexChanged;
		public static event EventHandler<IndexChangedArgs> ForceEditDbIndexChanged;
		public static event EventHandler<InterWindowArgs> WindowMessage;


		//Northwind events
		public static event EventHandler<NwGridArgs> NwCustomerSelected;

		// Used to broadcast to everyone
		public static event EventHandler<LoadedEventArgs> ViewerDataUpdated;
		public static event EventHandler<LoadedEventArgs> EditDbDataUpdated;
		public static event EventHandler<LoadedEventArgs> MultiViewerDataUpdated;
		public static event EventHandler<GlobalEventArgs> GlobalDataChanged;
		public static event EventHandler<LoadedEventArgs> RecordDeleted;

		public static event EventHandler<LoadedEventArgs> TransferDataUpdated;
		public static event EventHandler<LoadedEventArgs> DbChangedExternally;


		public static event EventHandler<LoadedEventArgs> TestDataChanged;
		public static event EventHandler<NotifyAllViewersOfViewSharingStatus> ViewSharingChanged;

		// Background loading Events
		public static event EventHandler<BackgroundDataLoadeEventArgs> BackgroundBankDataLoaded;
		public static event EventHandler<BackgroundDataLoadeEventArgs> BackgroundCustDataLoaded;
		public static event EventHandler<BackgroundDataLoadeEventArgs> BackgroundDetDataLoaded;

		#endregion ALL NEW EVENTS

		public Func<int, int, int, int> IntFuncsDelegate = CalcInts;
		public Func<int, int, int> MathDelegate;

		public EventControl ( )
		{
			//Func<int, int, int, int> IntFuncsDelegate;
			IntFuncsDelegate = CalcInts;

		}

		public static Func<int, int, int> CalcAdd = (x, y) => x + y;
		public static Func<int, int, int> CalcSub = (x, y) => x - y;
		public static Func<int, int, int> CalcMult = (x, y) => x * y;
		public static Func<int, int, int> CalcDiv = (x, y) => x / y;
		public static Func<int, int, int> CalcMod = (x, y) => x % y;
		public static Func<int, int, int> CalcRem = (x, y) => x + y == 0 ? 0 : 1;
		public static int CalcInts ( int Calctype , int in1 , int in2 )
		{
			int result = 0;
			switch ( Calctype )
			{
				case 1:         //ADD
					result = in1 + in2;
					break;
				case 2:         // SUBTRACT
					result = in1 - in2;
					break;
				case 3:         //MULTIPLY
					result = in1 * in2;
					break;
				case 4:         // DIVIDE
					result = in1 / in2;
					break;
				case 5:         // RETURN MODULO
					result = in1 % in2 == 0 ? 0 : 1;
					break;
				case 6:         // return remainder
					int divisor = in1 / in2;
					int divresult = in1 % in2;
					if ( divresult == 0 )
						result = 0;
					else
						result = in1 - ( in2 * divisor );
					break;
			}
			return result;
		}


		//------------------------------//
		// INDEX CHANGE EVENTS
		//------------------------------//

		public static void TriggerWindowMessage ( object obj , InterWindowArgs e )
		{
			if ( WindowMessage != null )
				WindowMessage?.Invoke ( obj , e );
		}

		#region index changes
		public static void TriggerViewSharingChanged ( object obj , NotifyAllViewersOfViewSharingStatus e )
		{
			Console . WriteLine ( $"DEBUG : In EventControl : Sending  ViewSharingChanged EVENT trigger from {e . Sender}" );
			if ( ViewSharingChanged != null )
				ViewSharingChanged?.Invoke ( obj , e );
		}
		public static void TriggerForceEditDbIndexChanged ( object obj , IndexChangedArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  ForceEditDbIndexChanged EVENT trigger" );
			if ( ForceEditDbIndexChanged != null )
				ForceEditDbIndexChanged?.Invoke ( obj , e );
		}
		public static void TriggerEditDbIndexChanged ( object obj , IndexChangedArgs e )
		{
			//			Console.WriteLine($"DEBUG : In EventControl : Sending  EditIndexChanged EVENT trigger");
			if ( EditIndexChanged != null )
				EditIndexChanged?.Invoke ( obj , e );
		}
		public static void TriggerViewerIndexChanged ( object obj , IndexChangedArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  ViewerIndexChanged EVENT trigger (from {obj?.ToString ( )})" );
			if ( ViewerIndexChanged != null )
				ViewerIndexChanged?.Invoke ( obj , e );
		}
		public static void TriggerMultiViewerIndexChanged ( object obj , IndexChangedArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  MultiViewerIndexChanged EVENT trigger (from{obj?.ToString ( )})" );
			if ( MultiViewerIndexChanged != null )
				MultiViewerIndexChanged?.Invoke ( obj , e );
		}


		public static void TriggerNwCustomerSelected ( object obj , NwGridArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  MultiViewerIndexChanged EVENT trigger (from{obj?.ToString ( )})" );
			if ( NwCustomerSelected != null )
				NwCustomerSelected?.Invoke ( obj , e );
		}
		#endregion index changes

		//------------------------------//
		// DATA CHANGE EVENTS
		//------------------------------//

		#region data changes
		public static void TriggerGlobalDataChanged ( object obj , GlobalEventArgs e )
		{
			//			Console.WriteLine($"DEBUG : In EventControl : Sending  Global DataChanged EVENT trigger (from{obj?.ToString()})");
			if ( GlobalDataChanged != null )
				GlobalDataChanged?.Invoke ( obj , e );
		}

		public static void TriggerTestDataChanged ( object obj , LoadedEventArgs e )
		{
			//			Console.WriteLine($"DEBUG : In EventControl : Sending  TestDataChanged EVENT trigger (from{obj?.ToString()})");
			if ( TestDataChanged != null )
				TestDataChanged?.Invoke ( obj , e );
		}
		public static void TriggerViewerDataUpdated ( object obj , LoadedEventArgs e )
		{
			//			Console.WriteLine($"DEBUG : In EventControl : Sending  ViewerDataUpdated EVENT trigger (from{obj?.ToString()})");
			if ( ViewerDataUpdated != null )
				ViewerDataUpdated?.Invoke ( obj , e );
		}
		public static void TriggerEditDbDataUpdated ( object obj , LoadedEventArgs e )
		{
			//			Console.WriteLine($"DEBUG : In EventControl : Sending  EditDbDataUpdated EVENT trigger (from{obj?.ToString()})");
			if ( EditDbDataUpdated != null )
				EditDbDataUpdated?.Invoke ( obj , e );
		}
		public static void TriggerMultiViewerDataUpdated ( object obj , LoadedEventArgs e )
		{
			//			Console.WriteLine($"DEBUG : In EventControl : Sending  MultiViewerDataUpdated EVENT trigger (from{obj?.ToString()})");
			if ( MultiViewerDataUpdated != null )
				MultiViewerDataUpdated?.Invoke ( obj , e );
		}
		public static void TriggerTransferDataUpdated ( object obj , LoadedEventArgs e )
		{
			//			Console.WriteLine($"DEBUG : In EventControl : Sending  TransferDataUpdated EVENT trigger (from{obj?.ToString()})");
			if ( TransferDataUpdated != null )
				TransferDataUpdated?.Invoke ( obj , e );
		}
		#endregion data changes

		//------------------------------//
		// DATA LOADED EVENTS
		//------------------------------//		
		
		public static void TriggerListSelectionChanged ( object obj , SelectionChangedArgs e )
		{
			if ( ListSelectionChanged != null )
				ListSelectionChanged?.Invoke ( obj , e );
		}
		public static void TriggerBackgroundBankDataLoaded ( object obj , BackgroundDataLoadeEventArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  TriggerBackgroundBankDataLoaded EVENT trigger" );
			if ( BackgroundBankDataLoaded != null )
				BackgroundBankDataLoaded?.Invoke ( obj , e );
		}
		public static void TriggerBackgroundCustDataLoaded ( object obj , BackgroundDataLoadeEventArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  TriggerBackgroundCustDataLoaded EVENT trigger" );
			if ( BackgroundCustDataLoaded != null )
				BackgroundCustDataLoaded?.Invoke ( obj , e );
		}
		public static void TriggerBackgroundDetDataLoaded ( object obj , BackgroundDataLoadeEventArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  TriggerBackgroundDetDataLoaded EVENT trigger" );
			if ( BackgroundDetDataLoaded != null )
				BackgroundDetDataLoaded?.Invoke ( obj , e );
		}

		public static void TriggerBankListDataLoaded ( object obj , LoadedEventArgs e )
		{
			if ( BankListDataLoaded != null )
				BankListDataLoaded?.Invoke ( obj , e );
		}
		public static void TriggerBankDataLoaded ( object obj , LoadedEventArgs e )
		{
			//			Console.WriteLine($"DEBUG : In EventControl : Sending  BankDataLoaded EVENT trigger (from{obj?.ToString()})");
			if ( BankDataLoaded != null )
				BankDataLoaded?.Invoke ( obj , e );
		}
		public static void TriggerCustDataLoaded ( object obj , LoadedEventArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  CustDataLoaded EVENT trigger (from{obj ?. ToString ( )})" );
			if ( CustDataLoaded != null )
				CustDataLoaded?.Invoke ( obj , e );
		}
		public static void TriggerDetDataLoaded ( object obj , LoadedEventArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  DetDataLoaded EVENT trigger (from{obj? . ToString ( )})" );
			if ( DetDataLoaded != null )
				DetDataLoaded?.Invoke ( obj , e );
		}
		public static void TriggerGenDataLoaded ( object obj , LoadedEventArgs e )
		{
			//			Console . WriteLine ( $"DEBUG : In EventControl : Sending  GenDataLoaded EVENT trigger (from{obj? . ToString ( )})" );
			if ( GenDataLoaded != null )
				GenDataLoaded?.Invoke ( obj , e );
		}

		
		public static void TriggerDbChangedExternally ( object obj , LoadedEventArgs e )
		{
			if ( DbChangedExternally != null )
				DbChangedExternally?.Invoke ( obj , e );
		}
		//------------------------------//
		// DATA DELETION EVENTS
		//------------------------------//
		public static void TriggerRecordDeleted ( object obj , LoadedEventArgs e )
		{

			if ( RecordDeleted != null )
				RecordDeleted?.Invoke ( obj , e );
			Console . WriteLine ( $"DEBUG : In EventControl : Sending  RecordDeleted  EVENT trigger" );
		}

		public static void TriggerWindowLinkChanged ( object obj , LinkageChangedArgs e )
		{
			//			if ( WindowLinkChanged != null )
			//				WindowLinkChanged?.Invoke ( obj, e );
		}
		#region DEBUG utilities

		public static Delegate [ ] GetEventCount4 ( )
		{
			Delegate[] dglist2 = null;
			if ( EditIndexChanged != null )
				dglist2 = EditIndexChanged?.GetInvocationList ( );
			return dglist2;
		}

		public static Delegate [ ] GetEventCount5 ( )
		{
			Delegate[] dglist2 = null;
			if ( ViewerIndexChanged != null )
				dglist2 = ViewerIndexChanged?.GetInvocationList ( );
			return dglist2;
		}

		public static Delegate [ ] GetEventCount6 ( )
		{
			Delegate[] dglist2 = null;
			if ( EventControl . BankDataLoaded != null )
				dglist2 = BankDataLoaded?.GetInvocationList ( );
			return dglist2;
		}

		public static Delegate [ ] GetEventCount7 ( )
		{
			Delegate[] dglist2 = null;
			if ( CustDataLoaded != null )
				dglist2 = CustDataLoaded?.GetInvocationList ( );
			return dglist2;
		}

		public static Delegate [ ] GetEventCount8 ( )
		{
			Delegate[] dglist2 = null;
			if ( DetDataLoaded != null )
				dglist2 = DetDataLoaded?.GetInvocationList ( );
			return dglist2;
		}

		public static Delegate [ ] GetEventCount9 ( )
		{
			Delegate[] dglist2 = null;
			if ( RecordDeleted != null )
				dglist2 = RecordDeleted?.GetInvocationList ( );
			return dglist2;
		}
		public static Delegate [ ] GetEventCount10 ( )
		{
			Delegate[] dglist2 = null;
			if ( ViewerIndexChanged != null )
				dglist2 = ViewerIndexChanged?.GetInvocationList ( );
			return dglist2;
		}
		//public static Delegate [ ] GetEventCount11 ( )
		//{
		//	Delegate [ ] dglist2 = null;
		//	if ( DataUpdated != null )
		//		dglist2 = DataUpdated?.GetInvocationList ( );
		//	return dglist2;
		//}

		#endregion DEBUG utilities

	}
}
