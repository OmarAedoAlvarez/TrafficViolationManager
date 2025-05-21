using TrafficViolationManager.DBManager;
using TrafficViolationManager.Domain;
using TrafficViolationManager.Persistence.DAO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TrafficViolationManager.Persistence.Impl
{
    public class RegistroInfraccionImpl : RegistroInfraccionDAO
    {
        private MySqlConnection con;
        private MySqlCommand com;

        public int Insertar(RegistroInfraccion registro)
        {
            MySqlParameter[] parametros = new MySqlParameter[4];

            parametros[0] = new MySqlParameter("_FECHA", registro.Fecha);
            parametros[1] = new MySqlParameter("_VEHICULO_ID", registro.VehiculoId);
            parametros[2] = new MySqlParameter("_CONDUCTOR_ID", registro.ConductorId);
            parametros[3] = new MySqlParameter("_INFRACCION_ID", registro.InfraccionId);

            return TrafficDBManager.getInstance().EjecutarProcedimiento("INSERTAR_REGISTRO_INFRACCION", parametros);
        }

        public int Modificar(RegistroInfraccion entidad)
        {
            throw new NotImplementedException("No implementado para clave compuesta");
        }

        public int Eliminar(int id)
        {
            throw new NotImplementedException("No implementado para clave compuesta");
        }

        public RegistroInfraccion ObtenerPorId(int id)
        {
            throw new NotImplementedException("No implementado para clave compuesta");
        }

        public List<RegistroInfraccion> ObtenerTodos()
        {
            var lista = new List<RegistroInfraccion>();

            con = TrafficDBManager.getInstance().Connection;
            com = con.CreateCommand();
            com.CommandText = "LISTAR_REGISTRO_INFRACCIONES";
            com.CommandType = CommandType.StoredProcedure;

            using (MySqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    var registro = new RegistroInfraccion
                    {
                        Fecha = reader.GetDateTime("FECHA"),
                        VehiculoId = reader.GetInt32("VEHICULO_ID"),
                        ConductorId = reader.GetInt32("CONDUCTOR_ID"),
                        InfraccionId = reader.GetInt32("INFRACCION_ID")
                    };
                    lista.Add(registro);
                }
            }

            TrafficDBManager.getInstance().CerrarConexion();
            return lista;
        }
    }
}
