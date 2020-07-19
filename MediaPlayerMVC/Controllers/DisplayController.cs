using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using MediaPlayerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace MediaPlayerMVC.Controllers
{
    public class DisplayController : Controller
    {


        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "njOwVqAgVmgIrxLO8HrDK92CZFiLetuEyuVReDiU",
            BasePath = "https://mediaplayerasp.firebaseio.com/"
        };

        IFirebaseClient client;

        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
           
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
           
            var list = new List<RegistrationModel>();
            foreach (var item in data)
            {
                Console.WriteLine(JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()).Name);
                list.Add(JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()));

            }
            
            return View(list);
        }

       
    }
}
