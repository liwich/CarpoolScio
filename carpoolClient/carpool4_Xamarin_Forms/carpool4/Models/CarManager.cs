using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Diagnostics;
using System;


namespace carpool4.Models
{
    public class CarManager
    {
        IMobileServiceTable<Car> carsTable;
        MobileServiceClient client;

        public CarManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            carsTable = client.GetTable<Car>();
        }

        public async Task SaveCarAsync(Car car)
        {
            if (car.Id == null)
            {
                await carsTable.InsertAsync(car);
            }
            else
            {
                await carsTable.UpdateAsync(car);
            }
        }

       
        public async Task<List<Car>> GetMyCarsAsync(User user)
        {
            try
            {
                return await carsTable.Where(cars => cars.Id_User == user.Id).ToListAsync();
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
