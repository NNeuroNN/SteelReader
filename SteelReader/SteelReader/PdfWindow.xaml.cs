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
using System.Windows.Shapes;

namespace SteelReader
{
    /// <summary>
    /// Логика взаимодействия для PdfWindow.xaml
    /// </summary>
    public partial class PdfWindow : Window
    {
        public PdfWindow()
        {
            InitializeComponent();
        }
        public PdfWindow(Uri str) {
            InitializeComponent();
            Browser.Navigate(str);
            //Browser.Refresh();
        }
    }
}
