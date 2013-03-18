using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Klmsncamp.Models
{
	public class DownloadModel
	{
		public List<FileNames> GetFiles()
		{
			List<FileNames> listFiles = new List<FileNames>();
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/UploadedFiles"));
			int i = 0;
			foreach (var item in directoryInfo.GetFiles())
			{
				FileNames file=new FileNames();
				file.FileID=i+1;
				file.FileName=item.Name;
				file.FilePath=directoryInfo.FullName+@"\"+item.Name;
				file.FileContentType = item.GetType().ToString();
				listFiles.Add(file);

				i = i + 1;
			}

			return listFiles;
		}
	}

	public class FileNames
	{
		public int FileID { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string FileContentType { get; set; }
	}
}