namespace ETModel
{
	[Config((int)(AppType.ClientH |  AppType.ClientM | AppType.Gate | AppType.Map))]
	public partial class BookDetailsCategory : ACategory<BookDetails>
	{
	}

	public class BookDetails: IConfig
	{
		public long Id { get; set; }
		public string Title;
		public string Text;
	}
}
