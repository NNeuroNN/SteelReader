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
            //var pdf =  AccessPDFcs.GetPdf("");
            //  if (pdf != null) { }
            //  foreach (var i in pdf.Pages().GetAnnots())
            //  {
            //      try
            //      {
            //          AnnotationTextBox.Text += i.GetAnnotItem(PdfName.CONTENTS).ToUnicodeString() + "\n";
            //      }
            //      catch(Exception){ }
            //  }
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
