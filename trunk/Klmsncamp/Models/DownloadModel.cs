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
				FileNames file = new FileNames();
				file.FileID = i + 1;
				file.FileName = item.Name;
				file.FilePath = directoryInfo.FullName + @"\" + item.Name;
				string mimeType = "application/unknown";
				//string ext = System.IO.Path.GetExtension(item).ToLower();
				Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(item.Extension);
				//if (regKey != null && regKey.GetValue("Content Type") != null)
				mimeType = regKey.GetValue("Content Type").ToString();
				//return mimeType;
				file.FileContentType = mimeType;
				file.FileByte = System.IO.File.ReadAllBytes(file.FilePath);
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
		public byte[] FileByte { get; set; }
	}
}