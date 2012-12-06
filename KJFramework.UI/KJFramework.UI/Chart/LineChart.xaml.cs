using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using KJFramework.UI.Struct;

namespace KJFramework.UI.Chart
{
    /// <summary>
    /// LineChart.xaml 的交互逻辑
    /// </summary>
    [Description("支持点线形式的性能图表，采用了\"值驱动\"的形式绘图。")]
    public partial class LineChart
    {
        #region 构造函数

        /// <summary>
        ///     支持线性显示的图标
        /// </summary>
        public LineChart()
        {
            InitializeComponent();
            _height = Height;
            _width = Width;
            InitializeBackground();
        }


        #endregion

        #region 成员

        private List<Line> _horizontalLines = new List<Line>();
        private List<Line> _verticalLines = new List<Line>();
        private List<LinePoint> _points = new List<LinePoint>();
        private List<Line> _performanceLines = new List<Line>();
        private delegate void DelegatePoint(double value);
        private ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim _lockerPoint = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim _lockerPerformance= new ReaderWriterLockSlim();
        private double _height;
        private double _width;

        private int _intervalSize = 15;
        /// <summary>
        ///     获取或设置单元格距离间隔
        /// </summary>
        [Description("获取或设置单元格距离间隔")]
        [Category("自定义控件属性")]
        public int IntervalSize
        {
            get { return _intervalSize; }
            set { _intervalSize = value; }
        }

        private int _chartIntervalSize = 3;
        /// <summary>
        ///     获取或设置前景面板线条距离间隔
        /// </summary>
        [Description("获取或设置前景面板线条距离间隔")]
        [Category("自定义控件属性")]
        public int ChartIntervalSize
        {
            get { return _chartIntervalSize; }
            set { _chartIntervalSize = value; }
        }

        private int _moveSize = 5;
        /// <summary>
        ///     获取或设置移动间隔
        /// </summary>
        [Description("获取或设置移动间隔")]
        [Category("自定义控件属性")]
        public int MoveSize
        {
            get { return _moveSize; }
            set { _moveSize = value; }
        }

        private double _strokeThickness = 0.5;
        /// <summary>
        ///     获取或设置背景线条粗细数值
        /// </summary>
        [Description("获取或设置背景线条粗细数值")]
        [Category("自定义控件属性")]
        public double StrokeThickness
        {
            get { return _strokeThickness; }
            set { _strokeThickness = value; }
        }

        private double _forwardStrokeThickness = 2;
        /// <summary>
        ///     获取或设置前景线条粗细数值
        /// </summary>
        [Description("获取或设置前景线条粗细数值")]
        [Category("自定义控件属性")]
        public double ForwardStrokeThickness
        {
            get { return _forwardStrokeThickness; }
            set { _forwardStrokeThickness = value; }
        }

        private Brush _backgroundColor = Brushes.LightGray;
        /// <summary>
        ///     获取或设置单元格线条颜色
        /// </summary>
        [Description("获取或设置单元格线条颜色")]
        [Category("自定义控件属性")]
        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                foreach (Line line in _horizontalLines)
                {
                    line.Stroke = value;
                }
                foreach (Line line in _verticalLines)
                {
                    line.Stroke = value;
                }
            }
        }

        private Brush _forwardBackgroundColor = Brushes.Red;
        /// <summary>
        ///     获取或设置单元格线条颜色
        /// </summary>
        [Description("获取或设置单元格线条颜色")]
        [Category("自定义控件属性")]
        public Brush ForwardBackgroundColor
        {
            get { return _forwardBackgroundColor; }
            set
            {
                _forwardBackgroundColor = value;
            }
        }

        private double _maxHorizontalValue = 450;
        /// <summary>
        ///     获取或设置横向最大数值
        /// </summary>
        [Description("获取或设置横向最大数值")]
        [Category("自定义控件属性")]
        public double MaxHorizontalValue
        {
            get { return _maxHorizontalValue; }
            set { _maxHorizontalValue = value; }
        }

        private double _maxVerticalValue = 200;
        /// <summary>
        ///     获取或设置纵向最大数值
        /// </summary>
        [Description("获取或设置纵向最大数值")]
        [Category("自定义控件属性")]
        public double MaxVerticalValue
        {
            get { return _maxVerticalValue; }
            set { _maxVerticalValue = value; }
        }

        #endregion

        #region 方法

        #region 绘制背景

        /// <summary>
        ///     初始化背景
        /// </summary>
        private void InitializeBackground()
        {
            DrowHorizontalBackground();
            DrowVerticalBackground();
        }

        /// <summary>
        ///     绘制背景水平线
        /// </summary>
        private void DrowHorizontalBackground()
        {
            if (_horizontalLines.Count > 0)
            {
                _horizontalLines.Clear();
            }
            int count = (int)_height / _intervalSize;
            int height = 0;
            for (int i = 0; i <= count; i++)
            {
                Line line;
                cvs_Background.Children.Add(line = new Line
                {
                    Stroke = Brushes.LightGray,
                    StrokeThickness = _strokeThickness,
                    X1 = 0,
                    X2 = _width,
                    Y1 = height,
                    Y2 = height
                });
                height += _intervalSize;
                _horizontalLines.Add(line);
            }
        }

        /// <summary>
        ///      绘制背景垂直线
        /// </summary>
        private void DrowVerticalBackground()
        {
            if (_verticalLines.Count > 0)
            {
                _verticalLines.Clear();
            }
            int count = (int)_width / _intervalSize;
            int width = 0;
            for (int i = 0; i <= count; i++)
            {
                Line line;
                cvs_Background.Children.Add(line = new Line
                {
                    Stroke = Brushes.LightGray,
                    StrokeThickness = _strokeThickness,
                    X1 = width,
                    X2 = width,
                    Y1 = 0,
                    Y2 = _height
                });
                width += _intervalSize;
                _verticalLines.Add(line);
            }
        }

        #endregion

        /// <summary>
        ///     移动背景线条方法
        /// </summary>
        private void MoveLine()
        {
            #region 移动背景部分

            Line tempLine;
            _locker.EnterReadLock();
            try
            {
                foreach (Line line in _verticalLines)
                {
                    line.X1 = line.X1 - _moveSize;
                    line.X2 = line.X2 - _moveSize;
                    if (line.X1 <= _width && line.Visibility == Visibility.Hidden)
                    {
                        line.Visibility = Visibility.Visible;
                    }
                }
                tempLine = _verticalLines[0];
            }
            finally
            {
                _locker.ExitReadLock();
            }
            if (tempLine.X1 < 0)
            {
                _locker.EnterWriteLock();
                try
                {
                    _verticalLines.Remove(tempLine);
                    tempLine.X1 = tempLine.X2 =_verticalLines[_verticalLines.Count - 1].X1 + _intervalSize;
                    tempLine.Y1 = 0;
                    tempLine.Y2 = _height;
                    if (tempLine.X1 > _width)
                    {
                        tempLine.Visibility = Visibility.Hidden;
                    }
                    _verticalLines.Add(tempLine);
                }
                finally
                {
                    _locker.ExitWriteLock();
                }
            }

            #endregion

            #region 移动前景线条部分

            _lockerPerformance.EnterReadLock();

            List<Line> deleteLines = null;
            try
            {
                foreach (Line line in _performanceLines)
                {
                    line.X1 = line.X1 - _moveSize;
                    line.X2 = line.X2 - _moveSize;
                    //如果该线条已经移动出了控件显示范围，则准备移除
                    if (line.X1 < 0)
                    {
                        if (deleteLines == null)
                        {
                            deleteLines = new List<Line>();
                        }
                        deleteLines.Add(line);
                        continue;
                    }
                    if (line.X2 <= _width && line.Visibility == Visibility.Hidden)
                    {
                        line.Visibility = Visibility.Visible;
                    }
                }
            }
            finally
            {
                _lockerPerformance.ExitReadLock();
            }
            //开始移除废弃的线条
            RemoveGarbageLines(deleteLines);

            #endregion
        }

        /// <summary>
        ///     添加一个点，从指定的纵向高度点上。
        /// </summary>
        /// <param name="verticalValue">高度值</param>
        public void AddPoint(double verticalValue)
        {
            if (verticalValue < 0 || verticalValue > _maxVerticalValue)
            {
                throw new System.Exception("无法添加一个节点，因为这个节点的纵向高度已经超过了预期的数值。");
            }
            Application.Current.Dispatcher.Invoke(new DelegatePoint(InvokeAddPoint),
                                                                 DispatcherPriority.Background, verticalValue);
        }

        /// <summary>
        ///     移除废弃的线条
        /// </summary>
        /// <param name="deleteLines">废弃的线条列表</param>
        private void RemoveGarbageLines(List<Line> deleteLines)
        {
            if (deleteLines != null && deleteLines.Count > 0)
            {
                foreach (Line deleteLine in deleteLines)
                {
                    DeleteLine(deleteLine);
                }
                deleteLines.Clear();
                deleteLines = null;
            }
        }

        /// <summary>
        ///     添加一个点，从指定的纵向高度点上。
        /// </summary>
        /// <param name="verticalValue">高度值</param>
        private void InvokeAddPoint(double verticalValue)
        {
            verticalValue = _height - ((_height / _maxVerticalValue) * verticalValue);
            _lockerPoint.EnterWriteLock();
            LinePoint next;
            try
            {
                //取出点列表中，最后一个点的位置，如果当前不存在至少一个点，则返回-1
                next = _points.Count > 0 ? _points[_points.Count - 1] : new LinePoint { X = -1, Y = -1 };
                //写入此点
                _points.Add(new LinePoint { X = next.X == -1 ? _width : next.X + _chartIntervalSize, Y = verticalValue });
                if (next.X != -1)
                {
                    //如果拥有前置点，则删除前置点
                    _points.RemoveAt(0);
                }
            }
            finally
            {
                _lockerPoint.ExitWriteLock();
            }
            Line newLine = MakeNewLine();
            newLine.X2 = next.X == -1 ? _width : next.X + _chartIntervalSize;
            newLine.Y2 = verticalValue;
            //将此新的线条加入线条集合中
            _lockerPerformance.EnterWriteLock();
            try
            {
                MakeBridge(newLine, next);
            }
            finally
            {
                _lockerPerformance.ExitWriteLock();
            }
            DrawLine(newLine);
            MoveLine();
        }

        /// <summary>
        ///     使当前新添加的线条与原有的线条连接起来
        /// </summary>
        /// <param name="newLine">新添加的线条</param>
        /// <param name="lastPoint">最后一个点的坐标</param>
        private void MakeBridge(Line newLine, LinePoint lastPoint)
        {
            //线条集合中原来存在了节点连线
            if (lastPoint.X >= 0)
            {
                Line line = _performanceLines[_performanceLines.Count - 1];
                //开始对接
                newLine.X1 = line.X2;
                newLine.Y1 = lastPoint.Y;
            }
            else
            {
                //开始对接
                newLine.X1 = _width;
                newLine.Y1 = newLine.Y2;
            }
            _performanceLines.Add(newLine);
        }

        /// <summary>
        ///     制造一个新的线条
        /// </summary>
        /// <returns>返回线条对象</returns>
        private Line MakeNewLine()
        {
            Line newLine = new Line();
            newLine.StrokeThickness = _forwardStrokeThickness;
            newLine.Visibility = Visibility.Hidden;
            newLine.Stroke = _forwardBackgroundColor;
            return newLine;
        }

        /// <summary>
        ///     绘制一条新的线，在前景面板上
        /// </summary>
        /// <param name="line">新的线条对象</param>
        private void DrawLine(Line line)
        {
            if (line.X2 > _width)
            {
                line.Visibility = Visibility.Hidden;
            }
            cvs_Forward.Children.Add(line);
        }

        /// <summary>
        ///     从前景面板中删除一个线条
        /// </summary>
        /// <param name="line">要删除的线条</param>
        private void DeleteLine(Line line)
        {
            _lockerPerformance.EnterWriteLock();
            try
            {
                cvs_Forward.Children.Remove(line);
                _performanceLines.Remove(line);
            }
            finally
            {
                _lockerPerformance.ExitWriteLock();
            }
            line = null;
        }

        #endregion
    }
}