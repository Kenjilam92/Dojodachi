using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dojodachi.Models;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace dojodachi.Controllers
{
    public class HomeController : Controller
    {   
        [HttpGet("dojodachi")]
        public IActionResult Index()
        {
            if (HttpContext.Session.Get("dojodachi") == null)
            {
                DojoDachi a = new DojoDachi();
                HttpContext.Session.Set("dojodachi", ObjectToByteArray(a));
            }
            DojoDachi b = (DojoDachi)ByteArrayToObject(HttpContext.Session.Get("dojodachi"));
            if (b.Fullness <= 0 || b.Happines <= 0)
            {
                b.Alive = false;
                b.History.Add("Dojodachi is depressed!");
            }
            if (b.Fullness >100 && b.Energy > 100 && b.Happines > 100)
            {
                b.Win = true;
                b.History.Add("Dojodachi is joyful! ");
            }
            return View(b);
        }

        [HttpGet("")]
        public IActionResult Home()
        {
            return Redirect("/dojodachi");
        }

        [HttpGet]
        [Route("dojodachi/{act}")]
        public IActionResult Index(string act)
        {   
            System.Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            System.Console.WriteLine(act);
            Random Rand = new Random();
            DojoDachi a = (DojoDachi)ByteArrayToObject(HttpContext.Session.Get("dojodachi"));
            if(act=="Feed")
            {
                if (a.Meals > 0)
                {
                    a.Meals--;
                    int temp = Rand.Next(5,11);
                    int dislike = Rand.Next(1,5);
                    if (dislike == 4)
                    {
                        a.History.Add($"Dojodachi doesn't like your food");
                    }
                    else
                    {
                        a.Fullness+= temp;
                        a.History.Add($"Dojodachi ate 1 meal and gain {temp} fullness");
                    }
                }
                else 
                {
                    a.History.Add($"You have no more meal. Go to work and earn some!");
                }
            }
            else if(act == "Play")
            {
                if (a.Energy >= 5)
                {
                    a.Energy-=5;
                    int temp = Rand.Next(5,11);
                    int dislike = Rand.Next(1,5);
                    if (dislike == 4)
                    {
                        a.History.Add($"Dojodachi doesn't like to play with you");
                    }
                    else
                    {
                        a.Happines+=temp;
                        a.History.Add($"Dojodachi played with you and gain {temp} points happiness");
                    }
                }
                else
                {
                    a.History.Add("No More Energy! Eat something or Sleep to retore engery!");
                }
            }
            else if(act == "Work")
            {
                if (a.Energy >= 5)
                {
                    a.Energy-=5;
                    int temp = Rand.Next(1,4);
                    a.Meals+= temp;
                    a.History.Add($"Dojodachi went to work and earned {temp} meal(s)");
                }
                else
                {
                    a.History.Add("No More Energy! Eat something or Sleep to retore engery!");
                }
            }
            else if (act=="Sleep")
            {
                a.Fullness-=5;
                a.Happines-=5;
                a.Energy+=15;
                a.History.Add("Dojodachi took a nap and restored 15 energy points, lost 5 points in both fullness and happines");
            }
            HttpContext.Session.Set("dojodachi", ObjectToByteArray(a));
            return Redirect("/dojodachi");
        }
        [Route("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // public IActionResult Privacy()
        // {
        //     return View();
        // }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }

        // Convert an object to a byte array
        private byte[] ObjectToByteArray(Object obj)
        {
            if(obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // Convert a byte array to an Object
        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object) binForm.Deserialize(memStream);

            return obj;
        }
    }
}
