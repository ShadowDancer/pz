﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
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
				catch (Exception ex)
				{
					return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne zapytanie " + ex.Message))) };
				}

				var userManager = (new UserManager<UserViewModel>(new UserStore<UserViewModel>(new ApplicationDbContext())));
				userManager.UserValidator = new UserValidator<UserViewModel>(userManager) { AllowOnlyAlphanumericUserNames = false };
				if (input.Action == "createAccount")
				{
					return await CreateAccount(input, userManager);
				}

				UserViewModel user = null;
				if (this.User != null && this.User.Identity.IsAuthenticated)
				{
					using (var db = new PZEntities())
					{
						var PZUser = db.User.FirstOrDefault(n => n.Email == User.Identity.Name);
						user = new UserViewModel()
						{
							UserName = PZUser.Email
						};
						input.Username = PZUser.Email;
					}

				}
				else
				{
					user = await userManager.FindAsync(input.Username, input.Password);
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
					case "setCart":
						return SetCart(input);
					case "getCart":
						return GetCart(input);
					case "example":
						return CreateExample(input);
					case "getOrders":
						return GetOrders(user);
					case "requestPayment":
						return RequestPayment(user);
					case "getReservations":
						return GetReservations(input, user);
					case "setPrice":
						return SetPrice(input, user);
					default:
						return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "nieznana akcja"))) };
				}
			}
			catch (Exception ex)
			{
				return ReturnMessage(false, "błąd podczas przetwarzania zapytania: " + ex.Message);
			}
		}

		private HttpResponseMessage RequestPayment(UserViewModel user)
		{
			using(var db  = new PZEntities())
			{
				var PZUser = db.User.Where(n => n.Email == user.UserName).Single();
				var Orders = PZUser.Order.Where(n => n.State == OrderState.realised).ToList();

				foreach (var order in Orders)
				{
					order.State = OrderState.paymentRequested;
				}
				db.SaveChanges();
			}
			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OrderBundleDto(user))) };
			
		}

		private HttpResponseMessage SetPrice(PostRequestDTO input, UserViewModel user)
		{
			try
			{

				if (!this.User.IsInRole("admin"))
				{
					return ReturnMessage(false, "Brak uprawnień");
				}

				string[] split = input.Data.Split(' ');
				decimal price = decimal.Parse(split[0]);

				using (var db = new PZEntities())
				{


					DishPrices dp = new DishPrices();
					dp.DishID = input.Table.Value;
					dp.DateFrom = DateTime.Now;
					dp.Price = price;

					if (split.Length > 1)
					{
						dp.DateTo = DateTime.Parse(split[1]);
					}
					else
					{
						dp.DateTo = null;
					}

					db.DishPrices.Add(dp);
					db.SaveChanges();

				}

				return ReturnMessage(true, "");
			}
			catch
			{

			}
			return ReturnMessage(false, "Nieznany błąd");
		}

		private static HttpResponseMessage GetReservations(PostRequestDTO input, UserViewModel user)
		{
			try
			{
			    int userID = -1;
			    using (var db = new PZEntities())
			    {
			        var PZUser = db.User.FirstOrDefault(n => n.Email == user.UserName);
			        userID = PZUser.ID;
			    }

                return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new ReservationListViewModel(input.ReservationDate, userID))) };
			}
			catch (Exception ex)
			{
				return ReturnMessage(false, ex.ToString());
			}
		}

		private HttpResponseMessage GetOrders(UserViewModel user)
		{
			try
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OrderBundleDto(user))) };
			}
			catch (Exception ex)
			{
				return ReturnMessage(false, ex.ToString());
			}
		}

		private static HttpResponseMessage GetCart(PostRequestDTO input)
		{
			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new ShoppingCartViewModel(input.Username))) };
		}

		private static HttpResponseMessage AddToCart(PostRequestDTO input)
		{
			if (input.CartAmount == null || input.CartItems == null || input.CartItems.Count == 0 || input.CartItems.Count != input.CartAmount.Count)
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne dane"))) };
			}

			try
			{
				using (var db = new PZEntities())
				{
					var User = db.User.FirstOrDefault(n => n.Email == input.Username);

					if (User != null)
					{
						for (var i = 0; i < input.CartAmount.Count; i++)
						{
							User.ShoppingCart.Add(new ShoppingCart() { DishID = input.CartItems[i], Quantity = input.CartAmount[i], UserID = User.ID });
						}
						db.SaveChanges();
					}

				}
			}
			catch (Exception ex)
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, ex.Message))) };
			}

			return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(true, ""))) };
		}

		private static HttpResponseMessage SetCart(PostRequestDTO input)
		{
			if (input.CartAmount == null || input.CartItems == null || input.CartItems.Count == 0 || input.CartItems.Count != input.CartAmount.Count)
			{
				return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new OperationResultDTO(false, "niepoprawne dane"))) };
			}

			try
			{
				using (var db = new PZEntities())
				{
					var User = db.User.FirstOrDefault(n => n.Email == input.Username);

					if (User != null)
					{
						for (var i = 0; i < input.CartAmount.Count; i++)
						{
							ShoppingCart item = User.ShoppingCart.Where(n => n.DishID == input.CartItems[i]).FirstOrDefault();
							if (item != null)
							{
								if (input.CartAmount[i] == 0)
								{
									db.ShoppingCart.Remove(item);
								}
								else
								{
									item.Quantity = input.CartAmount[i];
								}
							}
						}
						db.SaveChanges();
					}

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
				var newUser = new User();
				newUser.Email = input.Username;

				using (var db = new PZEntities())
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

			using (var db = new PZEntities())
			{
				var newReservarion = new Reservation_List();
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
			int PZUserID = user.PZUser.ID;
			using (var db = new PZEntities())
			{
				var newOrder = new Order() { IssueDate = DateTime.Now, State = 1, TableID = (int)input.Table, UserID = PZUserID };
				db.Order.Add(newOrder);
				try
				{
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					return ReturnMessage(false, "Błąd podczas tworzenia zamówienia:\r\n" + ex.ToString());
				}

				var suborders = new List<SubOrder>();
				for (var i = 0; i < input.CartItems.Count; i++)
				{
					var subOrder = new SubOrder();
					subOrder.DishID = input.CartItems[i];
					subOrder.Quantity = input.CartAmount[i];
					subOrder.OrderID = newOrder.ID;
					db.SubOrder.Add(subOrder);
					suborders.Add(subOrder);
				}

				var shoppingCartItems = db.ShoppingCart.Where(n => n.UserID == PZUserID);
				foreach (var shoppingCartItem in (shoppingCartItems))
				{
					var connectedSuborder = suborders.FirstOrDefault(n => n.DishID == shoppingCartItem.DishID);
					if (connectedSuborder == null)
					{
						var subOrder = new SubOrder();
						subOrder.DishID = shoppingCartItem.DishID;
						subOrder.Quantity = shoppingCartItem.Quantity;
						subOrder.OrderID = newOrder.ID;
						db.SubOrder.Add(subOrder);
						suborders.Add(subOrder);
					}
					else
					{
						connectedSuborder.Quantity += shoppingCartItem.Quantity;
					}
				}
				db.ShoppingCart.RemoveRange(shoppingCartItems);

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