
using log4net;
using System;
using System.IO;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Drawing;

namespace MACRO.WebApi.Util
{
    public class RawPrinterHelper
    {
        private Font verdana10Font;
        private StreamReader reader;
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
       

        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, int dwCount)
        {
            try
            {
                using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
                //using (Impersonator imp = new Impersonator("timasterrp", "AQP", "Dl%mEfma5"))
                {
                    int dwError = 0, dwWritten = 0;
                    IntPtr hPrinter = new IntPtr(0);
                    DOCINFOA di = new DOCINFOA();
                    bool bSuccess = false; // Assume failure unless you specifically succeed.
                    di.pDocName = "Print Zebra";
                    di.pDataType = "RAW";

                    // Open the printer.
                    if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero)) //szPrinterName.Normalize()
                    {
                        // Start a document.
                        if (StartDocPrinter(hPrinter, 1, di))
                        {
                            // Start a page.
                            if (StartPagePrinter(hPrinter))
                            {
                                // Write your bytes.
                                bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                                EndPagePrinter(hPrinter);
                            }
                            EndDocPrinter(hPrinter);
                        }
                        ClosePrinter(hPrinter);
                    }
                    // If you did not succeed, GetLastError may give more information
                    // about why not.
                    if (bSuccess == false)
                    {
                        dwError = Marshal.GetLastWin32Error();
                    }
                    return bSuccess;
                }
            }
            catch (Exception e)
            {
                return false;
            }            
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName,int Copies)
        {
            log4net.Config.BasicConfigurator.Configure();
            ILog log = LogManager.GetLogger("MACRO");
            bool bSuccess = false;
            // Open the file.
            try
            {
                FileStream fs = new FileStream(szFileName, FileMode.Open);
                // Create a BinaryReader on the file.
                BinaryReader br = new BinaryReader(fs);
                // Dim an array of bytes big enough to hold the file's contents.
                byte[] bytes = new Byte[fs.Length];
               
                // Your unmanaged pointer.
                IntPtr pUnmanagedBytes = new IntPtr(0);
                int nLength;

                nLength = Convert.ToInt32(fs.Length);
                // Read the contents of the file into the array.
                bytes = br.ReadBytes(nLength);
                // Allocate some unmanaged memory for those bytes.
                pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
                // Copy the managed byte array into the unmanaged array.
                Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
                // Send the unmanaged bytes to the printer.
                for (int i=0;i<Copies;i++)
                    bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
                // Free the unmanaged memory that you allocated earlier.
                Marshal.FreeCoTaskMem(pUnmanagedBytes);
                br.Close();
                fs.Close();
            }
            catch (Exception e){
                log.Error(e.Message);
            }
            return bSuccess;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            int dwCount;

            // How many characters are in the string?
            // Fix from Nicholas Piasecki:
            // dwCount = szString.Length;
            dwCount = (szString.Length + 1) * Marshal.SystemMaxDBCSCharSize;

            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }

        public bool PrintText(string filename, string impresora, short copias) // UsuarioToken user byte[] bytesArchivo
        {
            //Stream stream = new MemoryStream(bytesArchivo);
            //var printerSettings = new PrinterSettings
            //{
            //    PrinterName = impresora,
            //    Copies = copias,

            //};
            //printerSettings.DefaultPageSettings.PaperSize = new PaperSize("ticket", 283, 1500);
            //printerSettings.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            //using (Impersonator imp = new Impersonator(user.Usuario, user.Dominio, user.Password))            
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.Print();

                void pd_PrintPage(object sender, PrintPageEventArgs ev)
                {
                    System.Drawing.Font printFont = new System.Drawing.Font("IDAutomationHC39M", 9);
                    SolidBrush br = new SolidBrush(Color.Black);
                    ev.Graphics.DrawString("test", printFont, Brushes.Black, new RectangleF(5, 30, 180, 50));
                }
                //using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
                //{
                //    reader = new StreamReader(filename);
                //    //Create a Verdana font with size 10  
                //    verdana10Font = new Font("Verdana", 10);
                //    //Create a PrintDocument object  
                //    PrintDocument pd = new PrintDocument();
                //    //Add PrintPage event handler  
                //    pd.PrintPage += new PrintPageEventHandler(this.PrintTextFileHandler);
                //    //Call Print Method  
                //    pd.Print();
                //    //Close the reader  
                //   
                //    if (reader != null)
                //        reader.Close();
                //}
                return true;
            }
            catch (Exception e)
            {
                // Log.Error(e);
                return false;
            }
        }

        private void PrintTextFileHandler(object sender, PrintPageEventArgs ppeArgs)
        {
            //Get the Graphics object  
            Graphics g = ppeArgs.Graphics;
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            //Read margins from PrintPageEventArgs  
            float leftMargin = ppeArgs.MarginBounds.Left;
            float topMargin = ppeArgs.MarginBounds.Top;
            string line = @"CT~~CD,~CC^~CT~^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH0,0^JMA^PR6,6~SD18^JUS^LRN^CI0^XZ~DG000.GRF,02304,024,,::::::::::::::::::S0QFBFA0,R01FPFBFHF,O03FE3FPFBFIFE,N01FFE7FVFE0,N0IFE3FPFBFKF8,M01FIF7FPFBFLF,M0JFE3FPFBFLFC0,L01FIFE7FPFBFLFE0,L03FIFE3FPFBFMF0,L0LF7FPFBFMFC,K01FJFE3FPFBFMFC,K01FC0H0E7FPFBFMF0,K07E02A003FPFBFLFE380,K0703FF007FPF3FLF8780,K083FHFE03FPF3FLF1F80,L0KF83FPF3FKFC1F01F0,K03FJFE3FOFE3EBFIF87E0FHF,K07FKF1FOFC0H01FHF0F81FHFC0,K0MF8FOFC0FE87FE1F83FHFE0,J01FLFC7FNF87FFE1F87E07FIF8,J03FLFE3FNF8FIF8F0FE0FJF0,J03FMF1FMFE1FIFC61FC1FIFC0,J03FMF8AMA03FIFE07FC3FHFE,J03FMFC0N07FJF0FFC7FHF8,J03FMFE7FLF8FKF8FFC7FFE020,I0H7NFE7FLF9FKFCFFC7FF87F0,I0H7NFE3FLF9FKFCFFE3FE0FF8,I0E7FMFE7FLF9FKFC7FF0703FF8,H01E7FNF3FLFBFKFE7FF803FHF8,H01F7FNF3FLF3FLF7FHF0FIF0,H03E7FNF3FLFBFLF7FKF0,H03E7FNF3FLF3FLF3FJFE0,H03E7FNFBFLFBFLF3FKF0,H03E7FNF3FLF3FLF3FKF0,H03EFOFBFLFBFLFBFKFC,H03E7FNF3FLF3FLF3FKFC,H03E7FNF3FLFBFLFBFKFE,H03E7FNF3FLFBFLF1FLF,H03E7FNF3FLFBFLFBFLF80,H01E3FNF7FLFBFLF3FLFE0,H01E3FMFE7FLFBFLFBFMF0,H01F3FMFE7FLF9FLF3FMFC,I0E3FMFCFMFDFLF3FNF80,I0F1FMF8FMFDFLF7FNF80,I070FMF8FMFEFKFE7FNF80,I0707FKFE1FMFE7FJFC7FNF80,I0387FKFE3FMFE7FJFCFOF80,J081FKFC7FNF3FJFCFOF80,L0LF87FNFBFJF1FOF80,L07FJF0FOF9FJF3FOF80,L03FIFE1FOF9FIFE3FKFEFFE,L03FIF83FOF8FIFC3FKFE1FC,L03FHFE07FOFCFIF93FKFE020,L03FHF847FOFCFIF7CFLF0,L07FHF0C3FOFCFHFE7CFJFE80,L07FFC3C0FOFC7FFCFE01FC,L0IF9FE02FMFE8FHFBFF,L0IF3F80H017FHFD0H07FF3FF,L0HFCFF0R0HFE7FE,L0HF1FE0R0HFC7FC,L0HF1FE0R0HF87FC,L0HF1FF0Q01FF07FC,L0HF0FF80P03FE03FE,L07F07FC0P07FC03FF,L0HF07FE0P0HF803FF,L07F01FF0P0HF800FF,L07F80FF80O0HFI0HF80,L07FC07FC0O07F0H07F80,L03FC03FE0O07F8003F80,L01FE01FF0O07F8001FC0,M07F007F80N03F80H0FE0,M03F003F80N03F80H0FE0,M03E0S03FC0H0HF0,gI03FC0I010,gI03EE,,:^XA^MMT^PW831^LL2233^LS0^FT530,425^XG000.GRF,1,1^FS^FT126,162^A0N,86,79^FH^FD3.2^FS^BY3,3,110^FT80,450^BCN,,Y,N^FD>;13260007185003>77774^FS^FT710,401^A0N,34,33^FH^FDOGN^FS^FT536,333^A0N,34,33^FH^FDAREQUIPA-PERU^FS^FT445,225^A0N,34,33^FH^FDEXT :^FS^FT32,232^A0N,34,33^FH^FDUND:^FS^FT458,169^A0N,34,33^FH^FDFV :^FS^FT530,460^A0N,28,16^FH^FDSACO GRASA DE CERDO B CONG^FS^FT34,280^A0N,28,20^FH^FDRegistro:^FS^FT111,235^A0N,68,50^FH^FD1^FS^FT522,228^A0N,39,38^FH^FD4181116^FS^FT521,172^A0N,39,38^FH^FD08-11-2022^FS^FT455,111^A0N,34,33^FH^FDLT :^FS^FT126,280^A0N,25,25^FH^FD18/11/2021 05:03^FS^FT36,330^A0N,25,21^FH^FDCod. A.S.000045-MINAGRI-SENASA-AREQUIPA^FS^FT25,61^A0N,34,33^FH^FD043308014^FS^FT521,113^A0N,39,38^FH^FD21470111^FS^FT31,125^A0N,28,31^FH^FDPeso^FS^FT31,155^A0N,28,31^FH^FDNeto^FS^FT95,140^A0N,28,31^FH^FD:^FS^FT180,61^A0N,31,28^FH^FDGRASA DE CERDO B CONG^FS^PQ1,0,1,Y^XZ^XA^ID000.GRF^FS^XZ";
            //Calculate the lines per page on the basis of the height of the page and the height of the font  
            linesPerPage = ppeArgs.MarginBounds.Height / verdana10Font.GetHeight(g);

            g.DrawString(line, verdana10Font, Brushes.Black, leftMargin, yPos, new StringFormat());
            //Now read lines one by one, using StreamReader  
            //while (count < linesPerPage && ((line = reader.ReadLine()) != null))
            //{
            //    //Calculate the starting position  
            //    yPos = topMargin + (count * verdana10Font.GetHeight(g));
            //    //Draw text  
            //    g.DrawString(line, verdana10Font, Brushes.Black, leftMargin, yPos, new StringFormat());
            //    //Move to next line  
            //    count++;
            //}
            //If PrintPageEventArgs has more pages to print  
            if (line != null)
            {
                ppeArgs.HasMorePages = true;
            }
            else
            {
                ppeArgs.HasMorePages = false;
            }
        }
    }

}