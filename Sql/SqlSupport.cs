﻿using Dapper;

using MyDev . Dapper;
using MyDev . Models;
using MyDev . ViewModels;
using MyDev . Views;

using System;
using System . Collections;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Data;
using System . Data . SqlClient;
using System . Diagnostics;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;

namespace MyDev . SQL
{
	public class SqlSupport
	{
		//********************************************************************************************************************************************************************************//
		#region Wrapper methods to Fetch data & return an Observablecollection
		public static ObservableCollection<BankAccountViewModel> LoadBank ( string Sqlcommand , int max = 0 , bool Notify = false , bool isMultiMode = false )
		{
			DataTable dt = LoadBankData( Sqlcommand ,max, isMultiMode);
			ObservableCollection<BankAccountViewModel> bvm = LoadBankCollection ( dt , Notify);
			return bvm;
		}
		public static ObservableCollection<CustomerViewModel> LoadCustomer ( string Sqlcommand , int max = 0 , bool Notify = false , bool isMultiMode = false )
		{
			DataTable dt = LoadCustData( Sqlcommand ,max, isMultiMode);
			ObservableCollection< CustomerViewModel > cvm = LoadCustomerCollection ( dt , Notify);
			return cvm;
		}
		public static ObservableCollection<DetailsViewModel> LoadDetails ( string Sqlcommand , int max = 0 , bool Notify = false , bool isMultiMode = false )
		{
			DataTable dt = LoadDetailsData( Sqlcommand ,max, isMultiMode);
			ObservableCollection<DetailsViewModel> dvm = LoadDetailsCollection ( dt , Notify);
			return dvm;
		}
		public static ObservableCollection<GenericClass> LoadGeneric ( string Sqlcommand , out string ResultString , int max = 0 , bool Notify = false , bool isMultiMode = false )
		{
			ObservableCollection<GenericClass> generics = new ObservableCollection<GenericClass>();
			ExecuteStoredProcedure ( Sqlcommand ,
			generics ,
			out ResultString ,
			"" ,
			"" ,
			null ,
			false );
			return generics;
		}
		#endregion Wrapper methods to Fetch data & load into collections

		//********************************************************************************************************************************************************************************//
		// DataTable loading methods
		#region Data loading to DataTable via Sql
		public static DataTable LoadBankData ( string Sqlcommand , int max = 0 , bool isMultiMode = false )
		//Load data from Sql Server
		{
			DataTable dtBank = new DataTable();
			try
			{
				SqlConnection con;
				string commandline="";
				string ConString = Flags . CurrentConnectionString;
				//ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
				con = new SqlConnection ( ConString );
				using ( con )
				{
					if ( Flags . IsMultiMode )
					{
						// Create a valid Query Command string including any active sort ordering
						commandline = $"SELECT * FROM BANKACCOUNT WHERE CUSTNO IN "
							+ $"(SELECT CUSTNO FROM BANKACCOUNT "
							+ $" GROUP BY CUSTNO"
							+ $" HAVING COUNT(*) > 1) ORDER BY ";

						//	commandline = Utils . GetDataSortOrder ( commandline );
					}
					else if ( Flags . FilterCommand != "" )
					{
						commandline = Flags . FilterCommand;
					}
					else
					{
						// Create a valid Query Command string including any active sort ordering
						commandline = Sqlcommand;
						//	commandline = Utils . GetDataSortOrder ( commandline );
					}
					SqlCommand cmd = new SqlCommand ( commandline, con );
					SqlDataAdapter sda = new SqlDataAdapter ( cmd );
					if ( dtBank == null )
						dtBank = new DataTable ( );
					sda . Fill ( dtBank );
				}
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"Failed to load Bank Details - {ex . Message}, {ex . Data}" );
				return null;
			}
			return dtBank;
		}

		public static DataTable LoadCustData ( string Sqlcommand , int max = 0 , bool isMultiMode = false )
		//Load data from Sql Server
		{
			SqlConnection con;
			DataTable dtCust = new DataTable();
			string ConString = Flags . CurrentConnectionString;
			//			ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
			//			Debug . WriteLine ( $"Making new SQL connection in CUSTCOLLECTION" );
			con = new SqlConnection ( ConString );
			//			Debug . WriteLine ( $"CUSTCOLLECTION : No connecting to load SQL..." );
			try
			{
				using ( con )
				{
					//					Debug . WriteLine ( $"Loading dtCust in CUSTCOLLECTION" );
					string commandline = "";

					if ( Flags . IsMultiMode )
					{
						// Create a valid Query Command string including any active sort ordering
						commandline = $"SELECT * FROM CUSTOMER WHERE CUSTNO IN "
							  + $"(SELECT CUSTNO FROM CUSTOMER  "
							  + $" GROUP BY CUSTNO"
							  + $" HAVING COUNT(*) > 1) ORDER BY ";
						//commandline = Utils . GetDataSortOrder ( commandline );
					}
					else if ( Flags . FilterCommand != "" )
					{
						commandline = Flags . FilterCommand;
					}
					else
					{
						// Create a valid Query Command string including any active sort ordering
						if ( max == 0 ) //&& bottomrec == 0 && toprec == 0 )
						{
							commandline = Sqlcommand;
							//							commandline = Utils . GetDataSortOrder ( commandline );
						}
						//else if ( max > 0)// && bottomrec > 0 && toprec > 0 )
						//{
						//	commandline = $"Select top ({max})  Id, BankNo, CustNo ,AcType, FName, LName, Addr1, Addr2, Town, County, PCode, Phone, Mobile, Dob, ODate, CDate  from Customer";
						//	commandline = Utils . GetDataSortOrder ( commandline );
						//}
						//else
						//{
						//	commandline = "Select * from Customer  order by ";
						//	commandline = Utils . GetDataSortOrder ( commandline );
						//}
					}
					SqlCommand cmd = new SqlCommand ( commandline, con );
					SqlDataAdapter sda = new SqlDataAdapter ( cmd );
					sda . Fill ( dtCust );
					//					Debug . WriteLine ( $"CUSTOMERS : dtCust loaded [{dtCust . Rows . Count}] ...." );
				}
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"Failed to load Customer Details - {ex . Message}, {ex . Data}" );
				//MessageBox . Show ( $"Failed to load Customer Details - {ex . Message}" );
				return dtCust;
			}
			finally
			{
				con . Close ( );
			}

			return dtCust;
		}

		public static DataTable LoadDetailsData ( string Sqlcommand , int max = 0 , bool isMultiMode = false )
		{
			SqlConnection con;
			string filterline = "";
			DataTable dtDetails = new DataTable();
			string ConString = Flags . CurrentConnectionString;
			//			ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
			//			Debug . WriteLine ( $"Making new SQL connection in DETAILSCOLLECTION,  Time elapsed = {timer . ElapsedMilliseconds}" );

			con = new SqlConnection ( ConString );
			try
			{
				Debug . WriteLine ( $"Using new SQL connection in DETAILSCOLLECTION" );
				using ( con )
				{
					if ( Flags . IsMultiMode )
					{
						// Create a valid Query Command string including any active sort ordering
						filterline = $"SELECT * FROM SECACCOUNTS WHERE CUSTNO IN "
							+ $"(SELECT CUSTNO FROM SECACCOUNTS  "
							+ $" GROUP BY CUSTNO"
							+ $" HAVING COUNT(*) > 1) ORDER BY ";
						//						filterline = Utils . GetDataSortOrder ( filterline );
					}
					else if ( Flags . FilterCommand != "" )
					{
						filterline = Flags . FilterCommand;
					}
					else
					{
						// Create a valid Query Command string including any active sort ordering
						filterline = Sqlcommand;
						//						filterline = Utils . GetDataSortOrder ( filterline );
					}
					SqlCommand cmd = new SqlCommand ( filterline , con );
					SqlDataAdapter sda = new SqlDataAdapter ( cmd );
					sda . Fill ( dtDetails );
				}
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"DETAILS : ERROR in LoadDetailsDataSql(): Failed to load Details Details - {ex . Message}, {ex . Data}" );
				MessageBox . Show ( $"DETAILS : ERROR in LoadDetailsDataSql(): Failed to load Details Details - {ex . Message}, {ex . Data}" );
			}
			finally
			{
				//				Console . WriteLine ( $" SQL data loaded : Time elapsed = {timer . ElapsedMilliseconds}" );
				con . Close ( );
			}
			return dtDetails;
		}

		public static DataTable LoadGenericData ( string Sqlcommand , string DbName="", int max = 0 , bool isMultiMode = false )
		{
			SqlConnection con;
			string filterline = "";
			DataTable dtGeneric= new DataTable();

			// This resets the current database connection - should be used anywhere that We switch between databases in Sql Server
			if ( Utils . CheckResetDbConnection ( "IAN1" , out string constring ) == false )
			{
				Console . WriteLine ( $"Failed to set connection string for {DbName . ToUpper ( )} Db" );
				return null;
			}
			//GenericDbHandlers . CheckDbDomain ( "IAN1" );
			filterline = Sqlcommand;
			string ConString = Flags . CurrentConnectionString;

			con = new SqlConnection ( ConString );
			try
			{
				Debug . WriteLine ( $"Using new SQL connection in LOADGENERICDATA" );
				using ( con )
				{
					SqlCommand cmd = new SqlCommand ( filterline , con );
					SqlDataAdapter sda = new SqlDataAdapter ( cmd );
					sda . Fill ( dtGeneric );
				}
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"GENERIC : ERROR in LoadGenericData(): Failed to load Generic Data :  {ex . Message}, {ex . Data}" );
				MessageBox . Show ( $"GENERIC: ERROR in LoadGenericData(): Failed to load Generic Data : {ex . Message}, {ex . Data}" );
			}
			finally
			{
				//				Console . WriteLine ( $" SQL data loaded : Time elapsed = {timer . ElapsedMilliseconds}" );
				con . Close ( );
			}
			return dtGeneric;
		}
		#endregion Data loading to DataTable
		//********************************************************************************************************************************************************************************//
		#region Datatable loading from Datatables to collections

		public static ObservableCollection<BankAccountViewModel> LoadBankCollection ( DataTable dtBank , bool Notify = false )
		{
			int count = 0;
			ObservableCollection < BankAccountViewModel >     bvm = new ObservableCollection<BankAccountViewModel>();
			try
			{
				//				object bptr = new object ( );
				for ( int i = 0 ; i < dtBank . Rows . Count ; i++ )
				{
					bvm . Add ( new BankAccountViewModel
					{
						Id = Convert . ToInt32 ( dtBank . Rows [ i ] [ 0 ] ) ,
						BankNo = dtBank . Rows [ i ] [ 1 ] . ToString ( ) ,
						CustNo = dtBank . Rows [ i ] [ 2 ] . ToString ( ) ,
						AcType = Convert . ToInt32 ( dtBank . Rows [ i ] [ 3 ] ) ,
						Balance = Convert . ToDecimal ( dtBank . Rows [ i ] [ 4 ] ) ,
						IntRate = Convert . ToDecimal ( dtBank . Rows [ i ] [ 5 ] ) ,
						ODate = Convert . ToDateTime ( dtBank . Rows [ i ] [ 6 ] ) ,
						CDate = Convert . ToDateTime ( dtBank . Rows [ i ] [ 7 ] ) ,
					} );
					count = i;
				}
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"BANK : SQL Error in BankCollection(351) load function : {ex . Message}, {ex . Data}" );
				MessageBox . Show ( $"BANK : SQL Error in BankCollection (351) load function : {ex . Message}, {ex . Data}" );
			}
			finally
			{
				// This is ONLY called  if a requestor specifies the argument as TRUE
				if ( Notify )
				{
					Application . Current . Dispatcher . Invoke ( ( ) =>
					EventControl . TriggerBankDataLoaded ( null ,
						new LoadedEventArgs
						{
							CallerType = "SQLSUPPORT" ,
							DataSource = bvm ,
							RowCount = bvm . Count
						} )
					);
				}
			}
			return bvm;
		}
		public static ObservableCollection<CustomerViewModel> LoadCustomerCollection ( DataTable dtCust , bool Notify = false )
		{
			int count = 0;
			ObservableCollection<CustomerViewModel> cvm = new ObservableCollection<CustomerViewModel>();
			try
			{
				for ( int i = 0 ; i < dtCust . Rows . Count ; i++ )
				{
					cvm . Add ( new CustomerViewModel
					{
						Id = Convert . ToInt32 ( dtCust . Rows [ i ] [ 0 ] ) ,
						CustNo = dtCust . Rows [ i ] [ 1 ] . ToString ( ) ,
						BankNo = dtCust . Rows [ i ] [ 2 ] . ToString ( ) ,
						AcType = Convert . ToInt32 ( dtCust . Rows [ i ] [ 3 ] ) ,
						FName = dtCust . Rows [ i ] [ 4 ] . ToString ( ) ,
						LName = dtCust . Rows [ i ] [ 5 ] . ToString ( ) ,
						Addr1 = dtCust . Rows [ i ] [ 6 ] . ToString ( ) ,
						Addr2 = dtCust . Rows [ i ] [ 7 ] . ToString ( ) ,
						Town = dtCust . Rows [ i ] [ 8 ] . ToString ( ) ,
						County = dtCust . Rows [ i ] [ 9 ] . ToString ( ) ,
						PCode = dtCust . Rows [ i ] [ 10 ] . ToString ( ) ,
						Phone = dtCust . Rows [ i ] [ 11 ] . ToString ( ) ,
						Mobile = dtCust . Rows [ i ] [ 12 ] . ToString ( ) ,
						Dob = Convert . ToDateTime ( dtCust . Rows [ i ] [ 13 ] ) ,
						ODate = Convert . ToDateTime ( dtCust . Rows [ i ] [ 14 ] ) ,
						CDate = Convert . ToDateTime ( dtCust . Rows [ i ] [ 15 ] )
					} );
					count = i;
					//Console . WriteLine ($"{count}");
				}
				//Debug . WriteLine ( $"CUSTOMER : Sql data loaded into Customer ObservableCollection \"Custinternalcollection\" [{count}] ...." );
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"CUSTOMERS : ERROR {ex . Message} + {ex . Data} ...." );
				cvm = null;
			}
			finally
			{
				if ( Notify && count > 0 )
				{
					Console . WriteLine ( $"Triggering event CustDataLoaded with {cvm . Count}" );
					Application . Current . Dispatcher . Invoke ( ( ) =>
					EventControl . TriggerCustDataLoaded ( null ,
						  new LoadedEventArgs
						  {
							  CallerType = "SQLSUPPPORT" ,
							  DataSource = cvm ,
							  RowCount = cvm . Count
						  } )
					);
				}
			}
			Console . WriteLine ( $"Customers Db Total = {cvm?.Count}" );
			return cvm;
		}
		public static ObservableCollection<DetailsViewModel> LoadDetailsCollection ( DataTable dtDetails , bool Notify = false )
		{
			int count = 0;
			ObservableCollection < DetailsViewModel > dvm = new ObservableCollection<DetailsViewModel>();
			try
			{
				Console . WriteLine ( $" Loading Datable with {dtDetails . Rows . Count} records" );
				dvm . Clear ( );
				for ( int i = 0 ; i < dtDetails . Rows . Count ; i++ )
				{
					dvm . Add ( new DetailsViewModel
					{
						Id = Convert . ToInt32 ( dtDetails . Rows [ i ] [ 0 ] ) ,
						BankNo = dtDetails . Rows [ i ] [ 1 ] . ToString ( ) ,
						CustNo = dtDetails . Rows [ i ] [ 2 ] . ToString ( ) ,
						AcType = Convert . ToInt32 ( dtDetails . Rows [ i ] [ 3 ] ) ,
						Balance = Convert . ToDecimal ( dtDetails . Rows [ i ] [ 4 ] ) ,
						IntRate = Convert . ToDecimal ( dtDetails . Rows [ i ] [ 5 ] ) ,
						ODate = Convert . ToDateTime ( dtDetails . Rows [ i ] [ 6 ] ) ,
						CDate = Convert . ToDateTime ( dtDetails . Rows [ i ] [ 7 ] ) ,
					} );
					count = i;
				}
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"DETAILS : ERROR in  LoadDetCollection() : loading Details into ObservableCollection \"DetCollection\" : [{ex . Message}] : {ex . Data} ...." );
				MessageBox . Show ( $"DETAILS : ERROR in  LoadDetCollection() : loading Details into ObservableCollection \"DetCollection\" : [{ex . Message}] : {ex . Data} ...." );
				return null;
			}
			finally
			{
				if ( Notify )
				{
					EventControl . TriggerDetDataLoaded ( null ,
						new LoadedEventArgs
						{
							CallerType = "SQLSERVER" ,
							DataSource = ( object ) dvm ,
							RowCount = dvm . Count
						} );
				}
			}
			Console . WriteLine ( $" DETAILS DB Loading () ALL FINISHED :  Records = [{dvm . Count}]" );
			return dvm;
		}
		public static ObservableCollection<GenericClass> LoadGenericCollection ( DataTable dtgeneric , bool Notify = false )
		{
			int count = 0;
			ObservableCollection < GenericClass > gvm = new ObservableCollection<GenericClass>();
			try
			{
				Console . WriteLine ( $" Loading Datable with {dtgeneric . Rows . Count} records" );
				gvm . Clear ( );
				int colcount = dtgeneric.Columns.Count;
				if ( colcount > 20 )
					colcount = 20;
				for ( int i = 0 ; i < dtgeneric . Rows . Count ; i++ )
				{
					switch ( colcount )
					{
						case 20:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
								field13 = dtgeneric?.Rows [ i ] [ 12 ] . ToString ( ) ,
								field14 = dtgeneric?.Rows [ i ] [ 13 ] . ToString ( ) ,
								field15 = dtgeneric?.Rows [ i ] [ 14 ] . ToString ( ) ,
								field16 = dtgeneric?.Rows [ i ] [ 15 ] . ToString ( ) ,
								field17 = dtgeneric?.Rows [ i ] [ 16 ] . ToString ( ) ,
								field18 = dtgeneric?.Rows [ i ] [ 17 ] . ToString ( ) ,
								field19 = dtgeneric?.Rows [ i ] [ 18 ] . ToString ( ) ,
								field20 = dtgeneric?.Rows [ i ] [ 19 ] . ToString ( )
							} );
							break;
						case 19:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
								field13 = dtgeneric?.Rows [ i ] [ 12 ] . ToString ( ) ,
								field14 = dtgeneric?.Rows [ i ] [ 13 ] . ToString ( ) ,
								field15 = dtgeneric?.Rows [ i ] [ 14 ] . ToString ( ) ,
								field16 = dtgeneric?.Rows [ i ] [ 15 ] . ToString ( ) ,
								field17 = dtgeneric?.Rows [ i ] [ 16 ] . ToString ( ) ,
								field18 = dtgeneric?.Rows [ i ] [ 17 ] . ToString ( ) ,
								field19 = dtgeneric?.Rows [ i ] [ 18 ] . ToString ( ) ,
							} );
							break;
						case 18:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
								field13 = dtgeneric?.Rows [ i ] [ 12 ] . ToString ( ) ,
								field14 = dtgeneric?.Rows [ i ] [ 13 ] . ToString ( ) ,
								field15 = dtgeneric?.Rows [ i ] [ 14 ] . ToString ( ) ,
								field16 = dtgeneric?.Rows [ i ] [ 15 ] . ToString ( ) ,
								field17 = dtgeneric?.Rows [ i ] [ 16 ] . ToString ( ) ,
								field18 = dtgeneric?.Rows [ i ] [ 17 ] . ToString ( ) ,
							} );
							break;
						case 17:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
								field13 = dtgeneric?.Rows [ i ] [ 12 ] . ToString ( ) ,
								field14 = dtgeneric?.Rows [ i ] [ 13 ] . ToString ( ) ,
								field15 = dtgeneric?.Rows [ i ] [ 14 ] . ToString ( ) ,
								field16 = dtgeneric?.Rows [ i ] [ 15 ] . ToString ( ) ,
								field17 = dtgeneric?.Rows [ i ] [ 16 ] . ToString ( ) ,
							} );
							break;
						case 16:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
								field13 = dtgeneric?.Rows [ i ] [ 12 ] . ToString ( ) ,
								field14 = dtgeneric?.Rows [ i ] [ 13 ] . ToString ( ) ,
								field15 = dtgeneric?.Rows [ i ] [ 14 ] . ToString ( ) ,
								field16 = dtgeneric?.Rows [ i ] [ 15 ] . ToString ( ) ,
							} );
							break;
						case 15:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
								field13 = dtgeneric?.Rows [ i ] [ 12 ] . ToString ( ) ,
								field14 = dtgeneric?.Rows [ i ] [ 13 ] . ToString ( ) ,
								field15 = dtgeneric?.Rows [ i ] [ 14 ] . ToString ( ) ,
							} );
							break;
						case 14:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
								field13 = dtgeneric?.Rows [ i ] [ 12 ] . ToString ( ) ,
								field14 = dtgeneric?.Rows [ i ] [ 13 ] . ToString ( ) ,
							} );
							break;
						case 13:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
								field13 = dtgeneric?.Rows [ i ] [ 12 ] . ToString ( ) ,
							} );
							break;
						case 12:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
								field12 = dtgeneric?.Rows [ i ] [ 11 ] . ToString ( ) ,
							} );
							break;
						case 11:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
								field11 = dtgeneric?.Rows [ i ] [ 10 ] . ToString ( ) ,
							} );
							break;
						case 10:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
								field10 = dtgeneric?.Rows [ i ] [ 9 ] . ToString ( ) ,
							} );
							break;
						case 9:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
								field9 = dtgeneric?.Rows [ i ] [ 8 ] . ToString ( ) ,
							} );
							break;
						case 8:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
								field8 = dtgeneric?.Rows [ i ] [ 7 ] . ToString ( ) ,
							} );
							break;
						case 7:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
								field7 = dtgeneric?.Rows [ i ] [ 6 ] . ToString ( ) ,
							} );
							break;
						case 6:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
								field6 = dtgeneric?.Rows [ i ] [ 5 ] . ToString ( ) ,
							} );
							break;
						case 5:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
								field5 = dtgeneric?.Rows [ i ] [ 4 ] . ToString ( ) ,
							} );
							break;
						case 4:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
								field4 = dtgeneric?.Rows [ i ] [ 3 ] . ToString ( ) ,
							} );
							break;
						case 3:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
								field3 = dtgeneric?.Rows [ i ] [ 2 ] . ToString ( ) ,
							} );
							break;
						case 2:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
								field2 = dtgeneric?.Rows [ i ] [ 1 ] . ToString ( ) ,
							} );
							break;
						case 1:
							gvm . Add ( new GenericClass
							{
								field1 = dtgeneric?.Rows [ i ] [ 0 ] . ToString ( ) ,
							} );
							break;
					}
					count = i;
				}
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"GENERICS : ERROR in  LoadGenCollection() : loading Generic into ObservableCollection \"GenCollection\" : [{ex . Message}] : {ex . Data} ...." );
				//MessageBox . Show ( $"DETAILS : ERROR in  LoadDetCollection() : loading Details into ObservableCollection \"DetCollection\" : [{ex . Message}] : {ex . Data} ...." );
				//return null;
			}
			finally
			{
				if ( Notify )
				{
					EventControl . TriggerGenDataLoaded ( null ,
						new LoadedEventArgs
						{
							CallerType = "SQLSERVER" ,
							DataSource = ( object ) gvm ,
							RowCount = gvm . Count
						} );
				}
			}
			Console . WriteLine ( $" DETAILS DB Loading () ALL FINISHED :  Records = [{gvm . Count}]" );
			return gvm;
		}

		#endregion Datatable loading from tables via SQL
		//********************************************************************************************************************************************************************************//
		#region	PERFORMSQLEXECUTECOMMAND

		#region Stored Procedures execution
		public static int PerformSqlExecuteCommand ( string SqlCommand , string [ ] args , out string err )
		//--------------------------------------------------------------------------------------------------------------------------------------------------------
		{
			//####################################################################################//
			// Handles running a dapper stored procedure call with transaction support & thrws exceptions back to caller
			//####################################################################################//
			int gresult = -1;
			string Con= Flags . CurrentConnectionString;
			//			string Con = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
			SqlConnection sqlCon=null;
			err = "";

			try
			{
				using ( sqlCon = new SqlConnection ( Con ) )
				{
					var parameters = new DynamicParameters();
					sqlCon . Open ( );
					using ( var tran = sqlCon . BeginTransaction ( ) )
					{
						if ( ( SqlCommand . ToUpper ( ) == "SPINSERTSPECIFIEDROW" || SqlCommand . ToUpper ( ) == "SPCREATETABLE" || SqlCommand . ToUpper ( ) == "SPDROPTABLE" ) && args . Length > 0 )
						{
							if ( args [ 0 ] != "" )
								parameters . Add ( "Tablename" , args [ 0 ] , DbType . String , ParameterDirection . Input , args [ 0 ] . Length );
							if ( args [ 1 ] != "" )
								parameters . Add ( "cmd" , args [ 1 ] , DbType . String , ParameterDirection . Input , args [ 1 ] . Length );
							if ( args [ 2 ] != "" )
								parameters . Add ( "Values" , args [ 2 ] , DbType . String , ParameterDirection . Input , args [ 2 ] . Length );

							gresult = sqlCon . Execute ( @SqlCommand , parameters , commandType: CommandType . StoredProcedure , transaction: tran );
						}
						else
						{
							// Perform the sql command requested
							//							var parameters = "";
							gresult = sqlCon . Execute ( @SqlCommand , parameters , commandType: CommandType . StoredProcedure , transaction: tran );// as IEnumerable<GenericClass>;
																											 //var result  = sqlCon . Query( SqlCommand ,
																											 //  args,null,false, null,
																											 //   CommandType.StoredProcedure).ToList();
						}
						// Commit the transaction
						tran . Commit ( );
					}
				}
			}
			catch ( Exception ex )
			{
				Console . WriteLine ( $"Error {ex . Message}, {ex . Data}" );
				err = $"Error {ex . Message}";
			}

			Utils . trace ( );

			return gresult;
		}
		#endregion Stored Procedures execution
		#endregion
		//********************************************************************************************************************************************************************************//

		public static ObservableCollection<GenericClass> ExecuteStoredProcedure ( string SqlCommand ,
			ObservableCollection<GenericClass> generics ,
			out string ResultString ,
			string DbName = "" ,
			string Arguments = "" ,
			RoutedEventArgs e = null ,
			bool displayData = false )

		{
			ResultString = "";
			string SavedValue = SqlCommand;
#pragma warning disable CS0219 // The variable 'dbnametoopen' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'command' is assigned but its value is never used
			string command = "", dbnametoopen = "";
#pragma warning restore CS0219 // The variable 'command' is assigned but its value is never used
#pragma warning restore CS0219 // The variable 'dbnametoopen' is assigned but its value is never used
			string errormsg="";
#pragma warning disable CS0219 // The variable 'WhereClause' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'OrderByClause' is assigned but its value is never used
			string  WhereClause="", OrderByClause="";
#pragma warning restore CS0219 // The variable 'OrderByClause' is assigned but its value is never used
#pragma warning restore CS0219 // The variable 'WhereClause' is assigned but its value is never used
#pragma warning disable CS0219 // The variable 'CheckingArgsOnly' is assigned but its value is never used
			bool CheckingArgsOnly = false;
#pragma warning restore CS0219 // The variable 'CheckingArgsOnly' is assigned but its value is never used
			int totalcolumns = 0;
			ObservableCollection<BankAccountViewModel> bvmparam = new ObservableCollection<BankAccountViewModel>();
			Dictionary <string, object>dict = new Dictionary<string, object>();
#pragma warning disable CS0219 // The variable 'DbResult' is assigned but its value is never used
			IEnumerable DbResult=null;
#pragma warning restore CS0219 // The variable 'DbResult' is assigned but its value is never used
			//============
			// Sanity checks
			//============
			// If it is a CopyDb Procedure, bale out, use the Copy button
			if ( SqlCommand . ToUpper ( ) . Contains ( "SPCOPYDB" ) )
			{
				MessageBox . Show ( $"Please use the 'Copy Db' button at top right to perform this operation.." , "Input error" , MessageBoxButton . OK );
				return null;
			}
			if ( SavedValue == "spGetFullSchema" && Arguments == "FULL" )
			{
				Arguments = "";
			}
			{
				//if ( SavedValue == "spGetSpecificSchema" )
				//{
				//	if ( Arguments == "" )
				//	{
				//		if ( SPArgs . Text == "Enter arguments here ..." )
				//		{
				//			MessageBox . Show ( "You need to specify the SP that you want to view the ARG's\nfor in the field 'Enter arguments here ...'??" ,
				//				     "Details required" , MessageBoxButton . OK , MessageBoxImage . Warning );
				//			return null;
				//		}
				//		var reslt = MessageBox . Show ("Click Yes to see ALL occurences of @ARG, No for just the header declarations?","Details required", MessageBoxButton.YesNo, MessageBoxImage.Question  );
				//		if ( reslt == MessageBoxResult . Yes )
				//		{
				//			showall = true;
				//			CheckingArgsOnly = true;
				//		}
				//	}
				//	else if ( DbName != "COMMENTS" )
				//	{
				//		//var reslt = MessageBox . Show ( "This will get the @ARGS for the currently selected SP in the Combo above\nClick Yes to proceed or No to select a  different SP in the combo'??" , "Confirmation required" ,
				//		//	  MessageBoxButton.YesNoCancel, MessageBoxImage .Question);
				//		//if ( reslt == MessageBoxResult . Yes )
				//		//{
				//		var reslt = MessageBox . Show ( "Click Yes to see ALL occurences of @ARG, No for just the header declarations?" , "Details required" , MessageBoxButton . YesNo , MessageBoxImage . Question );
				//		if ( reslt == MessageBoxResult . Yes )
				//			showall = true;
				//		else
				//			showall = false;
				//		UpdateUniversalViewer ( );
				//		CheckingArgsOnly = true;
				//		GridData_Display . Visibility = Visibility . Hidden;
				//		SetViewButtons ( 2 , ( GridData_Display . Visibility == Visibility . Visible ? true : false ) , ( DisplayGrid . Visibility == Visibility . Visible ? true : false ) );
				//	}
				//}
				//****************************************************************************//
				// This is the MAIN call made to connect the SQL queries
				//****************************************************************************//
				//if ( Arguments != "" )
				//	Arguments = ParseArguments ( Arguments );
			}
			try
			{
				List<string> genericlist = new List<string>();
				bool usegeneric = false;
#pragma warning disable CS0219 // The variable 'outbuffer' is assigned but its value is never used
				string outbuffer="";
#pragma warning restore CS0219 // The variable 'outbuffer' is assigned but its value is never used

				if ( usegeneric )
				{
					GenericClass gc = new GenericClass ( );
					List<GenericClass> genlist = new List<GenericClass>();
					DapperGeneric<GenericClass , ObservableCollection<GenericClass> , List<GenericClass>> . ExecuteSPFullGenericClass (
						ref generics ,
						false ,
						ref generics ,
						SqlCommand ,
						Arguments ,
						"" ,
						"" ,
						ref genlist ,
						 out errormsg );
					ResultString = errormsg;
				}
				else
				{
					generics . Clear ( );
					totalcolumns = DapperSupport . CreateGenericCollection (
						ref generics ,
						SqlCommand ,
						Arguments ,
						"" ,
						"" ,
						ref genericlist ,
						ref errormsg );
					ResultString = errormsg;
				}
				return generics;

				//if ( errormsg == "" )
				//{
				//	int columncount = 0;
				//	if ( totalcolumns > 0 && generics . Count > 0 )
				//	{
				//		//LoadActiveRowsOnlyInGrid ( DisplayGrid , Generics , totalcolumns );
				//		////GridData_Display . Visibility = Visibility . Hidden;
				//		//DisplayGrid . SelectedIndex = 0;
				//		//DisplayGrid . Visibility = Visibility . Visible;
				//		//GridCount . Text = DisplayGrid . Items . Count . ToString ( );
				//		//UpdateUniversalViewer ( );
				//		//DisplayGrid . Refresh ( );
				//		//DisplayGrid . Focus ( );
				//		////Show Grid
				//		//SetViewButtons ( 1 , ( GridData_Display . Visibility == Visibility . Visible ? true : false ) , ( DisplayGrid . Visibility == Visibility . Visible ? true : false ) );
				//		//togglevisibility ( true , "" );
				//		//return Generics;

				//	}

				//	DataTable dt = new DataTable();
				//	dt = ProcessSqlCommand ( SqlCommand + " " + Arguments );
				//	if ( dt . Rows . Count == 0 )
				//	{
				//		// Dont dfisplay if it is an SQL TYPE/TALE etc  creation command
				//		if ( errormsg == "" && SqlCommand . Contains ( "SPCREATE_" ) == false )
				//			MessageBox . Show ( $"Datatable contains Zero records " , $"[{SqlCommand} {Arguments}] SP Script Error" , MessageBoxButton . OK , MessageBoxImage . Warning );
				//		else if ( SqlCommand . Contains ( "SPCREATE_" ) )
				//			DbCopiedResult . Text = $"[{SqlCommand}]\n\n'Special' command has been completed sucessfully ...";
				//		// setup grid as we have no new data
				//		SetDumyGridRow ( DisplayGrid );
				//		return null;
				//	}
				//	else
				//	{
				//		// Got the data, so display it
				//		int  count = 0;
				//		string  store="";
				//		columncount = totalcolumns;
				//		Generics . Clear ( );
				//		foreach ( var item in dt . Rows )
				//		{
				//			GenericClass  gc = new GenericClass ( );
				//			DataRow dr = item as DataRow;
				//			columncount = dr . ItemArray . Count ( );
				//			if ( columncount == 0 )
				//				columncount = 1;
				//			// we only need max cols - 1 here !!!
				//			for ( int x = 0 ; x < columncount ; x++ )
				//				store = dr . ItemArray [ x ] . ToString ( ) + ",";
				//			outbuffer += store;
				//			//						if(columncount > 1)
				//			CreateGenericRecord ( store , gc , Generics );
				//			//						else
				//			//							GridData_Display . Text = store;
				//			count++;
				//		}

				//		if ( DbName == "COMMENTS" )
				//		{
				//			string output="";
				//			string[] flds = outbuffer.Split('\n');
				//			try
				//			{
				//				foreach ( var item in flds )
				//				{
				//					if ( item . Length < 3 )
				//						continue;
				//					if ( item == "--\r" && item . Length <= 4 )
				//						break;
				//					else if ( item . Substring ( 0 , 2 ) == "--" )
				//						output += item + "\n";
				//				}
				//			}
				//			catch ( Exception ex ) {; }
				//			if ( output == "" )
				//			{
				//				Mouse . OverrideCursor = Cursors . Arrow;
				//				Utils . Mbox ( this , string1: "No comments were found in the currently selected script" , string2: "" , caption: "SP Comments !" , iconstring: "\\icons\\Information.png" , Btn1: MB . OK , Btn2: MB . NNULL , defButton: MB . OK );
				//				return null;
				//			}
				//			// hide grid
				//			togglevisibility ( false , "" );
				//			GridData_Display . Visibility = Visibility . Visible;
				//			GridData_Display . Text = output;
				//			//DisplayGrid . Visibility = Visibility . Hidden;
				//			SetViewButtons ( 1 , ( GridData_Display . Visibility == Visibility . Visible ? true : false ) , ( DisplayGrid . Visibility == Visibility . Visible ? true : false ) );
				//			UpdateUniversalViewer ( );
				//			GridData_Display . Focus ( );
				//			return null;
				//		}
				//		if ( SqlCommand != "spGetSpecificSchema" )
				//		{
				//			LoadActiveRowsOnlyInGrid ( DisplayGrid , Generics , columncount );
				//			//GridData_Display . Visibility = Visibility . Hidden;
				//			DisplayGrid . SelectedIndex = 0;
				//			DisplayGrid . Visibility = Visibility . Visible;
				//			GridCount . Text = DisplayGrid . Items . Count . ToString ( );
				//			UpdateUniversalViewer ( );
				//			DisplayGrid . Refresh ( );
				//			DisplayGrid . Focus ( );
				//			//Show Grid
				//			SetViewButtons ( 1 , ( GridData_Display . Visibility == Visibility . Visible ? true : false ) , ( DisplayGrid . Visibility == Visibility . Visible ? true : false ) );
				//			togglevisibility ( true , "" );
				//		}
				//		else
				//		{
				//			//							DisplayGrid . Visibility = Visibility . Hidden;
				//			ShowGrid . Content = "Show Grid Viewer";
				//			if ( CheckingArgsOnly )
				//			{
				//				store = ParseforArgsLines ( outbuffer , showall );
				//			}
				//			if ( store . Length > 0 )
				//			{
				//				if ( showall )
				//					store = "\nONLY Lines with lines containing '@' ANYWHERE in the selected script are shown :\n\n" + store;
				//				else
				//					store = "\nONLY Lines with lines containing @' in the HEADER block alone of the selected script are shown :\n\n" + store;
				//				GridData_Display . Text = store;
				//				GridData_Display . Visibility = Visibility . Visible;
				//				UpdateUniversalViewer ( );
				//				GridData_Display . Focus ( );
				//				// hide grid
				//				togglevisibility ( false , "" );
				//				SetViewButtons ( 2 , ( GridData_Display . Visibility == Visibility . Visible ? true : false ) , ( DisplayGrid . Visibility == Visibility . Visible ? true : false ) );
				//			}
				//			else
				//			{
				//				Mouse . OverrideCursor = Cursors . Arrow;
				//				Utils . Mbox ( this , string1: "The selected stored procedure does not appear to require ANY Arguments..." , string2: "" , caption: "SP Argument check" , iconstring: "\\icons\\Information.png" , Btn1: MB . OK , Btn2: MB . NNULL , defButton: MB . OK );
				//				return null;
				//			}
				//		}
				//	}
				//}
				//else if ( errormsg . Contains ( "DYNAMIC:" ) )
				//{
				//	// Process the data we have finally got into the Observabllecollection<GenricsClass>
				//	// which has 20 columns down to just whatever # of columns we actually have to work with/Display
				//	//Much cleaner and more pleasant to view
				//	if ( Generics . Count == 0 && errormsg . Contains ( "DYNAMIC:0" ) )
				//	{
				//		MessageBox . Show ( $"SQL Error : \nSql Query was : [{SqlCommand}]  returned ZERO records !" , "SQL Query Error" , MessageBoxButton . OK ,
				//			    MessageBoxImage . Error );
				//		this . Title = $"Sql Server Commands : Active Db : 'None'";
				//		DbCopiedResult . Text = $"SQL query [{SqlCommand}] retuned ZERO records";
				//		CurrentCommandLabel . Text = SqlCommand + " " + Arguments;
				//		return null;
				//	}
				//	string t = errormsg.Substring(8);
				//	int colcount =Convert.ToInt32(t );
				//	Console . WriteLine ( $"Db has {Generics . Count} records and {colcount} columns" );
				//	//LoadActiveRowsOnlyInGrid ( spViewerGrid , Generics , colcount );
				//	LoadActiveRowsOnlyInGrid ( DisplayGrid , Generics , colcount );
				//	UpdateUniversalViewer ( );
				//	DisplayGrid . SelectedIndex = 0;
				//	//spViewerGrid . SelectedIndex = 0;
				//	//spViewerGrid . Refresh ( );
				//	DisplayGrid . Refresh ( );
				//	//Show Grid
				//	togglevisibility ( true , GetDbNameFromCommand ( SqlCommand ) );
				//	// update  informatoin panels
				//	DbCopiedResult . Text = $"[{SqlCommand}] has been completed successfully....";
				//	CurrentCommandLabel . Text = $"[{SqlCommand}]";
				//	this . Title = $"Sql Server Commands : Active/Last Db/Command : '{SqlCommand}'";

				//	return null;
				//}
				//else if ( errormsg . Contains ( "SQL PARSE ERROR" ) || errormsg . Contains ( "SQLERROR :" ) )
				//{
				//	// Process the data we have finally got into the Observabllecollection<GenricsClass>
				//	// which has 20 columns down to just whatever # of columns we actually have to work with/Display
				//	//Much cleaner and more pleasant to view
				//	MessageBoxResult  dr = MessageBox . Show ( errormsg + $"Do you want to view  the full script contents ?" , "SQL Query Error" , MessageBoxButton .YesNo ,
				//		    MessageBoxImage . Error );
				//	this . Title = $"Sql Server Commands : Active Db : 'None'";
				//	DbCopiedResult . Text = $"SQL query [{SqlCommand}] failed to run  correctly";
				//	CurrentCommandLabel . Text = SqlCommand + " " + Arguments;
				//	if ( dr == MessageBoxResult . Yes )
				//	{
				//		ViewFullSP_Click ( null , null );
				//	}
				//	return null;
				//}
				//else
				//{
				//	// Unkown error ??
				//	int columncount = 0;
				//	DataTable dt = new DataTable();
				//	if ( SqlCommand . Substring ( 0 , 2 ) != "sp" )
				//	{
				//		dt = ProcessSqlCommand ( SqlCommand + " " + Arguments );
				//		if ( dt . Rows . Count == 0 )
				//		{
				//			// Dont dfisplay if it is an SQL TYPE/TALE etc  creation command
				//			if ( errormsg == "" && SqlCommand . Contains ( "SPCREATE_" ) == false )
				//				MessageBox . Show ( $"Datatable contains Zero records " , $"[{SqlCommand} {Arguments}] SP Script Error" , MessageBoxButton . OK , MessageBoxImage . Warning );
				//			else if ( SqlCommand . Contains ( "SPCREATE_" ) )
				//				DbCopiedResult . Text = $"[{SqlCommand}]\n\n'Special' command has been completed sucessfully ...";
				//			return null;
				//		}
				//		int  count = 0;
				//		columncount = 0;
				//		Generics . Clear ( );
				//		foreach ( var item in dt . Rows )
				//		{
				//			GenericClass  gc = new GenericClass ( );
				//			string  store="";
				//			DataRow dr = item as DataRow;
				//			columncount = dr . ItemArray . Count ( );
				//			if ( columncount == 0 )
				//				columncount = 1;
				//			// we only need max cols - 1 here !!!
				//			for ( int x = 0 ; x < columncount ; x++ )
				//				store += dr . ItemArray [ x ] . ToString ( ) + ",";
				//			CreateGenericRecord ( store , gc , Generics );
				//		}
				//		if ( SavedValue == "spGetSpecificSchema" )
				//		{
				//			MessageBoxResult dr= new MessageBoxResult();
				//			if ( Generics . Count > 0 )
				//			{
				//				string buff = GetSpecificSPArguments ( "RETURNEDRESULTS" , Generics [ 0 ] . field1, showall );
				//				if ( buff . Length == 0 )
				//				{
				//					dr = MessageBox . Show ( "The request succeeded, but the selected SP does not require any Arguments\nDo you want to view the full Script contents ?" , "Sql Information" , MessageBoxButton . YesNo , MessageBoxImage . Information );
				//					return null;
				//				}
				//				//DisplayGrid . Visibility = Visibility . Hidden;
				//				GridData_Display . Visibility = Visibility . Visible;
				//				GridData_Display . Text = buff;
				//				CurrentCommandLabel . Text = $"[{SqlCommand}]";
				//				DbCopiedResult . Text = $"[{SqlCommand}] completed successfully....";
				//				this . Title = $"Sql Server Commands : Active/Last Db/Command : '{SqlCommand}'";
				//				if ( dr == MessageBoxResult . Yes )
				//				{
				//					ViewFullSP_Click ( null , null );
				//				}
				//				// hide grid
				//				togglevisibility ( false , "" );
				//				SetViewButtons ( 2 , ( GridData_Display . Visibility == Visibility . Visible ? true : false ) , ( DisplayGrid . Visibility == Visibility . Visible ? true : false ) );
				//			}
				//			return null;
				//		}
				//		// Loads JUST the rows we actually have, not the full 20 columns !!
				//		//Much cleaner and more pleasant to view
				//		LoadActiveRowsOnlyInGrid ( DisplayGrid , Generics , columncount );
				//		UpdateUniversalViewer ( );
				//		//Show Grid
				//		if ( Arguments != "" )
				//			DbCopiedResult . Text = $"The Stored Procedure [{SqlCommand} [{Arguments}]] \nwas executed successfuly...";
				//		else
				//			DbCopiedResult . Text = $"The Stored Procedure [{SqlCommand}] \nwas executed successfuly...";
				//		DisplayGrid . SelectedIndex = 0;
				//		DisplayGrid . Visibility = Visibility . Visible;
				//		SetViewButtons ( 2 , ( GridData_Display . Visibility == Visibility . Visible ? true : false ) , ( DisplayGrid . Visibility == Visibility . Visible ? true : false ) );
				//		togglevisibility ( true , GetDbNameFromCommand ( SqlCommand ) );
				//		GridCount . Text = DisplayGrid . Items . Count . ToString ( );
				//		DisplayGrid . Refresh ( );
				//		DisplayGrid . Focus ( );
				//		// update  information panels
				//		CurrentCommandLabel . Text = $"[{SqlCommand}]";
				//		OriginalSqlCommand = SqlCommand;
				//		DbCopiedResult . Text = $"[{SqlCommand}] has been completed successfully....";
				//		CurrentCommandLabel . Text = $"[{SqlCommand}]";
				//		this . Title = $"Sql Server Commands : Active/Last Db/Command : '{SqlCommand}'";
				//	}
				//}

			}
			catch ( Exception ex )
			{
				MessageBox . Show ( $"SQL ERROR 1125 - {ex . Message}" );
				return null;
			}
//			return null;
		}

		private static object LoadListData ( object grid , ObservableCollection<GenericClass> genericcollection , int total )
		{
			if ( total == 1 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 2 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1,data.field2}).ToList();
				return res as object;
				//				Grid . ItemsSource = res;
			}
			else if ( total == 3 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2,data.field3}).ToList();
				return res as object;
				//				Grid . ItemsSource = res;
			}
			else if ( total == 4 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4}).ToList();
				return res as object;
				//				Grid . ItemsSource = res;
			}
			else if ( total == 5 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 6 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 7 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 8 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 9 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 10 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9 ,data.field10}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 11 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 12 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 13 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12,data.field13}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 14 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12,data.field13,data.field14}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 15 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12,data.field13,data.field14,data.field15}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 16 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12,data.field13,data.field14,data.field15,data.field16}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 17 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12,data.field13,data.field14,data.field15,data.field16,data.field17}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 18 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12,data.field13,data.field14,data.field15,data.field16,data.field17,data.field18}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 19 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12,data.field13,data.field14,data.field15,data.field16,data.field17,data.field18,data.field19}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			else if ( total == 20 )
			{
				var res =
				   ( from data in genericcollection
				     select new
				     {data.field1, data.field2, data.field3,data.field4,data.field5,data.field6,data.field7,data.field8,data.field9,data.field10,data.field11,data.field12,data.field13,data.field14,data.field15,data.field16,data.field17,data.field18,data.field19,data.field20}).ToList();
				return res as object;
				//Grid . ItemsSource = res;
			}
			return null;
		}
		//********************************************************************************************************************************************************************************//
		public static void LoadActiveRowsOnlyInGrid ( DataGrid grid , ObservableCollection<GenericClass> genericcollection , int total )
		//********************************************************************************************************************************************************************************//
		{
			DataGrid  Grid = grid ;
			// filter data to remove all "extraneous" columns
			Grid . ItemsSource = null;
			Grid . Items . Clear ( );
			List < GenericClass > lst = new List<GenericClass>();
			lst = LoadListData ( Grid , genericcollection , total ) as List<GenericClass>;
			Grid . ItemsSource = lst;
			Grid . SelectedIndex = 0;
			Grid . Visibility = Visibility . Visible;
			Grid . Refresh ( );
			Grid . Focus ( );
		}
		public static void LoadActiveRowsOnlyInLBox ( ListBox grid , ObservableCollection<GenericClass> genericcollection , int total )
		//********************************************************************************************************************************************************************************//
		{
			ListBox Grid = grid ;
			// filter data to remove all "extraneous" columns
			Grid . ItemsSource = null;
			Grid . Items . Clear ( );
			LoadListData ( Grid , genericcollection , total );
			Grid . SelectedIndex = 0;
			Grid . Visibility = Visibility . Visible;
			Grid . Refresh ( );
			Grid . Focus ( );
		}
		public static void LoadActiveRowsOnlyInLView ( ListView grid , ObservableCollection<GenericClass> genericcollection , int total )
		//********************************************************************************************************************************************************************************//
		{
			ListView Grid = grid ;
			// filter data to remove all "extraneous" columns
			Grid . ItemsSource = null;
			Grid . Items . Clear ( );
			LoadListData ( Grid , genericcollection , total );
			Grid . SelectedIndex = 0;
			Grid . Visibility = Visibility . Visible;
			Grid . Refresh ( );
			Grid . Focus ( );
		}
		public static bool Executestoredproc (string SqlCommand, string ConString )
		{
			using ( IDbConnection db = new SqlConnection ( ConString ) )
			{
				try
				{
					// Use DAPPER to to load Bank data using Stored Procedure
					// This syntax WORKS CORRECTLY

					//******************************************************************************************************//
					var result  = db . Query( SqlCommand , null,null,false, null,CommandType.StoredProcedure);
					//******************************************************************************************************//

					var arry =result .ToArray();
					//Console . WriteLine ( arry [ 0 ] . ToString ( ));
				}
				catch ( Exception ex )
				{
					Console . WriteLine ( $"SQL DAPPER error : {ex . Message}, {ex . Data}" );
					return false;
				}
				finally
				{
					Console . WriteLine ( $"SQL DAPPER command [ {SqlCommand}] completed successfuly" );
				}
				return true;
			}

		}
	}

}
