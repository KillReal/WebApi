using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ModelLibrary;

namespace Calcul
{
    public class personal
    {
        public string name = "";
        public int age = 0;
        public float height = 0;
        public float weight = 0;
        public int[] activity = new int[3] { 0, 0, 0 };
        public double Calculation_colories()
        {
            double[] k = new double[5] { 1.2, 1.375, 1.55, 1.725, 1.9 }; 
            double BMR;
            BMR = 66.56 + (11.3 * weight) + (3.95 * height) - (5 * age);
            for (int i = 0; i < 5; i++) if (activity[2] == i) BMR *= k[i];
            return BMR;
        }
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
            home.RemoveAllViews();
            List<Recipe> menu;
            string uri_ = "http://06ff2a3c3b68.ngrok.io";
            {
                WebClient client = new WebClient();
                menu = Serialization<Recipe>.ReadList(client.DownloadString(uri_ + "/recipes"));
            }
            int[] id_menu = new int[menu.Count];
            for (int i= 0; i < menu.Count; i++)
            {
                TextView tx_name = new TextView(home.Context);
                tx_name.LayoutParameters = new LinearLayout.LayoutParams(home.Width / 2 - 150, home.Height / 5);
                tx_name.TranslationY = (i + 1) * (home.Height / 5);
                tx_name.TranslationX = 25;
                tx_name.Text = menu[i].Name;
                tx_name.SetTextColor(Android.Graphics.Color.White);
                tx_name.TextSize = 17;
                tx_name.Gravity = GravityFlags.CenterVertical | GravityFlags.CenterHorizontal;
                home.AddView(tx_name);

                TextView tx_cal = new TextView(home.Context);
                tx_cal.LayoutParameters = new LinearLayout.LayoutParams(200, home.Height / 5);
                tx_cal.TranslationY = (i + 1) * (home.Height / 5);
                tx_cal.TranslationX = home.Width / 2 - 125;
                tx_cal.Text = menu[i].Colories.ToString() + "ккал.\n(" + menu[i].Greases.ToString() + ", " + 
                    menu[i].Proteins.ToString() + ", " + menu[i].Carbohydrates.ToString() + ")";
                tx_cal.SetTextColor(Android.Graphics.Color.White);
                tx_cal.TextSize = 17;
                tx_cal.Gravity = GravityFlags.CenterVertical | GravityFlags.CenterHorizontal;
                home.AddView(tx_cal);

                Button but_im = new Button(home.Context);
                but_im.LayoutParameters = new LinearLayout.LayoutParams(home.Width / 2 - 150, home.Height / 5);
                but_im.TranslationY = (i + 1) * (home.Height / 5);
                but_im.TranslationX = home.Width / 2 + 125;
                but_im.Text = "♦";
                but_im.Id = (int)menu[i].Id;
                but_im.Click += (s_, e_) => {
                    ImageView im = new ImageView(home.Context);
                    im.LayoutParameters = new LinearLayout.LayoutParams(home.Width / 2 - 150, home.Height / 5);
                    im.TranslationY = (but_im.Id) * (home.Height / 5);
                    im.TranslationX = home.Width / 2 + 125;
                    WebClient client = new WebClient();
                    byte[] s = client.DownloadData(uri_ + "/AdminRecipes/getImg/" + (menu[but_im.Id - 1].Id).ToString());
                    Bitmap bb = BitmapFactory.DecodeByteArray(s, 0, s.Length);
                    im.SetImageBitmap(bb);
                    but_im.Visibility = ViewStates.Invisible;
                    home.AddView(im);
                };
                home.AddView(but_im);
            }
            /*Button back_setting = new Button(home.Context);
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
            };*/
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
            home.Visibility = ViewStates.Invisible;
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_home);
        }

        TextView back_setting;
        private void Sett()
        {
            const int N = 7;
            personal ui = new personal();
            string[] text_sett_array = new string[N] { "Имя:", "Возраст:", "Рост:", "Текущий вес:", "Цель:", "Тип похудения:", "Физическая активность:" };
            EditText[] edit_sett = new EditText[N];
            TextView[] text_sett = new TextView[N];
            Spinner[] sp = new Spinner[N - 4];
            TextView text_cal;
            setting.RemoveAllViews();

            back_setting = new TextView(toolbar.Context);
            back_setting.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            back_setting.Text = "☑";
            back_setting.TextSize = 20;
            back_setting.Gravity = GravityFlags.CenterVertical | GravityFlags.Right;
            back_setting.TranslationX = -50;
            back_setting.SetTextColor(Android.Graphics.Color.White);
            toolbar.AddView(back_setting);
            back_setting.Click += (sender, e) =>
            {
                toolbar.RemoveView(back_setting);
                setting.Visibility = ViewStates.Invisible;
                user = ui;
                user.name = edit_sett[0].Text;
                for (int i = 0; i < 3; i++) user.activity[i] = (int)sp[i].SelectedItemId;
            };
            text_cal = new TextView(setting.Context);
            text_cal.LayoutParameters = new LinearLayout.LayoutParams(setting.Width, 250);
            text_cal.TranslationY = setting.Height - 250;
            text_cal.SetBackgroundColor(Android.Graphics.Color.Indigo);
            text_cal.Text = user.name + '\n' + user.age.ToString();
            setting.AddView(text_cal);

            FrameLayout sett_and = new FrameLayout(setting.Context);
            sett_and.LayoutParameters = new LinearLayout.LayoutParams(setting.Width - 100, setting.Width - 500);
            sett_and.SetBackgroundColor(Android.Graphics.Color.Indigo);
            sett_and.TranslationX = 50;
            sett_and.TranslationY = setting.Height / 2 - (setting.Width - 500) / 2;
            for (int i = 0; i < N; i++)
            {
                text_sett[i] = new TextView(setting.Context);
                text_sett[i].LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

                edit_sett[i] = new EditText(setting.Context);
                edit_sett[i].LayoutParameters = new LinearLayout.LayoutParams(setting.Width / 2, 100);
                if (i % 2 == 0)
                {
                    text_sett[i].TranslationY = i * 100;
                    if (i != 0) edit_sett[i].TranslationY = i * 150;
                    else edit_sett[i].TranslationY = 50;
                }
                else if (i != 0)
                {
                    text_sett[i].TranslationY = (i - 1) * 100;
                    text_sett[i].TranslationX = setting.Width / 2;
                    if (i != 1) edit_sett[i].TranslationY = (i - 1) * 150;
                    else edit_sett[i].TranslationY = 50;
                    edit_sett[i].TranslationX = setting.Width / 2;
                }
                edit_sett[i].TextSize = 15;
                text_sett[i].TextSize = 17;
                text_sett[i].Text = text_sett_array[i];

                setting.AddView(text_sett[i]);
                setting.AddView(edit_sett[i]);
                sett_and.Visibility = ViewStates.Invisible;
                setting.Touch += (s_, e_) => { if (sett_and.Visibility == ViewStates.Visible) sett_and.Visibility = ViewStates.Invisible; };
                //edit_sett[0].Touch += (s_, e_) => { if (sett_and.Visibility == ViewStates.Visible) sett_and.Visibility = ViewStates.Invisible; };
                text_sett[i].Touch += (s_, e_) => { if (sett_and.Visibility == ViewStates.Visible) sett_and.Visibility = ViewStates.Invisible; };
                if (i != 0)
                    edit_sett[i].Touch += (s_, e_) =>
                    {
                        sett_and.RemoveAllViews();
                        if (s_ == edit_sett[1])
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
                        }
                        else
                        {
                            sett_and.RemoveAllViews();
                            TextView tx = new TextView(sett_and.Context);
                            for (int j = 0; j < N; j++) if (s_ == edit_sett[j]) tx.Text = text_sett_array[j];
                            tx.TextSize = 20;
                            NumberPicker num_h = new NumberPicker(sett_and.Context);
                            num_h.LayoutParameters = new LinearLayout.LayoutParams(sett_and.Width / 4, sett_and.Height - 200);
                            num_h.TranslationX = sett_and.Width / 2 - sett_and.Width / 4 - sett_and.Width / 32;
                            num_h.TranslationY = 50;
                            num_h.MaxValue = 299;
                            num_h.MinValue = 30;
                            NumberPicker num_l = new NumberPicker(sett_and.Context);
                            num_l.LayoutParameters = new LinearLayout.LayoutParams(sett_and.Width / 4, sett_and.Height - 200);
                            num_l.TranslationX = sett_and.Width / 2 + sett_and.Width / 32;
                            num_l.TranslationY = 50;
                            num_l.MaxValue = 9;
                            num_l.MinValue = 0;

                            Button sv = new Button(sett_and.Context);
                            sv.LayoutParameters = new LinearLayout.LayoutParams(250, 150);
                            sv.TranslationX = sett_and.Width - 275;
                            sv.TranslationY = sett_and.Height - 175;
                            sv.Text = "♦";
                            sv.Click += (s, ee) =>
                            {
                                if (s_ == edit_sett[2]) edit_sett[2].Text = num_h.Value.ToString() + "," + num_l.Value.ToString();
                                if (s_ == edit_sett[3]) edit_sett[3].Text = num_h.Value.ToString() + "," + num_l.Value.ToString();
                                sett_and.Visibility = ViewStates.Invisible;
                                ui.height = num_h.Value + num_l.Value / 10;
                            };

                            sett_and.AddView(tx);
                            sett_and.AddView(num_h);
                            sett_and.AddView(num_l);
                            sett_and.AddView(sv);
                        }
                        sett_and.Visibility = ViewStates.Visible;
                    };
            }
            List<string[]> data = new List<string[]>();
            string[] item = new string[3] { "Поддержать вес", "Сбросить вес", "Набрать вес" };
            data.Add(item);
            item = new string[3] { "Медленное", "Нормальное", "Быстрое" };
            data.Add(item);
            item = new string[5] { "Минимум или отсутствие физической нагрузки",
                "Занятие фитнесом 3 - 5 раз в неделю",
                "Интенсивная тренировка 5 раз в неделю",
                "Занятия фитнесом каждый день",
                "Ежедневная интенсивная нагрузка"};
            data.Add(item);
            for (int i = 0; i < 3; i++)
            {
                sp[i] = new Spinner(setting.Context);
                if (i != 2) sp[i].LayoutParameters = new LinearLayout.LayoutParams(setting.Width / 2, 250);
                else sp[i].LayoutParameters = new LinearLayout.LayoutParams(setting.Width, 250);
                if (i != 2) sp[i].TranslationY = 425;
                else sp[i].TranslationY = 625;
                ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, data[i]);
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                sp[i].Adapter = adapter;
                setting.AddView(sp[i]);
                setting.RemoveView(edit_sett[N - 1 - i]);
            }
            text_sett[N - 2].Visibility = ViewStates.Invisible;
            sp[1].TranslationX = setting.Width / 2;
            sp[1].Visibility = ViewStates.Invisible;
            sp[0].ItemSelected += (s_, e_) =>
            {
                if (sp[0].SelectedItemId == 1)
                {
                    text_sett[N - 2].Visibility = ViewStates.Visible;
                    sp[1].Visibility = ViewStates.Visible;
                }
                else
                {
                    text_sett[N - 2].Visibility = ViewStates.Invisible;
                    sp[1].Visibility = ViewStates.Invisible;
                }
            };
            edit_sett[0].Text = user.name;
            setting.AddView(sett_and);
            if (user.age != 0) edit_sett[1].Text = user.age.ToString();
            if (user.height != 0) edit_sett[2].Text = user.height.ToString();
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
            home.Visibility = ViewStates.Invisible;
            setting.RemoveAllViews();
            toolbar.RemoveView(back_setting);
            switch (item.ItemId)
            {
                case Resource.Id.nav_home:
                    home.Visibility = ViewStates.Visible;
                    Home_Create();
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

