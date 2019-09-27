namespace ETModel
{
	[Config((int)(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map))]
	public partial class TriggerAreaConfigCategory : ACategory<TriggerAreaConfig>
	{
	}

	public class TriggerAreaConfig: IConfig
	{
		public long Id { get; set; }
		public string EventName;
		public string ShowWindow;
		public string UIType;
		public int BookIndex;
	}
}
