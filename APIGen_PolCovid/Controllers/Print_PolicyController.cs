using SignLib;
using SignLib.Pdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib.Zip;

namespace APIGen_PolCovid.Controllers
{
    public class Print_PolicyController : ApiController
    {
        string urlJs3 = Properties.Settings.Default.Url_Js3;
        string urlJs2 = Properties.Settings.Default.Url_Js2;
        string SvJs = "JS3";
        string RootPathFilePolCovid = Properties.Settings.Default.PathWriteFilePolicyCovid;

        //@" http://jasper.smk.co.th:8080/jasperserver/rest_v2/reports/reports/DEV/Nonmotor";
        ////string url = @" http://jasper.smk.co.th:8080/jasperserver/rest_v2/reports/reports/interactive/Nonmotor";

        HttpResponseMessage _response = new HttpResponseMessage();
        [HttpGet]
        [Route("Get/PolicyCovid/{policy}")]
        [Route("Get/PolicyCovid/{policy}/{password}")]
        [Route("Get/PolicyCovid/SEL/{SeverJs}/{policy}")]
        [Route("Get/PolicyCovid/SEL/{SeverJs}/{policy}/{password}")]
        public HttpResponseMessage GenPolicyCovid(string policy, string password = null, string SeverJs = null)
        {
            if (SeverJs == "JS2") SvJs = "JS2";
            else SvJs = "JS3";
            _response = new HttpResponseMessage();
            _response.StatusCode = HttpStatusCode.InternalServerError;
            byte[] BytStr = null;
            try
            {
                BytStr = GenFileFromJsper(policy, password);
            }
            catch (Exception e)
            {

            }
            //   HttpResponseMessage _response = new HttpResponseMessage(HttpStatusCode.OK);
            _response.StatusCode = HttpStatusCode.OK;
            _response.Content = new ByteArrayContent(BytStr);  //new  new StreamContent(new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read));
            _response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            return _response;
            //OpenViewPDF(filepath + filename);
        }

        [HttpPost]
        [Route("Post/PolicyCovidRange")]
        [Route("Post/PolicyCovidRange/{SeverJs}")]
        public HttpResponseMessage PolicyCovidRange([FromBody] DataTable _Reqbody, string SeverJs = null)
        {
            if (SeverJs == "JS2") SvJs = "JS2";
            else SvJs = "JS3";
            DataTable ReqBody = _Reqbody;
            DataTable _ReData = new DataTable();
            DataTable _ReData1 = new DataTable();
            //============================================//
            //              SET DATATABLE PAGE 
            //============================================//
            _ReData1.Columns.Add("Policy");
            _ReData1.Columns.Add("LocalPath");
            _ReData1.Columns.Add("VisualPath");
            _ReData.Columns.Add("Policy");
            _ReData.Columns.Add("Pol_Type");
            _ReData.Columns.Add("LocalPath");


            GENFILEPOLCOVID_RANGE("Policy", ReqBody, _ReData);
            GENFILEPOLCOVID_RANGE("ePolicy", ReqBody, _ReData);
            foreach (DataRow _dr in ReqBody.Rows)
            {
                _ReData1.Rows.Add(_dr["policy"].ToString(), _dr["file_name"].ToString(), _dr["file_name"].ToString());// UrlFile.Add(PathFileName_Out);
            }

            _response.Content = new ObjectContent(_ReData1.GetType(), _ReData1, new JsonMediaTypeFormatter());
            return _response;
        }

        #region Zip
        [HttpPost]
        [Route("Post/ZipPolicyCovidOld")]
        public HttpResponseMessage ZipPolicyCovidNotCer([FromBody] object _Reqbody)
        {

            _response = new HttpResponseMessage();

            byte[] Resulte = null;
            try
            {

                string inJson = "";
                ObjConvertJson(ref inJson, _Reqbody);
                DataTable _dt = JsonConvert.DeserializeObject<DataTable>(inJson);
                if (_dt.Rows.Count > 1)
                {
                    string _Fliename = "CVP" + DateTime.Now.ToString("ddMMyyyHHmmss") + ".zip";
                    string FileOutPut = RootPathFilePolCovid + _Fliename;

                    ZipFile MyZip;
                    MyZip = ZipFile.Create(FileOutPut);
                    MyZip.BeginUpdate();
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        string Path = _dr["physical_path"].ToString()
                            , Filename = _dr["policy_no"].ToString() + ".pdf";
                        MyZip.Add(Path, Filename);

                    }
                    MyZip.CommitUpdate();

                    MyZip.Close();
                    Resulte = File.ReadAllBytes(FileOutPut);
                    File.Delete(FileOutPut);
                }
                else
                {
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        string Path = _dr["physical_path"].ToString()
                            , Filename = _dr["policy_no"].ToString() + ".pdf";
                        Resulte = File.ReadAllBytes(Path);
                    }

                }
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            _response.Content = new ObjectContent(Resulte.GetType(), Resulte, new JsonMediaTypeFormatter());
            return _response;

        }
        #endregion

        #region GENFILEPOLCOVID_RANGE
        public void GENFILEPOLCOVID_RANGE(string FlagType_Pol, DataTable ReqBody, DataTable Pass)
        {
            DataTable _ReData = new DataTable();
            ClsCer CC = new ClsCer();
            string PathFolderS = RootPathFilePolCovid;
            string pols = "";
            List<string> _list_Pol = new List<string>();
            try
            {
                _response = new HttpResponseMessage();

                //===================================================
                //                  ต่อเลขกรมธรรม์
                //===================================================
                foreach (DataRow _dr in ReqBody.Rows)
                {
                    string pol = _dr["policy"].ToString()
                        , LocalPath = _dr["physical_path"].ToString()
                        , PDFNAME = _dr["file_name"].ToString();
                    pols = pols + ((pols == "") ? pols = pol : "," + pol);

                    if (!Directory.Exists(LocalPath))
                    {
                        Directory.CreateDirectory(LocalPath);
                    }
                }
                //======================================================
                //                   GEN FILE JASPER 
                //======================================================
                string file_attached = string.Format(@"covid_attached"); //+ user + "_" + Guid.NewGuid().ToString() + ".PDF";
                string file_policy = string.Format(@"covid_policy"); //+ user + "_" + Guid.NewGuid().ToString() + ".PDF";
                string file_receipt = string.Format(@"covid_receipt");// + user + "_" + Guid.NewGuid().ToString() + ".PDF";
                string cert = "1";
                if (FlagType_Pol == "ePolicy") cert = "0";

                Dictionary<string, string> DataPO = new Dictionary<string, string>();
                Dictionary<string, string> DataREC = new Dictionary<string, string>();
                Dictionary<string, string> DataAt = new Dictionary<string, string>();
                //=============================================
                DataPO["@cert"] = cert;
                DataREC["@cert"] = cert;
                DataAt["@cert"] = cert;
                //=============================================
                DataPO["@policy"] = pols;
                DataPO["@print_header"] = string.Empty;
                DataPO["@sub_cls"] = "696";
                //=============================================
                DataREC["@policy"] = pols;
                DataREC["@print_header"] = string.Empty;
                DataREC["@sub_cls"] = "696";
                //=============================================
                DataAt["@policy"] = pols;
                DataAt["@sub_cls"] = "696";
                //=============================================

                string file1 = ExportReport("/JRpt_696/JRpt_696_policy.PDF", PathFolderS, file_policy, DataPO);
                string file2 = ExportReport("/JRpt_602_receipt.PDF", PathFolderS, file_receipt, DataREC);
                string file3 = ExportReport("/JRpt_696/JRpt_696_attached.PDF", PathFolderS, file_attached, DataAt);
                //======================================================
                //                   จำนวจหน้าที่ต้องแยก 
                //======================================================
                List<string> _list = new List<string>();
                _list.Add(file_policy);
                _list.Add(file_receipt);
                _list.Add(file_attached);
                //======================================================
                //                      แยก ไฟล์
                //======================================================
                string PathFileName_Inp = "";//@"D:\PDF\SMKPO_Covid19.PDF";
                string PathFileName_Out = "";
                foreach (string _l in _list.ToArray())
                {
                    List<int> Page = new List<int>();
                    Page.Add(1);
                    PathFileName_Inp = PathFolderS + _l;
                    foreach (DataRow _dr in ReqBody.Rows)
                    {
                        string _file_Pol = _dr["file_name"].ToString()
                           , _file_ePol = _dr["pwd_file_name"].ToString();
                        if (FlagType_Pol == "ePolicy")
                        {
                            PathFileName_Out = _dr["physical_path"].ToString() + _l + _file_ePol;
                        }
                        else PathFileName_Out = _dr["physical_path"].ToString() + _l + _file_Pol;
                        PDFSplit(PathFileName_Inp, PathFileName_Out, Page, _l);
                    }
                    File.Delete(PathFolderS + _l);
                }
                //======================================================
                //                     รวมไฟล์
                //======================================================
                foreach (DataRow _dr in ReqBody.Rows)
                {
                    string _file_Pol = _dr["file_name"].ToString()
                        , _file_ePol = _dr["pwd_file_name"].ToString();

                    if (FlagType_Pol == "ePolicy")
                    {
                        //======================================================
                        //                     ไฟล์ePolicy
                        //======================================================
                        MergePage(_list, _dr["physical_path"].ToString(), _file_ePol);
                        CC.SignPdf(_dr["physical_path"].ToString() + _file_ePol
                            , _dr["physical_path"].ToString() + _dr["pwd_file_name"].ToString()
                            , _dr["password"].ToString());
                        Pass.Rows.Add(_dr["policy"].ToString(), "ePolicy", _dr["physical_path"].ToString());
                    }
                    else
                    {
                        //======================================================
                        //                     ไฟล์ปกติ
                        //======================================================
                        MergePage(_list, _dr["physical_path"].ToString(), _file_Pol);
                        Pass.Rows.Add(_dr["policy"].ToString(), "Policy", _dr["physical_path"].ToString());
                    }
                }
                //BytStr = GenFileFromJsper(policy, password);
            }
            catch (Exception e)
            {

            }
        }
        #endregion

        #region  PDFSplit
        private void PDFSplit(string PathFileName_Inp, string PathFileName_Out, List<int> Page, string flag)   //สร้างไฟล์แบบแยก
        {
            //------------ สร้างไฟล์ --------------///

            int CountPage = 1;
            if (flag == "covid_attached") CountPage = 5;
            PdfReader reader = null;
            Document document = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;
            reader = new PdfReader(PathFileName_Inp);
            string outputPdfPath = PathFileName_Out;

            // Capture the correct size and orientation for the page:
            document = new Document(reader.GetPageSizeWithRotation(1));

            // Initialize an instance of the PdfCopyClass with the source 
            // document and an output file stream:
            pdfCopyProvider = new PdfCopy(document,
                new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            document.Open();

            // Extract the desired page number:

            for (int i = 0; i < CountPage; i++)
            {
                importedPage = pdfCopyProvider.GetImportedPage(reader, Page[0]);
                pdfCopyProvider.AddPage(importedPage);
                Page[0]++;
            }
            document.Close();
            pdfCopyProvider.Close();
            reader.Close();
        }
        #endregion

        #region MERGE
        private void MergePage(List<string> _Form, string pathOut, string filename)   //รวมไฟล์
        {
            int i = 0;
            string PathFileName_Out = pathOut + filename;
            #region  Document
            Document document = new Document();
            PdfCopy writer = new PdfCopy(document, new FileStream(PathFileName_Out, FileMode.Create));
            document.Open();
            i = 0;
            foreach (string Form in _Form.ToArray())
            {
                string FilePart = pathOut + Form + filename;



                PdfReader reader = new PdfReader(FilePart);
                //reader.ConsolidateNamedDestinations();

                // step 4: we add content
                for (int x = 1; x <= reader.NumberOfPages; x++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, x);
                    writer.AddPage(page);
                }

                File.Delete(FilePart);

                reader.Close();
                i++;
                #endregion
            }
            // step 5: we close the document and writer
            writer.Close();
            document.Close();
        }
        #endregion

        #region Function
        private byte[] GenFileFromJsper(string policy, string Password)
        {
            string _Cer = "1"; //ไม่ติดCer
            if (Password != null) _Cer = "0";
            Dictionary<string, string> DataPO = new Dictionary<string, string>();
            DataPO["@cert"] = _Cer;
            DataPO["@policy"] = policy;
            DataPO["@print_header"] = string.Empty;
            DataPO["@sub_cls"] = "696";

            Dictionary<string, string> DataREC = new Dictionary<string, string>();
            DataREC["@cert"] = _Cer;
            DataREC["@policy"] = policy;
            DataREC["@print_header"] = string.Empty;
            DataREC["@sub_cls"] = "696";

            Dictionary<string, string> DataAt = new Dictionary<string, string>();
            DataAt["@cert"] = _Cer;
            DataAt["@policy"] = policy;
            DataAt["@sub_cls"] = "696";

            Byte[] file1 = ExportReport("/JRpt_696/JRpt_696_policy.PDF", DataPO);
            Byte[] file2 = ExportReport("/JRpt_602_receipt.PDF", DataREC);
            Byte[] file3 = ExportReport("/JRpt_696/JRpt_696_attached.PDF", DataAt);

            List<Byte[]> _FileStream = new List<Byte[]>();
            _FileStream.Add(file1);
            _FileStream.Add(file2);
            _FileStream.Add(file3);

            Byte[] array = CombineMultiplePDFs(_FileStream);
            if (_Cer == "0")
            {
                ClsCer CC = new ClsCer();
                array = CC.SignPdf_Byte(array, Password);
            }
            return array;
        }
        private Byte[] ExportReport(string PahtFout = null, Dictionary<string, string> pl = null)
        {
            string vp_output_file = string.Empty;
            string par = GenParam(pl);
            string _url = urlJs3 + PahtFout + par;
            if (SvJs == "JS3") _url = urlJs3 + PahtFout + par;
            else if (SvJs == "JS2") _url = urlJs2 + PahtFout + par;
            Stream stream = null;
            Byte[] buffer = null;
            //if (!Directory.Exists(PahtFin)) Directory.CreateDirectory(PahtFin);

            HttpWebRequest httpRequest;
            HttpWebResponse httpResponse;
            httpRequest = (HttpWebRequest)WebRequest.Create(_url);
            httpRequest.Credentials = new NetworkCredential("jasperadmin", "jasperadmin");   // auto logon to get new session
            httpRequest.Method = "GET";    // use GET method

            httpRequest.KeepAlive = true;
            httpRequest.ProtocolVersion = HttpVersion.Version10;
            httpRequest.Timeout = 10 * 60 * 1000; // 10 sec. time out setting

            httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                //vp_output_file = PahtFin + Name;
                stream = httpResponse.GetResponseStream();
                BinaryReader breader = new BinaryReader(stream);
                buffer = breader.ReadBytes((int)httpResponse.ContentLength);
                //using (Stream stream = httpResponse.GetResponseStream())
                //{
                //    using (Stream destination = File.Create(vp_output_file))
                //    {
                //        int a;
                //        Byte[] rBuf = new Byte[1024 * 16];   // 16k

                //        while ((a = stream.Read(rBuf, 0, rBuf.Length)) > 0)
                //        {
                //            destination.Write(rBuf, 0, a);
                //        }
                //        destination.Flush();
                //        destination.Close();
                //        rBuf = null;
                //    }
                //}
            }
            httpResponse.Close();
            httpResponse = null;
            return buffer;
        }
        private string ExportReport(string PahtFout = null, string PahtFin = null, string Name = null, Dictionary<string, string> pl = null, string dd = null)
        {
            string vp_output_file = string.Empty;
            string par = GenParam(pl);
            string _url = urlJs3 + PahtFout + par;
            if (SvJs == "JS3") _url = urlJs3 + PahtFout + par;
            else if (SvJs == "JS2") _url = urlJs2 + PahtFout + par;
            //if (!Directory.Exists(PahtFin)) Directory.CreateDirectory(PahtFin);

            HttpWebRequest httpRequest;
            HttpWebResponse httpResponse;
            httpRequest = (HttpWebRequest)WebRequest.Create(_url);
            httpRequest.Credentials = new NetworkCredential("jasperadmin", "jasperadmin");   // auto logon to get new session
            httpRequest.Method = "GET";    // use GET method

            httpRequest.KeepAlive = true;
            httpRequest.ProtocolVersion = HttpVersion.Version10;
            httpRequest.Timeout = 10 * 60 * 1000; // 10 sec. time out setting
            httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                vp_output_file = PahtFin + Name;
                using (Stream stream = httpResponse.GetResponseStream())
                {
                    using (Stream destination = File.Create(vp_output_file))
                    {
                        int a;
                        Byte[] rBuf = new Byte[1024 * 16];   // 16k

                        while ((a = stream.Read(rBuf, 0, rBuf.Length)) > 0)
                        {
                            destination.Write(rBuf, 0, a);
                        }
                        destination.Flush();
                        destination.Close();
                        rBuf = null;
                    }
                }
            }
            httpResponse.Close();
            httpResponse = null;
            return vp_output_file;
        }
        private string GenParam(Dictionary<string, string> paramList)
        {
            string param = "";
            if (paramList != null)
            {
                foreach (KeyValuePair<string, string> item in paramList)
                {
                    if (string.IsNullOrEmpty(param)) param = "?"; else param += "&";
                    param += item.Key.Trim() + "=" + HttpUtility.UrlEncode(item.Value.Trim());
                }
            }
            return param;
        }
        private Byte[] CombineMultiplePDFs(List<Byte[]> fileNames)
        {
            Byte[] _Byte = null;
            Document document = new Document();
            MemoryStream _PageAll = new MemoryStream();
            PdfCopy writer = new iTextSharp.text.pdf.PdfCopy(document, _PageAll);
            if (writer == null)
            {
                return _Byte;
            }
            // step 3: we open the document
            document.Open();
            foreach (Byte[] fileName in fileNames.ToArray())
            {
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(fileName);
                reader.ConsolidateNamedDestinations();
                // step 4: we add content
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }
                PRAcroForm form = reader.AcroForm;
                if (form != null)
                {
                    writer.CopyAcroForm(reader);
                }
                reader.Close();

            }
            // step 5: we close the document and writer
            writer.Close();
            document.Close();


            return _PageAll.ToArray();
        }
        public byte[] ReadFully(MemoryStream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private void CombineMultiplePDFsCer(string[] fileNames, string outFile, bool deleteSource)
        {
            // step 1: creation of a document-object
            Document document = new Document();
            //create newFileStream object which will be disposed at the end
            using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
            {
                // step 2: we create a writer that listens to the document
                PdfCopy writer = new iTextSharp.text.pdf.PdfCopy(document, newFileStream);
                if (writer == null)
                {
                    return;
                }
                // step 3: we open the document
                document.Open();
                foreach (string fileName in fileNames)
                {
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();
                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }
                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        writer.CopyAcroForm(reader);
                    }
                    reader.Close();

                    if (deleteSource)
                    {
                        File.Delete(fileName);
                    }
                }
                // step 5: we close the document and writer
                writer.Close();
                document.Close();
            }//disposes the newFileStream object
        }
        public void ObjConvertJson(ref string StrJ, object inpput)
        {
            StrJ = JsonConvert.SerializeObject(inpput, Formatting.Indented);
        }
        #endregion

        [Route("Post/ZipPolicyCovid")]
        public HttpResponseMessage ZipPolicyCovid([FromBody] DataTable _Reqbody)
        {

            _response = new HttpResponseMessage();

            byte[] Resulte = null;
            string FileResulte = "";
            string _type = "";
            try
            {

                DataTable _dt = _Reqbody;
                if (_dt.Rows.Count > 1)
                {
                    string _Fliename = "CVP" + DateTime.Now.ToString("ddMMyyyHHmmssfff") + ".zip";
                    string FileOutPut = RootPathFilePolCovid + _Fliename;

                    ZipFile MyZip;
                    MyZip = ZipFile.Create(FileOutPut);
                    try
                    {
                        MyZip.BeginUpdate();
                        foreach (DataRow _dr in _dt.Rows)
                        {
                            string Path = _dr["physical_path"].ToString()
                                , Filename = _dr["policy_no"].ToString() + ".pdf";
                            MyZip.Add(Path, Filename);

                        }
                        MyZip.CommitUpdate();

                        MyZip.Close();
                        Resulte = File.ReadAllBytes(FileOutPut);
                        _type = MimeMapping.GetMimeMapping(Path.GetExtension(FileResulte));
                        File.Delete(FileOutPut);
                    }
                    catch (Exception)
                    {
                        _response.StatusCode = HttpStatusCode.InternalServerError;
                    }

                    finally
                    {
                        if (MyZip != null)
                        {
                            MyZip.Close();
                        }
                    }
                }
                else
                {
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        string _Path = _dr["physical_path"].ToString()
                            , Filename = _dr["policy_no"].ToString() + ".pdf";
                        FileResulte = _Path;
                        _type = MimeMapping.GetMimeMapping(Path.GetExtension(FileResulte));
                        Resulte = File.ReadAllBytes(FileResulte);
                    }

                }
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }



            _response.Content = new ByteArrayContent(Resulte);
            _response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_type);
            return _response;
        }


    }
}
