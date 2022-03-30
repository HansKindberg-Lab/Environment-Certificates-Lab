using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Application.Models.Views.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
	[Authorize]
	public class HomeController : SiteController
	{
		#region Constructors

		public HomeController(IHostEnvironment hostEnvironment, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			this.HostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
		}

		#endregion

		#region Properties

		protected internal virtual IHostEnvironment HostEnvironment { get; }

		#endregion

		#region Methods

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
		public virtual async Task<IActionResult> CertificateStore()
		{
			var model = new CertificateStoreViewModel();

			foreach(var storeLocation in Enum.GetValues<StoreLocation>().OrderBy(storeLocation => storeLocation.ToString()))
			{
				foreach(var storeName in Enum.GetValues<StoreName>().OrderBy(storeName => storeName.ToString()))
				{
					var certificates = new List<Certificate>();
					var certificateStore = new CertificateStore
					{
						HtmlId = $"{storeLocation.ToString().ToLowerInvariant()}-{storeName.ToString().ToLowerInvariant()}",
						Text = $"{storeLocation} - {storeName}"
					};

					try
					{
						using(var store = new X509Store(storeName, storeLocation, OpenFlags.ReadOnly))
						{
							foreach(var certificate in store.Certificates)
							{
								using(certificate)
								{
									certificates.Add(new Certificate
									{
										Issuer = certificate.Issuer,
										SerialNumber = certificate.SerialNumber,
										Subject = certificate.Subject,
										Thumbprint = certificate.Thumbprint
									});
								}
							}

							certificates.Sort((first, second) => string.Compare(first?.Subject, second?.Subject, StringComparison.Ordinal));

							foreach(var certificate in certificates)
							{
								certificateStore.Certificates.Add(certificate);
							}
						}
					}
					catch(Exception exception)
					{
						certificateStore.Exception = exception;
					}

					model.CertificateStores.Add(certificateStore);
				}
			}

			return await Task.FromResult(this.View(model));
		}

		public virtual async Task<IActionResult> Environment()
		{
			return await Task.FromResult(this.View());
		}

		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		[SuppressMessage("Security", "CA3003:Review code for file path injection vulnerabilities")]
		public virtual async Task<IActionResult> Index(string path, bool recursive, bool search, string searchPattern)
		{
			var root = new DirectoryInfo(Directory.GetDirectoryRoot(this.HostEnvironment.ContentRootPath));

			var model = new FileSystemCertificatesViewModel
			{
				Path = string.IsNullOrEmpty(path) ? root.FullName : path,
				Recursive = recursive,
				Root = root,
				Search = search
			};

			if(!string.IsNullOrEmpty(searchPattern))
				model.SearchPattern = searchPattern;

			try
			{
				if(Directory.Exists(model.Path))
				{
					var directory = new DirectoryInfo(model.Path);

					while(directory != null)
					{
						var selected = !model.Breadcrumb.Any();

						model.Breadcrumb.Insert(0, new SelectListItem(directory.Name, directory.FullName, selected));

						directory = directory.Parent;
					}

					foreach(var directoryPath in Directory.GetDirectories(model.Path, "*", new EnumerationOptions()).OrderBy(directoryPath => directoryPath))
					{
						model.Directories.Add(new DirectoryInfo(directoryPath));
					}

					if(search)
					{
						var enumerationOptions = new EnumerationOptions
						{
							AttributesToSkip = 0,
							IgnoreInaccessible = true,
							MatchType = MatchType.Win32,
							RecurseSubdirectories = model.Recursive
						};

						foreach(var filePath in Directory.GetFiles(model.Path, model.SearchPattern, enumerationOptions))
						{
							var certificateFile = new CertificateFile
							{
								Path = filePath
							};

							try
							{
								var certificates = new X509Certificate2Collection();
								certificates.ImportFromPemFile(filePath);

								foreach(var certificate in certificates)
								{
									using(certificate)
									{
										certificateFile.Certificates.Add(new Certificate
										{
											Issuer = certificate.Issuer,
											SerialNumber = certificate.SerialNumber,
											Subject = certificate.Subject,
											Thumbprint = certificate.Thumbprint
										});
									}
								}
							}
							catch(Exception exception)
							{
								certificateFile.Exception = new InvalidOperationException($"Could not load certificates from file \"{filePath}\".", exception);
							}

							model.CertificateFiles.Add(certificateFile);
						}
					}
				}
				else
				{
					model.Exception = new InvalidOperationException($"The directory \"{model.Path}\" does not exist.");
				}
			}
			catch(Exception exception)
			{
				model.Exception = exception;
			}

			return await Task.FromResult(this.View(model));
		}

		#endregion
	}
}