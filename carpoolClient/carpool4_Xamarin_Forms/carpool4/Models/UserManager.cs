using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace carpool4.Models
{
    public class UserManager
    {
        IMobileServiceTable<User> usersTable;
        MobileServiceClient client;

        public UserManager()
        {
            this.client = new MobileServiceClient(
                Constants.ApplicationURL);

            this.usersTable = client.GetTable<User>();
        }


        public async Task<List<User>> GetUsersWhere(Expression<Func<User, bool>> linq)
        {
            try
            {
                List<User> newUser = await usersTable.Where(linq).ToListAsync();
                return newUser;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }
            return null;
        }


        public async Task<User> GetUserWhere(Expression<Func<User, bool>> linq)
        {
            try
            {
                List<User> newUser = await usersTable.Where(linq).Take(1).ToListAsync();
                return newUser.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }
            return null;
        }

        public async Task<User> SaveGetUserAsync(User user)
        {
            if (user.Id == null)
            {
                await usersTable.InsertAsync(user);
            }
            else
            {
                await usersTable.UpdateAsync(user);
            }

            try
            {
                List<User> newUser = await usersTable.Where(userSelect => userSelect.Email == user.Email).ToListAsync();
                return newUser.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

            return null;
        }
    }
}
