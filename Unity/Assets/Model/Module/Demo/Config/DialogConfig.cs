namespace ETModel
{
	[Config((int)(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map))]
	public partial class DialogConfigCategory : ACategory<DialogConfig>
	{
	}

	public class DialogConfig: IConfig
	{
		public long Id { get; set; }
		public int NextId;
		public int RoleId;
		public string Text;
	}
}
