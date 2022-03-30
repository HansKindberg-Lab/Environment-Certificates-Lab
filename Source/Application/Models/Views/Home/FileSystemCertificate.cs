using System;

namespace Application.Models.Views.Home
{
	public class FileSystemCertificate
	{
		#region Properties

		public virtual Certificate Certificate { get; set; }
		public virtual Exception Exception { get; set; }
		public virtual string Path { get; set; }

		#endregion
	}
}