using Google.Protobuf.Reflection;
using MySql.Data.MySqlClient;
using Mysqlx;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3JSON
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string coneccion = "server=localhost;Database=lab3json;User ID=root;password=mysql";
            string ruta = @"C:\Users\brizu\source\repos\Lab3JSON\escrituraJSON.json";
            List<Escritor> list = traerEscritores(coneccion);
            escribirjson( ruta,list);
        }

        private static List<Escritor> traerEscritores(string connecion)
        {
            List<Escritor> escritores = new List<Escritor>();
            using (MySqlConnection conn = new MySqlConnection(connecion))
            {
                try
                {
                    
                    conn.Open();
                    string query = "SELECT * FROM escritor ";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                        using (MySqlDataReader readerEscritores = cmd.ExecuteReader()) {
                            while (readerEscritores.Read())
                            {
                                Escritor escritor = new Escritor
                                {
                                    idEscritor = readerEscritores.GetInt32(0),
                                    apellido = readerEscritores.GetString(1),
                                    nombre = readerEscritores.GetString(2),
                                    dni = readerEscritores.GetInt64(3),
                                    libros = traerLibros(connecion,readerEscritores.GetInt32(0))

                                };
                                escritores.Add(escritor);
                            }
                        }
                    }

                }
                catch (Exception ex) {
                    Console.WriteLine("Error:" + ex);
                    return null;
                        }
            }
            return escritores;
        }

        private static List<Libro> traerLibros(string conection, int idescritor)
        {
            List<Libro> libros = new List<Libro>();
            using (MySqlConnection conn = new MySqlConnection(conection))
            {
                try
                {
                    conn.Open();
                    // Verificar si la conexión está abierta
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        Console.WriteLine("La conexión no está abierta. Intentando abrirla...");
                        conn.Open();
                    }

                    // Imprimir el valor de idEscritor
                    Console.WriteLine("Obteniendo libros para el escritor con id: " + idescritor);
                    string query = "SELECT * FROM LIBRO WHERE idEscritor = @idEscritor";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idescritor", idescritor);
                        using (MySqlDataReader readerLibros = cmd.ExecuteReader())
                        {

                            while (readerLibros.Read())
                            {
                                Libro libro = new Libro
                                {
                                    nombre = readerLibros.GetString(1),
                                    aniopublicacion = readerLibros.GetInt32(2),
                                    editorial = readerLibros.GetString(3)
                                };
                                libros.Add(libro);
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex);

                }
            }
            return libros;
        }

        private static void escribirjson( string ruta,List<Escritor> escritores)
        {
            string jsonFile = JsonConvert.SerializeObject(escritores, Formatting.Indented);
            File.WriteAllText( ruta, jsonFile);
        }
    }
}
