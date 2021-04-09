﻿using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Contexts;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Net;

namespace Calcul
{
    public class personal
    {
        public string name = "";
        public int age = 0;
        public float heigt = 0;
    }
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
    DrawerLayout drawer;
    NavigationView navigationView;
    FrameLayout setting; 
    personal user = new personal();
    Android.Support.V7.Widget.Toolbar toolbar;
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
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
        }

        [Obsolete]
        private void Sett()
        {
            const int N = 3;
            personal ui = new personal();
            string[] text_sett_array = new string[N] { "Name:", "Age:", "Heaight:" };
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
                    user.heigt = ui.heigt;
                    navigationView.Visibility = ViewStates.Visible;
                };
                toolbar.NavigationClick += (s, ee) =>
                {
                    if (setting.Visibility == ViewStates.Visible)
                        navigationView.Visibility = ViewStates.Invisible;
                    else
                        navigationView.Visibility = ViewStates.Visible;
                };
            }
            for (int i = 0; i < N; i++)
            {
                text_sett[i] = new TextView(setting.Context);
                text_sett[i].LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

                edit_sett[i] = new EditText(setting.Context);
                if (i != 0) edit_sett[i].LayoutParameters = new LinearLayout.LayoutParams(setting.Width / 2, 100);
                else edit_sett[i].LayoutParameters = new LinearLayout.LayoutParams(setting.Width, 100);
                if (i % 2 != 0)
                {
                    text_sett[i].TranslationY = i * 200;
                    edit_sett[i].TranslationY = i * 250;
                }
                else if (i != 0)
                {
                    text_sett[i].TranslationY = (i - 1) * 200;
                    text_sett[i].TranslationX = setting.Width / 2;
                    edit_sett[i].TranslationY = (i - 1) * 250;
                    edit_sett[i].TranslationX = setting.Width / 2;
                }
                else edit_sett[i].TranslationY = 50;
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
                        if (sender_ == edit_sett[2])
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
                                edit_sett[2].Text = num_h.Value.ToString() + "," + num_l.Value.ToString();
                                sett_and.Visibility = ViewStates.Invisible;
                                ui.heigt = num_h.Value + num_l.Value / 10;
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
            if (user.heigt != 0) edit_sett[2].Text = user.heigt.ToString();
        }
            

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
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
            setting.RemoveAllViews();
            switch (item.ItemId)
            {
                case Resource.Id.nav_camera:
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

