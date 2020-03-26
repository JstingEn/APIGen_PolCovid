using SignLib.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace APIGen_PolCovid.Controllers
{
    public class ClsCer
    {
        #region PDFSigner

        public void SignPdf(string inputFile, string Password)
        {
            try
            {

                string _PathCer = System.Web.Hosting.HostingEnvironment.MapPath("~/Resource/SMK.pfx");

                X509Certificate2 card = new X509Certificate2(_PathCer, "adminsmk", X509KeyStorageFlags.MachineKeySet);
                string outputFile = inputFile;
                PdfSignature pdfSign = BuildPDFSignatureObject(inputFile, card);
                pdfSign.AppendSignature = false;
                //  set the document restrictions
                pdfSign.Encryption.DocumentRestrictions = PdfDocumentRestrictions.AllowContentCopying |
                PdfDocumentRestrictions.AllowPrinting;
                //set the encryption algorithm
                pdfSign.Encryption.EncryptionAlgorithm =
                PdfEncryptionAlgorithm.StandardEncryption128BitRC4;
                //set the encryption method
                pdfSign.Encryption.EncryptionMethod = PdfEncryptionMethod.PasswordSecurity;
                //set the owner password
                pdfSign.Encryption.UserPassword = Password;
                // ============================== การนำ ภาพมาซ้อนในไฟล์ PDF ========================================================
                string Path_Img = System.Web.Hosting.HostingEnvironment.MapPath("~/Resource/Blank.png");//ตำแหน่งไฟล์รูปภาพ
                pdfSign.SignatureImage = File.ReadAllBytes(Path_Img);                               //รูปภาพ
                pdfSign.SignatureImageType = SignatureImageType.ImageWithNoText;                   //ใช้รูปภาพ + ไม่มีข้อความ
                pdfSign.SignatureAppearsOnAllPages = true;                                        //รุปแสดงทุกหน้า
                pdfSign.SignatureAdvancedPosition = new System.Drawing.Rectangle(0, 0, 595, 842); //ใช้กำหนดขนาด+ตำแหน่ง
                pdfSign.HashAlgorithm = SignLib.HashAlgorithm.SHA1;
                //===============================================================================================================
                File.WriteAllBytes(outputFile, pdfSign.ApplyDigitalSignature());
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred: " + ex.Message);
            }
        }
        public Byte[] SignPdf_Byte(Byte[] inputFile, string Password)
        {
            Byte[] _File = null;
            try
            {
                string serialNumber = "00733ce7d766bae9cd55";
                string _PathCer = System.Web.Hosting.HostingEnvironment.MapPath("~/Resource/SMK.pfx");

                X509Certificate2 card = new X509Certificate2(_PathCer, "adminsmk", X509KeyStorageFlags.MachineKeySet);
                //string outputFile = inputFile;
                PdfSignature pdfSign =  BuildPDFSignatureObject(inputFile, card);

                pdfSign.AppendSignature = false;
                //  set the document restrictions
                pdfSign.Encryption.DocumentRestrictions = PdfDocumentRestrictions.AllowContentCopying |
                PdfDocumentRestrictions.AllowPrinting;
                //set the encryption algorithm
                pdfSign.Encryption.EncryptionAlgorithm =
                PdfEncryptionAlgorithm.StandardEncryption128BitRC4;
                //set the encryption method
                pdfSign.Encryption.EncryptionMethod = PdfEncryptionMethod.PasswordSecurity;
                //set the owner password
                pdfSign.Encryption.UserPassword = Password;
                // ============================== การนำ ภาพมาซ้อนในไฟล์ PDF ========================================================
                string Path_Img = System.Web.Hosting.HostingEnvironment.MapPath("~/Resource/Blank.png");//ตำแหน่งไฟล์รูปภาพ
                pdfSign.SignatureImage = File.ReadAllBytes(Path_Img);                               //รูปภาพ
                pdfSign.SignatureImageType = SignatureImageType.ImageWithNoText;                   //ใช้รูปภาพ + ไม่มีข้อความ
                pdfSign.SignatureAppearsOnAllPages = true;                                        //รุปแสดงทุกหน้า
                pdfSign.SignatureAdvancedPosition = new System.Drawing.Rectangle(0, 0, 595, 842); //ใช้กำหนดขนาด+ตำแหน่ง
                pdfSign.HashAlgorithm = SignLib.HashAlgorithm.SHA1;
                //===============================================================================================================
                //File.WriteAllBytes(outputFile, pdfSign.ApplyDigitalSignature());

                _File = pdfSign.ApplyDigitalSignature();
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred: " + ex.Message);
            }
            return _File;
        }
        public void SignPdf(string inputFile, string outputFile, string Password)
        {
            try
            {

                string _PathCer = System.Web.Hosting.HostingEnvironment.MapPath("~/Resource/SMK.pfx");

                X509Certificate2 card = new X509Certificate2(_PathCer, "adminsmk", X509KeyStorageFlags.MachineKeySet);
                PdfSignature pdfSign = BuildPDFSignatureObject(inputFile, card);
                pdfSign.AppendSignature = false;
                //  set the document restrictions
                pdfSign.Encryption.DocumentRestrictions = PdfDocumentRestrictions.AllowContentCopying |
                PdfDocumentRestrictions.AllowPrinting;
                //set the encryption algorithm
                pdfSign.Encryption.EncryptionAlgorithm =
                PdfEncryptionAlgorithm.StandardEncryption128BitRC4;
                //set the encryption method
                pdfSign.Encryption.EncryptionMethod = PdfEncryptionMethod.PasswordSecurity;
                //set the owner password
                pdfSign.Encryption.UserPassword = Password;
                // ============================== การนำ ภาพมาซ้อนในไฟล์ PDF ========================================================
                string Path_Img = System.Web.Hosting.HostingEnvironment.MapPath("~/Resource/Blank.png");//ตำแหน่งไฟล์รูปภาพ
                pdfSign.SignatureImage = File.ReadAllBytes(Path_Img);                               //รูปภาพ
                pdfSign.SignatureImageType = SignatureImageType.ImageWithNoText;                   //ใช้รูปภาพ + ไม่มีข้อความ
                pdfSign.SignatureAppearsOnAllPages = true;                                        //รุปแสดงทุกหน้า
                pdfSign.SignatureAdvancedPosition = new System.Drawing.Rectangle(0, 0, 595, 842); //ใช้กำหนดขนาด+ตำแหน่ง
                pdfSign.HashAlgorithm = SignLib.HashAlgorithm.SHA1;
                //===============================================================================================================
                File.WriteAllBytes(outputFile, pdfSign.ApplyDigitalSignature());
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occurred: " + ex.Message);
            }
        }

        static private X509Certificate2Collection GetCertificatesFromStore(bool validonly)
        {
            X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                st.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

                X509Certificate2Collection col = st.Certificates.Find(X509FindType.FindByIssuerName, "", validonly);

                return col;
            }
            catch
            {
                return null;
            }
            finally
            {
                st.Close();
            }
        }

        static private PdfSignature BuildPDFSignatureObject(string inputFile, X509Certificate2 certificate)
        {
            try
            {
                string serialNumber = "00733ce7d766bae9cd55";

                PdfSignature pdfObj = new PdfSignature(serialNumber);

                pdfObj.LoadPdfDocument(inputFile);

                pdfObj.DigitalSignatureCertificate = certificate;

                return pdfObj;

            }

            catch
            {
                throw;
            }

        }
        static private PdfSignature BuildPDFSignatureObject(Byte[] inputFile, X509Certificate2 certificate)
        {
            try
            {
                string serialNumber = "00733ce7d766bae9cd55";

                PdfSignature pdfObj = new PdfSignature(serialNumber);

                pdfObj.LoadPdfDocument(inputFile);

                pdfObj.DigitalSignatureCertificate = certificate;

                return pdfObj;

            }

            catch
            {
                throw;
            }

        }
        #endregion
    }
}