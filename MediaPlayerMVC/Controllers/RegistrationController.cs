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
            AuthSecret = "njOwVqAgVmgIrxLO8HrDK92CZFiLetuEyuVReDiU",
            BasePath = "https://mediaplayerasp.firebaseio.com/"
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
            try
            {
                AddUserToFirebase(registrationModel);
                ModelState.AddModelError(string.Empty, "Added Succesfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

            }

            return View(); 
        }

        private void AddUserToFirebase(RegistrationModel registrationModel)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = registrationModel;
            PushResponse response = client.Push("Users/", data);
            data.Key = response.Result.name;
            SetResponse setResponse = client.Set("Users/" + data.Key, data);
        }
    }
}
