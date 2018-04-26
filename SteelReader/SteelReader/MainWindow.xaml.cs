using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text.pdf;
using Word = Microsoft.Office.Interop.Word;
namespace SteelReader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Dictionary<string,string> pdfPathes = new Dictionary<string,string>();

        List<EzAnnotation> Annotations = new List<EzAnnotation>();


        public void Open() {
        
            OpenFileDialog ofd = new OpenFileDialog();
         
            ofd.Filter =" PDF Files | *.PDF";
          
            ofd.Multiselect = true;
          
            if (ofd.ShowDialog() != null)
            {
               
                foreach (var i in ofd.FileNames) {
                    pdfPathes.Add(i, getName(i));
                
                }
            }
          


        }

        public static string getName(string str) {
            int lastChar=0;
            for (int i = str.Length-1; i > 0; i--) {
                if(str[i]== '\\' )  {
                    lastChar = i+1;
                    return str.Substring(lastChar);
                }
               
            }
            return str;
        }

        
        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            Open();
          
        }

        private void ImportToWordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pdfPathes != null) {
                foreach (var i in pdfPathes) {

                    Annotations.AddRange(AccessPDFcs.GetPdf(i.Key).ToAnnotList());
                }

            }
            else { }
        }

        private void EraseBtn_Click(object sender, RoutedEventArgs e)
        {
            ExportWord();
        }

        private void ExportWord()
        { 
            //Creation
            Word.Application WordApp = new Word.Application();
            WordApp.Visible = true;
            WordApp.WindowState = Word.WdWindowState.wdWindowStateNormal;

            // Create Document 

            Word.Document WordDoc = WordApp.Documents.Add();

            //Add Content

            Word.Paragraph WordPara = WordDoc.Paragraphs.Add();
            Word.Table WordTable =  WordPara.Range.Tables.Add(WordPara.Range,1, 3);
            WordTable.Borders.Enable = 1;
            WordTable.Borders.InsideColor = Word.WdColor.wdColorRed;
            WordTable.Columns[1].Cells[1].Range.Text = "1";
            WordTable.Columns[2].Cells[1].Range.Text = "2";
            WordTable.Columns[3].Cells[1].Range.Text = "3";


            WordDoc.SaveAs2("Word.docx"); 
          

        }
    }

  
}
