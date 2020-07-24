using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediaPlayerMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediaPlayerMVC.Controllers
{
    public class LoginController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "c3jEy9HFaQDxohkPvy9pXt4ss7Pil29qZLNx7TN3",
            BasePath = "https://mediaplayer-46757.firebaseio.com/"
        };

        IFirebaseClient client;
        public IActionResult Index()
        {

                       return View();
        }
       

      

        [HttpPost]
        public  ActionResult Index([Bind]LoginModel model)
        {

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            bool valid = false;


            
            foreach (var item in data)
            {
                var uname = JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()).Username;
                var pass = JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()).Password;

                if(uname == model.Username && pass == model.Password)
                {
                    valid = true;
                    model.IsSignedIn = "true";
                }
                
                
              
            }
            
            if (valid == true)
            {
                HttpContext.Session.SetString("IsSignedIn", "true");
               
                TempData["storeUsername"] = model.Username;
                TempData["user"] = model.Username;
                return RedirectToAction("Index", "UploadFile");
            }
            else
            {
                TempData["msg"] = "Username or password is incorrect. Try again";
                return Redirect(Url.Action("Index", "Login"));
            }


            return View();
        }
        public IActionResult UploadFile()
        {
          

            ViewBag.isSignedIn = "true";
            return View();
        }



        public IActionResult Admin()
        {

            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
