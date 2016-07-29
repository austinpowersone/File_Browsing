using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Http;
using System;
using System.Net.Http;
using System.Net;

namespace File_System.Controllers
{
	public class DirectoryInfoController : ApiController
    {
		[HttpGet]
		public HttpResponseMessage Get(string path = @".")
		{			
			var directory = new DirectoryInfo (path); 
			try {
				if (!directory.Exists) {
					throw new DirectoryNotFoundException ();
				}
				var files = Process(path);		
				var directories = new List<string> ();
				if (directory.Parent != null) {								
					directories.Add (directory.Parent.FullName);
				}
				else{
					directories.AddRange(DriveInfo.GetDrives().Select(x => {
						return x.Name;
					}));
				}
				foreach (var dir in directory.GetDirectories()) {										
					directories.Add (dir.FullName);
				}

				var result = new DirectoryInformation () {
					Path = directory.FullName,
					Directories = directories,
					Files = files
				};

				return Request.CreateResponse (HttpStatusCode.OK, result);
			} catch (Exception e) {
				return Request.CreateResponse (HttpStatusCode.BadRequest, e.Message);
			}
		}

		private List<FileInformation> Process(string path, List<FileInformation> files = null)
		{
			if (files == null)
				files = new List<FileInformation> ();
			try{
				var directoryinfo = new DirectoryInfo(path);
				files.AddRange(directoryinfo.EnumerateFiles().Select(x => new FileInformation{
					Name = x.Name,
					Size = x.Length
				}));
				foreach(var directory in directoryinfo.EnumerateDirectories()){
					Process(directory.FullName, files);
				}
			}
			catch(Exception){
				//ignored
			}
			return files;
		}
    }

	public class FileInformation
	{		
		public string Name { get; set; }
		public long Size { get; set; }
	}

	public class DirectoryInformation
	{
		public string Path{ get; set; }
		public ICollection<string> Directories { get; set; }
		public IEnumerable<FileInformation> Files{ get; set; }
	}
}
