using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Hardware;
using Android.Content;
using Android.Runtime;

namespace KeyLogger.AndroidSensor
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity, ISensorEventListener
	{
        private TextView _tv;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

            _tv = (TextView)FindViewById(Resource.Id.tv);

            var sensorManager = (SensorManager)GetSystemService(SensorService);
            var sensor = sensorManager.GetDefaultSensor(SensorType.LinearAcceleration);
            sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        public void OnSensorChanged(SensorEvent e)
        {
            var str = string.Empty;
            foreach (var value in e.Values)
            {
                str += value.ToString() + "\n";
            }
            _tv.SetText(str, TextView.BufferType.Normal);
        }
    }
}

