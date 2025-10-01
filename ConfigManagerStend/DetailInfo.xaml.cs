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
        public static ListView AllModules;
        private DetailsInfoCommand _detailsCommand;
        internal DetailInfo(DetailsInfoCommand detailsInfoCommand)
        {
            InitializeComponent();
            _detailsCommand = detailsInfoCommand;
            DataContext = _detailsCommand;
            _detailsCommand.LoadModulesAsync().Wait();
            AllModules = ViewDetails;
        }

        private void updateInfo_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.UpdateDetailsDisplay();
        }
    }
}
