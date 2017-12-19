using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPI.Common
{

    public class AuthorizeWebAPIAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext context)
        {
            ValidateApplicationkey(context);
            ValidateOAuthHeader(context);
            return;
        }

        private void ValidateApplicationkey(System.Web.Http.Controllers.HttpActionContext context)
        {
            if (context.Request.Headers.Contains(HeaderKeys.AppKey))
            {
                string publicKey = context.Request.Headers.GetValues(HeaderKeys.AppKey).First().ToUpper();

                ClientAuth clientAuth = this.getClientAuthDetails(publicKey);

                if (clientAuth == null || String.IsNullOrWhiteSpace(clientAuth.customerid))
                {
                    throw new UnauthorizedAccessException(String.Format("Incorrect Application Key - {0}", publicKey));
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        private void ValidateOAuthHeader(System.Web.Http.Controllers.HttpActionContext context)
        {
            string publicKey = String.Empty, nonce = String.Empty, oauth_timestamp = String.Empty, signature = String.Empty;
            string normalizedUrl = String.Empty;
            string normalizedRequestParameters = String.Empty;

            if (context.Request.Headers.Contains(HeaderKeys.AppKey))
            {
                publicKey = context.Request.Headers.GetValues(HeaderKeys.AppKey).First();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }

            if (context.Request.Headers.Contains(HeaderKeys.Nonce))
            {
                nonce = context.Request.Headers.GetValues(HeaderKeys.Nonce).First();

                Guid guidOutput = Guid.NewGuid();
                if (!Guid.TryParse(nonce, out guidOutput))
                {
                    throw new UnauthorizedAccessException(String.Format("Invalid nonce provided - {0}", nonce));
                }
            }
            else
            {
                throw new UnauthorizedAccessException("Header doesn't contain Nonce");
            }

            if (context.Request.Headers.Contains(HeaderKeys.TimeStamp))
            {
                oauth_timestamp = context.Request.Headers.GetValues(HeaderKeys.TimeStamp).First();
                this.ValidateOAuthTimeStamp(oauth_timestamp);
            }
            else
            {
                throw new UnauthorizedAccessException("Header doesn't contain Timestamp");
            }

            if (context.Request.Headers.Contains(HeaderKeys.Signature))
            {
                signature = context.Request.Headers.GetValues(HeaderKeys.Signature).First();
            }
            else
            {
                throw new UnauthorizedAccessException("Header doesn't contain Signature");
            }

            ClientAuth clientAuth = this.getClientAuthDetails(publicKey);

            if (clientAuth == null || String.IsNullOrWhiteSpace(clientAuth.secretkey))
            {
                throw new UnauthorizedAccessException(String.Format("Unable to locate a registered client with key - {0}", publicKey));
            }

            OAuthBase oauth = new OAuthBase();
            string hash = oauth.GenerateSignature(
                            new Uri(""),//(context.Request.RequestUri.ToString()),
                            publicKey,
                            clientAuth.secretkey,
                            "POST",
                            oauth_timestamp,
                            nonce,
                            OAuthBase.SignatureTypes.HMACSHA1,
                            out normalizedUrl,
                            out normalizedRequestParameters
                          );

            if (hash != signature)
            {
                throw new UnauthorizedAccessException(String.Format("publickey-{0}, timestamp-{1}, nonce-{2}", publicKey, oauth_timestamp, nonce));
            }
        }

        private void ValidateOAuthTimeStamp(string timestamp)
        {
            if (String.IsNullOrWhiteSpace(timestamp))
            {
                throw new UnauthorizedAccessException("Invalid Timestamp provided");
            }
            double dTimeLimit = 30;
            DateTime dtTimeStamp = DateTime.UtcNow;
            DateTime time = Convert.ToDateTime(timestamp);
            double dRequestAge = DateTime.UtcNow.Second - time.Second;

            if (dRequestAge < 0 || dRequestAge >= dTimeLimit)
            {
                throw new UnauthorizedAccessException(String.Format("Current Request signature is older than threshold. Difference in seconds - {0}", dRequestAge));
            }
        }

        private ClientAuth getClientAuthDetails(string publicKey)
        {
            ClientAuth clientAuth = new ClientAuth();
            clientAuth = JsonConvert.DeserializeObject<ClientAuth>(File.ReadAllText("ClientAuth.json"));
            return clientAuth;
        }
    }
}
