using System;
using System.Collections.Generic;

namespace Application.Models.Views.Home
{
	public class CertificateFile
	{
		#region Properties

		public virtual IList<Certificate> Certificates { get; } = new List<Certificate>();
		public virtual Exception Exception { get; set; }
		public virtual string Path { get; set; }

		#endregion
	}
}