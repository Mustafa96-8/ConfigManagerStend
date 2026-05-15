using ConfigManagerStend.Forms;
using ConfigManagerStend.Infrastructure.Commands;
using ConfigManagerStend.Infrastructure.Enums;
using System.Windows;
using System.Windows.Controls;
namespace ConfigManagerStend
{
    public partial class MainWindow : Window
    {
        private DetailsInfoCommand _detailsCommand;

        public static ListView AllModules = new ();

        public static ListView AllStands = new();

        public MainWindow() 
        {
            InitializeComponent();
            _detailsCommand = new DetailsInfoCommand();
            DataContext = _detailsCommand;
            _detailsCommand.LoadStandsAsync().Wait();
            AllModules = ModuleListView;
            AllStands = StandListView;
        }

        private void openStandWin_Click(object sender, RoutedEventArgs e)
        {
            StendWindow sw = new();
            sw.ShowDialog();
        }

        private void RefreshStandList_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.UpdateGlobalDisplay();
        }

        private void StartWebBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.DoIISCommand(IssAppType.Web, IssCommands.Start);
        }
        private void StartSrvBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.DoIISCommand(IssAppType.Srv,IssCommands.Start);
        }
        private void RestartWebBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.DoIISCommand(IssAppType.Web, IssCommands.Restart);
        }
        private void RestartSrvBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.DoIISCommand(IssAppType.Srv,IssCommands.Restart);
        }
        private void StopWebBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.DoIISCommand(IssAppType.Web, IssCommands.Stop);
        }
        private void StopSrvBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.DoIISCommand(IssAppType.Srv, IssCommands.Stop);
        }
        private void RefreshIisStatusBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.UpdateIISStatus();
        }

        private void RefreshListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            _detailsCommand.UpdateModuleDisplay(true);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}");
            }
            e.Handled = true;
        }
    }
}