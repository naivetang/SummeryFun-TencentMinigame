namespace ETModel
{
	[Config((int)(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map))]
	public partial class RoleConfigCategory : ACategory<RoleConfig>
	{
	}

	public class RoleConfig: IConfig
	{
		public long Id { get; set; }
		public string Name;
		public string Pos;
	}
}
