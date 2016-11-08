using Android.Gms.Maps.Model;
using Java.Text;
using Java.Util;

namespace MyWorldTrips
{
    public class CustomMarkerOptions
    {
        public Calendar MarkerCalendar { get; set; }
        public MarkerOptions MarkerOptions { get; set; }
        public Marker Marker { get; set; }

        public CustomMarkerOptions(MarkerOptions markerOptions, Marker marker)
        {
            Marker = marker;
            MarkerOptions = markerOptions;
            MarkerCalendar = Calendar.GetInstance(TimeZone.Default);
        }

        /// <summary>
        /// Returns the time using Calendar class
        /// </summary>
        /// <returns></returns>
        public string GetTimeString ()
        {
            //Could implement different time types, lengths, formats
            SimpleDateFormat asd = new SimpleDateFormat("dd.MM.yyy HH:mm:ss z");
            string asGmt = asd.Format(MarkerCalendar.Time);
            return asGmt;
        }
    }
}