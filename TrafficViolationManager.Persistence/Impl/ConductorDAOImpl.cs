using TrafficViolationManager.DBManager;
using TrafficViolationManager.Domain;
using TrafficViolationManager.Persistence.DAO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TrafficViolationManager.Persistence.Impl
{
    public class ConductorImpl : ConductorDAO
    {
        private MySqlConnection con;
        private MySqlCommand com;

        public int Insertar(Conductor conductor)
        {
            MySqlParameter[] parametros = new MySqlParameter[6];

            parametros[0] = new MySqlParameter("_CONDUCTOR_ID", MySqlDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            parametros[1] = new MySqlParameter("_PATERNO", conductor.Paterno);
            parametros[2] = new MySqlParameter("_MATERNO", conductor.Materno ?? (object)DBNull.Value);
            parametros[3] = new MySqlParameter("_NOMBRES", conductor.Nombres);
            parametros[4] = new MySqlParameter("_NUM_LICENCIA", conductor.NumLicencia);
            parametros[5] = new MySqlParameter("_TIPO_LICENCIA_ID", conductor.TipoLicenciaId);

            TrafficDBManager.getInstance().EjecutarProcedimiento("INSERTAR_CONDUCTOR", parametros);

            conductor.ConductorId = Convert.ToInt32(parametros[0].Value);
            return conductor.ConductorId;
        }

        public int Modificar(Conductor conductor)
        {
            MySqlParameter[] parametros = new MySqlParameter[6];

            parametros[0] = new MySqlParameter("_CONDUCTOR_ID", conductor.ConductorId);
            parametros[1] = new MySqlParameter("_PATERNO", conductor.Paterno);
            parametros[2] = new MySqlParameter("_MATERNO", conductor.Materno ?? (object)DBNull.Value);
            parametros[3] = new MySqlParameter("_NOMBRES", conductor.Nombres);
            parametros[4] = new MySqlParameter("_NUM_LICENCIA", conductor.NumLicencia);
            parametros[5] = new MySqlParameter("_TIPO_LICENCIA_ID", conductor.TipoLicenciaId);

            return TrafficDBManager.getInstance().EjecutarProcedimiento("MODIFICAR_CONDUCTOR", parametros);
        }

        public int Eliminar(int id)
        {
            MySqlParameter[] parametros = new MySqlParameter[1];
            parametros[0] = new MySqlParameter("_CONDUCTOR_ID", id);

            return TrafficDBManager.getInstance().EjecutarProcedimiento("ELIMINAR_CONDUCTOR", parametros);
        }

        public Conductor ObtenerPorId(int id)
        {
            Conductor conductor = null;

            con = TrafficDBManager.getInstance().Connection;
            com = con.CreateCommand();
            com.CommandText = "OBTENER_CONDUCTOR_POR_ID";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("_CONDUCTOR_ID", id);

            using (MySqlDataReader reader = com.ExecuteReader())
            {
                if (reader.Read())
                {
                    conductor = new Conductor
                    {
                        ConductorId = reader.GetInt32("CONDUCTOR_ID"),
                        Paterno = reader.GetString("PATERNO"),
                        Materno = reader.IsDBNull(reader.GetOrdinal("MATERNO")) ? null : reader.GetString("MATERNO"),
                        Nombres = reader.GetString("NOMBRES"),
                        NumLicencia = reader.GetString("NUM_LICENCIA"),
                        TipoLicenciaId = reader.GetInt32("TIPO_LICENCIA_ID"),
                        PuntosAcumulados = reader.GetInt32("PUNTOS_ACUMULADOS")
                    };
                }
            }

            TrafficDBManager.getInstance().CerrarConexion();
            return conductor;
        }

        public List<Conductor> ObtenerTodos()
        {
            var lista = new List<Conductor>();

            con = TrafficDBManager.getInstance().Connection;
            com = con.CreateCommand();
            com.CommandText = "LISTAR_CONDUCTORES";
            com.CommandType = CommandType.StoredProcedure;

            using (MySqlDataReader reader = com.ExecuteReader())
            {
                while (reader.Read())
                {
                    var conductor = new Conductor
                    {
                        ConductorId = reader.GetInt32("CONDUCTOR_ID"),
                        Paterno = reader.GetString("PATERNO"),
                        Materno = reader.IsDBNull(reader.GetOrdinal("MATERNO")) ? null : reader.GetString("MATERNO"),
                        Nombres = reader.GetString("NOMBRES"),
                        NumLicencia = reader.GetString("NUM_LICENCIA"),
                        TipoLicenciaId = reader.GetInt32("TIPO_LICENCIA_ID"),
                        PuntosAcumulados = reader.GetInt32("PUNTOS_ACUMULADOS")
                    };
                    lista.Add(conductor);
                }
            }

            TrafficDBManager.getInstance().CerrarConexion();
            return lista;
        }
    }
}
