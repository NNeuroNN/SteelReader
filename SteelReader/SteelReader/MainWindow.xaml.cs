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
        private DateTime downTime;
        private object downSender;

        public MainWindow()
        {
          
            InitializeComponent();
            PdfListBox.ItemsSource = pdfPathes;
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
                    PdfListBox.Items.Refresh();
                   // PdfListBox.Items.Add(pdfPathes[i]);
                  //  PdfListBox.Items.Clear();
                    
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
            ExportWord();
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
            WordApp.WindowState = Word.WdWindowState.wdWindowStateMaximize;

            // Create Document 

            Word.Document WordDoc = WordApp.Documents.Add();

            //Add Content

            Word.Paragraph WordPara = WordDoc.Paragraphs.Add();
            var Range = WordPara.Range;
           // WordPara.AL = Word.WdHorizontalLineAlignment.wdHorizontalLineAlignCenter;
           
            Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            Range.Font.Size = 14;
           
            WordPara.Range.Text += "Список замечаний по Договору №____ \n \n";
            for (int i = 0; i < Annotations.Count; i++)
            {
                
                Word.Table WordTable = WordPara.Range.Tables.Add(WordPara.Range, 1, 3);
                WordTable.Borders.Enable = 1;
                WordTable.Borders.InsideColor = Word.WdColor.wdColorBlack;
                WordTable.Columns[1].Cells[i+1].Range.Text = Annotations.ToArray()[i].Author;
                WordTable.Columns[2].Cells[i+1].Range.Text = Annotations.ToArray()[i].ADate;
                WordTable.Columns[3].Cells[i+1].Range.Text = Annotations.ToArray()[i].AContent;
            }

            
            WordDoc.SaveAs2("Word.docx"); 
          

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released &&
           sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {

                    var id = (sender as Image).Tag.ToString();
                    // AnnotationTextBox.Text += (sender as Image).Tag.ToString();


                    pdfPathes.Remove(id);
                    PdfListBox.Items.Refresh();

                    //if (PdfListBox.SelectedItem == null) return;
                    //var p = PdfListBox.SelectedItem;
                    //pdfPathes.Remove();
                    //PdfListBox.Items.Remove(p);
                    // Do click
                }
            }
        }
    }

  
}
