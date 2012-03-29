using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.Configuration;

namespace ActiveDirectoryForCloud.Proxy
{
    public interface IUserFinder
    {
        UserAttribute[] GetAttributes(string username);
    }

    public class UserFinder : IUserFinder
    {
        ContextType contextType;
        string server;
        string container;

        public UserFinder()
        {
            var contextTypeSetting = ConfigurationManager.AppSettings["ContextType"];
            var serverSetting = ConfigurationManager.AppSettings["Server"];
            var containerSetting = ConfigurationManager.AppSettings["Container"];

            if (contextTypeSetting == null)
            {
                this.contextType = ContextType.Machine;
            }
            else
            {
                this.contextType = (ContextType)Enum.Parse(typeof(ContextType), contextTypeSetting);
                this.server = (!string.IsNullOrEmpty(serverSetting)) ? serverSetting : null;
                this.container = (!string.IsNullOrEmpty(containerSetting)) ? containerSetting : null;
            }
        }

        public UserFinder(ContextType contextType)
            : this(contextType, null, null)
        {
        }

        public UserFinder(ContextType contextType, string server, string container)
        {
            this.contextType = contextType;
            this.server = server;
            this.container = container;
        }
        
        public UserAttribute[] GetAttributes(string username)
        {
            var attributes = new List<UserAttribute>();

            var identity = UserPrincipal.FindByIdentity(new PrincipalContext(this.contextType, this.server, this.container), IdentityType.SamAccountName, username);
            if (identity != null)
            {
                var groups = identity.GetGroups();
                
                foreach(var group in groups)
                {
                    attributes.Add(new UserAttribute { Name = "Group", Value = group.Name });
                }
                
                if(!string.IsNullOrEmpty(identity.DisplayName))
                    attributes.Add(new UserAttribute { Name = "DisplayName", Value = identity.DisplayName });
                
                if(!string.IsNullOrEmpty(identity.EmailAddress))
                    attributes.Add(new UserAttribute { Name = "EmailAddress", Value = identity.EmailAddress });
            }

            return attributes.ToArray();
        }
    }
}
