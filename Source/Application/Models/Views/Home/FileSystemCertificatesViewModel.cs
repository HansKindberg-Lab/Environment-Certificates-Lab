using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Models.Views.Home
{
	public class FileSystemCertificatesViewModel
	{
		#region Properties

		public virtual IList<SelectListItem> Breadcrumb { get; } = new List<SelectListItem>();
		public virtual IList<FileSystemCertificate> Certificates { get; } = new List<FileSystemCertificate>();
		public virtual IList<DirectoryInfo> Directories { get; } = new List<DirectoryInfo>();
		public virtual Exception Exception { get; set; }
		public virtual string Path { get; set; }
		public virtual bool Recursive { get; set; }
		public virtual DirectoryInfo Root { get; set; }
		public virtual bool Search { get; set; }
		public virtual string SearchPattern { get; set; } = "*.crt";

		#endregion
	}
}