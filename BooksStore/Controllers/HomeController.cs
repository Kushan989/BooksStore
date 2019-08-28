using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BooksStore.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BooksStore.Controllers
{
    public class HomeController : Controller
    {
        /**************************Get All Books****************************************/
        public async Task<IActionResult> Index()
        {
            List<Book> bookList = new List<Book>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:51369/api/books"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        bookList = JsonConvert.DeserializeObject<List<Book>>(apiResponse);
                    }
                }

            }catch (HttpRequestException)
            {
                Console.WriteLine("API Connection failed");
            }

            
            return View(bookList);
        }
        /**************************Get Book by Id****************************************/
        public ViewResult GetBook() => View();

        [HttpPost]
        public async Task<IActionResult> GetBook(string id)
        {
            Book book = new Book();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:51369/api/books/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        book = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }
                }
            }catch (HttpRequestException)
            {
                Console.WriteLine("API Connection failed");
            }

            return View(book);
        }

        /**************************Add a Book****************************************/
        public ViewResult AddBook() => View();

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            Book receivedBook = new Book();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
                try
                {
                    using (var response = await httpClient.PostAsync("http://localhost:51369/api/books", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        receivedBook = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }

                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("API Connection failed");
                }
                
            }
            return View(receivedBook);
        }

        /**************************Update a Book****************************************/
        public async Task<IActionResult> UpdateBook(string id)
        {
            Book book = new Book();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.GetAsync("http://localhost:51369/api/books/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        book = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("API Connection failed");
                }

            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook(string id, Book book)
        {
            Book receivedBook = new Book();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
                //var content = new MultipartFormDataContent();
                //content.Add(new StringContent(book.Id), "Id");
                //content.Add(new StringContent(book.BookName), "BookName");
                //content.Add(new StringContent(book.Price.ToString()),"Price");
                //content.Add(new StringContent(book.Category), "Category");
                //content.Add(new StringContent(book.Author), "Author");

                try
                {
                    using (var response = await httpClient.PutAsync("http://localhost:51369/api/books/"+ book.Id, content ))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = "Success";
                        receivedBook = JsonConvert.DeserializeObject<Book>(apiResponse);
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("API Connection failed");
                }
                
            }
            return View(receivedBook);
        }

        /**************************Delete a Book****************************************/
        [HttpPost]
        public async Task<IActionResult> DeleteBook(string id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    using (var response = await httpClient.DeleteAsync("http://localhost:51369/api/books/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Connection Invalid");
                }
                
            }

            return RedirectToAction("Index");
        }

        /**************************Send a Image****************************************/
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            string apiResponse = "";
            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                using (var fileStream = file.OpenReadStream())
                {
                    form.Add(new StreamContent(fileStream), "file", file.FileName);
                    using (var response = await httpClient.PostAsync("http://localhost:51369/api/books/UploadFile", form))
                    {
                        response.EnsureSuccessStatusCode();
                        apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return View((object)apiResponse);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
