using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using Javelin.Api;
using Javelin.Base;
using Javelin.Config;
using log4net;
using Quartz;

namespace Javelin
{
	partial class Bootstrapper : ServiceBase
	{
		private Bootstrapper(AutoResetEvent autoResetEvent)
			: this()
		{
			this.autoResetEvent = autoResetEvent;
		}

		public Bootstrapper()
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledException;
			InitializeComponent();
		}

		public static void Main(string[] args)
		{
			// Setting current directory to directory off assembly
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

			if (IsService(args))
			{
				Run(new Bootstrapper());
			}
			else
			{
				var autoResetEvent = new AutoResetEvent(false);

				var bootstrapper = new Bootstrapper(autoResetEvent);
				bootstrapper.OnStart(args);

				HandleStopKey(bootstrapper);

				autoResetEvent.WaitOne();
			}
		}

		private static void HandleStopKey(Bootstrapper bootstrapper)
		{
			Console.CancelKeyPress += (sender, eventArgs) =>
			{
				logger.Info("Exiting...");
				bootstrapper.Stop();
			};

			Console.Out.WriteLine("Press Ctrl+c to quit.");

			while (true)
				Console.ReadKey(true);
		}

		/// <summary>
		/// Starts bootstrapper
		/// </summary>
		protected override void OnStart(string[] args)
		{
			try
			{
				Start(args);
			}
			catch (Exception ex)
			{
				logger.Fatal("OnStart unhandled exception: ", ex);
				throw;
			}
		}

		private void Start(IEnumerable<string> args)
		{
			var bootstrapperContainer = BuildContainer(args);
			LoadAdditionalModules();
			LoadComponents(bootstrapperContainer);

			mainLifetimeScope = bootstrapperContainer.BeginLifetimeScope();
			bootstrapperConfig = mainLifetimeScope.Resolve<IBootstrapperConfig>();

			ActivateComponents();

			StartApi();

			StartScheduler();
		}

		private void StartApi()
		{
			host = mainLifetimeScope.Resolve<ServiceStackAppHost>();
			host.Init();
			host.Start(bootstrapperConfig.RootUri);
			logger.InfoFormat("Host started at {0}", bootstrapperConfig.RootUri);
		}

		private void StartScheduler()
		{
			if (bootstrapperConfig.IsMaster)
			{
				mainLifetimeScope.Resolve<IScheduler>().Start();
				logger.InfoFormat("Scheduler started");
			}
		}

		private void ActivateComponents()
		{
			logger.Info("Activating Components");
			foreach (var component in appComponents)
			{
				try
				{
					component.Activate(mainLifetimeScope);
					logger.InfoFormat("Component {0} activated successfully", component.Name);
				}
				catch (Exception ex)
				{
					logger.Error(
						string.Format("Error occurred while activating component {0}", component.Name),
						ex);
				}
			}
		}

		private void LoadComponents(IContainer bootstrapperContainer)
		{
			var componentBuilder = new ContainerBuilder();
			foreach (var type in AppDomain.CurrentDomain
										.GetAssemblies().SelectMany(x => x.GetTypes())
										.Where(t => t.IsAssignableTo<IComponent>() && t.IsClass && !t.IsAbstract))
			{
				try
				{
					var component = (IComponent)Activator.CreateInstance(type);

					foreach (var module in component.GetModules())
					{
						componentBuilder.RegisterModule(module);
					}

					appComponents.Add(component);
					logger.InfoFormat("Module {0} loaded successfully.", component.Name);
				}
				catch (Exception ex)
				{
					logger.Error("Error occurred while initializing " + type.Name +
								" Module ", ex);
				}
			}

			componentBuilder.Update(bootstrapperContainer);
		}

		private static void LoadAdditionalModules()
		{
			foreach (
				string filePath in
					Directory.GetFiles(ConfigurationManager.AppSettings["ModuleFolder"], ConfigurationManager.AppSettings["ModuleMask"])
				)
			{
				logger.DebugFormat("Loading Assembly {0}", filePath);
				Assembly.LoadFrom(filePath);
			}
		}

		private static IContainer BuildContainer(IEnumerable<string> args)
		{
			var containerBuilder = new ContainerBuilder();

			containerBuilder.RegisterModule(new MainModule());
			containerBuilder.RegisterModule(new RestModule());
			containerBuilder.RegisterModule(new TasksModule());
			containerBuilder.RegisterModule(new WebServiceModule());

			containerBuilder.Register(c => new ExecutionArgs(args)).As<IExecutionArgs>();

			logger.Info("Loading modules");

			if (!Directory.Exists(ConfigurationManager.AppSettings["ModuleFolder"]))
			{
				logger.Warn("CANNOT find modules folder: " + ConfigurationManager.AppSettings["ModuleFolder"]);
				Environment.Exit(1);
			}

			var bootstrapperContainer = containerBuilder.Build();
			return bootstrapperContainer;
		}

		private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			logger.Fatal("AppDomain unhandled exception: ", e.ExceptionObject as Exception);
		}

		protected override void OnStop()
		{
			foreach (var component in appComponents)
			{
				try
				{
					component.Deactivate(mainLifetimeScope);
				}
				catch (Exception ex)
				{
					logger.Error(
						string.Format("Error occurred while deactivating {0} component", component.Name),
						ex);
				}
			}

			StopApi();

			mainLifetimeScope.Resolve<IScheduler>().Shutdown();

			mainLifetimeScope.Dispose();

			if (autoResetEvent != null)
				autoResetEvent.Set();
		}

		private void StopApi()
		{
			if (host != null)
			{
				host.Stop();
				host = null;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				AppDomain.CurrentDomain.UnhandledException -= UnhandledException;
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private static bool IsService(string[] args)
		{
			return !Environment.UserInteractive && args.Length == 0;
		}

		public static ILog logger = LogManager.GetLogger(typeof(Bootstrapper));

		private readonly IList<IComponent> appComponents = new List<IComponent>();

		private ILifetimeScope mainLifetimeScope;
		private readonly AutoResetEvent autoResetEvent;
		private IBootstrapperConfig bootstrapperConfig;
		private ServiceStackAppHostBase host;
	}
}