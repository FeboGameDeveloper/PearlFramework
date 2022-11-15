//  Emailer.cs
//  http://www.mrventures.net/all-tutorials/sending-emails
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public struct SmtpClientData
    {
        public string emailAddress;
        public string password;
        public string host;
        public int port;
    }

    [Serializable]
    public struct GmailSmtpClientData
    {
        public string emailAddress;
        public string password;
    }

    [Serializable]
    public struct MessageData
    {
        public List<string> andresses;
        public string subject;
        public string body;
        public string pathAttachments;
        public string localizeTable;

        public MessageData(string andress, string subject, string body, string localizeTable = "", string pathAttachments = null)
        {
            andresses = new();
            andresses.Add(andress);
            this.subject = subject;
            this.body = body;
            this.pathAttachments = pathAttachments;
            this.localizeTable = localizeTable;
        }

        public MessageData(List<string> andresses, string subject, string body, string localizeTable = "", string pathAttachments = null)
        {
            this.andresses = andresses;
            this.subject = subject;
            this.body = body;
            this.pathAttachments = pathAttachments;
            this.localizeTable = localizeTable;
        }
    }


    public class EMailer : MonoBehaviour
    {
        private static int[] GetPort()
        {
            return new int[] { 587, 2525, 465, 25 };
        }

        public static bool SendAnEmail(GmailSmtpClientData data, MessageData message, out string error)
        {
            SmtpClientData genericData;
            genericData.emailAddress = data.emailAddress;
            genericData.password = data.password;
            genericData.port = 587;
            genericData.host = "smtp.gmail.com";
            return SendAnEmail(genericData, message, out error);
        }


        public static bool SendAnEmail(SmtpClientData smtpClient, MessageData message, out string error)
        {
            error = "";

            if (!OnlineExtend.IsValidEmail(message.andresses) || !OnlineExtend.IsValidEmail(smtpClient.emailAddress))
            {
                error = "emailinvalid";
                return false;
            }

            if (!OnlineExtend.IsThereIntrnet())
            {
                error = "notconnect";
                return false;
            }

            // Create mail
            MailMessage mail = new();
            mail.From = new MailAddress(smtpClient.emailAddress);

            if (message.andresses != null)
            {
                foreach (var andress in message.andresses)
                {
                    mail.To.Add(andress);
                }
            }

            mail.Subject = TextManager.ConvertString(message.localizeTable, message.subject, out _);
            mail.Body = TextManager.ConvertString(message.localizeTable, message.body, out _);

            if (!string.IsNullOrWhiteSpace(message.pathAttachments))
            {
                mail.Attachments.Add(new Attachment(message.pathAttachments));
            }

            // Setup server 
            SmtpClient smtpServer = new(smtpClient.host);
            smtpServer.Credentials = new NetworkCredential(smtpClient.emailAddress, smtpClient.password) as ICredentialsByHost;
            smtpServer.EnableSsl = true;
            smtpServer.Port = smtpClient.port;
            ServicePointManager.ServerCertificateValidationCallback = delegate
            (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                Debug.LogManager.Log("Email success with port " + smtpClient.port + "!");
                return true;
            };

            // Send mail to server, print results
            try
            {
                smtpServer.Send(mail);
            }
            catch (System.Exception e)
            {
                Debug.LogManager.Log("Email error with port " + smtpClient.port + ": " + e.Message);
                return false;
            }
            finally
            {
                Debug.LogManager.Log("Email sent!");
            }

            return true;
        }
    }
}