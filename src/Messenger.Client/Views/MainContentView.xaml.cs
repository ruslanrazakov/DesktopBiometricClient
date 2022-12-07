using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Messenger.Client.MVVM;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace Messenger.Client.Views
{
    /// <summary>
    /// Interaction logic for DetectionView.xaml
    /// </summary>
    public partial class MainContentView : UserControl
    {
        public MainContentView()
        {
            InitializeComponent();
        }
    }
}
