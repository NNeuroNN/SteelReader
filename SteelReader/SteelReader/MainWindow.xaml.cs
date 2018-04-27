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
using System.Web;
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
        /// <summary >
        /// Пути к Пдф документам
        /// </summary>
        Dictionary<string,string> pdfPathes = new Dictionary<string,string>();

        /// <summary >
        /// Комментарии в выбранных пдф документах
        /// </summary>
        List<EzAnnotation> Annotations = new List<EzAnnotation>();

        /// <summary >
        /// Выбор PDF Файлов,заполнение списка "PdfPathes",обновление контейнера "PdfListBox"
        /// </summary>
        public void Open() {
        
            OpenFileDialog ofd = new OpenFileDialog();
         
            ofd.Filter =" PDF Files | *.PDF";
          
            ofd.Multiselect = true;
          
            if (ofd.ShowDialog() != null)
            {

                foreach (var i in ofd.FileNames) {
                    try
                    {
                        pdfPathes.Add(i, getName(i));
                    }
                    catch (ArgumentException) { MessageBox.Show(i+", этот файл уже выбран!"); }
                    PdfListBox.Items.Refresh();
                    // PdfListBox.Items.Add(pdfPathes[i]);
                    //  PdfListBox.Items.Clear();
                    string pth = pdfPathes.Last().Key.ToString();
                    Uri iuri = new Uri(pth, UriKind.Absolute);
                    PdfBrowser.Navigate(iuri);
                }
            }
            


        }

        /// <summary >
        /// Изъятие из полного пути название файла
        /// </summary>
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

        /// <summary >
        /// Проверка списка путей , заполнение списка аннотаций, вызов функции экспорта в Word
        /// </summary>
        private void ImportToWordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pdfPathes.Values.Count >0) {
                Annotations.Clear();
                foreach (var i in pdfPathes) {

                    Annotations.AddRange(AccessPDFcs.GetPdf(i.Key).ToAnnotList());
                }
                if (Annotations.Count > 0)
                    ExportWord();
                else
                    MessageBox.Show("В выбраных файлах не было найдено комментариев");
            }
            else { MessageBox.Show("Список Пдф пуст, выберите документы"); }
           
        }

        private void EraseBtn_Click(object sender, RoutedEventArgs e)
        {
            pdfPathes.Clear();
            Annotations.Clear();
            PdfListBox.Items.Refresh();
        }

        /// <summary >
        /// Создание Файла .docx,заполнение его таблицами с аннотациями
        /// </summary>
        private void ExportWord()
        {
            int ind = 1;
            //Creation
            Word.Application WordApp = new Word.Application();
            WordApp.Visible = true;
            WordApp.WindowState = Word.WdWindowState.wdWindowStateMaximize;

            // Create Document 

            Word.Document WordDoc = WordApp.Documents.Add();

            //Add Content

            Word.Paragraph WordPara = WordDoc.Paragraphs.Add();
            var Range = WordPara.Range;
          
           
            Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            Range.Font.Size = 14;

            WordPara.Range.Text += "Список замечаний по Договору №____ \n \n";


            for (int i = 0; i < Annotations.Count; i++)
            {

               var Tab= WordPara.Range.Tables.Add(WordPara.Range, 1, 4);
            

                Tab.Borders.Enable = 1;
                Tab.Borders.InsideColor = Word.WdColor.wdColorBlack;
           
                Tab.Columns[1].Cells[1].Range.Text = ind.ToString();
                Tab.Columns[2].Cells[1].Range.Text = Annotations.ToArray()[i].Author;
                Tab.Columns[3].Cells[1].Range.Text = Annotations.ToArray()[i].ADate;
                Tab.Columns[4].Cells[1].Range.Text = Annotations.ToArray()[i].AContent;
                
              
                WordPara.Range.Text +=ind.ToString()+ ")Выполнено:  \n\n";
                Tab.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitContent);
                ind++;
            }
       
         
          

        }

        /// <summary >
        /// Обработчик клика по удалению элемента списка пдф
        /// </summary>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }

        /// <summary >
        /// Обработчик клика по удалению элемента списка пдф
        /// </summary>
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
