﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using carpool4Service.DataObjects;
using carpool4Service.Models;

namespace carpool4Service.Controllers
{
    public class ReservationController : TableController<Reservation>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            carpool4Context context = new carpool4Context();
            DomainManager = new EntityDomainManager<Reservation>(context, Request);
        }

        // GET tables/Reservation
        public IQueryable<Reservation> GetAllReservations()
        {
            return Query();
        }

        // GET tables/Reservation/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Reservation> GetReservation(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Reservation/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Reservation> PatchReservation(string id, Delta<Reservation> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Reservation
        public async Task<IHttpActionResult> PostReservation(Reservation item)
        {
            Reservation current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Reservation/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteReservation(string id)
        {
            return DeleteAsync(id); 
        }
    }
}