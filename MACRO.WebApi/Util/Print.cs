// Print class definition
//thanks MSDN for most of this code
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;

namespace MACRO.WebApi.Util
{
    class Print
    {
        private StreamReader streamToPrint;
        private Stream buff;
        private byte[] bytesString;
        private Font printFont;

        public Print()
        {
            streamToPrint = null;
            printFont = new Font("Arial", 10);
        }

        public bool PrintData(byte[] bytes, string ipPrinter, short copies, int size)
        {
            bytesString = bytes;
            Stream buff = new MemoryStream(bytes);
            var printerSettings = new PrinterSettings
            {
                PrinterName = ipPrinter,
                Copies = copies,

            };
            printerSettings.DefaultPageSettings.PaperSize = new PaperSize("printer", size, 1500);
            printerSettings.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            try
            {               
                streamToPrint = new StreamReader(buff);
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings = printerSettings;
                //pd.PrinterSettings.PrinterName = ipPrinter;
                //pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPageSimple);
                pd.Print();
                pd.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (streamToPrint != null)
                {
                    streamToPrint.Close();
                    streamToPrint.Dispose();
                }
                if (buff != null)
                {
                    buff.Close();
                    buff.Dispose();
                }                
            }
        }

        private void pd_PrintPageSimple(object sender, PrintPageEventArgs ev)
        {          
            ev.Graphics.AddMetafileComment(bytesString);               
            ev.Graphics.Dispose();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;
            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height / this.printFont.GetHeight(ev.Graphics);
            // Print each line of the stream
            while (count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;

            printFont.Dispose();
            ev.Graphics.Dispose();
        }
    }

    public enum SizePrint : int
    {
        Ticlek = 283,
        PrinterA4 = 1100,
        Zebra = 500
    }
}