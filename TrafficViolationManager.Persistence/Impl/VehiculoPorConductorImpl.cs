using TrafficViolationManager.DBManager;
using TrafficViolationManager.Domain;
using TrafficViolationManager.Persistence.DAO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TrafficViolationManager.Persistence.Impl
{
    public class VehiculoPorConductorImpl : VehiculoPorConductorDAO
    {
        private MySqlConnection con;
        private MySqlCommand com;

        public int Insertar(VehiculoPorConductor vpc)
        {
            MySqlParameter[] parametros = new MySqlParameter[3];

            parametros[0] = new MySqlParameter("_VEHICULO_ID", vpc.VehiculoId);
            parametros[1] = new MySqlParameter("_CONDUCTOR_ID", vpc.ConductorId);
            parametros[2] = new MySqlParameter("_FECHA_ADQUISICION", vpc.FechaAdquisicion);

            return TrafficDBManager.getInstance().EjecutarProcedimiento("INSERTAR_VEHICULO_POR_CONDUCTOR", parametros);
        }

        public int Modificar(VehiculoPorConductor entidad)
        {
            throw new NotImplementedException("No implementado para clave compuesta");
        }

        public int Eliminar(int id)
        {
            throw new NotImplementedException("No implementado para clave compuesta");
        }

        public VehiculoPorConductor ObtenerPorId(int id)
        {
            throw new NotImplementedException("No implementado para clave compuesta");
        }

        public List<VehiculoPorConductor> ObtenerTodos()
        {
            var lista = new List<VehiculoPorConductor>();

            con = TrafficDBManager.getInstance().Connection;
            com = con.CreateCommand();
            com.CommandText = "LISTAR_VEHICULOS_POR_CONDUCTOR";
            com.CommandType = CommandType.StoredProcedure;

            using (MySqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    var vpc = new VehiculoPorConductor
                    {
                        VehiculoId = reader.GetInt32("VEHICULO_ID"),
                        ConductorId = reader.GetInt32("CONDUCTOR_ID"),
                        FechaAdquisicion = reader.GetDateTime("FECHA_ADQUISICION")
                    };
                    lista.Add(vpc);
                }
            }

            TrafficDBManager.getInstance().CerrarConexion();
            return lista;
        }
    }
}
