using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common
{
    public class HeaderKeys
    {
        public const string IdentityAccessToken = "accesstoken";
        public const string TimeStamp = "timestamp";
        public const string AppKey = "appkey";
        public const string Nonce = "nonce";
        public const string Signature = "signature";
        public const string ClientTransactionID = "X-TransactionID";
    }

    public class ClientAuth
    {
        public string customerid { get; set; }
        public string customername { get; set; }
        public string publickey { get; set; }
        public string secretkey { get; set; }
        public string clientid { get; set; }
    }
}
