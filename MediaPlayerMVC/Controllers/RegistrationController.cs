using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using MediaPlayerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediaPlayerMVC.Controllers
{
    public class RegistrationController : Controller
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "c3jEy9HFaQDxohkPvy9pXt4ss7Pil29qZLNx7TN3",
            BasePath = "https://mediaplayer-46757.firebaseio.com/"
        };

        IFirebaseClient client;

       
       

        [HttpGet]
        public IActionResult Index()
        {
          

            return View();
        }

        [HttpPost]
        public IActionResult Index(RegistrationModel registrationModel)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<RegistrationModel>();
            foreach (var item in data)
            {

                var uname=JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()).Username;
                list.Add(JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()));

                if (uname == registrationModel.Username)
                {
                    TempData["msg"] = "Username already exists! Try with another username";
                    return Redirect(Url.Action("Index", "Registration"));
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    AddUserToFirebase(registrationModel);
                 
                    return Redirect(Url.Action("Index", "Login"));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            }

            return View(); 
        }

        private void AddUserToFirebase(RegistrationModel registrationModel)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = registrationModel;
            PushResponse response = client.Push("Users/", data);
            data.Key = response.Result.name;
            TempData["key"] = data.Key;
            
            SetResponse setResponse = client.Set("Users/" + data.Key, data);

            
        }
    }
}
