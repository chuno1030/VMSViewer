using System;
using System.Windows;
using System.Windows.Controls;  

namespace VMSViewer
{
    /// <summary>
    /// ucGridViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ucGridViewer : UserControl
    {
        public ucGridViewer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitProc();
        }

        private void InitProc()
        {
            SplitGridViewer();
        }

        private void SplitGridViewer()
        {
            int splitCount = 4;

            for (int row = 0; row < splitCount; row++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.ActualHeight / splitCount, GridUnitType.Star)});
            }

            for (int col = 0; col < splitCount; col++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(this.ActualWidth / splitCount, GridUnitType.Star)});
            }

            for (int row = 0; row < splitCount; row++)
            {
                for (int column = 0; column < splitCount; column++)
                {
                    ucViewer ucViewer = new ucViewer();

                    Grid.SetRow(ucViewer, row);
                    Grid.SetColumn(ucViewer, column);

                    mainGrid.Children.Add(ucViewer);
                }
            }
        }
    }
}
