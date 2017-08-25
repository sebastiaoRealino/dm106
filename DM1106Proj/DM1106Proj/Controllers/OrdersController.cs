﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DM1106Proj.Models;
using System.Diagnostics;

namespace DM1106Proj.Controllers
{
    public class OrdersController : ApiController
    {
        private OrderContext db = new OrderContext();

        // GET: api/Orders
        [Authorize(Roles = "ADMIN")]
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);

            if (User.IsInRole("ADMIN") || (order.userEmail == User.Identity.Name))
            {
                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            } else {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) {
                    Content = new StringContent("Usuário não possui autorização para visualizar pedido!"),
                    ReasonPhrase = "Acesso não autorizado" 
                };
                return ResponseMessage(msg);
                
            }
            
        }
        // PATCH: api/Orders/5
        [HttpPatch]
        [ResponseType(typeof(void))]
        public IHttpActionResult PatchOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (User.IsInRole("ADMIN") || (order.userEmail == User.Identity.Name))
            {
                if (order.status != "fechado")
                {
                    if (order.freightRate == 0)
                    {
                        var msg = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("Frete ainda não foi calculado!"),
                            ReasonPhrase = "Erro no frete"
                        };
                        return ResponseMessage(msg);
                    }
                    else
                    {
                        order.status = "fechado";
                        db.SaveChanges();
                        var msg = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("Status do pedido foi alterado para: fechado"),
                            ReasonPhrase = "Status alterado com sucesso!"
                        };
                        return ResponseMessage(msg);
                    }
                }
                else
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.NotModified)
                    {
                        Content = new StringContent("Pedido já se encontra fechado"),
                        ReasonPhrase = "Pedido já fechado!"
                    };
                    return ResponseMessage(msg);
                }
            }
            else
            {
                return Unauthorized();
            }

            
        }
        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (User.IsInRole("ADMIN") || User.IsInRole("USER"))
            {
                order.status = "novo";
                order.userEmail = User.Identity.Name;
                order.totalWeight = 0;
                order.freightRate = 0;
                order.totalPrice = 0;
                order.creationDate = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Orders.Add(order);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
            }
            else
            {
                return Unauthorized();
            }
            
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (User.IsInRole("ADMIN") || (order.userEmail == User.Identity.Name))
            {
                if (order == null)
                {
                    return NotFound();
                }

                db.Orders.Remove(order);
                db.SaveChanges();

                return Ok(order);
            }
            else
            {
                return Unauthorized();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}