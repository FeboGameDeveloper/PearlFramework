using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Pearl
{
    public class OnlineExtend : MonoBehaviour
    {
        public static bool IsValidEmail(ICollection<string> emails)
        {
            if (emails == null)
            {
                return false;
            }

            foreach (var email in emails)
            {
                if (!IsValidEmail(email))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsValidEmail(string email)
        {
            if (email == null)
            {
                return false;
            }

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsThereIntrnet()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public static bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        public static bool CheckURLValid(string source)
        {
            return Uri.IsWellFormedUriString(source, UriKind.Absolute);
        }
    }
}
