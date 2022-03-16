// if set, Datatable is cleared and reloaded, otherwise it is not reloaded
//#define PERSISTENTDATA
#define USETASK
#undef USETASK

using Dapper;

using MyDev . Views;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Data;
using System . Data . SqlClient;
using System . Diagnostics;
using System . Linq;
using System . Threading . Tasks;
using System . Windows . Controls;
using System . Windows . Data;



namespace MyDev . ViewModels
{
	[Serializable]
	public partial class BankAccountViewModel//: BaseViewModel
	{
		#region PropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged ( string propertyName )
		{
			if ( Flags . SqlBankActive == false )
				//				this . VerifyPropertyName ( propertyName );

				if ( this . PropertyChanged != null )
				{
					var e = new PropertyChangedEventArgs ( propertyName );
					this . PropertyChanged ( this , e );
				}
		}
		#endregion PropertyChanged

		#region CONSTRUCTOR
		public BankAccountViewModel ( )
		{
			//bvm = this;
			//			BankCollectionView = CollectionViewSource . GetDefaultView ( BankViewObservableCollection );
			//BindingOperations . EnableCollectionSynchronization ( BankCollectionView , _lock );
		}
		#endregion CONSTRUCTOR

//		public static DataGrid ActiveEditDbViewer = null;

		//BankAccountViewModel bvm;
		#region STANDARD CLASS PROPERTIES SETUP

		private int id;
		private string bankno;
		private string custno;
		private int actype;
		private decimal balance;
		private decimal intrate;
		private DateTime odate;
		private DateTime cdate;

		public int Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
				OnPropertyChanged ( Id . ToString ( ) );
			}
		}

		public string BankNo
		{
			get
			{
				return bankno;
			}
			set
			{
				bankno = value;
				OnPropertyChanged ( BankNo );
			}
		}

		public string CustNo
		{
			get
			{
				return custno;
			}
			set
			{
				custno = value;
				OnPropertyChanged ( CustNo );
			}
		}

		public int AcType
		{
			get
			{
				return actype;
			}

			set
			{
				actype = value;
				OnPropertyChanged ( AcType . ToString ( ) );
			}
		}

		public decimal Balance
		{
			get
			{
				return balance;
			}

			set
			{
				balance = value;
				OnPropertyChanged ( Balance . ToString ( ) );
			}
		}

		public decimal IntRate
		{
			get
			{
				return intrate;
			}
			set
			{
				intrate = value;
				OnPropertyChanged ( IntRate . ToString ( ) );
			}
		}

		public DateTime ODate
		{
			get
			{
				return odate;
			}
			set
			{
				odate = value;
				OnPropertyChanged ( ODate . ToString ( ) );
			}
		}

		public DateTime CDate
		{
			get
			{
				return cdate;
			}
			set
			{
				cdate = value;
				OnPropertyChanged ( CDate . ToString ( ) );
			}
		}
		public string ToString ( bool full = false )
		{
			return base . ToString ( );
		}
		public override string ToString ( )
		{
			return base . ToString ( );
		}
		#endregion STANDARD CLASS PROPERTIES SETUP

		// NOT USED
		public  ObservableCollection<BankAccountViewModel> GetBankAccounts( ObservableCollection<BankAccountViewModel> collection , string SqlCommand = "" , bool Notify = false , string Caller = "" )
		{
			object  Bankcollection = new object();
			ObservableCollection<BankAccountViewModel> bvmcollection = new ObservableCollection<BankAccountViewModel>();
			//			bvmcollection = collection;
			//			IDictionary <int, string> BankDict = new Dictionary<int, string>();
			List<BankAccountViewModel> bvmlist = new List<BankAccountViewModel>();

			string ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
			using ( IDbConnection db = new SqlConnection ( ConString ) )
			{
				try
				{
					if ( SqlCommand == "" )
						bvmlist = db . Query<BankAccountViewModel> ( "Select * From BankAccount" ) . ToList ( );
					else
						bvmlist = db . Query<BankAccountViewModel> ( SqlCommand ) . ToList ( );

					if ( bvmlist . Count > 0 )
					{
						foreach ( var item in bvmlist )
						{
							bvmcollection . Add ( item );
							//Console . WriteLine ( $"SQL DAPPER Dictionary : Adding {item . BankNo} " );
							//if ( BankDict . ContainsKey ( int . Parse ( item . BankNo ) ) == false )
							//	BankDict . Add ( int . Parse ( item . BankNo ) , item . Balance . ToString ( ) );
						}
						collection = bvmcollection;
					}
					if ( Notify )
					{
						EventControl . TriggerBankDataLoaded ( null ,
							new LoadedEventArgs
							{
								CallerType = "BANKACCOUNTVIEWMODEL" ,
								CallerDb = Caller ,
								DataSource = collection ,
								RowCount = collection . Count
							} );
					}
				} catch ( Exception ex )
				{
					Console . WriteLine ( $"SQL DAPPER error : {ex . Message}, {ex . Data}" );
				}
			}
			return bvmcollection;
		}
		// NOT USED
		public static async Task<ObservableCollection<BankAccountViewModel>> GetBankDataAsObsCollectionAsync ( ObservableCollection<BankAccountViewModel> collection , string SqlCommand = "" , bool Notify = true , string Caller = "" )
		{
			//			//			object  Bankcollection = new object();
			ObservableCollection<BankAccountViewModel> bvmcollection =collection;
			//			//			List<BankAccountViewModel> bvmlist = new List<BankAccountViewModel>();
			//			string ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];

			//			using ( IDbConnection db = new SqlConnection ( ConString ) )
			//			{
			//				try
			//				{
			//					if ( SqlCommand == "" )
			//						bvmcollection = await db . QueryAsync<BankAccountViewModel> ( "Select * From BankAccount" ) . ConfigureAwait ( false ) as ObservableCollection<BankAccountViewModel>;
			//					else
			//						bvmcollection = await db . QueryAsync<BankAccountViewModel> ( SqlCommand ) . ConfigureAwait ( false ) as ObservableCollection<BankAccountViewModel>;
			//				}
			//				catch ( Exception ex )
			//				{
			//					Console . WriteLine ( $"SQL DAPPER error : {ex . Message}, {ex . Data}" );
			//				}
			//			}
			//			if ( Notify )
			//			{
			//				collection = bvmcollection;
			//				EventControl . TriggerBankDataLoaded ( null ,
			//					new LoadedEventArgs
			//					{
			//						CallerType = "SQLSERVER" ,
			//						CallerDb = Caller ,
			//						DataSource = bvmcollection ,
			//						RowCount = bvmcollection . Count
			//					} );
			//			}
			return bvmcollection;

		}
	}
}

/*
 *
 #if USETASK
			{
				int? taskid = Task.CurrentId;
				DateTime start = DateTime.Now;
				Task<bool> DataLoader = FillBankAccountDataGrid ();
				DataLoader.ContinueWith
				(
					task =>
					{
						LoadBankAccountIntoList (dtBank);
					},
					TaskScheduler.FromCurrentSynchronizationContext ()
				);
				Console.WriteLine ($"Completed AWAITED task to load BankAccount  Data via Sql\n" +
					$"task =Id is [ {taskid}], Completed status  [{DataLoader.IsCompleted}] in {(DateTime.Now - start)} Ticks\n");
			}
#else
			{
* */
