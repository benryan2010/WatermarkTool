using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace AddWatermarks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*********************************");
            Console.WriteLine("***Watermark Stuff***************");
            Console.WriteLine("*********************************");

            Console.WriteLine();
            Console.WriteLine("Specify Directory:");

            string path = Console.ReadLine();

            var handler = new FileHandler();

            var files = handler.GetFiles(path);

            foreach(var nextFile in files)
            {
                //create pdfreader object to read sorce pdf
                PdfReader pdfReader = new PdfReader(nextFile);

                //create stream of filestream or memorystream etc. to create output file
                FileStream stream = new FileStream(path + "\\" + "Watermarked" + "\\" + Path.GetFileName(nextFile), FileMode.OpenOrCreate);

                //create pdfstamper object which is used to add addtional content to source pdf file
                PdfStamper pdfStamper = new PdfStamper(pdfReader, stream);

                //iterate through all pages in source pdf
                for (int pageIndex = 1; pageIndex <= pdfReader.NumberOfPages; pageIndex++)
                {
                    //Rectangle class in iText represent geomatric representation... in this case, rectanle object would contain page geomatry
                    Rectangle pageRectangle = pdfReader.GetPageSizeWithRotation(pageIndex);

                    //pdfcontentbyte object contains graphics and text content of page returned by pdfstamper
                    PdfContentByte pdfData = pdfStamper.GetUnderContent(pageIndex);

                    //create fontsize for watermark
                    pdfData.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 40);

                    //create new graphics state and assign opacity
                    PdfGState graphicsState = new PdfGState();
                    graphicsState.FillOpacity = 0.4F;

                    //set graphics state to pdfcontentbyte
                    pdfData.SetGState(graphicsState);

                    //set color of watermark
                    pdfData.SetColorFill(BaseColor.GRAY);
                    //indicates start of writing of text
                    pdfData.BeginText();
                    //show text as per position and rotation
                    pdfData.ShowTextAligned(Element.ALIGN_CENTER, "Ventus Sample Form", pageRectangle.Width / 2, pageRectangle.Height / 2, 45);
                    //call endText to invalid font set
                    pdfData.EndText();
                }
                //close stamper and output filestream
                pdfStamper.Close();
                stream.Close();
            }
        }

        private void Watermark(string directory)
        {

        }

        
    }

    public class FileHandler
    {
        public List<string> GetFiles(string directory)
        {
            var result = new List<string>();

            foreach (var files in Directory.GetFiles(directory, "*.pdf"))
                result.Add(files);

            return result;
        }
    }
}
