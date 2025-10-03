using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_A.Models
{
    public partial class WorkSchedule
    {
        public int ScheduleId { get; set; }
        public int ManagerId { get; set; }
        public List<ScheduleItem> Items { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public WorkSchedule()
        {
            Items = new List<ScheduleItem>();
        }

        public WorkSchedule(int scheduleId, int managerId, string from, string to)
        {
            ScheduleId = scheduleId;
            ManagerId = managerId;
            From = from;
            To = to;
            Items = new List<ScheduleItem>();
        }

        //public override string ToString()
        //{
        //    return $"WorkSchedule{{ScheduleId={ScheduleId}, ManagerId={ManagerId}, From='{From}', To='{To}', ItemsCount={Items.Count}}}";
        //}

        public override bool Equals(object obj)
        {
            return obj is WorkSchedule schedule &&
                   ScheduleId == schedule.ScheduleId &&
                   ManagerId == schedule.ManagerId &&
                   From == schedule.From &&
                   To == schedule.To &&
                   EqualityComparer<List<ScheduleItem>>.Default.Equals(Items, schedule.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ScheduleId, ManagerId, From, To, Items);
        }
    }
}
