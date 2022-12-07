
namespace Messenger.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DetectionButton.IsChecked = true;
            this.DataContext = new MainVewModel();
        }

        private void RegisterButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            DetectionButton.IsChecked = false;
            SettingsButton.IsChecked = false;
        }

        private void DetectionButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            RegisterButton.IsChecked = false;
            SettingsButton.IsChecked = false;
        }

        private void SettingsButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            DetectionButton.IsChecked = false;
            RegisterButton.IsChecked = false;
        }
    }
}