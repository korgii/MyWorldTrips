using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace MyWorldTrips
{
    [Activity(Label = "AddMarkerActivity")]
    public class AddMarkerActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddMarkerActivity);

            Button button = (Button)FindViewById(Resource.Id.button1);
            button.Text = "Save information";

            TextView view1 = (TextView)FindViewById(Resource.Id.textView1);
            view1.Text = string.Format("Markers date: {0}", Intent.GetStringExtra("Time"));


            var longi = Intent.GetDoubleExtra("Longitude", 0);
            var lati = Intent.GetStringExtra("Latitude", 0);
            TextView view2 = (TextView)FindViewById(Resource.Id.textView2);
            view2.Text = string.Format("Longitude: {0} Latitude: {1}", longi, lati);

            button.Click += delegate
            {               
                base.OnBackPressed();  
            };

        }
    }
}