using Sengoku.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sengoku.API
{
    public class Helpers
    {
        public static Random rand = new Random();
        private static string GetRandom(IList<string> items)
        {
            return items[rand.Next(items.Count)];
        }

        public static string MakeUniqueUserName(List<string> names)
        {
            var maxNames = userPrefix.Count * userSuffix.Count;
            if(names.Count >= maxNames)
            {
                throw new System.InvalidOperationException("Maximum number of unique names exceeded");
            }
            var prefix = GetRandom(userPrefix);
            var suffix = GetRandom(userSuffix);
            var userName = prefix + suffix;

            if(names.Contains(userName))
            {
                MakeUniqueUserName(names);
            }
            return userName;
        }
        public static string MakeUniqueEventName(List<string> names = null)
        {
            var maxNames = eventPrefix.Count * eventSuffix.Count;
            var prefix = GetRandom(eventPrefix);
            var suffix = GetRandom(eventSuffix);
            var eventName = prefix + suffix;

            if(names != null)
            {
                if (names.Contains(eventName))
                {
                    MakeUniqueEventName(names);
                }
            }
            return eventName;
        }
        public static string MakeUserEmail(string userName)
        {
            return $"contact@{userName.ToLower()}.com";
        }
        public static string GetRandomState()
        {
            return GetRandom(usStates);
        }
        public static DateTime GetRandomOrderPlaced()
        {
            var end = DateTime.Now;
            var startDate = end.AddDays(-90);

            TimeSpan possibleSpan = end - startDate;
            TimeSpan newSpan = new TimeSpan(0, rand.Next(0, (int)possibleSpan.TotalMinutes), 0);

            return startDate + newSpan;
        }
        public static DateTime? GetRandomOrderCompleted(DateTime orderPlaced)
        {
            var now = DateTime.Now;
            var minLeadTime = TimeSpan.FromDays(7);
            var timePassed = now - orderPlaced;

            if (timePassed < minLeadTime)
            {
                return null;
            }

            return orderPlaced.AddDays(rand.Next(7, 14));
        }
        public static string GetRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            while (0 < length--)
            {
                res.Append(valid[rand.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public static string MakeRandomID(List<string> eventIds = null, int length = 8)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder res = new StringBuilder();
            while (0 < length--)
            {
                res.Append(valid[rand.Next(valid.Length)]);
            }
            string newId = res.ToString();
            if(eventIds != null)
            {
                if (eventIds.Contains(newId))
                {
                    MakeUniqueEventName(eventIds);
                }
            }
            return newId;
        }
        public static string GetRandomCity()
        {
            return GetRandom(Cities);
        }
        public static string GetRandomGame()
        {
            return GetRandom(gamesList);
        }
        public static string GetRandomZipCode(int length = 5)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            while( 0 < length--)
            {
                res.Append(valid[rand.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public static string GetRandomStreetNum(int length = 4)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            while (0 < length--)
            {
                res.Append(valid[rand.Next(valid.Length)]);
            }
            return res.ToString();
        }
/*        public static Address MakeUniqueAddress(List<Address> addresses = null)
        {
            var maxAddresses = streetNamePreFix.Count * streetNameSuffix.Count;
            var streetNum = GetRandomStreetNum();
            var prefix = GetRandom(streetNamePreFix);
            var suffix = GetRandom(streetNameSuffix);
            var streetName = prefix + suffix;
            var zipCode = GetRandomZipCode();

            var newAddress = new Address
            {
                Building = streetNum,
                Street = streetName,
                Zipcode = zipCode
            };
            return newAddress;
        }*/
        private static readonly List<string> usStates = new List<string>()
        {
            "AK", "AL", "AR", "AS", "AZ", "CA", "CO", "CT",
            "DC", "DE", "FL", "GA", "GU", "HI", "IA", "ID",
            "IL", "IN", "KS", "KY", "LA", "MA", "MD", "ME",
            "MI", "MN", "MO", "MP", "MS", "MT", "NC", "ND",
            "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK",
            "OR", "PA", "PR", "RI", "SC", "SD", "TN", "TX",
            "UM", "UT", "VA", "VI", "VT", "WA", "WI", "WV", "WY"
        };
        private static readonly List<string> Cities = new List<string>()
        {
            "Phoenix", "Atlanta", "New York", "Raleigh", "Orlando", "Oakland",
            "Anaheim", "Peoria", "Chicago"
        };

        private static readonly List<string> userPrefix = new List<string>()
        {
            "Michael",
            "Jordan",
            "Joe",
            "Stephen",
            "Michael",
            "Sarah",
            "Kaitlyn",
            "Bridgett"
        };
        private static readonly List<string> userSuffix = new List<string>()
        {
            "Johson",
            "Whitson",
            "Brown",
            "Random",
            "Duffy",
            "Gooding",
            "Hoe",
            "Hernandez"
        };
        private static readonly List<string> eventPrefix = new List<string>()
        {
            "Uber",
            "Masters",
            "N00b",
            "Applied",
            "ScrubLords",
            "Frosty",
            "Pound"
        };
        private static readonly List<string> eventSuffix = new List<string>()
        {
            "Tournament",
            "Me",
            "Fausties",
            "Masters",
            "International",
            "Bests",
            "Blasters",
            "Pros",
            "Regionales",
            "Handies"
        };
        private static readonly List<string> gamesList = new List<string>()
        {
            "GuiltyGear",
            "DragonBallFightersZ",
            "SmashBrosUltimate",
            "SmashBrosMelee",
            "Tekken7",
            "StreetFighterV"
        };
        private static readonly List<string> streetNamePreFix = new List<string>()
        {
            "Stillwell",
            "Huntson",
            "Scott",
            "Peachtree",
            "Jamaican",
            "9th",
            "Utopia",
            "Ernest"
        };
        private static readonly List<string> streetNameSuffix = new List<string>()
        {
            "Street",
            "Boulevard",
            "Avenue",
            "Road",
            "Lane",
            "Main"
        };
    }
}
