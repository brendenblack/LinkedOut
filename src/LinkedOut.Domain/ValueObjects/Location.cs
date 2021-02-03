using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedOut.Domain.ValueObjects
{
    public class Location : IComparable, IComparer
    {
        private Location() { }

        public Location(string cityName, string province)
        {
            CityName = cityName;
            Province = province;
        }

        public string CityName { get; private set; }

        public string Province { get; private set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(CityName) && string.IsNullOrWhiteSpace(Province))
            {
                return CityName;
            }
            else if (string.IsNullOrWhiteSpace(CityName) && string.IsNullOrWhiteSpace(Province))
            {
                return "Parts Uknown";
            }
            else
            {
                return $"{CityName}, {Province}";
            }
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Location))
                return false;

            var other = obj as Location;

            if (CityName != other.CityName || Province != other.Province)
                return false;

            return true;
        }

        public int CompareTo(object obj)
        {
            Location b = (Location)obj;

            var provinceCompare = string.Compare(this.Province, b.Province);
            if (provinceCompare != 0)
            {
                return provinceCompare;
            }

            return string.Compare(this.CityName, b.CityName);
        }

        public int Compare(object x, object y)
        {
            Location l1 = (Location)x;
            Location l2 = (Location)y;

            var provinceCompare = string.Compare(l1.Province, l2.Province);
            if (provinceCompare != 0)
            {
                return provinceCompare;
            }

            return string.Compare(l1.CityName, l2.CityName);
        }

        public static bool operator ==(Location x, Location y) => x.Equals(y);
        public static bool operator !=(Location x, Location y) => !x.Equals(y);

        






        public static Location Ottawa => new Location("Ottawa", ONTARIO);
        public static Location Toronto => new Location("Toronto", ONTARIO);
        public static Location Victoria => new Location("Victoria", BRITISH_COLUMBIA);

        public static Location Remote => new Location("Remote", "");

        public static Location PartsUnknown => new Location("Parts Unknown", "");






        public static readonly string ALBERTA = "Alberta";
        public static readonly string BRITISH_COLUMBIA = "British Columbia";
        public static readonly string MANITOBA = "Manitoba";
        public static readonly string NEW_BRUNSWICK = "New Brunswick";
        public static readonly string NEWFOUNDLAND_AND_LABRADOR = "Newfoundland and Labrador";
        public static readonly string NORTHWEST_TERRITORIES = "Northwest Territories";
        public static readonly string NOVA_SCOTIA = "Nova Scotia";
        public static readonly string NUNAVUT = "Nunavut";
        public static readonly string ONTARIO = "Ontario";
        public static readonly string PEI = "Prince Edward Island";
        public static readonly string QUEBEC = "Quebec";
        public static readonly string SASKATCHEWAN = "Saskatchewan";
        public static readonly string YUKON = "Yukon";

        public static IReadOnlyCollection<string> PROVINCES = new List<string>
        {
            ALBERTA,
            BRITISH_COLUMBIA,
            MANITOBA,
            NEW_BRUNSWICK,
            NEWFOUNDLAND_AND_LABRADOR,
            NORTHWEST_TERRITORIES,
            NOVA_SCOTIA,
            NUNAVUT,
            ONTARIO,
            PEI,
            QUEBEC,
            SASKATCHEWAN,
            YUKON
        };

    }
}
