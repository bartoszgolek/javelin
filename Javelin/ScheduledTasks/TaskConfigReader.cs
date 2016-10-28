using System;
using System.Reflection;
using Javelin.Base.Tasks;

namespace Javelin.ScheduledTasks
{
	public class TaskConfigReader : ITaskConfigReader
	{
		public object GetTaskConfig(ITaskConfig taskConfig)
		{
			return GetConfigurationFromTaskConfig(taskConfig);
		}

		private static object GetConfigurationFromTaskConfig(ITaskConfig taskConfig)
		{
			Type configurationType = GetConfigType(taskConfig.TaskType);
			MethodInfo method = taskConfig.GetType().GetMethod("GetTaskConfig").MakeGenericMethod(new[] { configurationType });

			return method.Invoke(taskConfig, null);
		}

		private static Type GetConfigType(Type type)
		{
			var currentType = type;
			while (currentType != null)
			{
				if (currentType.IsGenericType)
				{
					var genericType = currentType.GetGenericTypeDefinition();
					if (genericType == typeof(Task<>))
						return currentType.GetGenericArguments()[0];
				}
				currentType = currentType.BaseType;
			}

			throw new Exception(string.Format("Cannot recognize configuration type for task '{0}'", type));
		}
	}
}