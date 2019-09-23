namespace ETModel
{
	[Config((int)(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map))]
	public partial class DialogCategory : ACategory<Dialog>
	{
	}

	public class Dialog: IConfig
	{
		public long Id { get; set; }
		public int NextId;
		public int RoleId;
		public string Text;
	}
}
