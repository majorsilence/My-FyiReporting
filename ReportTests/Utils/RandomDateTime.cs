using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportTests.Utils
{
    public class RandomDateTime
    {
        DateTime start;
        Random gen = new Random();
        int range;

        public RandomDateTime(int yearStart,int monthStart,int dayStart)
        {
            start = new DateTime(yearStart, monthStart, dayStart);
            gen = new Random();
            range = (DateTime.Today - start).Days;
        }

        public DateTime Next()
        {
            return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
        }
    }
}
