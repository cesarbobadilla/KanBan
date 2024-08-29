using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace MACRO.Servicio
{
    public class Impersonator : IDisposable
    {
        private WindowsImpersonationContext impersonationContext = null;
        public WindowsIdentity tempWindowsIdentity { get; set; }
        public IntPtr token = IntPtr.Zero;

        public Impersonator(string userName, string domainName, string password)
        {
            ImpersonateValidUser(userName, domainName, password);
        }

        public Impersonator(IntPtr Token)
        {
            ImpersonateValidUser(Token);
        }

        public void Dispose()
        {
            UndoImpersonation();
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LogonUser(
            string lpszUserName,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        private void ImpersonateValidUser(string userName, string domain, string password)
        {
            tempWindowsIdentity = null;
            try
            {
                if (RevertToSelf())
                {
                    if (LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(token);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }
            }
        }

        private void ImpersonateValidUser(IntPtr token)
        {
            tempWindowsIdentity = null;
            try
            {
                tempWindowsIdentity = new WindowsIdentity(token);
                impersonationContext = tempWindowsIdentity.Impersonate();
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }
            }
        }

        private void UndoImpersonation()
        {
            if (impersonationContext != null)
            {
                impersonationContext.Undo();
            }
        }
    }
}
