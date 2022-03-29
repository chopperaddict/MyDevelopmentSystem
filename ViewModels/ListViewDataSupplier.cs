using System;
using System. Collections. Generic;
using System. Collections. ObjectModel;
using System. Data;
using System. Diagnostics;
using System. Linq;
using System. Text;
using System. Threading. Tasks;
using System. Windows;

using MyDev. Views;

namespace MyDev. ViewModels
{
    public static class ListViewDataSupplier
    {
        private static Collection<BankAccountViewModel> BankDataCollection = new Collection<BankAccountViewModel> ( );
        public static Collection<BankAccountViewModel> GetBankData ( bool Notify = false, int MaxRecs = -1 )
        {
            BankDataCollection. Clear ( );
            DataTable dt = BankCollection. LoadBankData ( MaxRecs );
            BankDataCollection = LoadBankCollection ( dt, MaxRecs, Notify ); 
            return BankDataCollection;
        }
        public static Collection<BankAccountViewModel> LoadBankCollection ( DataTable dtBank, int MaxRecs, bool Notify = false )
        {
            int count = 0;
            if ( MaxRecs == -1 )
                MaxRecs = dtBank. Rows. Count;
            try
            {
                object bptr = new object ( );
                for ( int i = 0 ; i < MaxRecs ; i++ )
                {
                    BankDataCollection. Add ( new BankAccountViewModel
                    {
                        Id = Convert. ToInt32 ( dtBank. Rows[i][0] ),
                        BankNo = dtBank. Rows[i][1]. ToString ( ),
                        CustNo = dtBank. Rows[i][2]. ToString ( ),
                        AcType = Convert. ToInt32 ( dtBank. Rows[i][3] ),
                        Balance = Convert. ToDecimal ( dtBank. Rows[i][4] ),
                        IntRate = Convert. ToDecimal ( dtBank. Rows[i][5] ),
                        ODate = Convert. ToDateTime ( dtBank. Rows[i][6] ),
                        CDate = Convert. ToDateTime ( dtBank. Rows[i][7] ),
                    } );
                    count = i;
                }
            }
            catch ( Exception ex )
            {
                Debug. WriteLine ( $"BANK : SQL Error in BankCollection(351) load function : {ex. Message}, {ex. Data}" );
                MessageBox. Show ( $"BANK : SQL Error in BankCollection (351) load function : {ex. Message}, {ex. Data}" );
            }
            finally
            {
                // This is ONLY called  if a requestor specifies the argument as TRUE
                if ( Notify )
                {
                    EventControl. TriggerBankDataLoaded ( null,
                        new LoadedEventArgs
                        {
                            CallerType = "DataGridDataSupplier",
                            CallerDb = "",
                            DataSource = BankDataCollection,
                            RowCount = BankDataCollection. Count
                        } );
                }
            }
            return BankDataCollection;
        }

    }
}
