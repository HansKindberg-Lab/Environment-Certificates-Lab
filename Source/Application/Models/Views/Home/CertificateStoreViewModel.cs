using System.Collections.Generic;

namespace Application.Models.Views.Home
{
	public class CertificateStoreViewModel
	{
		#region Properties

		public virtual IList<CertificateStore> CertificateStores { get; } = new List<CertificateStore>();

		#endregion
	}
}