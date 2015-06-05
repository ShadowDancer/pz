using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZ.Models
{
	public class ReservationListViewModel
	{
		public ReservationListViewModel(DateTime Date)
		{
			OpeningHour = 15;
			ClosignHour = 22;
			this.Date = Date.Date;


			using (var db = new PZEntities())
			{
				Data = new List<bool[]>();

				List<Reservation_List> reservations = db.Reservation_List
					.OrderBy(n => n.TableID)
					.ThenBy(n => n.From)
					.ToList();

				reservations = reservations.Where(n => DateTime.Compare(n.From.Date, Date) == 0).ToList();

				Tables = db.Table.OrderBy(n => n.ID).Select(n => new TableViewModel() { ID = n.ID, Capacity = n.Capacity, Comment = n.Comment }).ToList();

				var span = ClosignHour - OpeningHour;
				DayLength = span;
				
				foreach(var table in Tables)
				{
					var reserved = new bool[span];

					var tableReservations = reservations.FindAll(n => n.TableID == table.ID).ToList();
					foreach(var reservation in tableReservations)
					{
						var startHour = reservation.From.Hour - OpeningHour;
						var finishHour = reservation.To.Hour - OpeningHour;
						if (startHour < 0)
						{
							startHour = 0;
						}
									
						for(var i = startHour; i < finishHour && i < span; i++)
						{
							reserved[i] = true;
						}
					}


					Data.Add(reserved);
				}



			}





		}

		public DateTime Date { get; set; }
		public int OpeningHour { get; set; }
		public int ClosignHour { get; set; }
		public int DayLength { get; set; }
		public List<TableViewModel> Tables { get; set; }
		public List<bool[]> Data { get; set; }
	}
}