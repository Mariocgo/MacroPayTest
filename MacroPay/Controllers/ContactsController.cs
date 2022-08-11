using Macropay.Controllers;
using MacroPay.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;


namespace MacroPay.Controllers
{
    public class ContactsController : DefaultController
    {
        private string conexion = @"C:\Users\mario\source\repos\MacroPay\MacroPay\Data\fakedatabase.js";

        #region Get ListContacts
        [HttpGetAttribute()]
        public ActionResult<List<Contacts>> Get()
        {
            try
            {
                Request.ContentType = "application/json";
                var request = HttpContext.Request.Query["phrase"];
                if (request.ToString().Length == 0 || request.ToString() == null)
                {
                    return BadRequest();
                }
                string fileJson = System.IO.File.ReadAllText(conexion);
                var res = JsonSerializer.Deserialize<List<Contacts>>(fileJson).AsQueryable().OrderBy(
                    x => x.name).ToList();
                var result = res.FindAll(
                    x => x.name.ToUpper().Contains(request.ToString().ToUpper()));
                return result;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Exception: [ContactsController]: " + ex.Message);
                System.Console.WriteLine("Inner exception: [ContactsController]: " + ex.InnerException?.Message);
                return new BadRequestObjectResult(new { Success = false, Message = ex.Message });
            }
        }
        #endregion

        #region Get Id Contacts
        [HttpGetAttribute("{id}")]
        public ActionResult<Contacts> Get(string id)
        {
            try {
            string fileJson = System.IO.File.ReadAllText(conexion);
            var res = JsonSerializer.Deserialize<List<Contacts>>(fileJson);
            var result = res.Find(x => x.id == id);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return NotFound();
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Exception: [ContactsController]: " + ex.Message);
                System.Console.WriteLine("Inner exception: [ContactsController]: " + ex.InnerException?.Message);
                return new BadRequestObjectResult(new { Success = false, Message = ex.Message });
            }
        }
        #endregion

        #region Delete id
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                string fileJson = System.IO.File.ReadAllText(conexion);
                var res = JsonSerializer.Deserialize<List<Contacts>>(fileJson);
                foreach (var item in res)
                {
                    if (item.id == id)
                    {
                        var validate = res.Remove(item);
                        TextWriter writeFile = new StreamWriter(conexion);
                        writeFile.Write(JsonSerializer.Serialize(res));
                        writeFile.Flush();
                        writeFile.Close();
                        if (validate)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Exception: [ContactsController]: " + ex.Message);
                System.Console.WriteLine("Inner exception: [ContactsController]: " + ex.InnerException?.Message);
                return new BadRequestObjectResult(new { Success = false, Message = ex.Message });
            }

            return NotFound();
        }
        #endregion
    }
}
