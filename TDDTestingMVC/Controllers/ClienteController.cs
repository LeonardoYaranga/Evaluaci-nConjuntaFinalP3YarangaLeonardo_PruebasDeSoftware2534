using Microsoft.AspNetCore.Mvc;
using TDDTestingMVC.Data;
using TDDTestingMVC.Models;

namespace TDDTestingMVC.Controllers
{
    public class ClienteController : Controller
    {
        ClienteDataAccessLayer objClienteDAL = new ClienteDataAccessLayer();
        public IActionResult Index()
        {   
            List<Cliente> clientes = new List<Cliente>();
            clientes=objClienteDAL.getAllClientes().ToList();
            return View(clientes);
        }

        public int numeroEntero()
        {
            return 2000;
        }
        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                string mensaje = objClienteDAL.addCliente(cliente);

                if (mensaje == "Cliente agregado correctamente.")
                    return RedirectToAction("Index");

                ViewBag.Mensaje = mensaje;
            }

            return View(cliente);
        }


        //Update
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Cliente cliente = objClienteDAL.getClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind] Cliente cliente)
        {
            if (ModelState.IsValid)
            {

                string mensaje = objClienteDAL.updateCliente(cliente);
                if (mensaje == "Cliente editado correctamente.")
                    return RedirectToAction("Index");

                ViewBag.Mensaje = mensaje;
            }
            return View(cliente);
        }

       
        //Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Cliente cliente = objClienteDAL.getClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            objClienteDAL.deleteCliente(id);
            return RedirectToAction("Index");
        }


    }
}
