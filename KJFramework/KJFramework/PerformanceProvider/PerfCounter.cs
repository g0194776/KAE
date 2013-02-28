using System.Diagnostics;

namespace KJFramework.PerformanceProvider
{
    public class PerfCounter
    {
        private PerformanceCounter _counter;
        private PerformanceCounter _base;

        public PerfCounter(string category, string instance, PerfCounterAttribute attr)
        {
            if (!string.IsNullOrEmpty(category) && attr != null)
            {
                _counter = new PerformanceCounter(category, attr.Name, instance, false);
                if (attr.HasBase())
                    _base = new PerformanceCounter(category, attr.BaseName, instance, false);
            }
        }

        public float Read()
        {
            float result = 0.0f;
            if (_counter != null)
                result = _counter.NextValue();
            return result;
        }

        public long Increment()
        {
            long result = 0;
			if (_counter != null)
				result = _counter.Increment();
            if (_base != null) 
                _base.Increment();
            return result;
        }

        public long IncrementBy(long value)
        {
			long result = 0;
			if (_counter != null)
				result = _counter.IncrementBy(value);
            if (_base != null) 
                _base.Increment();
            return result;
        }

        public long IncrementBy(long value, long @base)
        {
			long result = 0;
			if (_counter != null)
				result = _counter.IncrementBy(value);
			if (_base != null)
				_base.IncrementBy(@base);
            return result;
        }

        public long Decrement()
        {
			long result = 0;
			if (_counter != null)
				result = _counter.Decrement();
            if (_base != null)
                _base.Increment();
            return result;
        }

        public void Reset()
        {
			if (_counter != null)
				_counter.RawValue = 0L;
            if (_base != null)
                _base.RawValue = 0L;
        }

        public void Close()
        {
            if (_counter != null) 
                _counter.Close();
            if (_base != null) 
                _base.Close();
        }

        public string Instance
        {
            get 
            {
				if (_counter == null)
					return string.Empty;
                return _counter.InstanceName;
			}
            set
            {
				if (_counter != null)
					_counter.InstanceName = value;
                if (_base != null)
                    _base.InstanceName = value; 
            }
        }
    }
}