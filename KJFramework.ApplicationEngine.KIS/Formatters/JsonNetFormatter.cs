using KJFramework.Tracing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace KJFramework.ApplicationEngine.KIS.Formatters
{
    /// <summary>
    ///     Invoke JSON.NET to serialize model
    /// </summary>
    public class JsonNetFormatter : MediaTypeFormatter
    {
        #region Constructor

        /// <summary>
        ///     Invoke JSON.NET to serialize model
        /// </summary>
        public JsonNetFormatter()
        {
            this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
        }

        #endregion

        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (JsonNetFormatter));

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanWriteType(Type type)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanReadType(Type type)
        {
            return true;
        }

        /// <summary>
        /// Read from stream
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <param name="contentHeaders"></param>
        /// <param name="formatterContext"></param>
        /// <returns></returns>
        public override Task<object> ReadFromStreamAsync(Type type, Stream stream, System.Net.Http.HttpContent contentHeaders, IFormatterLogger formatterContext)
        {

            var task = Task<object>.Factory.StartNew(() =>
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    };

                    var sr = new StreamReader(stream);
                    var jreader = new JsonTextReader(sr);

                    var ser = new JsonSerializer();
                    ser.Converters.Add(new IsoDateTimeConverter());
                    object val = ser.Deserialize(jreader, type);
                    return val;
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                    return null;
                }
            });

            return task;
        }

        /// <summary>
        /// Write to stream
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <param name="content"></param>
        /// <param name="transportContext"></param>
        /// <returns></returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream stream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                    };
                    string json = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonConverter[1] { new IsoDateTimeConverter() });
                    //string json = JsonConvert.SerializeObject(value);
                    byte[] buf = System.Text.Encoding.UTF8.GetBytes(json);
                    stream.Write(buf, 0, buf.Length);
                    stream.Flush();
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                }
            });
            return task;
        }

        #endregion
    }
}