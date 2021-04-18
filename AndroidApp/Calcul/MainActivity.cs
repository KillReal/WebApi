﻿using System;
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
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using ModelLibrary;
using SkiaSharp;
using SkiaSharp.Views.Android;

namespace Calcul
{
    public class personal
    {
        public string name = "";
        public int age = 0;
        public double height = 0;
        public double current_weight = 0;
        public double target_weight = 0;
        public int food = 0; // food {нет, веган, вегетарианец}
        public int purpose = 1; // purpose {сбросить вес, поддержать вес, набрать вес}
        public int activity = 0; // activity {низкий, средний, высокий, очень высокий}
        public bool gender = true; // true - Ж, false - М
        public string Convert_purpose()
        {
            switch (purpose)
            {
                case 0: return "Сбросить вес";
                case 1: return "Поддержать вес";
                case 2: return "Набрать вес";
            }
            return "";
        }
        public string Convert_activity()
        {
            switch (activity)
            {
                case 0: return "Низкий";
                case 1: return "Средний";
                case 2: return "Высокий";
                case 3: return "Очень высокий";
            }
            return "";
        }
        public double Calculation_colories()
        {
            double[] k = new double[4] { 1.2, 1.375, 1.55, 1.725 }; 
            double BMR;
            BMR = 66.56 + (11.3 * current_weight) + (3.95 * height) - (5 * age);
            for (int i = 0; i < 4; i++) if (activity == i) BMR *= k[i];
            return BMR;
        }
        public async void RequestAsync ()
        {
            List<Recipe> menu;
            string uri_ = "http://06ff2a3c3b68.ngrok.io";
            {
                WebClient client = new WebClient();
                Uri add = new Uri(uri_ + "/recipes");
                menu = Serialization<Recipe>.ReadList(await client.DownloadStringTaskAsync(add));
            }
        }
    }
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
    DrawerLayout drawer;
    NavigationView navigationView;
    FrameLayout setting, home;
        RelativeLayout basis;
    personal user = new personal();
        int idd = 0;
    Android.Support.V7.Widget.Toolbar toolbar;

        public FrameLayout Create_framelayout(int w_, int h_, float t_x, float t_y, Android.Graphics.Color color)
        {
            FrameLayout framelayout = new FrameLayout(drawer.Context);
            framelayout.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            framelayout.TranslationX = t_x;
            framelayout.TranslationY = t_y;
            framelayout.SetBackgroundColor(color);
            return framelayout;
        }
        public Button Create_button(int w_, int h_, float t_x, float t_y, string t_, GravityFlags flag, TypefaceStyle style, int size)
        {
            Button button = new Button(drawer.Context);
            button.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            button.TranslationX = t_x;
            button.TranslationY = t_y;
            button.Text = t_;
            button.SetTextColor(Android.Graphics.Color.White);
            button.Gravity = flag;
            button.TextSize = size;
            button.SetTypeface(Typeface.SansSerif, style);
            //button.SetBackgroundColor(Android.Graphics.Color.Indigo);
            return button;
        }
        public ImageView Create_imageview(int w_, int h_, float t_x, float t_y)
        {
            ImageView imageview = new ImageView(drawer.Context);
            imageview.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            imageview.TranslationX = t_x;
            imageview.TranslationY = t_y;
            imageview.Click += (s_, e_) => { };
            //button.SetBackgroundColor(Android.Graphics.Color.Indigo);
            return imageview;
        }
        public TextView Create_textview(int w_, int h_, float t_x, float t_y, string t_, GravityFlags flag, TypefaceStyle style, int size)
        {
            TextView textview = new TextView(drawer.Context);
            textview.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            textview.Text = t_;
            textview.TranslationX = t_x;
            textview.TranslationY = t_y;
            textview.SetTextColor(Android.Graphics.Color.White);
            textview.Gravity = flag;
            textview.TextSize = size;
            textview.SetTypeface(Typeface.SansSerif, style);
            //textview.SetBackgroundColor(Android.Graphics.Color.Indigo);
            return textview;
        }
        public EditText Create_edittext(int w_, int h_, float t_x, float t_y, string t_, GravityFlags flag, TypefaceStyle style, int size)
        {
            EditText edittext = new EditText(drawer.Context);
            edittext.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            edittext.Text = t_;
            edittext.TranslationX = t_x;
            edittext.TranslationY = t_y;
            edittext.SetTextColor(Android.Graphics.Color.White);
            edittext.Gravity = flag;
            edittext.TextSize = size;
            edittext.SetTypeface(Typeface.SansSerif, style);
            //edittext.SetBackgroundColor(Android.Graphics.Color.Indigo);
            return edittext;
        }
        public Spinner Create_spinner(int w_, int h_, string[] t_, float t_x, float t_y) 
        {
            Spinner spinner = new Spinner(drawer.Context);
            spinner.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            spinner.TranslationX = t_x;
            spinner.TranslationY = t_y;
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, t_);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            return spinner;
        }
        public NumberPicker Create_numberpicker(int w_, int h_, int[] t_, float t_x, float t_y)
        {
            NumberPicker numberpicker = new NumberPicker(drawer.Context);
            numberpicker.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            numberpicker.Value = t_[0];
            numberpicker.MinValue = t_[1];
            numberpicker.MaxValue = t_[2];
            numberpicker.TranslationX = t_x;
            numberpicker.TranslationY = t_y;
            return numberpicker;
        }
        public SKCanvasView Create_canvasview(int w_, int h_, float t_x, float t_y)
        {
            double k = user.target_weight - user.current_weight; //цель - текущее
            SKColor background = new SKColor(0x99, 0x99, 0x99); //#999999
            SKColor graf =  new SKColor(0xCC, 0xCC, 0xCC); //#CCCCCC

            var canvasView = new SKCanvasView(basis.Context);
            canvasView.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            canvasView.TranslationX = t_x;
            canvasView.TranslationY = t_y;
            canvasView.PaintSurface += (s_, e_) =>
            {
                float a = 25;
                float b = h_ - 50;
                float d = (b - a);
                float c = (b + a) / 2;
                // получаем текущую поверхность из аргументов
                var surface = e_.Surface;
                // Получаем холст на котором будет рисовать
                var canvas = surface.Canvas;
                // Создаем основу пути
                var pathStroke = new SKPaint
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    Color = graf,
                    StrokeWidth = 10
                };
                // Создаем путь
                var path = new SKPath();
                if (k < 0) k = a; else if (k > 0) k = b; else k = (a + b) / 2;
                SKPoint x1 = new SKPoint(w_ / 4 - 25, d);
                SKPoint x2 = new SKPoint(w_ / 2 - 25, c);
                SKPoint x3 = new SKPoint(w_ - 50, b - (float)k + 25);
                path.MoveTo(25, (float)k);
                path.CubicTo(x1, x2, x3);
                canvas.DrawCircle(25, (float)k, 5, pathStroke);
                canvas.DrawCircle(w_ - 50, b - (float)k + 25, 5, pathStroke);
                // Рисуем путь
                canvas.DrawPath(path, pathStroke);
            };
            return canvasView;
        }

        List<Recipe> menu; string uri_ = "http://99349f52c6d7.ngrok.io";
        
        public async void Request_data()
        {
            WebClient client = new WebClient();
            Uri add = new Uri(uri_ + "/recipes");
            menu = Serialization<Recipe>.ReadList(await client.DownloadStringTaskAsync(add));
        }
        public Bitmap Request_image(Button imm)
        {
            WebClient client = new WebClient();
            byte[] s = client.DownloadData(uri_ + "/AdminRecipes/getImg/" + (menu[imm.Id - 1].Id).ToString());
            Bitmap bb = BitmapFactory.DecodeByteArray(s, 0, s.Length);
            return bb;
        }
        //Профильный framelayout
        public void Create_ProfileFramelayout()
        {
            toolbar.RemoveAllViews();
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
            TextView prof = Create_textview(toolbar.Width, toolbar.Height, 0, 0,
                "Профиль", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
            toolbar.AddView(prof);
            basis.RemoveAllViews();

            FrameLayout profile = Create_framelayout(basis.Width, basis.Height, 0, 0, Android.Graphics.Color.Aqua);
            profile.RemoveAllViews();

            FrameLayout user_name = Create_framelayout(basis.Width - 100, basis.Height / 5, 50, 50, Android.Graphics.Color.DarkOrange);
            user_name.SetBackgroundResource(Resource.Drawable.draver_fram);
            user_name.RemoveAllViews();
            TextView user_name_textview = Create_textview((basis.Width - 100) * 2 / 3, basis.Height / 5, (basis.Width - 100) / 3, 0,
                user.name.ToUpper(), GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 25);
            user_name.AddView(user_name_textview);
            ImageView user_image = Create_imageview((basis.Width - 100) / 6, basis.Height / 10, (basis.Width - 100) / 12, basis.Height / 20);
            user_image.SetImageResource(Resource.Drawable.design_ic_visibility_off);
            user_name.AddView(user_image); 
            TextView setting_text = Create_textview(150, 150, basis.Width - 250, 0,
                 "⚙", GravityFlags.Center, TypefaceStyle.Bold, 30);
            setting_text.Click += (s_, e_) => { Create_SettingFramelayout(); };
            user_name.AddView(setting_text);

            Button statistics_button = Create_button(basis.Width / 2, 250, 0, basis.Height / 5 + 100, "Статистика", GravityFlags.Center, TypefaceStyle.Bold, 15);
            Button history_button = Create_button(basis.Width / 2, 250, basis.Width / 2, basis.Height / 5 + 100, "История", GravityFlags.Center, TypefaceStyle.Bold, 15);

            FrameLayout user_grafic = Create_framelayout(basis.Width - 100, basis.Height / 5 + 100, 50, basis.Height / 5 + 400, Android.Graphics.Color.DarkOrange);
            user_grafic.SetBackgroundResource(Resource.Drawable.draver_fram);
            user_grafic.RemoveAllViews();
            SKCanvasView graficview = Create_canvasview(basis.Width - 200, basis.Height / 5 - 100, 100, 75);
            user_grafic.AddView(graficview);
            {
                TextView head_ = Create_textview(basis.Width - 100, 75, 0, 0, 
                    "Ц Е Л Ь", GravityFlags.Center, TypefaceStyle.Bold, 20);
                user_grafic.AddView(head_);
            }
            {
                TextView head_ = Create_textview(basis.Width - 100, 75, 0, basis.Height / 5, 
                    "> >   " + user.Convert_purpose().ToUpper() + "   < <", GravityFlags.Center, TypefaceStyle.BoldItalic, 15);
                user_grafic.AddView(head_);
            }
            for (int i = 0; i < 2; i++)
            {
                double[] k = new double[3] {user.current_weight, user.target_weight, user.target_weight - user.current_weight };
                if (k[2] < 0) k[2] = 75; else if (k[2] > 0) k[2] = basis.Height / 5 - 100; else k[2] = (basis.Height / 5 - 25) / 2;
                if (i != 0) k[2] = basis.Height / 5 - 100 - k[2] + 75;
                TextView a = Create_textview(basis.Width - 100, 75, 10, (float)k[2], 
                    k[i].ToString(), GravityFlags.Left, TypefaceStyle.Italic, 15);
                user_grafic.AddView(a);
            }

            {
                TextView head_ = Create_textview(basis.Width, 75, 0, ((basis.Height / 5) * 2) + 550,
                      "Л И Ч Н Ы Е   Д А Н Н Ы Е", GravityFlags.Center, TypefaceStyle.Bold, 20);
                profile.AddView(head_);
            }
            {
                string s = $"\nВОЗРАСТ   >>   {user.age.ToString()} \n\nПОЛ   >>   {user.gender} " +
                    $"\n\nРОСТ   >>   {user.height.ToString()} \n\nТЕКУЩИЙ ВЕС   >>   {user.current_weight.ToString()} " +
                    $"\n\nУРОВЕНЬ АКТИВНОСТИ   >>   {user.Convert_activity()}";
                TextView head_ = Create_textview(basis.Width - 20, 2 * (basis.Height / 5), 10, ((basis.Height / 5) * 2) + 625,
                      s, GravityFlags.Left, TypefaceStyle.Normal, 15);
                profile.AddView(head_);
            }

            profile.AddView(history_button);
            profile.AddView(statistics_button);
            profile.AddView(user_name);
            profile.AddView(user_grafic);
            basis.AddView(profile);
        }
        //Настройки framelayout
        private void Create_SettingFramelayout()
        {
            FrameLayout add_ = Create_framelayout(basis.Width - 100, (basis.Width - 100) / 2, 50, (basis.Height - (basis.Width - 100)) / 2, Color.DarkOrange);
            add_.SetBackgroundResource(Resource.Drawable.draver_fram);
            add_.Visibility = ViewStates.Invisible;
            FrameLayout setting = Create_framelayout(basis.Width, basis.Height, 0, 0, Color.Aqua);
            toolbar.RemoveAllViews();
            Button back = Create_button(toolbar.Height, toolbar.Height, -(int)(toolbar.Height * 1.25), 0, "<<", GravityFlags.Center, TypefaceStyle.Bold, 20);
            back.SetBackgroundDrawable(toolbar.Background);
            back.Click += (s_, e_) => { basis.RemoveView(setting); Create_ProfileFramelayout(); };
            toolbar.AddView(back);
            TextView prof = Create_textview(toolbar.Width, toolbar.Height, -(int)(toolbar.Height * 1.25), 0,
                "Настройки", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
            toolbar.AddView(prof);


            TextView head_ = Create_textview(basis.Width, 100, 0, 10,
                  "Ц Е Л Ь", GravityFlags.Center, TypefaceStyle.Bold, 20);
            setting.AddView(head_);

            Button target_button = Create_button(basis.Width - 50, 150, 25, 417,
                "Ц е л е в о й   в е с", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            if (user.purpose == 1) target_button.Visibility = ViewStates.Invisible;
            Button current_button = Create_button(basis.Width - 50, 150, 25, 266,
                "Т е к у щ и й   в е с", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            current_button.Click += Change_weight;
            target_button.Click += Change_weight;

            void Change_weight(object sender, EventArgs eventArgs)
            {
                double k = user.current_weight;
                string s = "Т е к у щ и й   в е с";
                if (sender == target_button)
                {
                    k = user.target_weight;
                    s = "Ц е л е в о й   в е с";
                }
                add_.RemoveAllViews();
                TextView h_ = Create_textview(basis.Width - 100, 100, 0, 10,
                    s, GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                EditText text = Create_edittext(basis.Width - 350, (basis.Width - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 50);
                text.InputType = Android.Text.InputTypes.ClassNumber;
                text.TextChanged += (s__, e__) => { if (text.Text != "") k = Convert.ToDouble(text.Text); else k = 0; };
                Button plus = Create_button(100, 100, 50, ((basis.Width - 100) / 2 - 100) / 2 - 50,
                    "+", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                plus.SetBackgroundResource(Resource.Drawable.draver_fram);
                plus.Click += (s__, e__) => {
                    k += 0.1;
                    k = Math.Round(k * 10) / 10;
                    text.Text = k.ToString();
                };
                Button minus = Create_button(100, 100, (basis.Width - 250), ((basis.Width - 100) / 2 - 100) / 2 - 50,
                    "−", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                minus.SetBackgroundResource(Resource.Drawable.draver_fram);
                minus.Click += (s__, e__) => {
                    if (k > 30) k -= 0.1;
                    k = Math.Round(k * 10) / 10;
                    text.Text = k.ToString();
                };
                Button save = Create_button(basis.Width - 200, 150, 50, ((basis.Width - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) => {
                    if (sender == current_button) user.current_weight = k;
                    else user.target_weight = k;
                    add_.Visibility = ViewStates.Invisible;
                    text.Focusable = false;
                };
                add_.AddView(plus);
                add_.AddView(minus);
                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(text);
                add_.Visibility = ViewStates.Visible;
            }

            Button purpose_button = Create_button(basis.Width - 50, 150, 25, 115,
                "М о я   ц е л ь", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            purpose_button.Click += (s_, e_) =>
            {
                add_.RemoveAllViews();
                for (int i = 0; i < 3; i++)
                {
                    string[] st = new string[3] { "сбросить вес", "поддержать вес", "набрать вес" };
                    int k = (basis.Width - 100) / 6 - 15;
                    TypefaceStyle ts = TypefaceStyle.Normal;
                    if (i == user.purpose) ts = TypefaceStyle.Italic;
                    Button bt = Create_button(basis.Width - 200, k, 50, i * k + 15, st[i], GravityFlags.Center, ts, 15);
                    bt.Id = i;
                    bt.SetBackgroundDrawable(purpose_button.Background);
                    if (bt.Id == user.purpose) bt.SetBackgroundColor(Color.Coral);
                    bt.Click += (s__, e__) =>
                    {
                        bt.SetBackgroundColor(Color.Coral);
                        user.purpose = bt.Id;
                        if (user.purpose == 1) target_button.Visibility = ViewStates.Invisible; else target_button.Visibility = ViewStates.Visible;
                        add_.Visibility = ViewStates.Invisible;
                    };
                    add_.AddView(bt);
                }
                add_.Visibility = ViewStates.Visible;
            };
            head_ = Create_textview(basis.Width, 100, 0, 582,
                              "Л И Ч Н Ы Е   Д А Н Н Ы Е", GravityFlags.Center, TypefaceStyle.Bold, 20);
            setting.AddView(head_);

            Button name_button = Create_button(basis.Width - 50, 150, 25, 687,
                "И м я", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            name_button.Click += (s, e) =>
            {
                string k = user.name;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(basis.Width - 100, 100, 0, 10,
                    "П о л", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);

                EditText name_ = Create_edittext(basis.Width - 350, (basis.Width - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 35);
                name_.InputType = Android.Text.InputTypes.ClassText;

                name_.TextChanged += (s__, e__) => {
                    if (name_.Length() > 10) name_.Text = name_.Text.Remove(name_.Text.Length - 1, 1);
                    name_.SetSelection(name_.Text.Length);
                    if (name_.Text != "") k = name_.Text; };

                Button save = Create_button(basis.Width - 200, 150, 50, ((basis.Width - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    user.name = name_.Text;
                    add_.Visibility = ViewStates.Invisible;
                    name_.Focusable = false;
                };
                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(name_);
                add_.Visibility = ViewStates.Visible;
            };
            Button genger_button = Create_button(basis.Width - 50, 150, 25, 838,
                "П о л", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            genger_button.Click += (s, e) =>
            {
                bool flag = user.gender;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(basis.Width - 100, 100, 0, 10,
                    "П о л", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                Button save = Create_button(basis.Width - 200, 150, 50, ((basis.Width - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    user.gender = flag;
                    add_.Visibility = ViewStates.Invisible;
                };

                Button w_ = Create_button((basis.Width - 100) / 2 - 25, 150, 25, 150,
                    "Ж", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                Button m_ = Create_button((basis.Width - 100) / 2 - 25, 150, (basis.Width - 100) / 2, 150,
                    "М", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                if (flag == false) m_.SetBackgroundColor(Color.Coral);
                else w_.SetBackgroundColor(Color.Coral);
                w_.Click += Change_gender;
                m_.Click += Change_gender;

                void Change_gender(object sender, EventArgs eventArgs) {
                    m_.SetBackgroundDrawable(save.Background);
                    w_.SetBackgroundDrawable(save.Background);
                    if (sender == m_) { m_.SetBackgroundColor(Color.Coral); flag = false; }
                    else { w_.SetBackgroundColor(Color.Coral); flag = true; } 
                }

                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(w_);
                add_.AddView(m_);
                add_.Visibility = ViewStates.Visible;
            };

            Button age_button = Create_button(basis.Width - 50, 150, 25, 989,
                "В о з р а с т", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            age_button.Click += (s, e) =>
            {
                int k = user.age;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(basis.Width - 100, 100, 0, 10,
                    "В о з р а с т", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                EditText text = Create_edittext(basis.Width - 350, (basis.Width - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 50);
                text.InputType = Android.Text.InputTypes.ClassNumber;
                text.TextChanged += (s__, e__) => { if (text.Text != "") k = Convert.ToInt32(text.Text); else k = 0; };
                Button plus = Create_button(100, 100, 50, ((basis.Width - 100) / 2 - 100) / 2 - 50,
                    "+", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                plus.SetBackgroundResource(Resource.Drawable.draver_fram);
                plus.Click += (s__, e__) =>
                {
                    k += 1;
                    text.Text = k.ToString();
                };
                Button minus = Create_button(100, 100, (basis.Width - 250), ((basis.Width - 100) / 2 - 100) / 2 - 50,
                    "−", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                minus.SetBackgroundResource(Resource.Drawable.draver_fram);
                minus.Click += (s__, e__) =>
                {
                    if (k > 0) k -= 1;
                    text.Text = k.ToString();
                };
                Button save = Create_button(basis.Width - 200, 150, 50, ((basis.Width - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    user.age = k;
                    add_.Visibility = ViewStates.Invisible;
                    text.Focusable = false;
                };
                add_.AddView(plus);
                add_.AddView(minus);
                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(text);
                add_.Visibility = ViewStates.Visible;
            };
            Button height_button = Create_button(basis.Width - 50, 150, 25, 1140,
                "Р о с т", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            height_button.Click += (s, e) =>
            {
                double k = user.height;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(basis.Width - 100, 100, 0, 10,
                    "Р о с т", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                EditText text = Create_edittext(basis.Width - 350, (basis.Width - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 50);
                text.InputType = Android.Text.InputTypes.ClassNumber;
                text.TextChanged += (s__, e__) => { if (text.Text != "") k = Convert.ToDouble(text.Text); else k = 0; };
                Button plus = Create_button(100, 100, 50, ((basis.Width - 100) / 2 - 100) / 2 - 50,
                    "+", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                plus.SetBackgroundResource(Resource.Drawable.draver_fram);
                plus.Click += (s__, e__) =>
                {
                    k += 0.1;
                    k = Math.Round(k * 10) / 10;
                    text.Text = k.ToString();
                };
                Button minus = Create_button(100, 100, (basis.Width - 250), ((basis.Width - 100) / 2 - 100) / 2 - 50,
                    "−", GravityFlags.Top | GravityFlags.CenterHorizontal, TypefaceStyle.Bold, 25);
                minus.SetBackgroundResource(Resource.Drawable.draver_fram);
                minus.Click += (s__, e__) =>
                {
                    if (k > 30) k -= 0.1;
                    k = Math.Round(k * 10) / 10;
                    text.Text = k.ToString();
                };
                Button save = Create_button(basis.Width - 200, 150, 50, ((basis.Width - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    user.height = k;
                    add_.Visibility = ViewStates.Invisible;
                    text.Focusable = false;
                };
                add_.AddView(plus);
                add_.AddView(minus);
                add_.AddView(save);
                add_.AddView(h_);
                add_.AddView(text);
                add_.Visibility = ViewStates.Visible;
            };
            Button activity_button = Create_button(basis.Width - 50, 150, 25, 1291,
                "У р о в е н ь   а к т и в н о с т и", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            activity_button.Click += (s_, e_) =>
            {
                add_.RemoveAllViews();
                for (int i = 0; i < 4; i++)
                {
                    string[] st = new string[4] { "низкий", "средний", "высокий", "очень высокий" };
                    int k = (basis.Width - 100) / 8 - 15;
                    TypefaceStyle ts = TypefaceStyle.Normal;
                    if (i == user.activity) ts = TypefaceStyle.Italic;
                    Button bt = Create_button(basis.Width - 200, k, 50, i * k + 15, st[i], GravityFlags.Center, ts, 15);
                    bt.Id = i;
                    bt.SetBackgroundDrawable(purpose_button.Background);
                    if (bt.Id == user.activity) bt.SetBackgroundColor(Color.Coral);
                    bt.Click += (s__, e__) =>
                    {
                        bt.SetBackgroundColor(Color.Coral);
                        user.activity = bt.Id;
                        add_.Visibility = ViewStates.Invisible;
                    };
                    add_.AddView(bt);
                }
                add_.Visibility = ViewStates.Visible;
            };
            Button food_button = Create_button(basis.Width - 50, 150, 25, 1442,
                "П р е д п о ч т е н и я   в   е д е", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            food_button.Click += (s_, e_) =>
            {
                add_.RemoveAllViews();
                for (int i = 0; i < 3; i++)
                {
                    string[] st = new string[3] { "нет", "веган", "вегетарианец" };
                    int k = (basis.Width - 100) / 6 - 15;
                    TypefaceStyle ts = TypefaceStyle.Normal;
                    if (i == user.food) ts = TypefaceStyle.Italic;
                    Button bt = Create_button(basis.Width - 200, k, 50, i * k + 15, st[i], GravityFlags.Center, ts, 15);
                    bt.Id = i;
                    bt.SetBackgroundDrawable(purpose_button.Background);
                    if (bt.Id == user.food) bt.SetBackgroundColor(Color.Coral);
                    bt.Click += (s__, e__) =>
                    {
                        bt.SetBackgroundColor(Color.Coral);
                        user.food = bt.Id;
                        add_.Visibility = ViewStates.Invisible;
                    };
                    add_.AddView(bt);
                }
                add_.Visibility = ViewStates.Visible;
            };

            basis.AddView(setting);
            basis.AddView(add_);
            setting.AddView(purpose_button);
            setting.AddView(current_button);
            setting.AddView(target_button);

            setting.AddView(name_button);
            setting.AddView(genger_button);
            setting.AddView(age_button);
            setting.AddView(height_button);
            setting.AddView(activity_button);
            setting.AddView(food_button);
        }

        private void Home_Create()
        {
            home.RemoveAllViews();
            //int[] id_menu = new int[menu.Count];
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
                    
                    im.SetImageBitmap(Request_image(but_im));
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

            basis = FindViewById<RelativeLayout>(Resource.Id.basis);
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
            //Request_data();
        }

        TextView back_setting;
        private void Sett()
        {
            /*const int N = 7;
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
            */
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
                    Create_ProfileFramelayout();
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

