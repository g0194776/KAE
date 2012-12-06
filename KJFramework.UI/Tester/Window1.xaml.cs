using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using KJFramework.Checker;

namespace Tester
{
	public partial class Window1
	{
        private BasicCpuPerformanceTaskChecker _checker = new BasicCpuPerformanceTaskChecker();
		public Window1()
		{
			InitializeComponent();
            MouseRightButtonDown += Window1_MouseRightButtonDown;
		    lc.MaxVerticalValue = 100;
		    _checker.CheckTimeSpan = 1000;
            _checker.PerformanceValueChanged += CheckerPerformanceValueChanged;
			// 在此点之下插入创建对象所需的代码。
		}

        void CheckerPerformanceValueChanged(object sender, KJFramework.EventArgs.PerformanceValueChangedEventArgs<float> e)
        {
            if (e.Value != 100)
            {
                Debug.WriteLine("==> new performance value : " + e.Value);
                lc.AddPoint(e.Value);
            }
        }

        void Window1_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContextMenu mainMenu = new ContextMenu();
            mainMenu.Height = 200;
            mainMenu.Width = 100;
            MenuItem item1 = new MenuItem();
            item1.Height = 20;
            item1.Header = "    Paste";
            item1.Style = (Style)FindResource("Normal_Style_QQMenuItem");
            mainMenu.Items.Add(item1);
            MenuItem item2 = new MenuItem();
            item2.Header = "    Clear";
            item1.Items.Add(item2);
            MenuItem item3 = new MenuItem();
            item3.Header = "    Copy";
            item1.Items.Add(item3);
            ContextMenu = mainMenu;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            _checker.Start();
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            _checker.Stop();
        }

        private void DrawClick(object sender, System.Windows.RoutedEventArgs e)
        {
            lc.AddPoint(27);
            lc.AddPoint(35);
            lc.AddPoint(80);
            lc.AddPoint(96);
            lc.AddPoint(130);
            lc.AddPoint(120);
            lc.AddPoint(100);
            lc.AddPoint(80);
            lc.AddPoint(90);
            lc.AddPoint(120);
            lc.AddPoint(135);
            lc.AddPoint(150);
            lc.AddPoint(189);
            lc.AddPoint(177);
            lc.AddPoint(200);
            lc.AddPoint(199);
            lc.AddPoint(178);
            lc.AddPoint(168);
        }
	}
}