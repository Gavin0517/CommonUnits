using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Common.Units
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public class SMTPEmail
    {
        private FileStream FileStream_my = null;
        public System.Net.Mail.SmtpClient SmtpClient = null;

        public SMTPEmail()
        {
            this.SmtpClient = new System.Net.Mail.SmtpClient();
        }

        public bool Attachment_MaiInit(string path)
        {
            bool flag;
            try
            {
                this.FileStream_my = new FileStream(path, FileMode.Open);
                string name = this.FileStream_my.Name;
                int num = (int)((this.FileStream_my.Length / 0x400L) / 0x400L);
                this.FileStream_my.Close();
                if (num > 10)
                {
                    throw new Exception("文件长度不能大于10M！你选择的文件大小为" + num.ToString() + "M");
                }
                flag = true;
            }
            catch (IOException exception)
            {
                throw exception;
            }
            return flag;
        }

        public void SendEmail(MailMessage MailMessage_Mai)
        {
            this.SmtpClient.SendAsync(MailMessage_Mai, "000000000");
        }

        public void SetAddressform(string MailAddress, string MailPwd)
        {
            NetworkCredential credential = new NetworkCredential(MailAddress, MailPwd);
            this.SmtpClient.Credentials = new NetworkCredential(MailAddress, MailPwd);
        }

        public void SetSmtpClient(string ServerHost, int Port)
        {
            this.SmtpClient.Host = ServerHost;
            this.SmtpClient.Port = Port;
        }
    }
}
