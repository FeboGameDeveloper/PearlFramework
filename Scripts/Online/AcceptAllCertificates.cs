// Based on https://www.owasp.org/index.php/Certificate_and_Public_Key_Pinning#.Net
using UnityEngine.Networking;

namespace Pearl
{
    class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}