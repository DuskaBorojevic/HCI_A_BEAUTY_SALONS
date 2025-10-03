using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class Shift
    {
      
        public string Name { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public int BeautySalonId { get; set; }

        public Shift() { }

        public Shift(string name, int beautySalonId, TimeSpan fromTime, TimeSpan toTime)
        {
            Name = name;
            BeautySalonId = beautySalonId;
            FromTime = fromTime;
            ToTime = toTime;
        }

            //public override string ToString()
            //{
            //    return $"Shift{{Name='{Name}', FromTime={FromTime}, ToTime={ToTime}, BeautySalonId={BeautySalonId}}}";
            //}

        public override bool Equals(object obj)
        {
            return obj is Shift shift &&
                    Name == shift.Name &&
                    FromTime == shift.FromTime &&
                    ToTime == shift.ToTime &&
                    BeautySalonId == shift.BeautySalonId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, FromTime, ToTime, BeautySalonId);
        }
    }
}

