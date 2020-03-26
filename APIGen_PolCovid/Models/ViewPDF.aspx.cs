using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APIGen_PolCovid.Models
{
    public partial class ViewPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string q1 = Request.QueryString["policy"];
            string MC = Request.QueryString["MC"];
            string url = @"http://localhost:4122/Get/PolicyCovid/{0}";
            if (MC != null) url = @"http://"+ MC + "/APIGenPolCovid/Get/PolicyCovid/{0}";
            WebClient df = new WebClient();
            var _Byt = df.DownloadData(string.Format(url, q1));

            //SendRequest("GET",);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            //Response.AddHeader("content-length", ConByte.Length.ToString());
            Response.BinaryWrite(_Byt);
            Response.End();
            Response.Close();
        }

        public string SendRequest(string Method, string _ApiUrl, object obj = null)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
            string getre = "";

            string _url = _ApiUrl;
            var baseAddress = _url;// URL
            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            //http.Headers[HttpRequestHeader.Authorization] = "a30023bc4aaf8b39264144c337859880113c9831d84031d4ca9f97309481b430";
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = Method;
            if (Method == "POST" || Method == "PUT")
            {
                //UTF8Encoding encoding = new UTF8Encoding();
                //string j = JsonConvert.SerializeObject(obj, Formatting.Indented).ToString();
                //Byte[] bytes = encoding.GetBytes(JsonConvert.SerializeObject(obj, Formatting.Indented));
                //Stream newStream = http.GetRequestStream();
                //newStream.Write(bytes, 0, bytes.Length);
                //newStream.Close();
            }

            //============== GetResponse ===============//
            var response = http.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            response.Dispose();
            //newStream.Dispose();
            return content;
        }
    }
}