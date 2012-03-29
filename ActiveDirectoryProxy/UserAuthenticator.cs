using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace ActiveDirectoryForCloud.Proxy
{
    public interface IUserAuthenticator
    {
        bool Authenticate(string username, string password);
    }

    public class UserAuthenticator : IUserAuthenticator
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(
            string lpszUsername,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            out IntPtr phToken
        );

        [DllImport("kernel32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public bool Authenticate(string username, string password)
        {
            var domain = "";
                        
            if(username.Contains("\\"))
            {
                var parts = username.Split('\\');
                domain = parts[0];
                username = parts[1];
            }
            
            IntPtr token;
            if(LogonUser(username, domain, password, 3, 0, out token))
            {
                CloseHandle(token);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
