using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps.Model;

namespace MyWorldTrips
{
    public class CustomMarkerOptions
    {
        public DateTime MarkerTime { get; set; }
        public MarkerOptions MarkerOptions { get; set; }
        public Marker Marker { get; set; }

        public DateTime m_MarkerTime;

        public CustomMarkerOptions(MarkerOptions markerOptions, Marker marker)
        {
            Marker = marker;
            MarkerOptions = markerOptions;
            MarkerTime = DateTime.Today;
        }

    }
}