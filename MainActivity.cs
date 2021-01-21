using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace BtcUsdCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        
        double priceInUSD;
        EditText btcs;
        EditText usds;
        TextView currPrice;
        Button btn_updateprice;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Match references from views in this class
            currPrice = FindViewById<TextView>(Resource.Id.currPrice);
            btcs = FindViewById<EditText>(Resource.Id.btcs);
            usds = FindViewById<EditText>(Resource.Id.usds);
            btn_updateprice = FindViewById<Button>(Resource.Id.btn_updateprice);
            
            GetCurrentPriceUsd();

            // Subscribe to event
            btn_updateprice.Click += UpdatePriceOnClick;
            btcs.TextChanged += CalculateUSDs;
            usds.TextChanged += CalculateBTCs;
            
        }

        void UpdatePriceOnClick(object sender, EventArgs e)
        {
            GetCurrentPriceUsd();
        }
        
        void CalculateUSDs(object sender, EventArgs e)
        {
            usds.TextChanged -= CalculateBTCs;
            usds.Text = "";
            double inBtcs;

            if (!(string.IsNullOrEmpty(btcs.Text)) && 
                double.TryParse(btcs.Text, out inBtcs))
            {
                double outUSDs = priceInUSD * inBtcs;
                usds.Text = outUSDs.ToString();
            }

            usds.TextChanged += CalculateBTCs;
        }
        void CalculateBTCs(object sender, EventArgs e)
        {
            btcs.TextChanged -= CalculateUSDs;
            btcs.Text = "";
            double inUSDs;

            if (!(string.IsNullOrEmpty(usds.Text)) &&
                double.TryParse(usds.Text, out inUSDs))
            {
                double outBTCs = inUSDs / priceInUSD;
                btcs.Text = outBTCs.ToString();
            }

            btcs.TextChanged += CalculateUSDs;
        }
        
        // Perform request to get usds value and set the price to be used by calculator.
        public async void GetCurrentPriceUsd()
        {
            RestService _restService = new RestService();
            BTCtoUSDto result = await _restService.GetCurrentPrice(Constants.btcEndpoint);
            currPrice.Text = result.Price.ToString();
            priceInUSD = Convert.ToDouble(result.Price);
            // System.Console.WriteLine("price: --> " + priceInUSD.ToString());
            // By default start calculation on 1 btc, done here because need to wait 
            // RestService get the price for caculations.
            btcs.Text = "1";
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

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
