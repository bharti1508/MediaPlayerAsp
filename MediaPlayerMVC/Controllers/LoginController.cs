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
using MediaPlayerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediaPlayerMVC.Controllers
{
    public class LoginController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "njOwVqAgVmgIrxLO8HrDK92CZFiLetuEyuVReDiU",
            BasePath = "https://mediaplayerasp.firebaseio.com/"
        };

        IFirebaseClient client;
        public IActionResult Index()
        {

                       return View();
        }
       

      

        [HttpPost]
        public async Task<IActionResult> Index([Bind]LoginModel model)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

            Console.WriteLine(model.Username);
            Console.WriteLine(model.Password);

            
            /*if (ModelState.IsValid)
            {
                Console.WriteLine("Successful Login username="+ model.Username);

                TempData["msg"] = "Successful Login";
            }
            else
                TempData["msg"] = "Username and password is invalid";
            */

            return View();
        }

        /* [HttpPost]
       [AllowAnonymous]
       [ValidateAntiForgeryToken]
       public IActionResult ExternalLogin(string provider, string returnUrl = null)
       {
           // Request a redirect to the external login provider.
           var redirectUrl = Url.Action( new { returnUrl });
           var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
           return Challenge(properties, provider);
       }*/
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
