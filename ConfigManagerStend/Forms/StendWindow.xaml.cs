using ConfigManagerStend.Infrastructure.Commands;
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

namespace ConfigManagerStend.Forms
{
    /// <summary>
    /// Interaction logic for StendWindow.xaml
    /// </summary>
    public partial class StendWindow : Window
    {
        public StendWindow()
        {
            InitializeComponent();
            DataContext = new StendWindCommand();
        }
    }
}
