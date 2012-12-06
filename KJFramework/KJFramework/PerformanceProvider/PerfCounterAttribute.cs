using System;
using System.Diagnostics;

namespace KJFramework.PerformanceProvider
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)][DebuggerStepThrough]
    public class PerfCounterAttribute : System.Attribute
    {
        private string _instance;

        private string _name;
        private PerformanceCounterType _type;
        private string _help;

        public PerfCounterAttribute(string name, PerformanceCounterType type)
        {
            _name = name;
            _type = type;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public PerformanceCounterType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Help
        {
            get { return _help; }
            set { _help = value; } 
        }

        public string Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        public bool HasBase()
        {
            return BaseType != PerformanceCounterType.ElapsedTime;
        }

        public string BaseName
        {
            get { return _name + " #base"; }
        }

        public PerformanceCounterType BaseType
        {
            get
            {
                switch (_type)
                {
                    case PerformanceCounterType.AverageCount64:
                    case PerformanceCounterType.AverageTimer32:
                        return PerformanceCounterType.AverageBase;
                    case PerformanceCounterType.CounterMultiTimer:
                    case PerformanceCounterType.CounterMultiTimer100Ns:
                    case PerformanceCounterType.CounterMultiTimer100NsInverse:
                    case PerformanceCounterType.CounterMultiTimerInverse:
                        return PerformanceCounterType.CounterMultiBase;
                    case PerformanceCounterType.RawFraction:
                        return PerformanceCounterType.RawBase;
                    case PerformanceCounterType.SampleCounter:
                    case PerformanceCounterType.SampleFraction:
                        return PerformanceCounterType.SampleBase;
                    default:
                        break;
                }
                return PerformanceCounterType.ElapsedTime;
            }
        }

        public string BaseHelp
        {
            get { return string.Empty; }
        }
    }
}