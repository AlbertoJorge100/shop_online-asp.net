using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shop_online.Models;
using shop_online.Models.utils;
using shop_online.Models.ViewModels;
using Newtonsoft.Json.Linq;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
//using Newtonsoft.Json;
//using System.Text.Json;

namespace shop_online.Controllers
{
    //[Route("/products/prod/")]
    public class productsController : Controller
    {
        // GET: products
        [HttpGet]
        public ActionResult Index(string msj = "")
        {
            //List<Object> lst;
            using (shopEntities db = new shopEntities())
            {
                int? a = null;                
                //var lst = db.products.Include("categories").Where(p => p.estado == 1).ToList();
                var lst = (from p in db.products
                           join c in db.categories on p.id_categoria equals c.id
                           where p.estado == 1
                           select new productsModel
                           {
                               id = p.id, 
                               name = p.name, 
                               price = (double)p.price, 
                               description = p.description,
                               id_categoria = p.id_categoria,
                               category = c
                           }).ToList();

                ViewBag.msj = msj;
                //ViewBag.products = lst;
                return View(lst);
            }            
        }

        [HttpGet]
        public ActionResult create()
        {            
            ViewBag.items = getCategories();            
            return View();
            //return RedirectToAction("Index", new { msg = "El producto fue ingresado exitosamente"});
        }

        [HttpGet]
        public ActionResult edit(int id)
        {
            productsModel prod = new productsModel();
            using(shopEntities db = new shopEntities())
            {
                products p = db.products.Find(id);
                prod.id = p.id;
                prod.name = p.name;
                prod.description = p.description;
                prod.price = (double)p.price;
                prod.id_categoria = p.id_categoria;                
            }

            ViewBag.items = getCategories();
            return View(prod);
        }

        [HttpPost]
        public ActionResult update(productsModel model)
        {
            string mensaje = "El producto fue modificado exitosamente";
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                using (shopEntities db = new shopEntities())
                {
                    var prod = db.products.Find(model.id);
                    
                    prod.name = model.name;
                    prod.price = (decimal)model.price;
                    prod.description = model.description;
                    prod.id_categoria = model.id_categoria;
                    prod.estado = 1;

                    db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
                mensaje = "Ha ocurrido una excepcion: " + e.Message;
            }
            //return View();
            return RedirectToAction("Index", new { msj = mensaje });
        }

        [HttpPost]
        public PartialViewResult find(String text)
        {
            List<productsModel> products;
            try
            {
                using (shopEntities db = new shopEntities())
                {
                    products = (from d in db.products.Where(d => d.estado == 1 && d.name.Contains(text))
                                select new productsModel
                                {
                                    id = d.id,
                                    name = d.name,
                                    price = (double)d.price,
                                    description = d.description,
                                    id_categoria = d.id_categoria
                                }                                
                            ).ToList();
                }                
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            return PartialView("~/Views/products/_partials/data.cshtml", products);
        }

        [HttpGet]
        public JsonResult getAjax()
        {
            List<productsModel> products;
            try
            {                
                using (shopEntities db = new shopEntities())
                {
                    products = (from d in db.products.Where(d => d.estado == 1)
                                select new productsModel
                                {
                                    id = d.id,
                                    name = d.name,
                                    price = (double)d.price,
                                    description = d.description,
                                    id_categoria = d.id_categoria
                                }).ToList();
                }
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            /*dynamic json = new JObject();
            json.code = 200;
            json.message = "processed correctly";
            json.products = JsonConvert.SerializeObject(products);*/

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        //Se usa "softDelete"
        [HttpPost]
        public ActionResult delete(int id)
        {
            int code = 400;
            string message = "El producto no pudo ser eliminado !";
            try
            {                
                using (shopEntities db = new shopEntities())
                {
                    var prod = db.products.Find(id);                    
                    prod.estado = 0;
                    db.Entry(prod).State = System.Data.Entity.EntityState.Modified;

                    if (db.SaveChanges() != 0)
                    {
                        code = 200;
                        message = "Producto eliminado exitosamente!";
                    }
                        
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);                
            }

            return Json(new {code = code, message = message });            
        }

        [HttpGet]
        [Route("products/getCategorie")]
        public ContentResult getCategorie(int id)
        {            
            //categories categorie = new categories();
            try
            {
                using (shopEntities db = new shopEntities())
                {
                    //categories categorie = db.categories.Where(d => d.id == id).First();
                    categories categorie = db.categories.Find(id);

                    return Content(categorie.name);                    
                }
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            return Content("categoria: " + id);
        }
        
        public List<SelectListItem> getCategories()
        {
            List<categoriesModel> categories;
            List<SelectListItem> lista;
            //categories categorie = new categories();
            try
            {
                using (shopEntities db = new shopEntities())
                {
                    //categories categorie = db.categories.Where(d => d.id == id).First();
                    categories = (from d in db.categories
                                    select new categoriesModel
                                    {
                                        id = d.id,
                                        name = d.name
                                    }).ToList();
                }

                lista = categories.ConvertAll(d =>
                {
                    return new SelectListItem()
                    {
                        Text = d.name,
                        Value = d.id.ToString(),
                        Selected = false
                    };
                });                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return lista;
        }

        [HttpPost]
        public ActionResult create(productsModel model)
        {
            string mensaje = "El producto fue ingresado exitosamente";
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.items = getCategories();
                    return View(model);
                }
                    
                using (shopEntities db = new shopEntities())
                {
                    var prod = new products();
                    prod.name = model.name;
                    prod.price = (decimal)model.price;
                    prod.description = model.description;
                    prod.id_categoria = model.id_categoria;
                    prod.estado = 1;

                    db.products.Add(prod);
                    db.SaveChanges();
                }
                                                    
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
                mensaje = "Ha ocurrido una excepcion: " + e.Message;
            }
            //return View();
            return RedirectToAction("Index", new { msj = mensaje});
        }

        [HttpGet]
        [Route("getApi/")]
        public JsonResult getApi(int _id)
        {
            List<productsModel> prod;
            using(shopEntities db = new shopEntities())
            {
                prod = (from p in db.products
                             where p.estado == 1
                             select new productsModel
                             {
                                 id = p.id,
                                 name = p.name,
                                 price = (double)p.price,
                                 description = p.description,
                                 id_categoria = p.id_categoria                         
                             }).ToList();
            }

            var json = new
                        {
                            id = _id,
                            code = 200,
                            message = "hola",
                            products = prod
                        };

            return Json(json, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public PartialViewResult modal(int id)
        {
            productsModel product;
            using (shopEntities db = new shopEntities())
            {
                product = (from p in db.products join c in db.categories on p.id_categoria equals c.id
                           where p.estado == 1 && p.id == id select new productsModel {
                               id = p.id,
                               name = p.name,
                               description = p.description,
                               id_categoria = p.id_categoria,
                               price = (double)p.price,
                               category = c
                            }).First();
            }

            ViewBag.items = getCategories();

            return PartialView("~/Views/products/_partials/modal.cshtml", product);
        }
    }    
}