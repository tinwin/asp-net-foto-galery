using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace Common
{
	public class Windsor
	{
		private static WindsorContainer _instance;

		public static WindsorContainer Instance
		{
			get
			{
				return _instance ?? (_instance = new WindsorContainer(
					new XmlInterpreter(
						new ConfigResource("windsor"))));
			}
		}
	}
}
