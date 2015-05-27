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
			try
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
				catch(Exception ex)
				{
					return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne zapytanie " + ex.Message ))) };
				}

				UserManager<UserViewModel> UserManager = (new UserManager<UserViewModel>(new UserStore<UserViewModel>(new ApplicationDbContext())));
				UserManager.UserValidator = new UserValidator<UserViewModel>(UserManager) { AllowOnlyAlphanumericUserNames = false };
				if (input.Action == "createAccount")
				{
					return await CreateAccount(input, UserManager);
				}

				UserViewModel user = null;
				if (this.User != null && this.User.Identity.IsAuthenticated)
				{


					int x = 10;
				}
				else
				{
					user = await UserManager.FindAsync(input.Username, input.Password);
				}
				if (user == null)
				{
					return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne dane logowania"))) };
				}

				switch (input.Action)
				{
					case "order":
						return CreateOrder(input, user);
					case "reserve":
						return CreateReservation(input, user);
					case "addToCart":
						return AddToCart(input);
					case "getCart":
						return GetCart(input);
					case "example":
						return CreateExample(input);
					default:
						return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "nieznana akcja"))) };
				}
			}
			catch (Exception ex)
			{
				return ReturnMessage(false, "błąd podczas przetwarzania zapytania: " + ex.Message);
			}
		}

		private static HttpResponseMessage GetCart(PostRequestDTO input)
		{
			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new ShoppingCartViewModel(input.Username))) };
		}

		private static HttpResponseMessage AddToCart(PostRequestDTO input)
		{
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
			catch (Exception ex)
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, ex.Message))) };
			}

			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(true, ""))) };
		}

		private async Task<HttpResponseMessage> CreateAccount(PostRequestDTO input, UserManager<UserViewModel> UserManager)
		{
			var result = await UserManager.CreateAsync(new UserViewModel() { UserName = input.Username }, input.Password);
			if (result.Succeeded)
			{
				User newUser = new User();
				newUser.Email = input.Username;

				using (PZEntities db = new PZEntities())
				{
					db.User.Add(newUser);
					try
					{
						db.SaveChanges();
						return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(true, ""))) };
					}
					catch (Exception ex)
					{
						return ReturnMessage(false, ex.Message);
					}
				}
			}
			else
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "tworzenie konta nie powiodło się:" + string.Join(";", result.Errors)))) };
			}
		}

		private static HttpResponseMessage CreateReservation(PostRequestDTO input, UserViewModel user)
		{
			if (input.ReservationLength == null || input.ReservationHour == null || input.ReservationDate == null || input.Table == null)
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne dane"))) };
			}

			using (PZEntities db = new PZEntities())
			{
				Reservation_List newReservarion = new Reservation_List();
				newReservarion.TableID = input.Table;
				newReservarion.UserID = user.ID;
				newReservarion.From = input.ReservationDate.AddHours((double)input.ReservationHour);
				newReservarion.To = newReservarion.From.AddHours((double)input.ReservationLength);
				db.Reservation_List.Add(newReservarion);
				try
				{
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					return ReturnMessage(false, "Błąd podczas tworzenia rezerwacji:\r\n" + ex.ToString());
				}
			}

			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(true, ""))) };
		}

		private static HttpResponseMessage CreateExample(PostRequestDTO input)
		{
			input.Username = "Username";
			input.Password = "Password";
			input.ReservationDate = DateTime.Now.Date;
			input.ReservationHour = 10;
			input.ReservationLength = 12;
			input.CartItems = new List<int>() { 1, 2, 3 };
			input.CartAmount = new List<int>() { 5, 10, 15 };
			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(input)) };
		}

		private static HttpResponseMessage CreateOrder(PostRequestDTO input, UserViewModel user)
		{
			if (input.CartAmount == null || input.CartItems == null || input.CartItems.Count == 0 || input.CartItems.Count != input.CartAmount.Count || input.Table == null)
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne dane"))) };
			}

			using (PZEntities db = new PZEntities())
			{
				Order newOrder = new Order() { IssueDate = DateTime.Now, State = "1", TableID = (int)input.Table, UserID = user.PZUser.ID };
				db.Order.Add(newOrder);
				try
				{
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					return ReturnMessage(false, "Błąd podczas tworzenia zamówienia:\r\n" + ex.ToString());
				}
				
				for (int i = 0; i < input.CartItems.Count; i++)
				{
					SubOrder subOrder = new SubOrder();
					subOrder.DishID = input.CartItems[i];
					subOrder.Quantity = input.CartAmount[i];
					subOrder.OrderID = newOrder.ID;
					db.SubOrder.Add(subOrder);
				}
				
				try
				{
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					return ReturnMessage(false, "Błąd podczas tworzenia pozycji zamówienia:\r\n" + ex.ToString());
				}
			}

			return ReturnMessage(true, "");
		}

		private static HttpResponseMessage ReturnMessage(bool result, string message)
		{
			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(result, message))) };
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