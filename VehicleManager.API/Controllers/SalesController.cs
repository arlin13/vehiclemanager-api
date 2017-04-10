using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VehicleManager.API.Data;
using VehicleManager.API.Models;

namespace VehicleManager.API.Controllers
{
    public class SalesController : ApiController
    {
        private VehicleManagerDataContext db = new VehicleManagerDataContext();

        // GET: api/Sales
        public IHttpActionResult GetSales()
        {
            var resultSet = db.Sales.Select(sale => new
            {
                sale.SaleId,
                sale.CustomerId,
                sale.VehicleId,
                sale.SalePrice,
                sale.InvoiceDate,
                sale.PaymentReceivedDate,
                VehicleName = sale.Vehicle.Year + " " + sale.Vehicle.Make + " " + sale.Vehicle.Model,
                CustomerName = sale.Customer.FirstName + " " + sale.Customer.LastName
            });
            return Ok(resultSet);
        }

        // GET: api/Sales/5
        [ResponseType(typeof(Sale))]
        public IHttpActionResult GetSale(int id)
        {
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                SaleId = sale.SaleId,
                CustomerId = sale.CustomerId,
                VehicleId = sale.VehicleId,
                SalePrice = sale.SalePrice,
                InvoiceDate = sale.InvoiceDate,
                PaymentReceivedDate = sale.PaymentReceivedDate
            });
        }

        // PUT: api/Sales/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSale(int id, Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sale.SaleId)
            {
                return BadRequest();
            }

            // Grab the sale from the database
            var dbSale = db.Sales.Find(id);

            // Manually update each property
            dbSale.CustomerId = sale.CustomerId;
            dbSale.VehicleId = sale.VehicleId;
            dbSale.InvoiceDate = sale.InvoiceDate;
            dbSale.PaymentReceivedDate = sale.PaymentReceivedDate;

            db.Entry(dbSale).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sales
        [ResponseType(typeof(Sale))]
        public IHttpActionResult PostSale(Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sales.Add(sale);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sale.SaleId }, new
            {
                sale.SaleId,
                sale.CustomerId,
                sale.VehicleId,
                sale.SalePrice,
                sale.InvoiceDate,
                sale.PaymentReceivedDate
                //VehicleName = sale.Vehicle.Year + " " + sale.Vehicle.Make + " " + sale.Vehicle.Model,
                //CustomerName = sale.Customer.FirstName + " " + sale.Customer.LastName
            });
        }

        // DELETE: api/Sales/5
        [ResponseType(typeof(Sale))]
        public IHttpActionResult DeleteSale(int id)
        {
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return NotFound();
            }

            db.Sales.Remove(sale);
            db.SaveChanges();

            return Ok(new
            {
                sale.SaleId,
                sale.CustomerId,
                sale.VehicleId,
                sale.SalePrice,
                sale.InvoiceDate,
                sale.PaymentReceivedDate,
                VehicleName = sale.Vehicle.Year + " " + sale.Vehicle.Make + " " + sale.Vehicle.Model,
                CustomerName = sale.Customer.FirstName + " " + sale.Customer.LastName
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SaleExists(int id)
        {
            return db.Sales.Count(e => e.SaleId == id) > 0;
        }
    }
}