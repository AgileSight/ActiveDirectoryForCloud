using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActiveDirectoryForCloud.Proxy;
using System.ServiceModel;

namespace CloudSecurityTokenService.Infrastructure
{
    public interface IAuthenticationClient
    {
        AuthenticationResponse Authenticate(AuthenticationRequest request);
    }
    
    public class AuthenticationClient : IAuthenticationClient
    {
        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            ChannelFactory<IAuthenticationService> channelFactory = null;
            AuthenticationResponse response = null;
            try
            {
                channelFactory = new ChannelFactory<IAuthenticationService>("authentication");
                channelFactory.Open();

                var service = channelFactory.CreateChannel();

                response = service.Authenticate(request);

                channelFactory.Close();
            }
            catch (CommunicationException)
            {
                if (channelFactory != null)
                {
                    channelFactory.Abort();
                }
            }
            catch (TimeoutException)
            {
                if (channelFactory != null)
                {
                    channelFactory.Abort();
                }
            }
            catch (Exception)
            {
                if (channelFactory != null)
                {
                    channelFactory.Abort();
                }
                throw;
            }

            return response;
        }
    }
}