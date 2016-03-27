using System;
using System.Data.Common;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Helpers;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Datas
{
    public class CmdParameter
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        private object _value;
        public object Value
        {
            get { return _value; }
        }

        public CmdParameter(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("argument invalid");
            _name = name;

            _value = value;
            while (true)
            {
                if (_value is DbParameter)
                    _value = (_value as DbParameter).Value;
                else if (_value is CmdParameter)
                    _value = (_value as CmdParameter).Value;
                else
                    break;
            }

            if (_value is string)
                _value = _value ?? string.Empty;
            else if (_value is DateTime)
                _value = ((DateTime)_value).DbNarrow();
        }

        public static implicit operator CmdParameter(object[] pair)
        {
            if (pair == null || pair.Length != 2 || pair[0] == null)
                throw new ArgumentException("argument invalid");
            if (pair[0].GetType() != typeof(string))
                throw new ArgumentException("argument invalid");
            string name = (string)pair[0];
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("argument invalid");
            return new CmdParameter(name, pair[1]);
        }
    }
}
