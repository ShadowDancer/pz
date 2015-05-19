using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using PZ.Controllers.Api.DTO;
using PZ.Models;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
/*
 * 
 * Example json
@"{'Username':'A','Password':'TestTest','Action':'example','CartItems':[1,2,3],'CartAmount':[5,10,15],'ReservationDate':'2015-05-19T00:00:00+02:00',
'ReservationHour':10,'ReservationLength':12}
";
*/
namespace PZ.Controllers
{
	public class WebController : ApiController
	{
		public object Get(string type)
		{
			return Get(type, null);
		}


		// GET api/<controller>/type/id
		public object Get(string type, string id)
		{
			using (var db = new PZ.Models.PZEntities())
			{
				switch (type)
				{
					case "Reservations":
						{
							if (string.IsNullOrEmpty(id))
							{
								return new ReservationListViewModel(DateTime.Now);
							}
							else
							{
								try
								{
									return new ReservationListViewModel(DateTime.Parse(id));
								}
								catch (System.FormatException)
								{
									return new OperationResultDTO(false, "Nieprawidłowy format daty");
								}
							}
						}
					//break;
					case "Menu":
						{
							return (object)new PZ.Models.MenuBundleViewModel().Menus;
						}
				}
			}




			return null;
		}

		// POST api/<controller>
		[HttpPost]
		public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
		{
			var inputText = request.Content.ReadAsStringAsync().Result;

			PostRequestDTO input = null;
			try
			{
				input = JsonConvert.DeserializeObject<PostRequestDTO>(inputText);
				if (input == null || string.IsNullOrEmpty(input.Action))
				{
					throw new Exception();
				}
			}
			catch
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne zapytanie"))) };
			}

			UserManager<UserViewModel> UserManager = new UserManager < UserViewModel > (new UserStore<UserViewModel>(new ApplicationDbContext()));
			var user = await UserManager.FindAsync(input.Username, input.Password);
			if (user == null)
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne dane logowania"))) };
			}
			
			switch (input.Action)
			{
				case "reserve":
					if (input.ReservationLength == null || input.ReservationHour == null || input.ReservationDate == null)
					{
						return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne dane"))) };
					}

					return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(true, ""))) };
				case "addToCart":
					if (input.CartAmount == null || input.CartItems == null || input.CartItems.Count == 0 || input.CartItems != input.CartAmount)
					{
						return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne dane"))) };
					}

					try
					{
						using (PZEntities db = new PZEntities())
						{
							var User = db.User.FirstOrDefault(n => n.Email == input.Username);

							for (int i = 0; i < input.CartAmount.Count; i++)
							{
								User.ShoppingCart.Add(new ShoppingCart() { DishID = input.CartItems[i], Quantity = input.CartItems[i], UserID = User.ID });
							}

							db.SaveChanges();
						}
					}
					catch(Exception ex)
					{
						return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, ex.Message))) };
					}

					return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(true, ""))) };
				case "getCart":
					return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new ShoppingCartViewModel(input.Username))) };
				case "example":
					input.Username = "Username";
					input.Password = "Password";
					input.ReservationDate = DateTime.Now.Date;
					input.ReservationHour = 10;
					input.ReservationLength = 12;
					input.CartItems = new List<int>() { 1, 2, 3 };
					input.CartAmount = new List<int>() { 5, 10, 15 };
					return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(input)) };
				default:
					return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "nieznana akcja"))) };
			}


			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "błąd serwera"))) };
		}

		// PUT api/<controller>/5
		public void Put(int id, [FromBody]string data)
		{
		}

		// DELETE api/<controller>/5
		public void Delete(int id)
		{
		}
	}
}