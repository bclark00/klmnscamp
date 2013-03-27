using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Klmsncamp.Models
{
	public class UploadedFile
	{
		public int ID { get; set; }

		[MaxLength(250,ErrorMessage="En çok 250 karakter olabilir")]
		[Display(Name="Klasör Adı")]
		public string FileName { get; set; }
		
		[MaxLength(250, ErrorMessage = "En çok 250 karakter olabilir")]
		[Display(Name = "Klasör Yolu")]
		public string FilePath { get; set; }
		
		[Display(Name = "Açıklama")]
		public string Description { get; set; }

		public string FileContentType { get; set; }

		[Display(Name="Aktif mi")]
		public bool IsActive { get; set; }
	}
}