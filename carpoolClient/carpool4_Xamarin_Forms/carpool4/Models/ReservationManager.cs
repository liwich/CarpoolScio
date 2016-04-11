using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace carpool4.Models
{
    public class ReservationManager
    {

        IMobileServiceTable<Reservation> reservationsTable;
        MobileServiceClient client;

        public ReservationManager()
        {
            client = new MobileServiceClient(
                Constants.ApplicationURL);

            reservationsTable = client.GetTable<Reservation>();
            var a = new Reservation { Id_Route = "res1" };
            reservationsTable.InsertAsync(a);
            var aaaaaa = reservationsTable.ToListAsync();
        }

        public async Task SaveReservationAsync(Reservation reservation)
        {
            if (reservation.Id == null)
            {
                await reservationsTable.InsertAsync(reservation);
            }
            else
            {
                await reservationsTable.UpdateAsync(reservation);
            }
        }

        public async Task<List<Reservation>> GetReservationsWhere(Expression<Func<Reservation, bool>> linq)
        {
            try
            {
                return await reservationsTable.Where(linq).ToListAsync();
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

        public async Task DeleteReservationsAsync(Reservation reservation)
        {
            try
            {
                List<Reservation> reservationsList = await GetReservationsWhere(res => res.Id_Route == reservation.Id_Route);
                foreach (var res in reservationsList)
                {
                    await reservationsTable.DeleteAsync(res);
                }

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

        }

        public async Task DeleteReservationAsync(Reservation reservation)
        {
            try
            {
                List<Reservation> reservationsList = await GetReservationsWhere(res => res.Id_Route == reservation.Id_Route && res.Id_User == reservation.Id_User);
                foreach (var res in reservationsList)
                {
                    await reservationsTable.DeleteAsync(res);
                }

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

        }

    }
}
