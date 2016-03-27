using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Types;
using KJFramework.Tracing;

namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///     智能对象父类，提供了对于与二进制数据交互的基础能力。
    /// </summary>
    public class IntellectObject : IIntellectObject
    {
        #region 成员

        protected bool _isBind;
        protected bool _isPickup;
        protected byte[] _body;
        protected bool _compatibleMode;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(IntellectObject));

        #endregion

        #region Implementation of IIntellectObject

        /// <summary>
        ///     获取或设置二进制数据体
        /// </summary>
        public virtual byte[] Body
        {
            get { return _body; }
            set { _body = value; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前是否已经从第三方客户数据转换为元数据。
        /// </summary>
        public virtual bool IsBind
        {
            get { return _isBind; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前实体类是不是以兼容模式解析的。
        /// </summary>
        public virtual bool CompatibleMode
        {
            get { return _compatibleMode; }
            internal set { _compatibleMode = value; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前是否已经从元数据第转换为三方客户数据。
        /// </summary>
        public virtual bool IsPickup
        {
            get { return _isPickup; }
            internal set { _isPickup = value; }
        }

        /// <summary>
        ///     绑定到元数据
        /// </summary>
        /// <exception cref="MethodAccessException">错误的类型权限定义</exception>
        public virtual void Bind()
        {
            try
            {
                _body = IntellectObjectEngine.ToBytes(this);
                _isBind = true;
            }
            catch (MethodAccessException ex)
            {
                _isBind = false;
                _body = null;
                _tracing.Error(ex, null);
                //redirect to friendly exception message.
                throw new MethodAccessException(string.Format(ExceptionMessage.EX_METHOD_ACCESS, GetType().FullName));
            }
            catch (System.Exception ex)
            {
                _isBind = false;
                _body = null;
                _tracing.Error(ex, null);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return ToString(string.Empty, false, null);
        }

        /// <summary>
        ///     内部方法，用于将一个对象转换为字符串的形式表现出来
        /// </summary>
        /// <param name="space">缩进空间</param>
        /// <param name="isArrayLoop">是否陷入数组循环的标示</param>
        /// <param name="property">字段信息</param>
        /// <returns>返回标示当前对象的字符串</returns>
        internal string ToString(string space, bool isArrayLoop, PropertyInfo property)
        {
            Type currentType = GetType();
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<int, GetObjectAnalyseResult> results = Analyser.GetObjectAnalyser.Analyse(currentType);
            if (results == null)
                stringBuilder.AppendLine(string.Format("{0}{1}: #Cannot find any intellect property for this type.", space, currentType.Name));
            else
            {
                string nextSpace = space + "  ";
                if(!_compatibleMode) stringBuilder.AppendLine(string.Format("{0}{1}:", space, currentType.Name)).Append(space).AppendLine("{");
                else stringBuilder.AppendLine(string.Format("{0}{1}(Compatible Mode):", space, currentType.Name)).Append(space).AppendLine("{");
                //log value for each property.
                foreach (GetObjectAnalyseResult analyseResult in results.Values)
                {
                    if (analyseResult.StringConverter != null)
                        analyseResult.StringConverter(analyseResult.Property.PropertyType, stringBuilder, analyseResult.Property, analyseResult.GetValue(this), nextSpace, false);
                    else
                    {
                        Type propertyType = analyseResult.Property.PropertyType;
                        if (propertyType.IsSubclassOf(typeof(IntellectObject)))
                        {
                            #region Process intellect object.

                            IntellectObject intellectObject = (IntellectObject)analyseResult.GetValue(this);
                            analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                            {
                                IntellectObject intellect = (IntellectObject)i;
                                if (intellect == null)
                                {
                                    if (isAl) s.Append(spc).AppendLine("NULL");
                                    else s.AppendLine(string.Format("{0}{1}: NULL", spc, p.Name));
                                }
                                else s.AppendLine(intellect.ToString(spc, false, p));
                            };
                            analyseResult.StringConverter(propertyType, stringBuilder, analyseResult.Property, intellectObject, nextSpace, false);

                            #endregion
                        }
                        else if (propertyType == typeof(Blob))
                        {
                            #region Process Blob object.

                            Blob intellectObject = (Blob)analyseResult.GetValue(this);
                            analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                            {
                                Blob blob = (Blob)i;
                                if (blob == null)
                                {
                                    if (isAl) s.Append(spc).AppendLine("NULL");
                                    else s.AppendLine(string.Format("{0}{1}: NULL", spc, p.Name));
                                }
                                else
                                {
                                    s.AppendLine(string.Format("{0}{1}:", spc, p.Name));
                                    s.AppendLine(string.Format("{0}{1}", spc, '{'));
                                    s.AppendLine(blob.ToString(spc + "  "));
                                    s.AppendLine(string.Format("{0}{1}", spc, '}'));
                                }
                            };
                            analyseResult.StringConverter(propertyType, stringBuilder, analyseResult.Property, intellectObject, nextSpace, false);

                            #endregion
                        }
                        else if (propertyType.IsArray)
                        {
                            #region Process array.

                            Type elementType = propertyType.GetElementType();
                            //array of intellect object(s).
                            if (elementType.IsSubclassOf(typeof(IntellectObject)))
                            {
                                #region Set handler of intellect object array

                                analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                                {
                                    if (i == null)
                                    {
                                        s.AppendLine(string.Format("{0}{1}: NULL", spc, p.Name));
                                        return;
                                    }
                                    Array array = (Array)i;
                                    string nxtSpace = spc + "  ";
                                    s.AppendLine(string.Format("{0}{1}: ", spc, p.Name)).Append(spc).AppendLine("{");
                                    for (int j = 0; j < array.Length; j++)
                                    {
                                        IntellectObject eleIntellectObject = (IntellectObject)array.GetValue(j);
                                        if (eleIntellectObject == null)
                                        {
                                            s.Append(nxtSpace).AppendLine("NULL");
                                            continue;
                                        }
                                        s.AppendLine(eleIntellectObject.ToString(nxtSpace, true, p));
                                    }
                                    s.Append(spc).AppendLine("}");
                                };

                                #endregion
                            }
                            else if (elementType == typeof(Guid))
                            {
                                #region Set handler of Guid array

                                analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                                {
                                    if (i == null)
                                    {
                                        s.AppendLine(string.Format("{0}{1}: NULL", spc, p.Name));
                                        return;
                                    }
                                    Array array = (Array)i;
                                    string nxtSpace = spc + "  ";
                                    s.AppendLine(string.Format("{0}{1}: ", spc, p.Name)).Append(spc).AppendLine("{");
                                    StringBuilder innerBuilder = new StringBuilder();
                                    innerBuilder.Append("[Management Guid] ");
                                    for (int j = 0; j < array.Length; j++)
                                    {
                                        Guid guid = (Guid)array.GetValue(j);
                                        foreach (byte element in guid.ToByteArray())
                                            innerBuilder.Append(element.ToString("x02")).Append(", ");
                                        s.AppendLine(string.Format("{0}{1}", nxtSpace, innerBuilder));
                                    }
                                    s.Append(spc).AppendLine("}");
                                };

                                #endregion
                            }
                            else if (elementType == typeof(byte))
                            {
                                #region Set handler of Byte array

                                analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                                {
                                    if (i == null)
                                    {
                                        s.AppendLine(string.Format("{0}{1}: NULL", spc, p.Name));
                                        return;
                                    }
                                    byte[] array = (byte[])i;
                                    string nxtSpace = spc + "  ";
                                    s.AppendLine(string.Format("{0}{1}: ", spc, p.Name)).Append(spc).AppendLine("{");
                                    int round = array.Length/8 + (array.Length%8>0 ? 1 : 0);
                                    int currentOffset, remainningLen;
                                    for (int j = 0; j < round; j++)
                                    {
                                        currentOffset = j*8;
                                        remainningLen = ((array.Length - currentOffset) >= 8 ? 8 : (array.Length - currentOffset));
                                        StringBuilder rawByteBuilder = new StringBuilder();
                                        rawByteBuilder.Append(nextSpace);
                                        for (int k = 0; k < remainningLen; k++)
                                        {
                                            rawByteBuilder.AppendFormat("0x{0}", array[currentOffset + k].ToString("X2"));
                                            if (k != remainningLen - 1) rawByteBuilder.Append(", ");
                                        }
                                        rawByteBuilder.Append(new string(' ', (remainningLen == 8 ? 5 : (8 - remainningLen) * 4 + (((8 - remainningLen) - 1)*2) + 7)));
                                        for (int k = 0; k < remainningLen; k++)
                                        {
                                            if ((char)array[currentOffset + k] > 126 || (char)array[currentOffset + k] < 32) rawByteBuilder.Append('.');
                                            else rawByteBuilder.Append((char)array[currentOffset + k]);
                                        }
                                        s.AppendLine(string.Format("{0}{1}", nxtSpace, rawByteBuilder));
                                    }
                                    //s.AppendLine(string.Format("{0}", nxtSpace));
                                    s.Append(spc).AppendLine("}");
                                };

                                #endregion
                            }
                            else if (elementType == typeof(IntPtr))
                            {
                                #region Set handler of IntPtr array

                                analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                                {
                                    if (i == null)
                                    {
                                        s.AppendLine(string.Format("{0}{1}: NULL", spc, p.Name));
                                        return;
                                    }
                                    Array array = (Array)i;
                                    string nxtSpace = spc + "  ";
                                    s.AppendLine(string.Format("{0}{1}: ", spc, p.Name)).Append(spc).AppendLine("{");
                                    for (int j = 0; j < array.Length; j++)
                                    {
                                        IntPtr intPtr = (IntPtr)array.GetValue(j);
                                        s.AppendLine(string.Format("{0}[Management IntPtr] {1}", nxtSpace, intPtr.ToInt32().ToString()));
                                    }
                                    s.Append(spc).AppendLine("}");
                                };

                                #endregion
                            }
                            else
                            {
                                #region Set handler of other normally array

                                analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                                {
                                    if (i == null)
                                    {
                                        s.AppendLine(string.Format("{0}{1}: NULL", spc, p.Name));
                                        return;
                                    }
                                    Array array = (Array)i;
                                    string nxtSpace = spc + "  ";
                                    s.AppendLine(string.Format("{0}{1}: ", spc, p.Name)).Append(spc).AppendLine("{");
                                    for (int j = 0; j < array.Length; j++)
                                    {
                                        Object obj = array.GetValue(j);
                                        if (obj == null) s.Append(nxtSpace).AppendLine("NULL");
                                        else s.AppendLine(string.Format("{0}{1}", nxtSpace, obj));
                                    }
                                    s.Append(spc).AppendLine("}");
                                };

                                #endregion
                            }
                            analyseResult.StringConverter(elementType, stringBuilder, analyseResult.Property, analyseResult.GetValue(this), nextSpace, true);

                            #endregion
                        }
                        else
                        {
                            if (propertyType == typeof(Guid))
                            {
                                #region Set handler of Guid.

                                analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                                {
                                    StringBuilder innerBuilder = new StringBuilder();
                                    innerBuilder.Append("[Management Guid] ");
                                    Guid guid = (Guid)i;
                                    foreach (byte element in guid.ToByteArray())
                                        innerBuilder.Append(element.ToString("x02")).Append(", ");
                                    s.AppendLine(string.Format("{0}{1}: {2}", spc, p.Name, innerBuilder));
                                };

                                #endregion
                            }
                            else if (propertyType == typeof(IntPtr))
                            {
                                #region Set handler of IntPtr.

                                analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                                {
                                    IntPtr intPtr = (IntPtr)i;
                                    s.AppendLine(string.Format("{0}{1}: [Management IntPtr] {2}", spc, p.Name, intPtr.ToInt32().ToString()));
                                };

                                #endregion
                            }
                            else
                            {
                                #region Set handler of other normally array

                                analyseResult.StringConverter = delegate(Type t, StringBuilder s, PropertyInfo p, Object i, string spc, bool isAl)
                                {
                                    if (i == null)
                                    {
                                        s.AppendLine(string.Format("{0}{1}: NULL", spc, p.Name));
                                        return;
                                    }
                                    s.AppendLine(string.Format("{0}{1}: {2}", spc, p.Name, i));
                                };

                                #endregion
                            }
                            analyseResult.StringConverter(propertyType, stringBuilder, analyseResult.Property, analyseResult.GetValue(this), nextSpace, false);
                        }
                    }
                }
                stringBuilder.Append(space).AppendLine("}");
            }
            return stringBuilder.ToString();
        }

        #endregion
    }
}