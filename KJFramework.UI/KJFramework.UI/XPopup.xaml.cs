using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace KJFramework.UI
{
	public partial class XPopup
	{
        public static readonly DependencyProperty DataListProperty = DependencyProperty.Register("DataList", typeof(ObservableCollection<MenuItem>), typeof(XPopup));
        private ObservableCollection<MenuItem> _items = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                SetValue(DataListProperty,_items);
            }
        }

		public XPopup()
		{
			this.InitializeComponent();
            _items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(_items_CollectionChanged);
			// 在此点之下插入创建对象所需的代码。
		}

        void _items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetValue(DataListProperty, _items);
            UpdateLayout();
        }

	}
}