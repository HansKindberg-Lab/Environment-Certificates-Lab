namespace Application.Models.Views.Home
{
	public class Certificate
	{
		#region Properties

		public virtual string Issuer { get; set; }
		public virtual string SerialNumber { get; set; }
		public virtual string Subject { get; set; }
		public virtual string Thumbprint { get; set; }

		#endregion
	}
}