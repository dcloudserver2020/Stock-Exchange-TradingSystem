using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData
{
    public class ApiData
    {
        public static string MetaData = "Meta Data";
        public static string TimeSeries = "Time Series (5min)";
        public static string information = "1. Information";
        public static string Symbol = "2. Symbol";
        public static string LastRefreshed = "3. Last Refreshed";
        public static string Interval = "4. Interval";
        public static string OutputSize = "5. Output Size";
        public static string TimeZone = "6. Time Zone";

        public static string Open = "1. open";
        public static string High = "2. high";
        public static string Low = "3. low";
        public static string Close = "4. close";
        public static string Volume = "5. volume";


    }
    public class OutputData
    {
        public DateTime? Date { get; set; }
        public string Open { get; set; }
        public  string High { get; set; }
        public  string Low { get; set; }
        public  string Close { get; set; }
        public  string Volume { get; set; }
    }
}
