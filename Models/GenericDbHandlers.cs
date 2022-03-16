using MyDev . Dapper;
using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Data;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Input;
using System . Windows;
using System . Collections . Specialized;
using MyDev . Views;
using System . Data . SqlClient;
using System . Diagnostics;
using Dapper;
using System . Collections;
using System . Linq . Expressions;
using System . Windows . Documents;

namespace MyDev . Models
{
	public static class GenericDbHandlers
	{
		public static Dictionary<string, string> dict = new Dictionary<string, string>();
		private static string ConnString { get; set; }

		public  static void CheckDbDomain(string DbDomain="IAN1")
		{
			if(Flags . ConnectionStringsDict == null || Flags . ConnectionStringsDict.Count == 0  )
				Utils . LoadConnectionStrings ( );
			Utils . CheckResetDbConnection ( DbDomain , out string constring );
			ConnString = constring;
			Flags . CurrentConnectionString = constring;
		}

		public static Dictionary<string , string> GetDbTableColumns ( ref ObservableCollection<GenericClass> Gencollection , ref List<string> list,  string dbName , string DbDomain= "IAN1")
		{
			// Make sure we are accessing the correct Db Domain
			CheckDbDomain ( DbDomain );
			dict = GetSpArgs ( ref Gencollection, ref list, dbName ,DbDomain );
			return dict;
		}
		private static Dictionary<string , string> GetSpArgs ( ref ObservableCollection<GenericClass> Gencollection , ref List<string> list, string dbName, string DbDomain )
		{
			string output = "";
			string errormsg="";
			int columncount = 0;
			bool IsSuccess = false;
			DataTable dt = new DataTable();
			GenericClass genclass = new GenericClass();
			Dictionary<string, string> dict = new Dictionary<string, string>();
			
			try
			{
				Gencollection . Clear ( );
				Gencollection = LoadDbAsGenericData ( ref Gencollection , ref list , "spGetTableColumns" ,dbName, DbDomain );
			} 
			catch ( Exception ex )
			{
				MessageBox . Show ( $"SQL ERROR 1125 - {ex . Message}" );
				dict . Clear ( );
				return dict;
			}

			dict . Clear ( );
			list . Clear ( );
			try
			{
				foreach ( var item in Gencollection )
				{
					GenericClass gc = new GenericClass ( );
					gc = item as GenericClass;
					dict . Add ( gc . field1 , gc . field2 );
					list . Add ( gc . field1 . ToString ( ) );
				}
			} catch ( Exception ex )
			{
				Console . WriteLine (ex.Message);
			}
			return dict;
		}
		private static ObservableCollection<GenericClass> LoadDbAsGenericData ( ref ObservableCollection<GenericClass> GenClass , ref  List<string> list, string SqlCommand , string Arguments , string DbDomain )
		{
			string result = "";
			bool IsSuccess = false;
			string arg1="", arg2="", arg3="", arg4="";
			string ConString="BankSysConnectionString";
			Dictionary<string , object> dict = new Dictionary<string, object>();
			ConString = Flags . CurrentConnectionString;
			//if(DbDomain == "IAN1")
			//	ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
			using ( IDbConnection db = new SqlConnection ( ConString ) )
			{
				try
				{
					// Use DAPPER to run  Stored Procedure
					// One or No arguments
					arg1 = Arguments;
					if ( arg1 . Contains ( "," ) )              // trim comma off
						arg1 = arg1 . Substring ( 0 , arg1 . Length - 1 );
					// Create our aguments using the Dynamic parameters provided by Dapper
					var Params = new DynamicParameters();
					if ( arg1 != "" )
						Params . Add ( "Arg1" , arg1 , DbType . String , ParameterDirection . Input , arg1 . Length );
					if ( arg2 != "" )
						Params . Add ( "Arg2" , arg2 , DbType . String , ParameterDirection . Input , arg2 . Length );
					if ( arg3 != "" )
						Params . Add ( "Arg3" , arg3 , DbType . String , ParameterDirection . Input , arg3 . Length );
					if ( arg4 != "" )
						Params . Add ( "Arg4" , arg4 , DbType . String , ParameterDirection . Input , arg4 . Length );
					// Call Dapper to get results using it's StoredProcedures method which returns
					// a Dynamic IEnumerable that we then parse via a dictionary into collection of GenericClass  records
					//		int colcount = 0, maxcols = 0;

					//		{
					//			// probably a stored procedure ?  							
					//			bool IsSuccess=false;
					//int fldcount = 0;

					//***************************************************************************************************************//
					// This returns the data from SP commands (only) in a GenericClass Structured format
					var reslt = db . Query ( SqlCommand , Params ,commandType: CommandType . StoredProcedure );
					//***************************************************************************************************************//

					if ( reslt != null )
					{
						//Although this is duplicated  with the one above we CANNOT make it a method()
						int dictcount = 0;
						dict . Clear ( );
						long zero= reslt.LongCount ();
						try
						{
							int colcount=0, fldcount=0;
							foreach ( var item in reslt )
							{
								GenericClass gc = new GenericClass();
								try
								{
									//	Create a dictionary for each row of data then add it to a GenericClass row then add row to Generics Db
									gc = DapperSupport . ParseDapperRow ( item , dict , out colcount );
									dictcount = 1;
									fldcount = dict . Count;
									if ( fldcount == 0 )
									{
										//no problem, we will get a Datatable anyway
										return GenClass;
									}
									string buffer="", tmp="";
									foreach ( var pair in dict )
									{
										try
										{
											if ( pair . Key != null && pair . Value != null )
											{
												DapperSupport . AddDictPairToGeneric ( gc , pair , dictcount++ );
												tmp = pair . Key . ToString ( ) + "=" + pair . Value . ToString ( );
												buffer += tmp + ",";
											}
										} catch ( Exception ex )
										{
											Console . WriteLine ( $"Dictionary ERROR : {ex . Message}" );
											result = ex . Message;
										}
									}
									IsSuccess = true;
									//string s = buffer . Substring (0, buffer . Length - 1 );
									//buffer = s;
									//genericlist . Add ( buffer );
								} catch ( Exception ex )
								{
									result = $"SQLERROR : {ex . Message}";
									Console . WriteLine ( result );
									return GenClass;
								}
								//										gc . ActiveColumns = dict . Count;
								//ParseListToDbRecord ( genericlist , out gc );
								GenClass . Add ( gc );
								dict . Clear ( );
								dictcount = 1;
							}
						} catch ( Exception ex )
						{
							Console . WriteLine ( $"OUTER DICT/PROCEDURE ERROR : {ex . Message}" );
							if ( ex . Message . Contains ( "not find stored procedure" ) )
							{
								result = $"SQL PARSE ERROR - [{ex . Message}]";
								//								errormsg = $"{result}";
								return GenClass;
							}
							else
							{
								long x= reslt.LongCount ();
								if ( x == ( long ) 0 )
								{
									result = $"ERROR : [{SqlCommand}] returned ZERO records... ";
									//									errormsg = $"DYNAMIC:0";
									return GenClass;
								}
								else
								{
									result = ex . Message;
									//									errormsg = $"UNKNOWN :{ex . Message}";
								}
								return GenClass;
							}
						}
					}
				} catch ( Exception ex )
				{ }
			}
			return GenClass;
		}

		#region Support methods
		private static DataTable ProcessSqlCommand ( string SqlCommand )
		{
			SqlConnection con;
			DataTable dt = new DataTable();
			string filterline = "";
			string ConString = Flags . CurrentConnectionString;
			//			string ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
			//Debug . WriteLine ( $"Making new SQL connection in DETAILSCOLLECTION,  Time elapsed = {timer . ElapsedMilliseconds}" );
			//SqlCommand += " TempDb";
			con = new SqlConnection ( ConString );
			try
			{
				Debug . WriteLine ( $"Using new SQL connection in PROCESSSQLCOMMAND" );
				using ( con )
				{
					SqlCommand cmd = new SqlCommand ( SqlCommand , con );
					SqlDataAdapter sda = new SqlDataAdapter ( cmd );
					sda . Fill ( dt );
				}
			} catch ( Exception ex )
			{
				Debug . WriteLine ( $"ERROR in PROCESSSQLCOMMAND(): Failed to load Datatable :\n {ex . Message}, {ex . Data}" );
				MessageBox . Show ( $"ERROR in PROCESSSQLCOMMAND(): Failed to load datatable\n{ex . Message}" );
			} finally
			{
				Console . WriteLine ( $" SQL data loaded from SQLCommand [{SqlCommand . ToUpper ( )}]" );
				con . Close ( );
			}
			return dt;
		}

		#endregion Support methods
	}
}
