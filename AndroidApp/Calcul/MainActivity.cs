using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ModelsLibrary;

namespace Calcul
{
    public class personal
    {
        public string name = "";
        public int age = 0;
        public float height = 0;
        public float weight = 0;
    }
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
    DrawerLayout drawer;
    NavigationView navigationView;
    FrameLayout setting, home; 
    personal user = new personal();
        int idd = 0;
    Android.Support.V7.Widget.Toolbar toolbar;
        private void Home_Create()
        {
            Button back_setting = new Button(home.Context);
            back_setting.LayoutParameters = new LinearLayout.LayoutParams(500, 500);
            back_setting.Text = "♦";
            back_setting.TranslationX = home.Width / 2;
            back_setting.SetBackgroundColor(Android.Graphics.Color.Indigo);
            back_setting.SetTextColor(Android.Graphics.Color.White);

            ImageView imag = new ImageView(home.Context);
            imag.LayoutParameters = new LinearLayout.LayoutParams(500, 500);
            imag.TranslationY = 600;

            TextView tx = new TextView(home.Context);
            tx.LayoutParameters = new LinearLayout.LayoutParams(500, 500);
            tx.TranslationX = 600;

            home.RemoveView(tx);
            home.AddView(tx);
            home.RemoveView(back_setting);
            home.AddView(back_setting);
            home.RemoveView(imag);
            home.AddView(imag);
            back_setting.Click += (sender, e) =>
            {

                WebClient client = new WebClient();
                string s = client.DownloadString("http://94353453b761.ngrok.io/recipes");
                List<Recipe> list = Serialization<Recipe>.ReadList(s);
                imag.SetImageBitmap(Android.Graphics.BitmapFactory.DecodeByteArray(list[0].Image, 0, list[0].Image.Length));
                tx.Text = list[0].Name + '\n' + list[0].Colories.ToString() + '\n' + list[0].Weight.ToString();
            };
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            setting = FindViewById<FrameLayout>(Resource.Id.manage);
            setting.Visibility = ViewStates.Invisible;
            home = FindViewById<FrameLayout>(Resource.Id.main);
            Home_Create();
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_home);
        }

        [Obsolete]
        private void Sett()
        {
            const int N = 4;
            personal ui = new personal();
            string[] text_sett_array = new string[N] { "Name:", "Age:", "Height:", "Weight:" };
            EditText[] edit_sett = new EditText[N];
            TextView[] text_sett = new TextView[N];
            setting.RemoveAllViews();
            {
                Button back_setting = new Button(toolbar.Context);
                back_setting.LayoutParameters = new LinearLayout.LayoutParams(100, 100);
                back_setting.Text = "♦";
                back_setting.TranslationX = toolbar.Width / 2;
                back_setting.SetBackgroundColor(Android.Graphics.Color.Indigo);
                back_setting.SetTextColor(Android.Graphics.Color.White);
                toolbar.AddView(back_setting);
                back_setting.Click += (sender, e) =>
                {
                    toolbar.RemoveView(back_setting);
                    setting.Visibility = ViewStates.Invisible;
                    user.name = edit_sett[0].Text;
                    user.age = ui.age;
                    user.height = ui.height;
                    user.weight = ui.weight;
                    home.Visibility = ViewStates.Visible;
                    navigationView.SetCheckedItem(Resource.Id.nav_home);
                };
                string[] data = new string[3] { "A", "B", "C" };
                Spinner better = new Spinner(setting.Context);
                better.LayoutParameters = new LinearLayout.LayoutParams(setting.Width / 2, 250); ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, data);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                better.Adapter = adapter;
                better.TranslationY = setting.Height / 2;
                setting.AddView(better);
                toolbar.NavigationClick += (s, ee) =>
                {
                    if (setting.Visibility != ViewStates.Visible) drawer.OpenDrawer(GravityCompat.Start);
                    else
                    {
                        View view = (View)s;
                        Snackbar.Make(view, "Click ♦)))", Snackbar.LengthLong)
                            .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                    }
                };
            }
            for (int i = 0; i < N; i++)
            {
                text_sett[i] = new TextView(setting.Context);
                text_sett[i].LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

                edit_sett[i] = new EditText(setting.Context);
                edit_sett[i].LayoutParameters = new LinearLayout.LayoutParams(setting.Width / 2, 100);
                if (i % 2 == 0)
                {
                    text_sett[i].TranslationY = i * 200;
                    if (i != 0) edit_sett[i].TranslationY = i * 250;
                    else edit_sett[i].TranslationY = 50;
                }
                else if (i != 0)
                {
                    text_sett[i].TranslationY = (i - 1) * 200;
                    text_sett[i].TranslationX = setting.Width / 2;
                    if (i != 1) edit_sett[i].TranslationY = (i - 1) * 250;
                    else edit_sett[i].TranslationY = 50;
                    edit_sett[i].TranslationX = setting.Width / 2;
                }
                edit_sett[i].TextSize = 15;
                text_sett[i].TextSize = 17;
                text_sett[i].Text = text_sett_array[i];

                FrameLayout sett_and = new FrameLayout(setting.Context);
                sett_and.LayoutParameters = new LinearLayout.LayoutParams(setting.Width - 100, setting.Width - 500);
                sett_and.SetBackgroundColor(Android.Graphics.Color.Indigo);
                sett_and.TranslationX = 50;
                sett_and.TranslationY = setting.Height / 2 - (setting.Width - 500) / 2;

                setting.AddView(text_sett[i]);
                setting.AddView(edit_sett[i]);
                setting.AddView(sett_and);
                sett_and.Visibility = ViewStates.Invisible;
                setting.Touch += (s_, e_) => { 
                    sett_and.Visibility = ViewStates.Invisible; };
                edit_sett[0].FocusChange += (s_, e_) => { if (sett_and.Visibility == ViewStates.Visible) sett_and.Visibility = ViewStates.Invisible; };
                if (i != 0)
                    edit_sett[i].Touch += (sender_, e_) =>
                    {
                        sett_and.Visibility = ViewStates.Invisible;
                        sett_and.RemoveAllViews();
                        if (sender_ == edit_sett[1])
                        {
                            TextView tx = new TextView(sett_and.Context);
                            tx.Text = text_sett_array[1];
                            tx.TextSize = 20;
                            NumberPicker num_age = new NumberPicker(sett_and.Context);
                            num_age.LayoutParameters = new LinearLayout.LayoutParams(sett_and.Width / 2, sett_and.Height - 200);
                            num_age.TranslationX = sett_and.Width / 4;
                            num_age.TranslationY = 50;
                            num_age.MaxValue = 99;
                            num_age.MinValue = 1;

                            Button sv_age = new Button(sett_and.Context);
                            sv_age.LayoutParameters = new LinearLayout.LayoutParams(250, 150);
                            sv_age.TranslationX = sett_and.Width - 275;
                            sv_age.TranslationY = sett_and.Height - 175;
                            sv_age.Text = "♦";
                            sv_age.Click += (s, ee) =>
                            {
                                edit_sett[1].Text = num_age.Value.ToString();
                                sett_and.Visibility = ViewStates.Invisible;
                                ui.age = num_age.Value;
                            };

                            sett_and.AddView(tx);
                            sett_and.AddView(num_age);
                            sett_and.AddView(sv_age);
                            sett_and.Visibility = ViewStates.Visible;
                        }
                        if (sender_ == edit_sett[2] || sender_ == edit_sett[3])
                        {
                            sett_and.RemoveAllViews();
                            sett_and.Visibility = ViewStates.Invisible;
                            TextView tx = new TextView(sett_and.Context);
                            tx.Text = text_sett_array[2];
                            tx.TextSize = 20;
                            NumberPicker num_h = new NumberPicker(sett_and.Context);
                            num_h.LayoutParameters = new LinearLayout.LayoutParams(sett_and.Width / 4, sett_and.Height - 200);
                            num_h.TranslationX = sett_and.Width / 2 - sett_and.Width / 4 - sett_and.Width / 32;
                            num_h.TranslationY = 50;
                            num_h.MaxValue = 299;
                            num_h.MinValue = 30;
                            NumberPicker num_l = new NumberPicker(sett_and.Context);
                            num_l.LayoutParameters = new LinearLayout.LayoutParams(sett_and.Width / 4, sett_and.Height - 200);
                            num_l.TranslationX = sett_and.Width / 2 + sett_and.Width / 32;// + sett_and.Width / 4;
                            num_l.TranslationY = 50;
                            num_l.MaxValue = 9;
                            num_l.MinValue = 0;

                            Button sv_age = new Button(sett_and.Context);
                            sv_age.LayoutParameters = new LinearLayout.LayoutParams(250, 150);
                            sv_age.TranslationX = sett_and.Width - 275;
                            sv_age.TranslationY = sett_and.Height - 175;
                            sv_age.Text = "♦";
                            sv_age.Click += (s, ee) =>
                            {
                                if (sender_ == edit_sett[2]) edit_sett[2].Text = num_h.Value.ToString() + "," + num_l.Value.ToString();
                                if (sender_ == edit_sett[3]) edit_sett[3].Text = num_h.Value.ToString() + "," + num_l.Value.ToString();
                                sett_and.Visibility = ViewStates.Invisible;
                                ui.height = num_h.Value + num_l.Value / 10;
                            };

                            sett_and.AddView(tx);
                            sett_and.AddView(num_h);
                            sett_and.AddView(num_l);
                            sett_and.AddView(sv_age);
                            sett_and.Visibility = ViewStates.Visible;
                        }
                    };
            }
            edit_sett[0].Text = user.name;
            if (user.age != 0) edit_sett[1].Text = user.age.ToString();
            if (user.height != 0) edit_sett[2].Text = user.height.ToString();
        }
            

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
                home.Visibility = ViewStates.Visible;
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //MenuInflater.Inflate(Resource.Menu.menu_main, menu);
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

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            setting.Visibility = ViewStates.Invisible;
            home.Visibility = ViewStates.Invisible;
            setting.RemoveAllViews();
            switch (item.ItemId)
            {
                case Resource.Id.nav_home:
                    home.Visibility = ViewStates.Visible;
                    return true;
                case Resource.Id.nav_gallery:
                    return true;
                case Resource.Id.nav_slideshow:
                    return true;
                case Resource.Id.nav_manage:
                    setting.Visibility = ViewStates.Visible; 
                    Sett();
                    return true;
                case Resource.Id.nav_share:
                    return true;
                case Resource.Id.nav_send:
                    return true;

            }
            return false;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

