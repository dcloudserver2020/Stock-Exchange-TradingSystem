using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string x;

        private void button1_Click(object sender, EventArgs e)
        {



            x = "Search";
            SendWebrequest_Get_Method();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            x = "Export";
            SendWebrequest_Get_Method();
        }

        private void SendWebrequest_Get_Method()
        {
            try
            {
                IList<OutputData> stockList = new List<OutputData>();
                String postData = this.textBox1.Text;
                HttpWebRequest request = null;
                if (x == "Search")
                {
                    request = (HttpWebRequest)WebRequest.Create("https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=NSE:" + postData + "&interval=5min&outputsize=compact&apikey=OAZHZ1AY7TISC7NI");
                }
                else if (x == "Export")
                {
                    request = (HttpWebRequest)WebRequest.Create("https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=NSE:" + postData + "&interval=5min&outputsize=compact&apikey=OAZHZ1AY7TISC7NI&datatype=csv");
                }
                request.ContentType = "application/json; charset=utf-8";
                request.Accept = "application/json";
                request.Method = WebRequestMethods.Http.Get;

                WebResponse response = request.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream());

                String json_text = sr.ReadToEnd();
                var jObj = JObject.Parse(json_text);
                var metadata = jObj["Meta Data"].ToObject<Dictionary<string, string>>();
                var timeseries = jObj["Time Series (5min)"].ToObject<Dictionary<string, Dictionary<string, string>>>();
                dynamic stuff = JsonConvert.DeserializeObject(json_text);
                //dynamic dynObj = GetConvertedObj(stuff);

                foreach (var key in timeseries.Keys)
                {
                    if (key != null)
                    {
                    OutputData obj = new OutputData();
                    obj.Date = ConvetToIStTime(key.ToString());
                    
                    foreach (var inerloop in timeseries[key])
                    {
                        if (inerloop.Key == ApiData.Open)
                        {
                            obj.Open = inerloop.Value;
                        }
                        if (inerloop.Key == ApiData.Close)
                        {
                            obj.Close = inerloop.Value;
                        }
                        if (inerloop.Key == ApiData.High)
                        {
                            obj.High = inerloop.Value;
                        }
                        if (inerloop.Key == ApiData.Low)
                        {
                            obj.Low = inerloop.Value;
                        }
                        if (inerloop.Key == ApiData.Volume)
                        {
                            obj.Volume = inerloop.Value;

                        }
                        
                    }
                        stockList.Add(obj);
                    }
            }
               //var env = stockList.OrderBy(c => c.Date).ThenBy(x => x.Date);
                //stockList.OrderBy(x => x.Date)

      ;
                if (stuff.error != null)
                {
                    richTextBox1.Text = "problem with getting data";
                    //MessageBox.Show("problem with getting data", "Error");

                }
                else
                {
                    richTextBox1.Text = json_text;
                    //MessageBox.Show(json_text, "success");
                }

                sr.Close();
            }

            catch (Exception ex)

            {
                //richTextBox1.Text = "Wrong request ! " + ex.Message;
                MessageBox.Show("Wrong request ! " + ex.Message, "Error");
            }
        }

        public dynamic GetConvertedObj(dynamic Obj)
        {
            dynamic d = new ExpandoObject(); ;
            d.info = new ExpandoObject();
            d.info.Information = Obj["Meta Data"]["1. Information"].ToString();
            d.info.Symbol = Obj["Meta Data"]["2. Symbol"].ToString();
            d.info.Refereshed = ConvetToIStTime(Obj["Meta Data"]["3. Last Refreshed"].ToString());
            d.info.Interval = Obj["Meta Data"]["4. Interval"].ToString();
            d.info.OutputSize = Obj["Meta Data"]["5. Output Size"].ToString();
            d.info.TimeZone = Obj["Meta Data"]["6. Time Zone"].ToString();
            //Obj["me Series (5min)"]



            return d;
        }

        public DateTime? ConvetToIStTime(String Date)
        {
            try
            {
                string[] strdate = Date.Split(' ');
                if (strdate.Length == 2)
                {
                    string[] strdate1 = strdate[0].Split('-');
                    string[] strdate2 = strdate[1].Split(':');
                    DateTime dt = DateTime.UtcNow;
                    TimeZoneInfo usEasternZone = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time"); // say
                    TimeZoneInfo indianZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // say
                    DateTime usEasternTime = TimeZoneInfo.ConvertTimeFromUtc(dt, usEasternZone);
                    DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(dt, indianZone);
                    TimeSpan timeDiff = indianTime - usEasternTime;
                    usEasternTime = new DateTime(Convert.ToInt32(strdate1[0]), Convert.ToInt32(strdate1[1]), Convert.ToInt32(strdate1[2]), Convert.ToInt32(strdate2[0]), Convert.ToInt32(strdate2[1]), Convert.ToInt32(strdate2[2])); // or whatever the datetime to convert is
                    indianTime = usEasternTime + timeDiff;
                    return indianTime;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

    }
}
