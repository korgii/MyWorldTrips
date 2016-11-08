using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Runtime;
using Android.Content;
using Android.Util;
using Android.Support.V4.Content;
using Android;
using System.Collections.Generic;

namespace MyWorldTrips
{
    [Activity(Label = "MyWorldTrips", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback, 
        ILocationListener, 
        GoogleMap.IOnMyLocationButtonClickListener, 
        GoogleMap.IOnMapLongClickListener, 
        GoogleMap.IOnMarkerClickListener
    {
        private GoogleMap m_Map;
        private LocationManager locationManager;
        private string provider;

        public List<CustomMarkerOptions> MarkerOptionsList { get; set; }

        public List<CustomMarkerOptions> m_CustomMarkerList = new List<CustomMarkerOptions >();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            //Maybe move these somwhere else if they dont belong to map setup
            locationManager = GetSystemService(Context.LocationService) as LocationManager;
            provider = locationManager.GetBestProvider(new Criteria(), false);

            //This is important
            SetUpMap();
        }

        public void SetUpMap()
        {
            if (m_Map == null)
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
        }

        //Called when GoogleMap object is ready
        public void OnMapReady(GoogleMap googleMap)
        {
            m_Map = googleMap; //make it global on this instance
            m_Map.SetOnMyLocationButtonClickListener(this);
            m_Map.SetOnMapLongClickListener(this);
            m_Map.SetOnMarkerClickListener(this);

            enableMyLocation();

            Location location = locationManager.GetLastKnownLocation(provider);
            if (location == null)
                Toast.MakeText(this, "No location found with your device", ToastLength.Short);

            m_Map.UiSettings.MapToolbarEnabled = false;
            m_Map.UiSettings.ZoomControlsEnabled = true;

            m_Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), 15f));
        }

        //Default user position. to be continued
        public MarkerOptions UpdateMarker (double lati, double longi)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(lati, longi));
            marker.SetTitle(string.Format("Latitude:{0}, Longitude:{1}, Time:{2}", lati, longi, DateTime.Today.ToShortDateString()));
            marker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));

            return marker;
        }

        private void enableMyLocation()
        {
            //check for access. this sucks
            //if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation))
            if (m_Map != null)          
                m_Map.MyLocationEnabled = true;
        }

        public void OnLocationChanged(Location location)
        {           
            Toast.MakeText(this, string.Format("Longitude:{0}, Altitude{1}", location.Longitude, location.Altitude), ToastLength.Short);
        }

        protected override void OnResume()
        {
            base.OnResume();
            string gpsProvider = LocationManager.GpsProvider;
            string netProvider = LocationManager.NetworkProvider;

            if (locationManager.IsProviderEnabled(gpsProvider))
                locationManager.RequestLocationUpdates(gpsProvider, 2000, 1, this);
            //DANGEROUSSS
            if (locationManager.IsProviderEnabled(netProvider))
                locationManager.RequestLocationUpdates(netProvider, 2000, 1, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public void OnProviderDisabled(string provider)
        {
            Toast.MakeText(this, string.Format("Provider:{0} disabled", provider), ToastLength.Short);
        }

        public void OnProviderEnabled(string provider)
        {
            Toast.MakeText(this, string.Format("Provider:{0} enabled", provider), ToastLength.Short);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            Toast.MakeText(this, string.Format("Status changed on provider:{0}", provider), ToastLength.Long);
        }

        public bool OnMyLocationButtonClick()
        {
            Toast.MakeText(this, "Your current location", ToastLength.Short).Show();
            return false;
        }

        public void OnMapLongClick(LatLng point)
        {
            var markerOptions = UpdateMarker(point.Latitude, point.Longitude);
            var marker = m_Map.AddMarker(markerOptions);

            m_CustomMarkerList.Add(new CustomMarkerOptions(markerOptions, marker));
        }

        public bool OnMarkerClick(Marker marker)
        {
            var foundMarker = m_CustomMarkerList.Find(p => p.Marker.Id == marker.Id);
            var time = foundMarker.MarkerTime.ToShortDateString();
            Toast.MakeText(this, string.Format("Time:{0}", time), ToastLength.Long);
            return false;
        }
    }
}

