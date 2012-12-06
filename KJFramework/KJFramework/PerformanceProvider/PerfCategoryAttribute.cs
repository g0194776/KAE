using System;
using System.Diagnostics;

namespace KJFramework.PerformanceProvider
{
    [AttributeUsage(AttributeTargets.Class , AllowMultiple = false)][DebuggerStepThrough]
    public class PerfCategoryAttribute : System.Attribute
    {
        private string _name;
        private PerformanceCounterCategoryType _type;
        private string _help;

        public PerfCategoryAttribute(string name) : this(name, PerformanceCounterCategoryType.MultiInstance) { }

        public PerfCategoryAttribute(string name, PerformanceCounterCategoryType type)
        {
            _name = name;
            _type = type;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public PerformanceCounterCategoryType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Help
        {
            get { return _help; }
            set { _help = value; }
        }
    }
}