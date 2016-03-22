using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;

namespace ActiveDirectoryExample
{
    /// <summary>
    /// Active Directory proof of concept console application.
    /// Checks and prints if current user is in a specific Active Directory group.
    /// Gets a List of users from a specific Active Directory group and prints them.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            var desiredUsers = new List<UserPrincipal>();
            // Replace this string with the Active Directory Group to test for.
            const string targetActiveDirectoryGroup = "TargetActiveDirectoryGroup";

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (identity != null)
            {
                var windowsPrincipal = new WindowsPrincipal(identity);                
                bool isUserInRole = windowsPrincipal.IsInRole(targetActiveDirectoryGroup);
                Console.WriteLine(isUserInRole);
            }

            using (var context = new PrincipalContext(ContextType.Domain))
            {
				using (var group = GroupPrincipal.FindByIdentity(context, targetActiveDirectoryGroup))
				{
					if (group != null)
					{
					    var users = group.GetMembers(true);
					    foreach (var user in users.Cast<UserPrincipal>())
					    {
					        desiredUsers.Add(user);
					        Console.WriteLine(user.Name);
					        Console.WriteLine(user.SamAccountName);
					    }
					}
				}
            }

            Console.ReadLine();
        }
    }
}
