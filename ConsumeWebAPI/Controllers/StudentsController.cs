using ConsumeWebAPI.Enums;
using ConsumeWebAPI.Models;
using ConsumeWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAPI.Models;
using System.Net.Http;
using System.Text;

namespace ConsumeWebAPI.Controllers
{
    public class StudentsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44302/api");
        private readonly HttpClient _client;

        public StudentsController()
        { 
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;  
        }

        [HttpGet]
        public IActionResult LoginPage()
        { 
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginPage(Administrator adminstrator)
        {
            string data = JsonConvert.SerializeObject(adminstrator);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Login/Login", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("HomePage");
            }
            else
            {
                return RedirectToAction("HomePage");
                //return View(adminstrator);
            }
                      
        }


        [HttpGet]
        public IActionResult HomePage()
        { 
            List<StudentViewModel> students = new List<StudentViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Student/GetStudents").Result;

            if (response.IsSuccessStatusCode)
            { 
                string data= response.Content.ReadAsStringAsync().Result;
                students = JsonConvert.DeserializeObject<List<StudentViewModel>>(data);
            }
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/Student/PostStudent", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = CommonServices.ShowAlert(Alerts.Success, "Student added");
                    return RedirectToAction("HomePage");
                }
                else
                {
                    TempData["Error"] = CommonServices.ShowAlert(Alerts.Danger, "Student is Already Present");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, "Some Error Occured!");
                throw;
            }
            

            return View();
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
           
            StudentViewModel student = new StudentViewModel();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Student/GetStudent/" + id).Result;

            if (response.IsSuccessStatusCode)
            { 
                string data=response.Content.ReadAsStringAsync().Result;
                student = JsonConvert.DeserializeObject<StudentViewModel>(data);

            }

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(StudentViewModel model)
        {
            try
            {                
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync(_client.BaseAddress + "/Student/PutStudent", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = CommonServices.ShowAlert(Alerts.Success, "Student Edited");
                    return RedirectToAction("HomePage");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Warning, "Some Error Occured!");
                throw;
            }


            return RedirectToAction("HomePage");
        }



        [HttpGet]
        public IActionResult Delete(int id)
        {
            StudentViewModel student = new StudentViewModel();
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Student/DeleteStudent/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                student = JsonConvert.DeserializeObject<StudentViewModel>(data);

            }

            return RedirectToAction("HomePage");
        }

    }
}
