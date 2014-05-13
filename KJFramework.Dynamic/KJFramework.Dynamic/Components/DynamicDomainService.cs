using System.Configuration;
using KJFramework.Dynamic.Configurations;
using KJFramework.Dynamic.Extends;
using KJFramework.Dynamic.Finders;
using KJFramework.Dynamic.Loaders;
using KJFramework.Dynamic.Pools;
using KJFramework.Dynamic.Structs;
using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Tracing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     动态程序域服务，提供了相关的基本操作。
    /// </summary>
    public class DynamicDomainService : MarshalByRefObject, IDynamicDomainService
    {
        #region Constructor

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        ///     <para>* 使用此构造将会从配置文件中读取服务相关信息</para>
        /// </summary>
        public DynamicDomainService()
            : this(Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf('\\') + 1), 
            new ServiceDescriptionInfo {
                Name = ConfigurationManager.AppSettings["Name"],
                ServiceName = ConfigurationManager.AppSettings["ServiceName"],
                Description = ConfigurationManager.AppSettings["Description"],
                Version = ConfigurationManager.AppSettings["Version"] })
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        /// </summary>
        /// <param name="description">服务描述信息</param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        public DynamicDomainService(ServiceDescriptionInfo description)
            : this(Process.GetCurrentProcess().MainModule.FileName.Substring(0, Process.GetCurrentProcess().MainModule.FileName.LastIndexOf('\\') + 1), description)
        {
        }

        /// <summary>
        ///     动态程序域服务，提供了相关的基本操作。
        /// </summary>
        /// <param name="workRoot">工作目录</param>
        /// <param name="description">
        ///     服务描述信息
        ///     <para>* 如果传递null, 则默认从XML配置文件中读取</para>
        /// </param>
        /// <exception cref="System.ArgumentNullException">参数错误</exception>
        /// <exception cref="DirectoryNotFoundException">工作目录错误</exception>
        public DynamicDomainService(String workRoot, ServiceDescriptionInfo description)
        {
            if (workRoot == null) throw new ArgumentNullException("workRoot");
            if (!Directory.Exists(workRoot)) throw new DirectoryNotFoundException("Current work root don't existed. #dir: " + workRoot);
            _workRoot = workRoot;
            if(description == null)
            {
                //try to parse the xml configuration file.
                _infomation = new ServiceDescriptionInfo
                {
                    Name = ConfigurationManager.AppSettings["Name"],
                    Description = ConfigurationManager.AppSettings["Description"],
                    ServiceName = ConfigurationManager.AppSettings["ServiceName"],
                    Version = ConfigurationManager.AppSettings["Version"]
                };
            }
            else _infomation = description;
            _id = Guid.NewGuid();
            _assemblyLoader = new AssemblyLoader(AppDomain.CurrentDomain);
            _assemblyLoader.Load(workRoot);
        }

        #endregion

        #region Members

        private readonly Guid _id;
        private readonly string _workRoot;
        private readonly AssemblyLoader _assemblyLoader;
        private readonly ServiceDescriptionInfo _infomation;
        private object _tag;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DynamicDomainService));
        //dynamic component key(EntryPoint)-values.
        private ConcurrentDictionary<string, DynamicDomainObject> _dynamicObjects = new ConcurrentDictionary<string, DynamicDomainObject>();

        #endregion

        #region Implementation of IDynamicDomainService

        /// <summary>
        ///     获取内部组件数量
        /// </summary>
        public int ComponentCount
        {
            get { return _dynamicObjects.Count; }
        }

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public Object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     获取唯一标示
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取或设置工作目录
        /// </summary>
        public string WorkRoot
        {
            get { return _workRoot; }
        }

        /// <summary>
        ///     获取服务描述信息
        /// </summary>
        public ServiceDescriptionInfo Infomation
        {
            get { return _infomation; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        protected IList<DynamicDomainObject> Initialize()
        {
            if (String.IsNullOrEmpty(_infomation.ServiceName)) throw new System.Exception("Service name cannot be *NULL*.");
            try
            {
                //开始查找插件
                using (IDynamicDomainComponentFinder finder = new BasicDynamicDomainComponentFinder())
                {
                    _tracing.Info("#Begin initializing dynamic domain component finder at path: " + _workRoot + "......");
                    List<DomainComponentEntryInfo> infos = finder.Execute(_workRoot);
                    _tracing.Info("#Find components done, result count: " + infos.Count + ".");
                    if (infos.Count == 0) return null;
                    IList<DynamicDomainObject> dynamicObjects = new List<DynamicDomainObject>(infos.Count);
                    foreach (DomainComponentEntryInfo domainComponentEntryInfo in infos)
                    {
                        _tracing.Info("#Prepare to wrap components, name: " + domainComponentEntryInfo.EntryPoint + "......");
                        DynamicDomainObject domainObject = domainComponentEntryInfo.Wrap();
                        _tracing.Info(domainObject != null
                                            ? "#Wrap components, name: " + domainComponentEntryInfo.EntryPoint + " successed!"
                                            :  "#Wrap components, name: " + domainComponentEntryInfo.EntryPoint + " failed!");
                        if(domainObject == null)
                        {
                            _tracing.Info(string.Format("#Component {0} lose the wrap chance.", domainComponentEntryInfo.EntryPoint));
                            #if(DEBUG)
                            Console.WriteLine(string.Format("#Component {0} lose the wrap chance.", domainComponentEntryInfo.EntryPoint));
                            #endif
                            continue;
                        }
                        domainObject.OwnService = this;
                        dynamicObjects.Add(domainObject);
                    }
                    return dynamicObjects;
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        /// <summary>
        ///     开始执行
        /// </summary>
        public virtual void Start()
        {
            //initialize components.
            IList<DynamicDomainObject> dynamicObjs = Initialize();
            if (dynamicObjs == null || dynamicObjs.Count <= 0)
                throw new System.Exception("You *MUST* have one component at least for current dynamic service!!!");
            Dictionary<string, string> tunnelAddresses = new Dictionary<string, string>();
            //一个一个的启动
            foreach (DynamicDomainObject dynamicObject in dynamicObjs)
            {
                try
                {
                    dynamicObject.Start();
                    if (!dynamicObject.Component.Enable)
                        throw new System.Exception("#Dynamic domain component start failed. #entry: " + dynamicObject.EntryInfo.EntryPoint);
                    if (!_dynamicObjects.TryAdd(dynamicObject.Component.Name, dynamicObject))
                        throw new System.Exception(string.Format("#Cannot add a dynamic object {0} to collection", dynamicObject.EntryInfo.EntryPoint));
                    if (dynamicObject.Component.IsUseTunnel)
                        tunnelAddresses.Add(dynamicObject.Component.Name, dynamicObject.Component.GetTunnelAddress());
                    dynamicObject.Exited += DomainObjectExited;
                    WorkingProcessHandler(new LightSingleArgEventArgs<string>("#Dynamic domain object : " + dynamicObject.EntryInfo.EntryPoint + " has been worked."));
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                    WorkingProcessHandler(new LightSingleArgEventArgs<string>("#Dynamic domain object : " + dynamicObject.EntryInfo.EntryPoint + " cannot be work."));
                }
            }
            //通知隧道地址
            foreach (DynamicDomainObject dynamicDomainObject in _dynamicObjects.Values)
                dynamicDomainObject.Component.SetTunnelAddresses(tunnelAddresses);
            //注册服务
            DynamicDomainServiceRegistation.Instance.Regist(this);
            StartWorkHandler(null);
        }

        /// <summary>
        ///     停止执行
        /// </summary>
        public virtual void Stop()
        {
            if (_dynamicObjects.Count > 0)
            {
                foreach (DynamicDomainObject dynamicObject in _dynamicObjects.Values)
                {
                    try 
                    {
                        dynamicObject.Exited -= DomainObjectExited;
                        dynamicObject.Stop(); 
                    }
                    catch (System.Exception ex) { _tracing.Error(ex, null); }
                }
            }
            //注册服务
            DynamicDomainServiceRegistation.Instance.UnRegist(_infomation.Name);
            EndWorkHandler(null);
        }

        /// <summary>
        ///     更新服务
        /// </summary>
        /// <returns>返回更新的状态</returns>
        public virtual bool Update()
        {
            UpdatingHandler(new LightSingleArgEventArgs<string>("*BEGIN* updating service......"));
            if (_dynamicObjects.Count == 0)
            {
                UpdatingHandler(new LightSingleArgEventArgs<string>("*UPDATE* service succeed!"));
                return true;
            }
            IList<string> keys = new List<string>();
            foreach (KeyValuePair<string, DynamicDomainObject> pair in _dynamicObjects)
            {
                try
                {
                    //当执行更新动作时，默认内部会先调用Stop方法 
                    pair.Value.Update();
                }
                catch (System.Exception ex)
                {
                    _tracing.Error(ex, null);
                    keys.Add(pair.Key);
                }
            }
            //检测更新失败元素
            if (keys.Count == 0)
            {
                UpdatingHandler(new LightSingleArgEventArgs<string>("*UPDATE* service succeed!"));
                return true;
            }
            foreach (string key in keys)
            {
                DynamicDomainObject removeObj;
                if (!_dynamicObjects.TryRemove(key, out removeObj)) continue;
                removeObj.Exited -= DomainObjectExited;
                UpdatingHandler(new LightSingleArgEventArgs<string>(string.Format("   ->*UPDATE* component {0} failed! ", removeObj.EntryInfo.EntryPoint)));
            }
            UpdatingHandler(new LightSingleArgEventArgs<string>("*END* update service, some domain object(s) update failed !"));
            return false;
        }

        /// <summary>
        ///     更新具有指定全名的组件
        /// </summary>
        /// <param name="fullname">组件全名</param>
        /// <returns>返回更新的状态</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public bool Update(string fullname)
        {
            if (string.IsNullOrEmpty(fullname)) throw new ArgumentNullException("fullname");
            DynamicDomainObject domainObject;
            if (!_dynamicObjects.TryGetValue(fullname, out domainObject))
            {
                _tracing.Info(string.Format("#UPDATE component {0} failed, because the target component don't existed.", fullname));
                #if(DEBUG)
                Console.WriteLine(string.Format("#UPDATE component {0} failed, because the target component don't existed.", fullname));
                #endif
                return false;
            }
            try
            {
                _tracing.Info(string.Format("*BEGIN* update component {0}...", fullname));
                domainObject.Update();
                _tracing.Info(string.Format("   ->*UPDATE* component {0} succeed!", fullname));
                return true;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                _tracing.Info(string.Format("   ->*UPDATE* component {0} failed, some useful message below:\r\n", ex.Message));
                DynamicDomainObject removedObj;
                _dynamicObjects.TryRemove(fullname, out removedObj);
                removedObj.Exited -= DomainObjectExited;
                #if(DEBUG)
                Console.WriteLine(string.Format("   ->*UPDATE* component {0} failed, some useful message below:\r\n", ex.Message));
                #endif
                return false;
            }
            finally { _tracing.Info(string.Format("*END* update component {0}...", fullname)); }
        }

        /// <summary>
        ///     检查健康状况
        /// </summary>
        /// <returns>返回健康状况</returns>
        public virtual HealthStatus CheckHealth()
        {
            HealthStatus worridStatus = HealthStatus.Exellect;
            List<HealthStatus> statuses = new List<HealthStatus>();
            if (_dynamicObjects.Count > 0)
            {
                Parallel.ForEach(_dynamicObjects, dynamicObject =>
                {
                    try
                    {
                        HealthStatus temp = dynamicObject.Value.CheckHealth();
                        statuses.Add(temp);
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, null);
                        statuses.Add(HealthStatus.Death);
                    }
                });
            }
            foreach (HealthStatus healthStatuse in statuses)
            {
                //计算枚举值大小(越大等级越低);
                if (healthStatuse > worridStatus)
                {
                    worridStatus = healthStatuse;
                }
            }
            return worridStatus;
        }

        /// <summary>
        ///     开始工作
        /// </summary>
        public event EventHandler StartWork;
        protected void StartWorkHandler(System.EventArgs e)
        {
            EventHandler handler = StartWork;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     停止工作
        /// </summary>
        public event EventHandler EndWork;
        protected void EndWorkHandler(System.EventArgs e)
        {
            EventHandler handler = EndWork;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     工作状态汇报事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<string>> WorkingProcess;
        protected void WorkingProcessHandler(LightSingleArgEventArgs<string> e)
        {
            EventHandler<LightSingleArgEventArgs<string>> handler = WorkingProcess;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     更新状态汇报事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<string>> Updating;
        protected void UpdatingHandler(LightSingleArgEventArgs<string> e)
        {
            EventHandler<LightSingleArgEventArgs<string>> handler = Updating;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        //event.
        void DomainObjectExited(object sender, System.EventArgs e)
        {
            DynamicDomainObject domainObject = (DynamicDomainObject)sender;
            domainObject.Exited -= DomainObjectExited;
            DynamicDomainObject removeObj;
            if (_dynamicObjects.TryRemove(domainObject.EntryInfo.EntryPoint, out removeObj))
                WorkingProcessHandler(new LightSingleArgEventArgs<string>(string.Format("Have one dynamic domain object has been exited. ({0})", removeObj.EntryInfo.FilePath)));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     根据组件名称获取一个程序域组件
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>返回获取到的程序域组件</returns>
        public IDynamicDomainComponent GetObject(String name)
        {
            if (name == null) throw new ArgumentNullException("name");
            var result = _dynamicObjects.Where(dyObject => dyObject.Value.Component.Name == name);
            if (result.Count() == 0) return null;
            //查找到了指定动态对象
            DynamicDomainObject domainObject = result.First().Value;
            //这里暂时不检测程序域对象的健康状况
            return domainObject.IsUpdating ? domainObject.OrgComponent : domainObject.Component;
        }

        #endregion
    }
}