using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nuevoProyecto.Data;
using nuevoProyecto.Models;
using System.IO;

namespace nuevoProyecto.Controllers
{
    public class MiniSQLController : Controller
    {
        // GET: MiniSQL
        public ActionResult PalabrasReservadas()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PalabrasReservadas(FormCollection collection)
        {
            try
            {
                //Enviar a singletone
               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MiniSQL/Create
        public ActionResult Consola()
        {
            return View();
        }

        // POST: MiniSQL/Create
        [HttpPost]
        public ActionResult Consola(FormCollection collection)
        {
            Singleton.Instance.Palabras_Reservadas();
            //try
            //{
                //Enviar a singletone
                Singleton.Instance.Input(collection["Data"]);
                return RedirectToAction("Consola");
            //}
            //catch
            //{
            //    return View();
            //}
        }  // GET: MiniSQL/Create
        public ActionResult TreeView()
        {
            return View(Singleton.Instance.TreeView());
        }
        public ActionResult VerSelect() //// ASiGNAR ESTA AR A LA DondE dice ver select

        {
            return View(Singleton.Instance.SelectLista);
        }
        public ActionResult DataGrid(string llave)
        {

            return View(Singleton.Instance.DevolverObjeto(llave));
        }
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string Nuevo = "";
                string path = Server.MapPath("~/archivo/");
                string[] Direccion = path.Split('\\');
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Nuevo = Direccion[0] + "/" + Direccion[1] + "/" + Direccion[2] + "/" + Direccion[3] + "/" + Direccion[4] + "/" + Direccion[5]+"/";
                filePath = Nuevo + Path.GetFileName(postedFile.FileName);
                if (Singleton.Instance.Ini == true)
                {
                    Singleton.Instance.LlenarPalabrasReservadas(filePath);
                    Singleton.Instance.Ini = false;
                    return RedirectToAction("Consola");
                }
            }

            return View();
        }
        public ActionResult Reservada()
        {
            Singleton.Instance.LlenarPalabrasReservadas("C:/Users/roche/Desktop/NuevoproyectoE1/nuevoProyecto/KeyWords.txt");
            return View("Consola");
        }
        public ActionResult CambiarPalabras()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CambiarPalabras(FormCollection collection)
        {

            try
            {
                //logica
                //igualar al custom
                if (collection["Select"] == "" || collection["Select"] == null)
                {

                }
                else
                {
                    Singleton.Instance.PalabrasCustom[0] = collection["Select"];
                }
                if (collection["From"] == "" || collection["From"] == null)
                {

                }
                else
                {
                    Singleton.Instance.PalabrasCustom[1] = collection["From"];
                }
                if (collection["Delete"] == "" || collection["Delete"] == null)
                {

                }
                else
                {

                    Singleton.Instance.PalabrasCustom[2] = collection["Delete"];
                }
                if (collection["Where"] == "" || collection["Where"] == null)
                {

                }
                else
                {
                    Singleton.Instance.PalabrasCustom[3] = collection["Where"];
                }
                if (collection["CreateTable"] == "" || collection["CreateTable"] == null)
                {

                }
                else
                {
                    Singleton.Instance.PalabrasCustom[4] = collection["Create"];
                }
                if (collection["DropTable"] == "" || collection["DropTable"] == null)
                {

                }
                else
                {

                    Singleton.Instance.PalabrasCustom[5] = collection["DropTable"];
                }
                if (collection["InsertInto"] == "" || collection["InsertInto"] == null)
                {

                }
                else
                {
                    Singleton.Instance.PalabrasCustom[6] = collection["InsertInto"];

                }
                if (collection["Value"] == "" || collection["Value"] == null)
                {

                }
                else
                {
                    Singleton.Instance.PalabrasCustom[7] = collection["Value"];
                }
                if (collection["Go"] == "" || collection["Go"] == null)
                {

                }
                else
                {
                   Singleton.Instance.PalabrasCustom[8] = collection["Go"];
                }

                return RedirectToAction("Consola");
            }
            catch
            {
                return View();
            }
        }

    }
}
