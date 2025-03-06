using Microsoft.AspNetCore.Mvc;
using TDDTestingMVC.Data;
using TDDTestingMVC.Models;

namespace TDDTestingMVC.Controllers
{
    public class PedidoController : Controller
    {
        PedidoDataAccessLayer objPedidoDAL = new PedidoDataAccessLayer();

        // Mostrar todos los pedidos
        public IActionResult Index()
        {
            List<Pedido> pedidos = new List<Pedido>();
            pedidos = objPedidoDAL.GetAllPedidos().ToList();
            return View(pedidos);
        }

        // Crear un nuevo pedido (GET)
        [HttpGet]
        public IActionResult Create()
        {
            var pedido = new Pedido
            {
                FechaPedido = DateTime.Now, // Valor por defecto
                Estado = "Pendiente"        // Valor por defecto
            };
            return View(pedido);
        }

        // Crear un nuevo pedido (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                string mensaje = objPedidoDAL.AddPedido(pedido);

                if (mensaje == "Pedido agregado correctamente.")
                    return RedirectToAction("Index");

                ViewBag.Mensaje = mensaje;
            }

            return View(pedido);
        }

        // Editar un pedido existente (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Pedido pedido = objPedidoDAL.GetPedidoById(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        // Editar un pedido existente (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                string mensaje = objPedidoDAL.UpdatePedido(pedido);
                if (mensaje == "Pedido actualizado correctamente.")
                    return RedirectToAction("Index");

                ViewBag.Mensaje = mensaje;
            }
            return View(pedido);
        }

        // Eliminar un pedido (GET)
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Pedido pedido = objPedidoDAL.GetPedidoById(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        // Eliminar un pedido (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            objPedidoDAL.DeletePedido(id);
            return RedirectToAction("Index");
        }
    }
}