using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
namespace SteelReader
{
    static class AccessPDFcs
    {

        static public PdfReader GetPdf(string path) {
            return new PdfReader(path);
        }

        static public List<PdfDictionary> Pages(this PdfReader pdf)
        {
            List<PdfDictionary> dict = new List<PdfDictionary>(); ;
            for (int i = 1; i < pdf.NumberOfPages; i++) {
                dict.Add(pdf.GetPageN(i));
            }
            return dict;
        }

        static public List<PdfDictionary> GetAnnots(this List<PdfDictionary> dicts) {
            List<PdfDictionary> list = new List<PdfDictionary>();
            foreach (var dict in dicts)
            {
                try
                {
                    PdfArray annotAray = dict.GetAsArray(PdfName.ANNOTS);
                    for (int i = 0; i < annotAray.Length; i++)
                    {

                        list.Add(annotAray.GetAsDict(i));
                    }
                }

                catch (Exception) { }
                
            }
         
            return list;
        }

        static public PdfString GetAnnotItem(this PdfDictionary dict, PdfName name) {
            try
            {
                return dict.GetAsString(name);
            }
            catch (Exception) { }
            return new PdfString("Неудача");
        }

        public static List<EzAnnotation> ToAnnotList(this PdfReader pdf) {

            List<EzAnnotation> list = new List<EzAnnotation>();


          
             if (pdf != null) { }
            foreach (var i in pdf.Pages().GetAnnots())
            {
                try
                {
                    var date = i.GetAnnotItem(PdfName.CREATIONDATE).ToUnicodeString();
                    string str ="Замечание от "+ date.Substring(2, 4)+"/"+ date.Substring(6, 2) + "/" + date.Substring(8, 2) + "   " + date.Substring(10, 2) + ":" + date.Substring(12, 2) ;

                    list.Add(new EzAnnotation { ADate = str,
                                                AContent = i.GetAnnotItem(PdfName.CONTENTS).ToUnicodeString(),
                                                Author = i.GetAnnotItem(PdfName.T).ToUnicodeString()
                    }); 
                }
                catch (Exception) { }
            }

            return list;
        }
    }
    public class EzAnnotation
    {
     public  string ADate;
     public string Author;
     public   string AContent;

    }
}
