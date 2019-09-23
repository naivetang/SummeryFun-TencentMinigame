using ETModel;

namespace ETHotfix
{
	[Config((int)(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map))]
	public partial class RoleCategory : ACategory<Role>
	{
	}

	public class Role: IConfig
	{
		public long Id { get; set; }
		public string Name;
		public string Pos;
	}
}
