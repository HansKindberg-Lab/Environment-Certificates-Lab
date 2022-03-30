using System;
using System.Collections.Generic;

namespace Application.Models.Views.Home
{
	public class CertificateStore
	{
		#region Properties

		public virtual ICollection<Certificate> Certificates { get; } = new List<Certificate>();
		public virtual Exception Exception { get; set; }
		public virtual string HtmlId { get; set; }
		public virtual string Text { get; set; }

		#endregion
	}
}