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

namespace MyWorldTrips
{
    [Activity(Label = "MyWorldTrips", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        private GoogleMap m_Map;
        private LocationManager locationManager;
        private Spinner spinner;
        private string provider;

        //Users global location variable
        private MarkerOptions m_MarkerOptions;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            spinner = FindViewById<Spinner>(Resource.Id.spinner);

            //Maybe move these somwhere else if they dont belong to map setup
            spinner.ItemSelected += Spinner_ItemSelected;
            locationManager = GetSystemService(Context.LocationService) as LocationManager;
            provider = locationManager.GetBestProvider(new Criteria(), false);

            SetUpMap();
        }

        public void SetUpMap()
        {
            if (m_Map == null)
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            switch(e.Position)
            {
                case 0:
                    m_Map.MapType = GoogleMap.MapTypeHybrid;
                    break;
                case 1:
                    m_Map.MapType = GoogleMap.MapTypeNone;
                    break;
                case 2:
                    m_Map.MapType = GoogleMap.MapTypeNormal;
                    break;
                case 3:
                    m_Map.MapType = GoogleMap.MapTypeSatellite;
                    break;
                case 4:
                    m_Map.MapType = GoogleMap.MapTypeTerrain;
                    break;
                default:
                    m_Map.MapType = GoogleMap.MapTypeNone;
                    break;
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            m_Map = googleMap;

            Location location = locationManager.GetLastKnownLocation(provider);
            if (location == null)
                System.Diagnostics.Debug.WriteLine("No location");
            else
            {
                m_MarkerOptions = UpdateMarker(location.Latitude, location.Longitude);
                m_Map.AddMarker(m_MarkerOptions);
            }

            m_Map.UiSettings.ZoomControlsEnabled = true;
            m_Map.UiSettings.CompassEnabled = true;
            m_Map.UiSettings.MyLocationButtonEnabled = true;
            m_Map.UiSettings.MapToolbarEnabled = true;
            m_Map.MoveCamera(CameraUpdateFactory.ZoomIn());
        }

        //Default user position. to be continued
        public MarkerOptions UpdateMarker (double lati, double longi)
        {
            m_MarkerOptions = new MarkerOptions();
            m_MarkerOptions.SetPosition(new LatLng(lati, longi));
            m_MarkerOptions.SetTitle("My Position");
            m_MarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));

            return m_MarkerOptions;
        }

        public void OnLocationChanged(Location location)
        {           
            m_MarkerOptions = UpdateMarker(location.Latitude, location.Longitude);

            //Moves the camera to the location but zoom is still fucked
            float zuum = m_Map.MinZoomLevel + 1.0f;
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(new LatLng(location.Latitude, location.Longitude), zuum);
            m_Map.AnimateCamera(cameraUpdate);
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
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
    }
}

