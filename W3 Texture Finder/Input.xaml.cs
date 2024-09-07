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

namespace W3_Texture_Finder
{
    /// <summary>
    /// Interaction logic for Input.xaml
    /// </summary>
    public partial class Input : Window
    {
        public Input()
        {
            InitializeComponent();
            box.Focus();
        }

        private void set(object sender, RoutedEventArgs e)
        {
            if (box.Text.Trim().Length > 0) { DialogResult = true; }
        }

        private void EnterOK(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                set(null, null);
            }
           
        }
    }
}
