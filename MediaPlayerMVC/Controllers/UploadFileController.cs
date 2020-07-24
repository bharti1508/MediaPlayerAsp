using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using FireSharp.Extensions;
using MediaPlayerMVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MediaPlayerMVC.Controllers
{
    public class UploadFileController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "c3jEy9HFaQDxohkPvy9pXt4ss7Pil29qZLNx7TN3",
            BasePath = "https://mediaplayer-46757.firebaseio.com/"
        };

        IFirebaseClient client;


        private IHostingEnvironment _environment;

         

        public UploadFileController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        //default
        

        [Route("UploadFile/Index/{id?}")]

        public IActionResult Index()
        {
            var userName = TempData["storeUsername"].ToString();
          //  var Name = TempData["name"].ToString();
            client = new FireSharp.FirebaseClient(config);
        
            string[] temp = userName.Split("@");

            var path = Path.Combine(temp[0], "Files\\");
            FirebaseResponse response = client.Get(path);
            Console.WriteLine(response.Body.Length);
            if (response.Body== "null")
            {
                TempData["storeUsername"] = userName;
                TempData["message"] = "No files present!!";
                return View();
            }
            else

            {
                JObject resultData = JObject.Parse(response.Body);
            
                List<UploadFileModel> audioList = new List<UploadFileModel>();
                List<UploadFileModel> videoList = new List<UploadFileModel>();

                foreach (var item in resultData)
                {
                    JObject download = JObject.Parse(item.Value.ToString());
                    var name = download.Last.First.ToString();
                    string[] substrings=name.Split(".");
                    if(substrings[1]=="mp3")
                    {
                        audioList.Add(new UploadFileModel() { name = download.Last.First.ToString(), link = download.First.First.ToString() });

                    }
                    else
                    {
                        videoList.Add(new UploadFileModel() { name = download.Last.First.ToString(), link = download.First.First.ToString() });
                    }




                }

               
                // TempData["name"] = Name;
                TempData["storeUsername"] = userName;
                TempData["user"] = userName;
                ViewBag.AudioList = audioList;
                ViewBag.VideoList = videoList;
                return View();

            } 
        }


        [HttpPost("FileUpload")]

        public async Task<IActionResult> Index(IFormFile file, UploadFileModel model)
        {
          
            string userName = TempData["user"].ToString() ;

            //var Name = TempData["name"].ToString();
            List<string> downloadurls = new List<string>();


            var uploadedFiles = Path.Combine(_environment.WebRootPath, "uploadedFiles",file.FileName);
            
            
           //copy the contents of file in temp location
            using (var stream = new FileStream(uploadedFiles, FileMode.Create))
            { file.CopyTo(stream); }

            if (file.Length > 0)
            {
                var fileStream = System.IO.File.Open(uploadedFiles, FileMode.Open);
                Console.WriteLine(fileStream.Length);
                var task = new FirebaseStorage("mediaplayer-46757.appspot.com")
                              .Child(userName)
                              .Child(file.FileName)
                              .PutAsync(fileStream);
              

                Console.WriteLine("task=" + task.TargetUrl);
                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                try
                {
                    // error during upload will be thrown when you await the task
                    Console.WriteLine("Download link:\n" + await task);
                   var link = await task;
                    model.name = file.FileName.ToString();
                    model.link = link.ToString(); ;
                    var data = model;
                    client = new FireSharp.FirebaseClient(config);
                    string[] temp = userName.Split("@");
                    
                    var path = Path.Combine(temp[0], "Files\\");
                   
                  
                        Console.WriteLine("path=" + path);
                    PushResponse pushResponse = client.Push(path,data);



                    TempData["user"] = userName;
                    TempData["storeUsername"] = userName;
                    //TempData["name"] = Name;
                    TempData["message"] = "File Uploaded Sucessfully";
                    return RedirectToAction("Index", "UploadFile");

                }
                catch (Exception ex)
                {
                    TempData["message"] = "Error while Uploading";
                  
                    Console.WriteLine("Exception was thrown: {0}", ex);
                    return RedirectToAction("Index", "UploadFile");

                }
                
            }
           

            return View();

        }

      
    }
}



          /*client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);

bool valid = false;


var list = new List<RegistrationModel>();
            foreach (var item in data)
            {
                var uname = JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()).Username;
var key = JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()).Key;

                
                if(userName==uname)
                {

                    SetResponse setResponse = client.Set("Users/" + key + "/DownloadUrls/", link);

                }

                //Console.WriteLine(JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()).Name);
                //  list.Add(JsonConvert.DeserializeObject<RegistrationModel>(((JProperty)item).Value.ToString()));
*/