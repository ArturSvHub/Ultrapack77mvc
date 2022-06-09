using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpakUtilitiesLibrary
{
	public class Settings
	{
		public EmailSettings EmailSettings { get; set; } = new EmailSettings();
	}
	public class EmailSettings
	{
		public string FromTitle { get; set; } = "Администрация сайта upak77.ru";
		public string FromEmail { get; set; } = "upak@gkultra.ru";
		public string ToTitle { get; set; } = "";
		public string ToEmail { get; set; } = "";
		public string SmtpClientAdress { get; set; } = "mail.hosting.reg.ru";
		public string AuthLogin { get; set; } = "upak@gkultra.ru";
		public string AuthPass { get; set; } = "Upaksite2022.";
	}


}
