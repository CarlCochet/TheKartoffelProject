using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.Core.Xml.Config;
using Stump.ORM;
using Stump.Server.BaseServer.Benchmark;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Exceptions;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;
using SharpRaven;
using SharpRaven.Data;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer
{
    // this methods should be accessible by the BaseServer assembly
    public abstract class ServerBase
    {
        internal static ServerBase InstanceAsBase;

        [Variable]
        public static int IOTaskInterval = 50;

        [Variable]
        public static bool ScheduledAutomaticShutdown = true;

        /// <summary>
        /// In minutes
        /// </summary>
        [Variable]
        public static int AutomaticShutdownTimer = 6 * 60;

        [Variable]
        public static string CommandsInfoFilePath = "./commands.xml";

        [Variable(Priority = 10, DefinableRunning = true)]
        public static bool IsExceptionLoggerEnabled = false;

        [Variable(Priority = 10)]
        public static string ExceptionLoggerDSN = "";

        public RavenClient ExceptionLogger
        {
            get;
            protected set;
        }
        public string ScheduledShutdownReason { get; protected set; }

        public int? DisconnectTime = Settings.InactivityDisconnectionTime;

        protected Dictionary<string, Assembly> LoadedAssemblies;
        protected Logger logger;
        private bool m_ignoreReload;

        public event Action ServerStarted;
        public Socket socketListener;



        protected ServerBase(string configFile, string schemaFile)
        {
            ConfigFilePath = configFile;
            SchemaFilePath = schemaFile;
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }

        public string ConfigFilePath
        {
            get;
            protected set;
        }

        public string SchemaFilePath
        {
            get;
            protected set;
        }

        public XmlConfig Config
        {
            get;
            protected set;
        }

        public ConsoleBase ConsoleInterface
        {
            get;
            protected set;
        }

        public DatabaseAccessor DBAccessor
        {
            get;
            protected set;
        }

        /// <summary>
        ///   Manage commands
        /// </summary>
        public CommandManager CommandManager
        {
            get;
            protected set;
        }

        public ClientManager ClientManager
        {
            get;
            protected set;
        }

        public SelfRunningTaskPool IOTaskPool
        {
            get;
            protected set;
        }

        public InitializationManager InitializationManager
        {
            get;
            protected set;
        }

        public PluginManager PluginManager
        {
            get;
            protected set;
        }

        public bool Running
        {
            get;
            protected set;
        }

        public DateTime StartTime
        {
            get;
            protected set;
        }

        public TimeSpan UpTime
        {
            get
            {
                return DateTime.Now - StartTime;
            }
        }

        public bool Initializing
        {
            get;
            protected set;
        }

        public bool IsInitialized
        {
            get;
            protected set;
        }

        public bool IsShutdownScheduled
        {
            get;
            protected set;
        }

        public bool IsNoExitShutdown
        {
            get;
            set;
        }

        public DateTime ScheduledShutdownDate
        {
            get;
            protected set;
        }

        public TimedTimerEntry ScheduledShutdownTimer
        {
            get;
            protected set;
        }

        public string Version
        {
            get;
            protected set;
        }

        public abstract void BeiginAcceptCallBack(IAsyncResult result);


        public virtual void Initialize()
        {
            ServerBase.InstanceAsBase = this;
            this.Initializing = true;
            NLogHelper.DefineLogProfile(true, true);
            NLogHelper.EnableLogging();
            this.logger = LogManager.GetCurrentClassLogger();
            if (!Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.OnUnhandledException);
                TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(this.OnUnobservedTaskException);
                Contract.ContractFailed += new EventHandler<ContractFailedEventArgs>(this.OnContractFailed);
            }
            else
                this.logger.Warn("Exceptions not handled cause Debugger is attatched");
            ServerBase.PreLoadReferences(Assembly.GetCallingAssembly());
            this.LoadedAssemblies = ((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies()).ToDictionary<Assembly, string>((Func<Assembly, string>)(entry => entry.GetName().Name));
            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(this.OnAssemblyLoad);
            if (((IEnumerable<string>)Environment.GetCommandLineArgs()).Contains<string>("-config"))
                this.UpdateConfigFiles();
            ConsoleBase.DrawAsciiLogo();
            Console.WriteLine();
            ServerBase.InitializeGarbageCollector();
            this.logger.Info("Initializing Configuration...");
            this.Config = new XmlConfig(this.ConfigFilePath);
            this.Config.AddAssemblies(this.LoadedAssemblies.Values.ToArray<Assembly>());
            if (!File.Exists(this.ConfigFilePath))
            {
                this.Config.Create(false);
                this.logger.Info("Config file created");
            }
            else
                this.Config.Load();
            this.logger.Info("Initialize Task Pool");
            this.IOTaskPool = new SelfRunningTaskPool(ServerBase.IOTaskInterval, "IO Task Pool");
            this.CommandManager = Singleton<CommandManager>.Instance;
            this.CommandManager.RegisterAll(Assembly.GetExecutingAssembly());
            this.logger.Info("Register Plugins...");
            this.InitializationManager = Singleton<InitializationManager>.Instance;
            this.InitializationManager.AddAssemblies((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies());
            this.PluginManager = Singleton<PluginManager>.Instance;
            this.PluginManager.PluginAdded += new PluginManager.PluginContextHandler(this.OnPluginAdded);
            this.PluginManager.PluginRemoved += new PluginManager.PluginContextHandler(this.OnPluginRemoved);
            this.logger.Info("Loading Plugins...");
            Singleton<PluginManager>.Instance.LoadAllPlugins();
            this.CommandManager.LoadOrCreateCommandsInfo(ServerBase.CommandsInfoFilePath);

        }

        public virtual void UpdateConfigFiles()
        {
            this.logger.Info("Recreate server config file ...");
            if (File.Exists(this.ConfigFilePath))
            {
                this.logger.Info("Update {0} file", this.ConfigFilePath);
                this.Config = new XmlConfig(this.ConfigFilePath);
                this.Config.AddAssemblies(this.LoadedAssemblies.Values.ToArray<Assembly>());
                this.Config.Load();
                this.Config.Create(true);
            }
            else
            {
                this.logger.Info("Create {0} file", this.ConfigFilePath);
                this.Config = new XmlConfig(this.ConfigFilePath);
                this.Config.AddAssemblies(this.LoadedAssemblies.Values.ToArray<Assembly>());
                this.Config.Create(false);
            }
            this.logger.Info("Recreate plugins config files ...", this.ConfigFilePath);
            this.PluginManager = Singleton<PluginManager>.Instance;
            Singleton<PluginManager>.Instance.LoadAllPlugins();
            foreach (PluginBase pluginBase in ((IEnumerable<PluginContext>)this.PluginManager.GetPlugins()).Select<PluginContext, IPlugin>((Func<PluginContext, IPlugin>)(entry => entry.Plugin)).OfType<PluginBase>())
            {
                if (pluginBase.UseConfig && pluginBase.AllowConfigUpdate)
                {
                    bool flag;
                    if (!(flag = File.Exists(pluginBase.GetConfigPath())))
                        this.logger.Info<string, string>("Create '{0}' config file => '{1}'", pluginBase.Name, Path.GetFileName(pluginBase.GetConfigPath()));
                    pluginBase.LoadConfig();
                    if (flag)
                    {
                        this.logger.Info<string, string>("Update '{0}' config file => '{1}'", pluginBase.Name, Path.GetFileName(pluginBase.GetConfigPath()));
                        pluginBase.Config.Create(true);
                    }
                }
            }
            this.logger.Info("All config files were correctly updated/created ! Shutdown ...");
            Thread.Sleep(TimeSpan.FromSeconds(2.0));
            Environment.Exit(0);
        }

        private static void PreLoadReferences(Assembly executingAssembly)
        {
            foreach (Assembly executingAssembly1 in ((IEnumerable<AssemblyName>)executingAssembly.GetReferencedAssemblies()).Where<AssemblyName>((Func<AssemblyName, bool>)(assemblyName => ((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies()).Count<Assembly>((Func<Assembly, bool>)(entry => entry.GetName().FullName == assemblyName.FullName)) <= 0)).Select<AssemblyName, Assembly>((Func<AssemblyName, Assembly>)(assemblyName => Assembly.Load(assemblyName))))
                ServerBase.PreLoadReferences(executingAssembly1);
        }

        protected virtual void OnPluginRemoved(PluginContext plugincontext)
        {
            this.logger.Info("Plugin Unloaded : {0}", plugincontext.Plugin.GetDefaultDescription());
        }

        protected virtual void OnPluginAdded(PluginContext plugincontext)
        {
            this.logger.Info("Plugin Loaded : {0}", plugincontext.Plugin.GetDefaultDescription());
        }

        private void OnClientConnected(BaseClient client)
        {
            this.logger.Info<BaseClient>("Client {0} connected", client);
        }

        private void OnClientDisconnected(BaseClient client)
        {
            this.logger.Info<BaseClient>("Client {0} disconnected", client);
        }

        private static void InitializeGarbageCollector()
        {
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;
        }

        private void OnAssemblyLoad(object sender, AssemblyLoadEventArgs e)
        {
            string name = e.LoadedAssembly.GetName().Name;
            if (this.LoadedAssemblies.ContainsKey(name))
                return;
            this.LoadedAssemblies.Add(name, e.LoadedAssembly);
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            this.HandleCrashException((Exception)e.Exception);
            e.SetObserved();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.HandleCrashException((Exception)e.ExceptionObject);
            if (!e.IsTerminating)
                return;
            this.Shutdown();
        }

        private void OnContractFailed(object sender, ContractFailedEventArgs e)
        {
            this.logger.Fatal("Contract failed : {0}", e.Condition);
            if (e.OriginalException != null)
                this.HandleCrashException(e.OriginalException);
            else
                this.logger.Fatal(string.Format(" Stack Trace:\r\n{0}", (object)Environment.StackTrace));
            e.SetHandled();
        }

        public void HandleCrashException(Exception e)
        {
            if (e is StackOverflowException)
            {
                File.WriteAllText("fichierStack.txt", e.StackTrace);
            }
            ExceptionManager.RegisterException(e);
            this.logger.Fatal(string.Format(" Crash Exception : {0}\r\n", (object)e.Message) + string.Format(" Source: {0} -> {1}\r\n", (object)e.Source, (object)e.TargetSite) + string.Format(" Stack Trace:\r\n{0}", (object)e.StackTrace));
            if (e.InnerException == null)
                return;
            this.HandleCrashException(e.InnerException);
        }

        public virtual void Start()
        {
            this.Running = true;
            this.Initializing = false;
            this.IOTaskPool.CallPeriodically((int)TimeSpan.FromSeconds(30.0).TotalMilliseconds, new Action(this.KeepSQLConnectionAlive));
            this.IOTaskPool.CallPeriodically((int)TimeSpan.FromSeconds(5.0).TotalMilliseconds, new Action(this.CheckScheduledShutdown));
        }

        protected virtual void KeepSQLConnectionAlive()
        {
            try
            {
                this.DBAccessor.Database.Execute("DO 1", Array.Empty<object>());
            }
            catch (Exception ex1)
            {
                this.logger.Error<Exception>("Cannot ping SQL connection : {0}", ex1);
                this.logger.Warn("Try to Re-open the connection");
                try
                {
                    this.DBAccessor.CloseConnection();
                    this.DBAccessor.OpenConnection();
                }
                catch (Exception ex2)
                {
                    this.logger.Error<Exception>("Cannot reopen the SQL connection : {0}", ex2);
                }
            }
        }

        //protected void DisconnectAfkClient()
        //{
        //    foreach (BaseClient baseClient in this.Clients.FindAll((Predicate<BaseClient>)(client =>
        //    {
        //        double totalSeconds = DateTime.Now.Subtract(client.LastActivity).TotalSeconds;
        //        int? disconnectionTime = Settings.InactivityDisconnectionTime;
        //        return totalSeconds >= (double)disconnectionTime.GetValueOrDefault() && disconnectionTime.HasValue;
        //    })))
        //        baseClient.Stop();
        //}

        protected abstract BaseClient CreateClient(Socket s);
        protected abstract void OnShutdown();
        //protected virtual void OnShutdown()
        //{

        //    this.IOTaskPool.Stop(false);
        //}

        public virtual void ScheduleShutdown(TimeSpan timeBeforeShuttingDown, string reason)
        {
            this.ScheduledShutdownReason = reason;
            this.ScheduleShutdown(timeBeforeShuttingDown);
        }

        public virtual void ScheduleShutdown(TimeSpan timeBeforeShuttingDown)
        {
            this.IsShutdownScheduled = true;
            this.ScheduledShutdownDate = DateTime.Now + timeBeforeShuttingDown;
        }

        public virtual void CancelScheduledShutdown()
        {
            this.IsShutdownScheduled = false;
            this.ScheduledShutdownDate = DateTime.MaxValue;
            this.ScheduledShutdownReason = (string)null;
        }

        protected virtual void CheckScheduledShutdown()
        {
            if ((!ServerBase.ScheduledAutomaticShutdown || this.UpTime.TotalMinutes <= (double)ServerBase.AutomaticShutdownTimer) && (!this.IsShutdownScheduled || !(this.ScheduledShutdownDate <= DateTime.Now)))
                return;
            this.Shutdown();
        }

        public void Shutdown()
        {
            bool lockTaken = false;
            try
            {
                Monitor.Enter((object)this, ref lockTaken);
                this.Running = false;
                if (socketListener.Connected)
                    socketListener.Shutdown(SocketShutdown.Both);
                this.OnShutdown();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Console.WriteLine("Application is now terminated. Wait " + (object)Definitions.ExitWaitTime + " seconds to exit ... or press any key to cancel");
                Func<bool> func = (Func<bool>)(() => Console.KeyAvailable);
                int timeout = Definitions.ExitWaitTime * 1000;
                int interval = 20;
                Func<bool> predicate;
                if (ConditionWaiter.WaitFor(func, timeout, interval))
                {
                    Console.ReadKey(true);
                    Thread.Sleep(500);
                    Console.WriteLine("Press now a key to exit...");
                    Console.ReadKey(true);
                }
                Environment.Exit(0);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit((object)this);

            }
        }
        #region Events

        public delegate void ConnectionAcceptedDelegate(Socket acceptedSocket);
        public event ConnectionAcceptedDelegate ConnectionAccepted;
        public void OnConnectionAccepted(Socket client)
        {
            if (ConnectionAccepted != null)
                ConnectionAccepted(client);
        }
    }

        #endregion
        public abstract class ServerBase<T> : ServerBase
        where T : class
    {
        /// <summary>
        ///   Class singleton
        /// </summary>
        public static T Instance;


        protected ServerBase(string configFile, string schemaFile)
            : base(configFile, schemaFile)
        {
        }

        public override void Initialize()
        {
            Instance = this as T;
            base.Initialize();
        }
    }
}