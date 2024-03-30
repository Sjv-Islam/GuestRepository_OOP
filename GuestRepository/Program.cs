using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestRepository
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (IRepository<Guest> guests = GuestRepository.Instance)
                {

                    guests.Add(new Guest(9) { FirstName = "David", LastName = "Kim", Address = "Washington", NId = 74585621, Email = "David@gmail.com", MobileNo = "092365462", BirthDate = Convert.ToDateTime("01-Jan-1980"), Group = GuestDescription.Deluxe });
                    guests.Add(new Guest(10) { FirstName = "Jerrad", LastName = "Foe", Address = "New Jersy", NId = 75864262, Email = "Jerrad@gmail.com", MobileNo = "092365454", BirthDate = Convert.ToDateTime("01-Jan-1980"), Group = GuestDescription.Deluxe });
                    guests.Add(new Guest(11) { FirstName = "Rodoshy", LastName = "Clerk", Address = "California", NId = 75864262, Email = "Rodoshy@gmail.com", MobileNo = "092365454", BirthDate = Convert.ToDateTime("19-Dec-1997"), Group = GuestDescription.Suite });

                    var g = guests.FindById(3);





                    guests.Update(g);


                    Console.WriteLine($"Guest entry {g.GuestId} updated successfully");

                    Console.WriteLine(g.ToString());


                    if (guests.Delete(g))
                        Console.WriteLine($"Guest detail {g.GuestId} deleted successfully");








                    var data = guests.Search("Rodoshy");
                    Console.WriteLine();
                    Console.WriteLine($"Total Guest {data.Count()}");
                    Console.WriteLine("                 ");

                    foreach (var c in data)
                    {
                        Console.WriteLine(c.ToString());

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ReadLine();
            }

        }
        public interface IModel : IDisposable
        {
            int GuestId { get; }
            bool IsValid();
        }
        public interface IRepository<T> : IDisposable, IEnumerable<T> where T : IModel
        {

            IEnumerable<T> Data { get; }
            void Add(T model);
            bool Delete(T model);
            void Update(T model);
            T FindById(int Id);
            IEnumerable<T> Search(string value);

        }
        public enum GuestDescription
        {
            Standard = 15,
            Deluxe = 7,
            Suite = 5

        }
        public sealed class Guest : IModel
        {
            public int GuestId { get; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get => $"{this.FirstName} {this.LastName}"; }
            public string Address { get; set; }
            public int NId { get; set; }
            public string MobileNo { get; set; }

            public string Email { get; set; }
            public DateTime? BirthDate { get; set; }
            public GuestDescription Group { get; set; }

            public Guest()
            {

            }
            public Guest(int GuestId)
            {
                this.GuestId = GuestId;
                this.BirthDate = null;
                this.Group = GuestDescription.Standard;
            }
            public Guest(int GuestId, string MobileNo, string FirstName, string LastName = null, string Address = "N/A", string Email = null, DateTime? BirthDate = null, int NId = 0, GuestDescription Group = GuestDescription.Standard)
            {
                this.GuestId = GuestId;
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.MobileNo = MobileNo;
                this.Address = Address;
                this.NId = NId;
                this.Email = Email;
                this.BirthDate = BirthDate;
                this.Group = Group;
            }


            public bool IsValid()
            {
                bool isValid = true;


                if (string.IsNullOrWhiteSpace(FirstName))
                    isValid = false;
                else if (string.IsNullOrEmpty(LastName))
                    isValid = false;
                else if (string.IsNullOrWhiteSpace(MobileNo))
                    isValid = false;
                else if (string.IsNullOrWhiteSpace(Address))
                    isValid = false;
                else if (this.BirthDate?.Date > DateTime.Now)
                    isValid = false;

                return isValid;
            }
            public override string ToString()
            {
                string text = "Guest Info\n";
                text = text + $"Guest ID : {this.GuestId}\n";
                text += $"Name  : {this.FullName}\n";
                text += $"Address  : {this.Address}\n";
                text += $"Mobile  : {this.MobileNo} \n";
                text += $"Email  : {this.Email} \n";
                text += $"Date of Birth : {this.BirthDate?.ToString("d")}\n";
                text += $"Group : {this.Group} \n";


                return text;
            }
            public void Dispose()
            {

            }

        }
        public sealed class GuestRepository : IRepository<Guest>
        {


            private static GuestRepository _instance;
            public static GuestRepository Instance
            {
                get
                {
                    _instance = _instance ?? new GuestRepository();
                    return _instance;
                }
            }
            List<Guest> Data;

            private GuestRepository()
            {
                Data = new List<Guest>
                {
                new Guest(GuestId: 1, MobileNo: "09458621", "Jhon",  "Potter", "New Jersy", "Jhon@gmail.com"),
                new Guest(GuestId: 2, MobileNo: "09458622", "Jeniffer", "Emil", "California","Jeniffer@gmail.com"),
                new Guest(GuestId: 3, MobileNo: "09458623", "Team", "Pain","Liverpool", "Team@gmail.com"),
                new Guest(GuestId: 4, MobileNo: "09458624", "David", "Correy","Manchester", "David@gmail.com"),
                new Guest(GuestId: 5, MobileNo: "09458625", "Charles", "Darwein","NewYork", "Charles@gmail.com"),
                new Guest(GuestId: 6, MobileNo: "09458626", "Stephen", "Hodge", "New Jersy", "Stephen@gmail.com"),
                new Guest(GuestId: 7, MobileNo: "09458627", "Brad", "Pitt", "Fresno", "Brad@gmail.com"),
                new Guest(GuestId: 12, MobileNo: "09458627", "Brad", "Pitt", "Fresno", "Brad@gmail.com"),
                new Guest(GuestId: 13, MobileNo: "09458628", "Rodoshy", "Islam", "California", "Rodoshy@gmail.com")
                };

            }
            public void Dispose()
            {
                this.Data.Clear();
            }


            IEnumerable<Guest> IRepository<Guest>.Data { get => this.Data; }


            public Guest this[int index]
            {
                get
                {
                    return Data[index];
                }
            }
            public void Add(Guest entity)
            {
                if (Data.Any(c => c.GuestId == entity.GuestId))
                {
                    throw new Exception("Duplicate guest Id, try another");
                }
                else if (entity.IsValid())
                {
                    Data.Add(entity);
                }
                else
                {
                    throw new Exception("Guest is invalid");
                }
            }
            public bool Delete(Guest entity)
            {
                return Data.Remove(entity);
            }

            public void Update(Guest entity)
            {

                Data[Data.FindIndex(c => c.GuestId == entity.GuestId)] = entity;

            }
            public Guest FindById(int Id)
            {
                var result = (from r in Data where r.GuestId == Id select r).FirstOrDefault();
                return result;
            }

            public IEnumerable<Guest> Search(string value)
            {

                var result = from r in Data.AsParallel()
                             where
                             r.GuestId.ToString().Contains(value) ||
                             r.FirstName.StartsWith(value) ||
                             r.LastName.StartsWith(value) ||
                             r.Address.StartsWith(value) ||
                             r.MobileNo.Contains(value) ||
                             (r.Email != null && r.Email.Contains(value)) ||
                             (r.BirthDate != null && r.BirthDate.ToString().Contains(value))
                             orderby r.FirstName ascending
                             select r;
                return result;
            }
            public IEnumerator<Guest> GetEnumerator()
            {
                foreach (var c in Data)
                {
                    yield return c;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                foreach (var c in Data)
                {
                    yield return c;
                }
            }
        }
    }
}
    

