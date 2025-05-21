using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TrafficViolationManager.DBManager
{
    public class TrafficDBManager
    {
        private static TrafficDBManager dbManager;
        private string url;
        private string nombreArchivo = "properties.txt";
        private MySqlConnection con;
        private MySqlCommand com;

        private TrafficDBManager()
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreArchivo);
            if (File.Exists(ruta))
            {
                var config = new System.Collections.Generic.Dictionary<string, string>();

                foreach (string line in File.ReadLines(ruta))
                {
                    if (!string.IsNullOrWhiteSpace(line) && line.Contains("="))
                    {
                        var partes = line.Split('=');
                        if (partes.Length == 2)
                            config[partes[0].Trim()] = partes[1].Trim();
                    }
                }

                url = $"server={config["server"]};port={config["port"]};database={config["database"]};user={config["user"]};password={config["password"]};";
            }
            else
            {
                throw new FileNotFoundException("No se encontró el archivo de configuración properties.txt");
            }

            con = new MySqlConnection(url);
        }

        public static TrafficDBManager getInstance()
        {
            if (dbManager == null)
                dbManager = new TrafficDBManager();
            return dbManager;
        }
        public string Url => url;

        public MySqlConnection Connection
        {
            get
            {
                AbrirConexion();
                return con;
            }
        }

        private void AbrirConexion()
        {
            if (con.State != ConnectionState.Open)
                con.Open();
        }

        public void CerrarConexion()
        {
            if (con.State != ConnectionState.Closed)
                con.Close();
        }

        public int EjecutarProcedimiento(string nombreProcedimiento, MySqlParameter[] parametros)
        {
            int resultado = 0;
            try
            {
                AbrirConexion();
                com = con.CreateCommand();
                com.CommandText = nombreProcedimiento;
                com.CommandType = CommandType.StoredProcedure;

                if (parametros != null)
                    com.Parameters.AddRange(parametros);

                resultado = com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el procedimiento: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }

            return resultado;
        }

        public bool ProbarConexion()
        {
            try
            {
                con.Open();
                Console.WriteLine("Conexión exitosa a la base de datos.");
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error de conexión: " + ex.Message);
                return false;
            }
        }
    }
}
