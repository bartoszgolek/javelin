using System;
using System.ServiceProcess;

namespace Javelin.Tasks.WindowsService
{
	public class ServiceManager
	{
		public static void StopService(string serviceName, TimeSpan timeout)
		{
			var service = new ServiceController(serviceName);

			service.Stop();
			service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
		}

		public static void StartService(string serviceName, TimeSpan timeout)
		{
			var service = new ServiceController(serviceName);

			service.Start();
			service.WaitForStatus(ServiceControllerStatus.Running, timeout);
		}
	}
}