using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

using VMSViewer.Module;
using System.Windows.Input;

namespace VMSViewer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private double OriginalWidth;
        private double OriginalHeight;
        private ScaleTransform scale = new ScaleTransform();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitProc();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DoFinal();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        private void mainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize(e.NewSize.Width, e.NewSize.Height);
        }

        private void InitProc()
        {
            OriginalWidth = this.Width;
            OriginalHeight = this.Height;

            if (this.WindowState == WindowState.Maximized)
                ChangeSize(this.ActualWidth, this.ActualHeight);

            this.SizeChanged += mainGrid_SizeChanged;
        }

        private void DoFinal()
        {
            LogManager.Shared.AddLog("### 프로그램 종료 ###");
        }

        private void ChangeSize(double width, double height)
        {
            scale.ScaleX = width / OriginalWidth;
            scale.ScaleY = height / OriginalHeight;

            FrameworkElement rootElement = this.Content as FrameworkElement;

            rootElement.LayoutTransform = scale;
        }

        private void topGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Released) return;

            if (e.ClickCount == 2)
            {
                switch (this.WindowState)
                {
                    case WindowState.Normal:
                        this.WindowState = WindowState.Maximized;
                        break;
                    case WindowState.Maximized:
                        this.Width = 1600;
                        this.Height = 900;
                        this.WindowState = WindowState.Normal;
                        break;
                }
            }
            else
                DragMove();
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null) return;
            if (btn.Tag == null) return;

            int Tag = Convert.ToInt32(btn.Tag);

            switch ((TitleBarButtonType)Tag)
            {
                case TitleBarButtonType.Minimize:
                    this.WindowState = WindowState.Minimized;
                    break;
                case TitleBarButtonType.Maxmize:
                    if (this.WindowState == WindowState.Normal)
                        this.WindowState = WindowState.Maximized;
                    else
                    {
                        this.Width = 1600;
                        this.Height = 900;
                        this.WindowState = WindowState.Normal;
                    }
                    break;
                case TitleBarButtonType.Close:
                    this.Close();
                    break;
            }
        }
    }
}
