using System;
using System . Threading;

namespace MyDev . Views
{

    public class CountGrid : System . Windows . Controls . Grid
    {
        public CountGrid ( )
        {
            Count++;
            Console . WriteLine ( "Instance # " + Count );
            Thread . Sleep ( 100 );
        }

        public static int Count;
    }
}