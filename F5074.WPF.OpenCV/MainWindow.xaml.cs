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
using System.Windows.Threading;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;


namespace F5074.WPF.OpenCV
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        CvCapture cap;
        DispatcherTimer TimerClock;
        WriteableBitmap wb;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (InitWebCamera())
            {
                StartTimer();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error");
            }
        }

        private bool InitWebCamera()
        {
            try
            {
                cap = CvCapture.FromCamera(CaptureDevice.DShow, 0); // 0 First Camera
                cap.FrameWidth = 640;
                cap.FrameHeight = 480;
                wb = new WriteableBitmap(cap.FrameWidth, cap.FrameHeight, 96, 96, PixelFormats.Bgr24, null);
                image.Source = wb;
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void StartTimer()
        {
            TimerClock = new DispatcherTimer();
            TimerClock.Interval = new TimeSpan(0, 0, 0, 0, 33);
            TimerClock.IsEnabled = true;
            TimerClock.Tick += TimerClock_Tick;
            
        }

        private void TimerClock_Tick(object sender, EventArgs e)
        {
            using(IplImage src = cap.QueryFrame())
            {
                WriteableBitmapConverter.ToWriteableBitmap(src, wb);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TimerClock.IsEnabled = false;
            if (cap != null) cap.Dispose();
        }
    }
}
