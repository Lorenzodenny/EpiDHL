using EpiDHL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace EpiDHL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            List<Cliente> clienti = new List<Cliente>();
            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti", Db.conn);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while ( reader.Read())
                    {
                        var cliente = new Cliente()
                        {
                            Cliente_ID = Convert.ToInt32(reader["Cliente_ID"]),
                            Azienda = (bool)reader["Azienda"],
                            Cod_Fisc = reader["Cod_Fisc"].ToString(),
                            PI = reader["PI"].ToString(),
                            Email = reader["Email"].ToString(),
                            Tel = reader["Tel"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString()
                        };

                        clienti.Add(cliente);
                    }

                    reader.Close();
                }


            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }

            return View(clienti);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Db.conn.Open();

                    string query = @"INSERT INTO Clienti (Azienda, Cod_Fisc, PI, Email, Tel, Nome, Cognome)
                             VALUES (@azienda, @cod_fisc, @pi, @email, @tel, @nome, @cognome)";

                    using (SqlCommand cmd = new SqlCommand(query, Db.conn))
                    {
                        cmd.Parameters.AddWithValue("@azienda", cliente.Azienda);
                        cmd.Parameters.AddWithValue("@cod_fisc", string.IsNullOrEmpty(cliente.Cod_Fisc) ? DBNull.Value : (object)cliente.Cod_Fisc);
                        cmd.Parameters.AddWithValue("@pi", string.IsNullOrEmpty(cliente.PI) ? DBNull.Value : (object)cliente.PI);
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(cliente.Email) ? DBNull.Value : (object)cliente.Email);
                        cmd.Parameters.AddWithValue("@tel", string.IsNullOrEmpty(cliente.Tel) ? DBNull.Value : (object)cliente.Tel);
                        cmd.Parameters.AddWithValue("@nome", string.IsNullOrEmpty(cliente.Nome) ? DBNull.Value : (object)cliente.Nome);
                        cmd.Parameters.AddWithValue("@cognome", string.IsNullOrEmpty(cliente.Cognome) ? DBNull.Value : (object)cliente.Cognome);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
                finally
                {
                    Db.conn.Close();
                }

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Cliente cliente = null;

            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti WHERE Cliente_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    cliente = new Cliente
                    {
                        Cliente_ID = Convert.ToInt32(reader["Cliente_ID"]),
                        Azienda = (bool)reader["Azienda"],
                        Cod_Fisc = reader["Cod_Fisc"].ToString(),
                        PI = reader["PI"].ToString(),
                        Email = reader["Email"].ToString(),
                        Tel = reader["Tel"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };

                }
                reader.Close();

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }

            return View(cliente);

        }

        public ActionResult Delete(Cliente cliente)
        {
           try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("DELETE FROM Clienti WHERE Cliente_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id", cliente.Cliente_ID);
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                Db.conn.Close();
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Cliente cliente = null;

            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti WHERE Cliente_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    cliente = new Cliente()
                    {
                        Cliente_ID = Convert.ToInt32(reader["Cliente_ID"]),
                        Azienda = (bool)reader["Azienda"],
                        Cod_Fisc = reader["Cod_Fisc"].ToString(),
                        PI = reader["PI"].ToString(),
                        Email = reader["Email"].ToString(),
                        Tel = reader["Tel"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };
                }


            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }

            return View(cliente);
        }

        [HttpPost]
        public ActionResult Edit(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Db.conn.Open();

                    var cmd = new SqlCommand(@"UPDATE Clienti
                                        SET Azienda = @azienda,
                                            Cod_Fisc = @cod_fisc,
                                            PI = @pi,
                                            Email = @email,
                                            Tel = @tel,
                                            Nome = @nome,
                                            Cognome = @cognome
                                            WHERE Cliente_ID = @id", Db.conn);

                    cmd.Parameters.AddWithValue("@azienda", cliente.Azienda);
                    cmd.Parameters.AddWithValue("@cod_fisc", cliente.Cod_Fisc);
                    cmd.Parameters.AddWithValue("@pi", cliente.PI);
                    cmd.Parameters.AddWithValue("@email", cliente.Email);
                    cmd.Parameters.AddWithValue("@tel", cliente.Tel);
                    cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@cognome", cliente.Cognome);
                    cmd.Parameters.AddWithValue("@id", cliente.Cliente_ID);

                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(cliente); // Ritorna la vista con il modello (cliente) che contiene gli errori di validazione
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }
        }

        


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}