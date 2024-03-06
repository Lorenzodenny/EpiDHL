using EpiDHL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
                    Db.conn.Close();
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
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Azienda, Cod_Fisc, PI, Email, Tel, Nome, Cognome")] Cliente cliente)
        {
            if (cliente.Azienda == true )
            {
                if (cliente.Cod_Fisc == null && cliente.PI == null)
                {
                    ViewBag.ErrorMessage = "Partita Iva è un campo obbligatorio";
                }
            }
            else if (cliente.Azienda == false)
            {
                if (cliente.Cod_Fisc == null && cliente.PI == null)
                {
                    ViewBag.ErrorMessage = "Codice Fiscale è un campo obbligatorio";
                }
            }

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

                        if (cliente.Azienda)
                        {
                         
                            if (string.IsNullOrEmpty(cliente.PI))
                            {
                                if (ModelState.IsValid)
                                {
                                    return View(cliente);
                                }
                            }
                            cmd.Parameters.AddWithValue("@pi", cliente.PI);
                            cmd.Parameters.AddWithValue("@cod_fisc", DBNull.Value); 
                        }
                        else 
                        {
                           
                            if (string.IsNullOrEmpty(cliente.Cod_Fisc))
                            {
                                if(ModelState.IsValid)
                                {
                                    return View(cliente);
                                }
                             
                            }
                            cmd.Parameters.AddWithValue("@cod_fisc", cliente.Cod_Fisc);
                            cmd.Parameters.AddWithValue("@pi", DBNull.Value); 
                        }

                        
                        cmd.Parameters.AddWithValue("@email", cliente.Email);
                        cmd.Parameters.AddWithValue("@tel", cliente.Tel);
                        cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                        cmd.Parameters.AddWithValue("@cognome", cliente.Cognome);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "Cliente_ID")] Cliente cliente)
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Azienda, Cod_Fisc, PI, Email, Tel, Nome, Cognome")] Cliente cliente)
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
                    return View(cliente);
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

        [HttpGet]
        public ActionResult Ricerca()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ricerca(Cliente cliente)
        {
            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti WHERE Nome = @nome", Db.conn);
                cmd.Parameters.AddWithValue("@nome", cliente.Nome);

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

                    reader.Close();
                    return View(cliente);
                }
                else
                {
                    ViewBag.ErrorMessage = "Nessun cliente trovato con questo nome";
                }


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage =  "Errore nella ricerca" ;
            }
            finally
            {
                Db.conn.Close();
            }

            return View();
        }

       public ActionResult Dettagli(int id)
        {
            

            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti WHERE Cliente_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();
                var cliente = new Cliente();
                if (reader.HasRows)
                {
                    reader.Read();
                    cliente.Cliente_ID = (int)reader["Cliente_ID"];
                    cliente.Azienda = (bool)reader["Azienda"];
                    cliente.Cod_Fisc = (string)reader["Cod_Fisc"];
                    cliente.PI = (string)reader["PI"];
                    cliente.Email = (string)reader["Email"];
                    cliente.Tel = (string)reader["Tel"];
                    cliente.Nome = (string)reader["Nome"];
                    cliente.Cognome = (string)reader["Cognome"];
                        

                }
                else
                {
                    ViewBag.ErrorMessage = " il cliente non esiste";
                }
                return View(cliente);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "problemi col dettaglio ";
            }
            finally
            {
                Db.conn.Close();
            }
            return View();

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