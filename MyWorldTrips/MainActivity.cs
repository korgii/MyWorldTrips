using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using System;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Runtime;

namespace MyWorldTrips
{
    [Activity(Label = "MyWorldTrips", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        private GoogleMap m_Map;
        private LocationManager locationManager;
        private Spinner spinner;
        private string provider;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            spinner = FindViewById<Spinner>(Resource.Id.spinner);

            //Maybe move these somwhere else if they dont belong to map setup
            spinner.ItemSelected += Spinner_ItemSelected;
            locationManager = (LocationManager)this.GetSystemService(LocationService);
            provider = locationManager.GetBestProvider(new Criteria(), false);
            Location location = locationManager.GetLastKnownLocation(provider);
            if (location == null)
            {
                System.Diagnostics.Debug.WriteLine("No location");
            }

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

            //MarkerOptions mOptions = new MarkerOptions();
            //mOptions.SetPosition(new LatLng(65.0121, 25.4651));
            //mOptions.SetTitle("Oulu");
            //m_Map.AddMarker(mOptions);

            m_Map.UiSettings.ZoomControlsEnabled = true;
            m_Map.UiSettings.CompassEnabled = true;
            m_Map.MoveCamera(CameraUpdateFactory.ZoomIn());
        }

        public void OnLocationChanged(Location location)
        {
            Double lat, lng;
            lat = location.Latitude;
            lng = location.Longitude;

            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(lat, lng));
            markerOptions.SetTitle("My Position");
            m_Map.AddMarker(markerOptions);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(lat, lng));
            CameraPosition cameraPos = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPos);
            m_Map.MoveCamera(cameraUpdate);
        }

        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(provider, 400, 1, this);
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

