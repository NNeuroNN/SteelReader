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
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
