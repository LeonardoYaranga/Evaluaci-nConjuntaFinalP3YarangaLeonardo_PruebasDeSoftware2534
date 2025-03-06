using System.Data;
using System.Data.SqlClient;
using TDDTestingMVC.Data;

namespace TDDTestingMVC.Models
{
    public class ClienteDataAccessLayer
    {
        //ESTO NO HACER
        //string connectionString = "Data Source= LEOTRIKIS; Initial Catalog=Productos, User ID=sa;Password=leo123";
        string connectionString = "Server=LEOTRIKIS; database=Productos; User ID=sa; Password=leo123; TrustServerCertificate=True; MultipleActiveResultSets=True";
        public virtual List<Cliente> getAllClientes()
        {
            List<Cliente> lstCliente = new List<Cliente>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("cliente_SelectAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.Codigo = Convert.ToInt32(rdr["Codigo"]);
                    cliente.Cedula = rdr["Cedula"].ToString();
                    cliente.Nombres = rdr["Nombres"].ToString();
                    cliente.Apellidos = rdr["Apellidos"].ToString();
                    cliente.FechaNacimiento = Convert.ToDateTime(rdr["FechaNacimiento"]);
                    cliente.Mail = rdr["Mail"].ToString();
                    cliente.Telefono = rdr["Telefono"].ToString();
                    cliente.Direccion = rdr["Direccion"].ToString();
                    cliente.Estado = Convert.ToBoolean(rdr["Estado"]);
                    lstCliente.Add(cliente);
                }
                con.Close();
            }
            return lstCliente;

        }
        //Validar cedula ecu
        public static bool ValidarCedula(string cedula)
        {
            int total = 0;
            int longitud = cedula.Length;
            int longcheck = longitud - 1;

            // Verifica que la cédula tenga 10 caracteres
            if (!string.IsNullOrEmpty(cedula) && longitud == 10)
            {
                for (int i = 0; i < longcheck; i++)
                {
                    if (i % 2 == 0)
                    {
                        int aux = int.Parse(cedula[i].ToString()) * 2;
                        if (aux > 9) aux -= 9;
                        total += aux;
                    }
                    else
                    {
                        total += int.Parse(cedula[i].ToString());
                    }
                }

                total = total % 10 != 0 ? 10 - total % 10 : 0;

                // Verifica si el dígito final de la cédula coincide con el resultado
                if (cedula[longitud - 1] == total.ToString()[0])
                {
                    return true; // Cédula válida
                }
                else
                {
                    return false; // Cédula inválida
                }
            }
            else
            {
                return false; // Cédula inválida
            }
        }


        //addCliente
        public virtual string addCliente(Cliente cliente)
        {
            try
            {
                // Validar la cédula antes de continuar
                if (!ValidarCedula(cliente.Cedula))
                {
                    return "Error: La Cedula no es valida.";
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("cliente_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Cedula", cliente.Cedula);
                    cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("@Nombres", cliente.Nombres);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Mail", cliente.Mail);
                    cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                    cmd.Parameters.AddWithValue("@Estado", cliente.Estado);

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
                            return "Cliente agregado correctamente.";
                        case -1:
                            return "Error: La Cedula ya esta registrada.";
                        case -2:
                            return "Error: El Correo ya esta registrado.";
                        case -3:
                            return "Error: El Telefono ya esta registrado.";
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

        //updateCliente
        public virtual string updateCliente(Cliente cliente)
        {
            try
            {
                // Validar la cédula antes de continuar
                if (!ValidarCedula(cliente.Cedula))
                {
                    return "Error: La Cedula no es valida.";
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("cliente_Update", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Codigo", cliente.Codigo);
                    cmd.Parameters.AddWithValue("@Cedula", cliente.Cedula);
                    cmd.Parameters.AddWithValue("@Nombres", cliente.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Mail", cliente.Mail);
                    cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                    cmd.Parameters.AddWithValue("@Estado", cliente.Estado);

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
                                return "Cliente editado correctamente.";
                            case -1:
                                return "Error: La Cedula ya esta registrada.";
                            case -2:
                                return "Error: El Correo ya esta registrado.";
                            case -3:
                                return "Error: El Telefono ya esta registrado.";
                            default:
                                return "Error desconocido.";
                        }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en la base de datos: " + ex.Message);
            }
        }


        public virtual Cliente getClienteById(int codigo)
        {
            Cliente cliente = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("get_cliente_by_id", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo", codigo);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cliente = new Cliente
                    {
                        Codigo = Convert.ToInt32(reader["Codigo"]),
                        Cedula = reader["Cedula"].ToString(),
                        Nombres = reader["Nombres"].ToString(),
                        Apellidos = reader["Apellidos"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                        Mail = reader["Mail"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        Estado = Convert.ToBoolean(reader["Estado"])
                    };
                }
                reader.Close();
                con.Close();
            }
            return cliente;
        }

        public virtual void deleteCliente(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("cliente_Delete", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }


    }
}
