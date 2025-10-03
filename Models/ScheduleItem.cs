using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class ScheduleItem
    {
        public string DayOfWeek { get; set; }
        public int BeauticianId { get; set; }
        public Shift Shift { get; set; }
        public int ScheduleId { get; set; }

        public ScheduleItem() { }

        public ScheduleItem(string dayOfWeek, int beauticianId, Shift shift, int scheduleId)
        {
            DayOfWeek = dayOfWeek;
            BeauticianId = beauticianId;
            Shift = shift;
            ScheduleId = scheduleId;
        }

        //public override string ToString()
        //{
        //    return $"ScheduleItem{{DayOfWeek='{DayOfWeek}', BeauticianId={BeauticianId}, Shift={Shift}, ScheduleId={ScheduleId}}}";
        //}

        public override bool Equals(object obj)
        {
            return obj is ScheduleItem item &&
                   DayOfWeek == item.DayOfWeek &&
                   BeauticianId == item.BeauticianId &&
                   EqualityComparer<Shift>.Default.Equals(Shift, item.Shift) &&
                   ScheduleId == item.ScheduleId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DayOfWeek, BeauticianId, Shift, ScheduleId);
        }
    }
}
