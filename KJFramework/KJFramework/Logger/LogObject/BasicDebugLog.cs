using System;
using KJFramework.Basic.Enum;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     基础的应用程序重复记录对象
    /// </summary>
    [Serializable]
    public class BasicDebugLog : IDebugLog
    {
        #region IDebugLog 成员

        private String _strackmessage;
        public string StrackMessage
        {
            get
            {
                return _strackmessage;
            }
            set
            {
                _strackmessage = value;
            }
        }

        private String _debugmessage;
        public string DebugMessage
        {
            get
            {
                return _debugmessage;
            }
            set
            {
                _debugmessage = value;
            }
        }

        private String _location;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        private DebugGrade _grade;
        public DebugGrade Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                _grade = value;
            }
        }

        #endregion

        #region ILog 成员

        private DateTime _time;
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }

        private String _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private String _user;
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        private LogTypes _logtype = LogTypes.ApplicationDebug;
        public LogTypes LogType
        {
            get { return _logtype; }
        }

        private String _tag;
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        #endregion
    }
}
