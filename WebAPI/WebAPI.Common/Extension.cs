using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common
{
    public static class Extensions
    {
        public static DateTime ConvertFromUnixTimestamp(this string timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            double dTimeStamp = 0;
            if (double.TryParse(timestamp, out dTimeStamp))
            {
                try
                {
                    return origin.AddSeconds(dTimeStamp);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    return origin.AddMilliseconds(dTimeStamp);
                }
            }
            return origin;
        }
    }
}
