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


namespace ConfigManagerStend
{
    /// <summary>
    /// Interaction logic for DetailInfo.xaml
    /// </summary>
    public partial class DetailInfo : Window
    {
        public static ListView AllDitails;
        private DetailsInfoCommand _detailsCommand;
        public DetailInfo(int standId)
        {
            InitializeComponent();
            _detailsCommand = new DetailsInfoCommand();
            DataContext = _detailsCommand;
            _detailsCommand.LoadModulesAsync().Wait();
            AllDitails = ViewDetails;
        }

        private void updateInfo_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.UpdateDisplay();
        }
    }
}
