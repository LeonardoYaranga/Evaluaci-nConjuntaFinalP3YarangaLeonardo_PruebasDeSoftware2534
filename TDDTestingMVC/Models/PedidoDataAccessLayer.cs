using System.Data;
using System.Data.SqlClient;
using TDDTestingMVC.Data;

namespace TDDTestingMVC.Models
{
    public class PedidoDataAccessLayer
    {
        // Cadena de conexión (ajustada como en ClienteDataAccessLayer)
        string connectionString = "Server=LEOTRIKIS; database=Productos; User ID=sa; Password=leo123; TrustServerCertificate=True; MultipleActiveResultSets=True";

        // Obtener todos los pedidos
        public virtual List<Pedido> GetAllPedidos()
        {
            List<Pedido> lstPedido = new List<Pedido>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("pedido_SelectAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Pedido pedido = new Pedido
                    {
                        PedidoID = Convert.ToInt32(rdr["PedidoID"]),
                        ClienteID = Convert.ToInt32(rdr["ClienteID"]),
                        FechaPedido = Convert.ToDateTime(rdr["FechaPedido"]),
                        Monto = Convert.ToDecimal(rdr["Monto"]),
                        Estado = rdr["Estado"].ToString()
                    };
                    lstPedido.Add(pedido);
                }
                con.Close();
            }
            return lstPedido;
        }

        // Agregar un nuevo pedido
        public virtual string AddPedido(Pedido pedido)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("pedido_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClienteID", pedido.ClienteID);
                    cmd.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
                    cmd.Parameters.AddWithValue("@Monto", pedido.Monto);
                    cmd.Parameters.AddWithValue("@Estado", pedido.Estado);

                    // Parámetro de salida para recibir el resultado del SP
                    SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Int);
                    resultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultado);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // Evaluar el resultado
                    switch ((int)resultado.Value)
                    {
                        case 1:
                            return "Pedido agregado correctamente.";
                        case -1:
                            return "Error: El cliente no existe.";
                        default:
                            return "Error desconocido.";
                    }
                }
            }
            catch (SqlException ex)
            {
                return "Error en la base de datos: " + ex.Message;
            }
        }

        // Actualizar un pedido existente
        public virtual string UpdatePedido(Pedido pedido)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("pedido_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PedidoID", pedido.PedidoID);
                    cmd.Parameters.AddWithValue("@ClienteID", pedido.ClienteID);
                    cmd.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
                    cmd.Parameters.AddWithValue("@Monto", pedido.Monto);
                    cmd.Parameters.AddWithValue("@Estado", pedido.Estado);

                    // Parámetro de salida para recibir el resultado del SP
                    SqlParameter resultado = new SqlParameter("@Resultado", SqlDbType.Int);
                    resultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultado);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // Evaluar el resultado
                    switch ((int)resultado.Value)
                    {
                        case 1:
                            return "Pedido actualizado correctamente.";
                        case 0:
                            return "Error: El pedido no fue encontrado.";
                        case -1:
                            return "Error: El cliente no existe.";
                        default:
                            return "Error desconocido.";
                    }
                }
            }
            catch (SqlException ex)
            {
                return "Error en la base de datos: " + ex.Message;
            }
        }

        // Obtener un pedido por ID
        public virtual Pedido GetPedidoById(int pedidoId)
        {
            Pedido pedido = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("get_pedido_by_id", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoID", pedidoId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    pedido = new Pedido
                    {
                        PedidoID = Convert.ToInt32(rdr["PedidoID"]),
                        ClienteID = Convert.ToInt32(rdr["ClienteID"]),
                        FechaPedido = Convert.ToDateTime(rdr["FechaPedido"]),
                        Monto = Convert.ToDecimal(rdr["Monto"]),
                        Estado = rdr["Estado"].ToString()
                    };
                }
                rdr.Close();
                con.Close();
            }
            return pedido;
        }

        // Eliminar un pedido
        public virtual void DeletePedido(int pedidoId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("pedido_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoID", pedidoId);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}