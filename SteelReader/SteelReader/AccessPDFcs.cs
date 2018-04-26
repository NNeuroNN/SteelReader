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
                PdfArray annotAray = dict.GetAsArray(PdfName.ANNOTS);
                for (int i = 0; i < annotAray.Length; i++) {
                    try
                    {
                        list.Add(annotAray.GetAsDict(i));
                    }
                    catch (Exception) { }
                }
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
    }
}
