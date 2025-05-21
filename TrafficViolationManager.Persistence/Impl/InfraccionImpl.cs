using TrafficViolationManager.DBManager;
using TrafficViolationManager.Domain;
using TrafficViolationManager.Persistence.DAO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TrafficViolationManager.Persistence.Impl
{
    public class InfraccionImpl : InfraccionDAO
    {
        private MySqlConnection con;
        private MySqlCommand com;

        public int Insertar(Infraccion infraccion)
        {
            MySqlParameter[] parametros = new MySqlParameter[5];

            parametros[0] = new MySqlParameter("_INFRACCION_ID", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            parametros[1] = new MySqlParameter("_DESCRIPCION", infraccion.Descripcion);
            parametros[2] = new MySqlParameter("_MONTO_MULTA", infraccion.MontoMulta);
            parametros[3] = new MySqlParameter("_GRAVEDAD", infraccion.Gravedad.ToString());
            parametros[4] = new MySqlParameter("_PUNTOS", infraccion.Puntos);

            TrafficDBManager.getInstance().EjecutarProcedimiento("INSERTAR_INFRACCION", parametros);

            infraccion.InfraccionId = Convert.ToInt32(parametros[0].Value);
            return infraccion.InfraccionId;
        }

        public int Modificar(Infraccion infraccion)
        {
            MySqlParameter[] parametros = new MySqlParameter[5];

            parametros[0] = new MySqlParameter("_INFRACCION_ID", infraccion.InfraccionId);
            parametros[1] = new MySqlParameter("_DESCRIPCION", infraccion.Descripcion);
            parametros[2] = new MySqlParameter("_MONTO_MULTA", infraccion.MontoMulta);
            parametros[3] = new MySqlParameter("_GRAVEDAD", infraccion.Gravedad.ToString());
            parametros[4] = new MySqlParameter("_PUNTOS", infraccion.Puntos);

            return TrafficDBManager.getInstance().EjecutarProcedimiento("MODIFICAR_INFRACCION", parametros);
        }

        public int Eliminar(int id)
        {
            MySqlParameter[] parametros = new MySqlParameter[1];
            parametros[0] = new MySqlParameter("_INFRACCION_ID", id);

            return TrafficDBManager.getInstance().EjecutarProcedimiento("ELIMINAR_INFRACCION", parametros);
        }

        public Infraccion ObtenerPorId(int id)
        {
            Infraccion infraccion = null;

            con = TrafficDBManager.getInstance().Connection;
            com = con.CreateCommand();
            com.CommandText = "OBTENER_INFRACCION_POR_ID";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("_INFRACCION_ID", id);

            using (MySqlDataReader reader = com.ExecuteReader())
            {
                if (reader.Read())
                {
                    infraccion = new Infraccion
                    {
                        InfraccionId = reader.GetInt32("INFRACCION_ID"),
                        Descripcion = reader.GetString("DESCRIPCION"),
                        MontoMulta = reader.GetDecimal("MONTO_MULTA"),
                        Gravedad = Enum.Parse<GravedadInfraccion>(reader.GetString("GRAVEDAD")),
                        Puntos = reader.GetInt32("PUNTOS")
                    };
                }
            }

            TrafficDBManager.getInstance().CerrarConexion();
            return infraccion;
        }

        public List<Infraccion> ObtenerTodos()
        {
            var lista = new List<Infraccion>();

            con = TrafficDBManager.getInstance().Connection;
            com = con.CreateCommand();
            com.CommandText = "LISTAR_INFRACCIONES";
            com.CommandType = CommandType.StoredProcedure;

            using (MySqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    var infraccion = new Infraccion
                    {
                        InfraccionId = reader.GetInt32("INFRACCION_ID"),
                        Descripcion = reader.GetString("DESCRIPCION"),
                        MontoMulta = reader.GetDecimal("MONTO_MULTA"),
                        Gravedad = (GravedadInfraccion)Enum.Parse(typeof(GravedadInfraccion), reader.GetString("GRAVEDAD")),
                        Puntos = reader.GetInt32("PUNTOS")
                    };
                    lista.Add(infraccion);
                }
            }

            TrafficDBManager.getInstance().CerrarConexion();
            return lista;
        }
    }
}
