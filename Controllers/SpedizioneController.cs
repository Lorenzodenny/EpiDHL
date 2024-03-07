using EpiDHL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiDHL.Controllers
{
    public class SpedizioneController : Controller
    {
        // GET: Spedizione
        public ActionResult Index()
        {
            List<Spedizione> spedizioni = new List<Spedizione>();

            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Spedizioni", Db.conn);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        var spedizione = new Spedizione()
                        {
                            Spedizione_ID = (int)reader["Spedizione_ID"],
                            Data_Spedizione = (DateTime)reader["Data_Spedizione"],
                            Cod_Sped = (int)reader["Cod_Sped"],
                            Peso = (decimal)reader["Peso"],
                            Citta_Dest = (string)reader["Citta_Dest"],
                            Indirizzo = (string)reader["Indirizzo"],
                            Destinatario = (string)reader["Destinatario"],
                            Conto = (decimal)reader["Conto"],
                            Data_Prev = (DateTime)reader["Data_Prev"],
                            Cliente_ID = (int)reader["Cliente_ID"]

                        };

                        spedizioni.Add(spedizione);
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

            return View(spedizioni);
        }

        public ActionResult Show(int? id)
        {

            
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                Db.conn.Open();

                var cmd = new SqlCommand(@"SELECT * FROM Spedizioni
                                   INNER JOIN Clienti ON (Spedizioni.Cliente_ID = Clienti.Cliente_ID)
                                   WHERE Spedizioni.Cliente_ID = @id", Db.conn);

                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                var spedizione = new Spedizione();

                if (reader.HasRows)
                {
                    reader.Read();

                    spedizione.Cliente = new Cliente()
                    {
                        Azienda = (bool)reader["Azienda"],
                        Cod_Fisc = (string)reader["Cod_Fisc"],
                        PI = (string)reader["PI"],
                        Email = (string)reader["Email"],
                        Tel = (string)reader["Tel"],
                        Nome = (string)reader["Nome"],
                        Cognome = (string)reader["Cognome"],
                    };

                    spedizione.Spedizione_ID = (int)reader["Spedizione_ID"];
                    spedizione.Data_Spedizione = (DateTime)reader["Data_Spedizione"];
                    spedizione.Cod_Sped = (int)reader["Cod_Sped"];
                    spedizione.Peso = (decimal)reader["Peso"];
                    spedizione.Citta_Dest = (string)reader["Citta_Dest"];
                    spedizione.Indirizzo = (string)reader["Indirizzo"];
                    spedizione.Destinatario = (string)reader["Destinatario"];
                    spedizione.Conto = (decimal)reader["Conto"];
                    spedizione.Data_Prev = (DateTime)reader["Data_Prev"];
                    spedizione.Cliente_ID = (int)reader["Cliente_ID"];
                }

                return View(spedizione);
            }
            catch (Exception ex)
            {
                
                ViewBag.ErrorMessage = "Si è verificato un errore durante il recupero dei dati dalla spedizione.";
                return View("Error"); 
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

        public ActionResult Ricerca(Spedizione pacco)
        {
            List<Spedizione> spedizioni = new List<Spedizione>();
            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand(@"SELECT * FROM Spedizioni
                                   INNER JOIN Clienti ON (Spedizioni.Cliente_ID = Clienti.Cliente_ID)
                                   WHERE Spedizioni.Cod_Sped = @cod", Db.conn);

                cmd.Parameters.AddWithValue("@cod", pacco.Cod_Sped);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Spedizione nuovaSpedizione = new Spedizione();

                    nuovaSpedizione.Cliente = new Cliente()
                    {
                        Azienda = (bool)reader["Azienda"],
                        Cod_Fisc = (string)reader["Cod_Fisc"],
                        PI = (string)reader["PI"],
                        Email = (string)reader["Email"],
                        Tel = (string)reader["Tel"],
                        Nome = (string)reader["Nome"],
                        Cognome = (string)reader["Cognome"],
                    };

                    nuovaSpedizione.Spedizione_ID = (int)reader["Spedizione_ID"];
                    nuovaSpedizione.Data_Spedizione = (DateTime)reader["Data_Spedizione"];
                    nuovaSpedizione.Cod_Sped = (int)reader["Cod_Sped"];
                    nuovaSpedizione.Peso = (decimal)reader["Peso"];
                    nuovaSpedizione.Citta_Dest = (string)reader["Citta_Dest"];
                    nuovaSpedizione.Indirizzo = (string)reader["Indirizzo"];
                    nuovaSpedizione.Destinatario = (string)reader["Destinatario"];
                    nuovaSpedizione.Conto = (decimal)reader["Conto"];
                    nuovaSpedizione.Data_Prev = (DateTime)reader["Data_Prev"];
                    nuovaSpedizione.Cliente_ID = (int)reader["Cliente_ID"];

                    spedizioni.Add(nuovaSpedizione);
                }

                reader.Close();

                if (spedizioni.Count > 0)
                {
                    return View(spedizioni);
                }
                else
                {
                    ViewBag.ErrorMessage = "Nessun ordine trovato con questo codice";
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Errore nella ricerca";
            }
            finally
            {
                Db.conn.Close();
            }

            return View();
        }




    }
}