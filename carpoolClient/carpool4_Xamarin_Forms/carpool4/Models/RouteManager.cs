using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;


namespace carpool4.Models
{

    class RouteManager
    {
        private IMobileServiceTable<Route> routesTable;
        private MobileServiceClient client;

        public RouteManager()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
            routesTable = client.GetTable<Route>();
        
        }

        public async Task SaveRouteAsync(Route route)
        {
            if (route.Id == null)
            {
                await routesTable.InsertAsync(route);
            }
            else
            {
                await routesTable.UpdateAsync(route);
            }
        }

        public async Task<Route> GetRouteWhere(Expression<Func<Route, bool>> linq)
        {
            try
            {
                List<Route> routesList = await routesTable.Where(linq).Take(1).ToListAsync();
                return routesList.First();
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

        public async Task<List<Route>> ListRoutesWhere(Expression<Func<Route, bool>> linq)
        {
            try
            {
                return new List<Route>
                (
                    await routesTable.Where(linq).ToListAsync()
                );
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
