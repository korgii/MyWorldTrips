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

namespace MyWorldTrips
{
    [Activity(Label = "MyWorldTrips", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener, GoogleMap.IOnMyLocationButtonClickListener
    {
        private GoogleMap m_Map;
        private LocationManager locationManager;
        private string provider;

        //Users global location variable
        private MarkerOptions m_MarkerOptions;

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
            enableMyLocation();

            Location location = locationManager.GetLastKnownLocation(provider);
            if (location == null)
                Toast.MakeText(this, "No location found with your device", ToastLength.Short);
            else {
                m_MarkerOptions = UpdateMarker(location.Latitude, location.Longitude);
                m_Map.AddMarker(m_MarkerOptions);
            }

            m_Map.UiSettings.ZoomControlsEnabled = true;
            m_Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), 15f));
        }

        //Default user position. to be continued
        public MarkerOptions UpdateMarker (double lati, double longi)
        {
            m_MarkerOptions = new MarkerOptions();
            m_MarkerOptions.SetPosition(new LatLng(lati, longi));
            m_MarkerOptions.SetTitle(string.Format("Latitude:{0}, Longitude:{1}", lati, longi));
            m_MarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));

            return m_MarkerOptions;
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
            //Do what the fuck ever when location is changed. here's a marker that's supposed to move
            m_MarkerOptions = UpdateMarker(location.Latitude, location.Longitude);
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
    }
}

