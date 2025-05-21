using TrafficViolationManager.DBManager;
using TrafficViolationManager.Domain;
using TrafficViolationManager.Persistence.DAO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TrafficViolationManager.Persistence.Impl
{
    public class VehiculoImpl : VehiculoDAO
    {
        private MySqlConnection con;
        private MySqlCommand com;

        public int Insertar(Vehiculo vehiculo)
        {
            MySqlParameter[] parametros = new MySqlParameter[5];

            parametros[0] = new MySqlParameter("_VEHICULO_ID", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            parametros[1] = new MySqlParameter("_PLACA", vehiculo.Placa);
            parametros[2] = new MySqlParameter("_MARCA", vehiculo.Marca);
            parametros[3] = new MySqlParameter("_MODELO", vehiculo.Modelo);
            parametros[4] = new MySqlParameter("_ANHO", vehiculo.Anho);

            TrafficDBManager.getInstance().EjecutarProcedimiento("INSERTAR_VEHICULO", parametros);

            vehiculo.VehiculoId = Convert.ToInt32(parametros[0].Value);
            return vehiculo.VehiculoId;
        }

        public int Modificar(Vehiculo vehiculo)
        {
            MySqlParameter[] parametros = new MySqlParameter[5];

            parametros[0] = new MySqlParameter("_VEHICULO_ID", vehiculo.VehiculoId);
            parametros[1] = new MySqlParameter("_PLACA", vehiculo.Placa);
            parametros[2] = new MySqlParameter("_MARCA", vehiculo.Marca);
            parametros[3] = new MySqlParameter("_MODELO", vehiculo.Modelo);
            parametros[4] = new MySqlParameter("_ANHO", vehiculo.Anho);

            return TrafficDBManager.getInstance().EjecutarProcedimiento("MODIFICAR_VEHICULO", parametros);
        }

        public int Eliminar(int id)
        {
            MySqlParameter[] parametros = new MySqlParameter[1];
            parametros[0] = new MySqlParameter("_VEHICULO_ID", id);

            return TrafficDBManager.getInstance().EjecutarProcedimiento("ELIMINAR_VEHICULO", parametros);
        }

        public Vehiculo ObtenerPorId(int id)
        {
            Vehiculo vehiculo = null;

            con = TrafficDBManager.getInstance().Connection;
            com = con.CreateCommand();
            com.CommandText = "OBTENER_VEHICULO_POR_ID";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("_VEHICULO_ID", id);

            using (MySqlDataReader reader = com.ExecuteReader())
            {
                if (reader.Read())
                {
                    vehiculo = new Vehiculo
                    {
                        VehiculoId = reader.GetInt32("VEHICULO_ID"),
                        Placa = reader.GetString("PLACA"),
                        Marca = reader.GetString("MARCA"),
                        Modelo = reader.GetString("MODELO"),
                        Anho = reader.GetInt32("ANHO")
                    };
                }
            }

            TrafficDBManager.getInstance().CerrarConexion();
            return vehiculo;
        }

        public List<Vehiculo> ObtenerTodos()
        {
            var lista = new List<Vehiculo>();

            con = TrafficDBManager.getInstance().Connection;
            com = con.CreateCommand();
            com.CommandText = "LISTAR_VEHICULOS";
            com.CommandType = CommandType.StoredProcedure;

            using (MySqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    var vehiculo = new Vehiculo
                    {
                        VehiculoId = reader.GetInt32("VEHICULO_ID"),
                        Placa = reader.GetString("PLACA"),
                        Marca = reader.GetString("MARCA"),
                        Modelo = reader.GetString("MODELO"),
                        Anho = reader.GetInt32("ANHO")
                    };
                    lista.Add(vehiculo);
                }
            }

            TrafficDBManager.getInstance().CerrarConexion();
            return lista;
        }
    }
}
