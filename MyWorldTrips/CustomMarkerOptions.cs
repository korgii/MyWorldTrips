using Android.Gms.Maps.Model;
using Java.Text;
using Java.Util;

namespace MyWorldTrips
{
    /// <summary>
    /// Decided to abandon this shit. cant jsonparse googles classes though
    /// </summary>
    public class CustomMarkerOptions
    {
        public MarkerOptions MarkerOptions { get; set; }
        public string MarkerDate { get; set; }
        public Marker Marker { get; set; }

        public CustomMarkerOptions(MarkerOptions markerOptions, Marker marker)
        {
            MarkerDate = GetTimeString(Calendar.GetInstance(TimeZone.Default));
            MarkerOptions = markerOptions;
            Marker = marker;
        }

        /// <summary>
        /// Returns the time using Calendar class
        /// </summary>
        /// <returns></returns>
        public string GetTimeString (Calendar c)
        {
            //Could implement different time types, lengths, formats
            SimpleDateFormat asd = new SimpleDateFormat("dd.MM.yyy HH:mm:ss z");
            string asGmt = asd.Format(c.Time);
            return asGmt;
        }
    }
}