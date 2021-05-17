using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using ModelLibrary;
using Plugin.Media;
using SkiaSharp;
using SkiaSharp.Views.Android;

namespace Calcul
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        DrawerLayout drawer;
        NavigationView navigationView;
        RelativeLayout basis;
        PersonalData user;
        MenuData week;
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
        public DatePicker Create_datepicker(int w_, int h_, float t_x, float t_y)
        {
            DatePicker datepicker = new DatePicker(drawer.Context);
            datepicker.LayoutParameters = new LinearLayout.LayoutParams(w_, h_);
            datepicker.TranslationX = t_x;
            datepicker.TranslationY = t_y;
            //calendarview.MaxDate = t_[0];
            //calendarview.MinDate = t_[1];
            return datepicker;
        }
        public SKCanvasView Create_canvasview(int w_, int h_, float t_x, float t_y)
        {
            double k = user.target_weight - user.current_weight; //цель - текущее
            SKColor background = new SKColor(0x99, 0x99, 0x99); //#999999
            SKColor graf = new SKColor(0xCC, 0xCC, 0xCC); //#CCCCCC

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



        private async Task AnimationObjectAsync(FrameLayout s_, int h, bool f)
        {
            if (f)
            {
                s_.Visibility = ViewStates.Visible;
                for (int i = 1; i < h; i += 8)
                {
                    s_.LayoutParameters = new FrameLayout.LayoutParams(s_.Width, i);
                    await Task.Delay(1);
                }
            }
            else
            {
                for (int i = h; i > 1; i -= 8)
                {
                    s_.LayoutParameters = new FrameLayout.LayoutParams(s_.Width, i);
                    await Task.Delay(1);
                }
                s_.Visibility = ViewStates.Invisible;
            }
        }

        //Профильный framelayout
        public void Create_ProfileFramelayout()
        {
            user.FileWR(user);
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
            if (user.image == null) user_image.SetImageResource(Resource.Drawable.design_ic_visibility_off);
            else user_image.SetImageBitmap(user.Convert_image_to(user.image));
            user_image.Click += async (s__, e__) =>
            {
                await CrossMedia.Current.Initialize();
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 40

                });
                if (file != null)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
                    user_image.SetImageBitmap(BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length));
                    user.image = imageArray;
                }
                else if (user.image == null) user_image.SetImageResource(Resource.Drawable.design_ic_visibility_off);
                else user_image.SetImageBitmap(user.Convert_image_to(user.image));
                Create_ProfileFramelayout();
            };
            user_name.AddView(user_image);
            TextView setting_text = Create_textview(150, 150, basis.Width - 250, 0,
                 "⚙", GravityFlags.Center, TypefaceStyle.Bold, 45);
            setting_text.Click += (s_, e_) => { Create_SettingFramelayout(); };
            user_name.AddView(setting_text);
            Button statistics_button = Create_button(basis.Width / 2, 250, 0, basis.Height / 5 + 100, "Статистика", GravityFlags.Center, TypefaceStyle.Bold, 15);
            statistics_button.Click += (s__, e__) => { };
            Button history_button = Create_button(basis.Width / 2, 250, basis.Width / 2, basis.Height / 5 + 100, "История", GravityFlags.Center, TypefaceStyle.Bold, 15);
            history_button.Click += (s__, e__) => { };

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
                double[] k = new double[3] { user.current_weight, user.target_weight, user.target_weight - user.current_weight };
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
                string s = $"\nВОЗРАСТ   >>   {user.age.ToString()} \n\nПОЛ   >>   {user.Convert_gender()} " +
                    $"\n\nРОСТ   >>   {user.height.ToString()} \n\nТЕКУЩИЙ ВЕС   >>   {user.current_weight.ToString()} " +
                    $"\n\nУРОВЕНЬ АКТИВНОСТИ   >>   {user.Convert_activity()} \n\nНОРМА КАЛОРИЙ   >>   {user.calories.ToString()}";
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
        //Настройки профиля framelayout
        private void Create_SettingFramelayout()
        {
            FrameLayout add_ = Create_framelayout(basis.Width - 100, (basis.Width - 100) / 2, 50, (basis.Height - (basis.Width - 100)) / 2, Color.DarkOrange);
            add_.SetBackgroundResource(Resource.Drawable.draver_fram);
            add_.Visibility = ViewStates.Invisible;
            FrameLayout setting = Create_framelayout(basis.Width, basis.Height, 0, 0, Color.Aqua);
            toolbar.RemoveAllViews();
            Button back = Create_button(toolbar.Height, toolbar.Height, -(int)(toolbar.Height * 1.25), 0, "<<", GravityFlags.Center, TypefaceStyle.Bold, 20);
            back.SetBackgroundDrawable(toolbar.Background);
            back.Click += (s_, e_) =>
            {
                bool fl = false;
                if (user.purpose == 1) { user.current_weight = user.target_weight; fl = true; }
                if (user.purpose == 0) if (user.current_weight > user.target_weight) fl = true;
                if (user.purpose == 2) if (user.current_weight < user.target_weight) fl = true;
                if (fl)
                {
                    basis.RemoveView(setting);
                    Create_ProfileFramelayout();
                }
                else
                {

                    View view = (View)s_;
                    Snackbar.Make(view, "Целевой и текущий вес не соответствуют поставленной цели.", Snackbar.LengthLong)
                        .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
            };
            toolbar.AddView(back);
            Button fon = Create_button(basis.Width, basis.Height, 0, 0, "", GravityFlags.Center, TypefaceStyle.Normal, 0);
            fon.SetBackgroundDrawable(setting.Background);
            fon.Click += (s__, e__) =>
            {
                for (int i = 0; i < add_.ChildCount; i++) add_.GetChildAt(i).Enabled = false;
                add_.Visibility = ViewStates.Invisible; fon.Visibility = ViewStates.Invisible;
            };
            fon.Visibility = ViewStates.Invisible;
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
                fon.Visibility = ViewStates.Visible;
                int k = user.current_weight;
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
                    if (k > 30) k -= 1;
                    text.Text = k.ToString();
                };
                Button save = Create_button(basis.Width - 200, 150, 50, ((basis.Width - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    if (sender == current_button) user.current_weight = k;
                    else user.target_weight = k;
                    add_.Visibility = ViewStates.Invisible;
                    fon.Visibility = ViewStates.Invisible;
                    text.Enabled = false;
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
                fon.Visibility = ViewStates.Visible;
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
                        fon.Visibility = ViewStates.Invisible;
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
                fon.Visibility = ViewStates.Visible;
                string k = user.name;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(basis.Width - 100, 100, 0, 10,
                    "И м я", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);

                EditText name_ = Create_edittext(basis.Width - 350, (basis.Width - 100) / 2 - 150, 125, 50,
                    k.ToString(), GravityFlags.Center, TypefaceStyle.Bold, 35);
                name_.InputType = Android.Text.InputTypes.ClassText;

                name_.TextChanged += (s__, e__) =>
                {
                    if (name_.Length() > 10) name_.Text = name_.Text.Remove(name_.Text.Length - 1, 1);
                    name_.SetSelection(name_.Text.Length);
                    if (name_.Text != "") k = name_.Text;
                };

                Button save = Create_button(basis.Width - 200, 150, 50, ((basis.Width - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    user.name = name_.Text;
                    add_.Visibility = ViewStates.Invisible;
                    name_.Focusable = false;
                    fon.Visibility = ViewStates.Invisible;
                    name_.Enabled = false;
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
                fon.Visibility = ViewStates.Visible;
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
                    fon.Visibility = ViewStates.Invisible;
                };

                Button w_ = Create_button((basis.Width - 100) / 2 - 25, 150, 25, 150,
                    "Ж", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                Button m_ = Create_button((basis.Width - 100) / 2 - 25, 150, (basis.Width - 100) / 2, 150,
                    "М", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
                if (flag == false) m_.SetBackgroundColor(Color.Coral);
                else w_.SetBackgroundColor(Color.Coral);
                w_.Click += Change_gender;
                m_.Click += Change_gender;

                void Change_gender(object sender, EventArgs eventArgs)
                {
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
                fon.Visibility = ViewStates.Visible;
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
                    fon.Visibility = ViewStates.Invisible;
                    text.Enabled = false;
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
                fon.Visibility = ViewStates.Visible;
                int k = user.height;
                add_.RemoveAllViews();
                TextView h_ = Create_textview(basis.Width - 100, 100, 0, 10,
                    "Р о с т", GravityFlags.Center, TypefaceStyle.BoldItalic, 20);
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
                    if (k > 30) k -= 1;
                    text.Text = k.ToString();
                };
                Button save = Create_button(basis.Width - 200, 150, 50, ((basis.Width - 100) / 2) - 175,
                    "Сохранить изменения", GravityFlags.Center, TypefaceStyle.Bold, 15);
                save.Click += (s__, e__) =>
                {
                    user.height = k;
                    add_.Visibility = ViewStates.Invisible;
                    text.Enabled = false;
                    fon.Visibility = ViewStates.Invisible;
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
                fon.Visibility = ViewStates.Visible;
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
                        fon.Visibility = ViewStates.Invisible;
                    };
                    add_.AddView(bt);
                }
                add_.Visibility = ViewStates.Visible;
            };
            Button food_button = Create_button(basis.Width - 50, 150, 25, 1442,
                "П р е д п о ч т е н и я   в   е д е", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 15);
            food_button.Click += (s_, e_) =>
            {
                fon.Visibility = ViewStates.Visible;
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
                        fon.Visibility = ViewStates.Invisible;
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
            setting.AddView(fon);
            setting.Visibility = ViewStates.Visible;
        }
        //Меню на день framelayout
        private void CreateMenuFramelayout(int datadef)
        {
            toolbar.RemoveAllViews();
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
            TextView prof = Create_textview(toolbar.Width, toolbar.Height, 0, 0,
                "Меню", GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 20);
            toolbar.AddView(prof);
            basis.RemoveAllViews();

            FrameLayout menuday = Create_framelayout(basis.Width, basis.Height, 0, 0, Android.Graphics.Color.DarkOrange);
            menuday.RemoveAllViews();

            FrameLayout add_ = Create_framelayout(basis.Width, basis.Width / 7 + 10, 0, 100, Android.Graphics.Color.DarkOrange);
            add_.RemoveAllViews();
            add_.Visibility = ViewStates.Invisible;

            bool flag = false;
            string date = week.menu_of_week[datadef].date;
            TextView head_ = Create_textview(basis.Width, 100, 0, 5,
                  date + "   ▼", GravityFlags.Center, TypefaceStyle.Bold, 25);
            head_.Click += (s__, e__) =>
            {
                if (flag == false)
                {
                    add_.Visibility = ViewStates.Visible;
                    head_.Text = date + "   ▲";
                    string[] weekbt = new string[7] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
                      //string[] datebt = user.Week();
                      for (int i = 0; i < 7; i++)
                    {
                        Button bt = Create_button(basis.Width / 7 - 4, basis.Width / 7 - 4, i * basis.Width / 7 - 4 + 2, 5,
                            weekbt[i], GravityFlags.Center, TypefaceStyle.Italic, 15);
                        bt.Id = i;
                        bt.Click += (s_, e_) =>
                        {
                            date = week.menu_of_week[bt.Id].date;
                            add_.Visibility = ViewStates.Invisible;
                            head_.Text = date + "   ▼";
                            flag = false;
                              ///
                              CreateMenuFramelayout(bt.Id);
                        };
                        add_.AddView(bt);
                    }
                    flag = true;
                }
                else
                {
                    add_.Visibility = ViewStates.Invisible;
                    head_.Text = date + "   ▼";
                    flag = false;
                }
            };
            menuday.AddView(head_);

            string[] head_but = new string[3] { "З А В Т Р А К", "О Б Е Д", "У Ж И Н" };
            string[] text_but = new string[3] { "", "", "" };
            for (int j = 0; j < week.menu_of_week[datadef].breakfast.Count; j++) text_but[0] += "-   " + week.menu_of_week[datadef].breakfast[j].Name + "\n\n";
            for (int j = 0; j < week.menu_of_week[datadef].lunch.Count; j++) text_but[1] += "-   " + week.menu_of_week[datadef].lunch[j].Name + "\n\n";
            for (int j = 0; j < week.menu_of_week[datadef].dinner.Count; j++) text_but[2] += "-   " + week.menu_of_week[datadef].dinner[j].Name + "\n\n";

            FrameLayout[] button_intake = new FrameLayout[3];
            for (int i = 0; i < 3; i++)
            {
                //400
                int h = (basis.Height - (basis.Width / 7 + 10 + 200)) / 3;
                button_intake[i] = Create_framelayout(basis.Width - 100, h - 100, 50, (basis.Width / 7 + 10 + 150) + (i * h), Color.AliceBlue);
                TextView intake_head = Create_textview(basis.Width - 120, h - 45, 10, 10,
                               head_but[i], GravityFlags.CenterHorizontal, TypefaceStyle.BoldItalic, 20);
                TextView intake_text = Create_textview(basis.Width - 120, h - 45, 10, 105,
                               text_but[i], GravityFlags.Left, TypefaceStyle.Normal, 17);
                button_intake[i].SetBackgroundResource(Resource.Drawable.draver_fram);
                button_intake[i].Id = i;
                button_intake[i].AddView(intake_head);
                button_intake[i].AddView(intake_text);
                menuday.AddView(button_intake[i]);
                button_intake[i].Click += (s_, e_) =>
                {
                    List<Recipe> mass_intake = new List<Recipe>();
                    int id_menu = 0;
                    for (int j = 0; j < 3; j++) if (s_ == button_intake[j]) id_menu = j;
                    switch (id_menu)
                    {
                        case 0:
                            mass_intake = week.menu_of_week[datadef].breakfast;
                            break;
                        case 1:
                            mass_intake = week.menu_of_week[datadef].lunch;
                            break;
                        case 2:
                            mass_intake = week.menu_of_week[datadef].dinner;
                            break;
                    };

                    FrameLayout intake_add = Create_framelayout(basis.Width, basis.Height, 0, 0, Color.Beige);

                    toolbar.RemoveAllViews();
                    Button back = Create_button(toolbar.Height, toolbar.Height, -(int)(toolbar.Height * 1.25), 0, "<<", GravityFlags.Center, TypefaceStyle.Bold, 20);
                    back.SetBackgroundDrawable(toolbar.Background);
                    back.Click += (s__, e__) => { basis.RemoveView(intake_add); CreateMenuFramelayout(datadef); };
                    toolbar.AddView(back);

                    TextView hd_ = Create_textview(basis.Width, 100, 0, 5,
                          head_but[id_menu] + "			" + date, GravityFlags.Center, TypefaceStyle.BoldItalic, 25);
                    intake_add.AddView(hd_);

                    int k = (basis.Height - 200) / 3;
                    for (int j = 0; j < mass_intake.Count; j++)
                    {
                        TextView nm_ = Create_textview(basis.Width - 50 - k, k - 50, 5, (k * j) + 150,
                              mass_intake[j].Name, GravityFlags.Left | GravityFlags.CenterVertical, TypefaceStyle.Bold, 17);
                        string bzhu_text = "Вес: " + mass_intake[j].Weight.ToString() + " г." + "			" +
                        "Б: " + mass_intake[j].Proteins.ToString() + "			" +
                        "Ж: " + mass_intake[j].Greases.ToString() + "			" +
                        "У: " + mass_intake[j].Carbohydrates.ToString();
                        TextView bzhu_ = Create_textview(basis.Width - 10, 50, 5, (k * j) + 150 + (k - 150),
                              bzhu_text, GravityFlags.Left, TypefaceStyle.Normal, 12);
                        TextView ccal_ = Create_textview(basis.Width - k + 100, k - 50, 5, (k * j) + 150,
                              mass_intake[j].Colories.ToString() + " кк.", GravityFlags.Right | GravityFlags.CenterVertical, TypefaceStyle.Normal, 17);
                        ImageView image_ = Create_imageview(k - 200, k - 200, basis.Width - k + 150, (k * j) + 250);
                        image_.SetImageBitmap(week.RequestIntakeImage(mass_intake[j].Id));

                        intake_add.AddView(nm_);
                        intake_add.AddView(bzhu_);
                        intake_add.AddView(ccal_);
                        intake_add.AddView(image_);
                    }
                    menuday.AddView(intake_add);
                };
            }

            basis.AddView(menuday);
            menuday.AddView(add_);
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
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.SetCheckedItem(Resource.Id.nav_home);

            user = new PersonalData();
            week = new MenuData();
            string path = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "userfile.txt");
            FileInfo ff = new FileInfo(path);
            if (ff.Exists)
            {
                user = user.FileRD();
                week = week.FileRD();
                if (week.data_update != DateTime.Now.ToString("dd.MM.yyyy"))
                {
                    week.RequestMenu();
                    week.FileWR(week);
                }
            }
            else
            {
                Create_SettingFramelayout();
                week.RequestMenu();
                week.FileWR(week);
            }
        }


        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
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
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            switch (item.ItemId)
            {
                case Resource.Id.nav_home:
                    string[] weekbt = new string[7] { "ПН", "ВТ", "СР", "ЧТ", "ПТ", "СБ", "ВС" };
                    for (int i = 0; i < 7; i++) if (weekbt[i].ToLower() == DateTime.Now.ToString("ddd"))
                            CreateMenuFramelayout(i);
                    return true;
                case Resource.Id.nav_gallery:
                    Create_ProfileFramelayout();
                    return true;
                case Resource.Id.nav_slideshow:
                    return true;
                case Resource.Id.nav_manage:
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
            user.FileWR(user);
            week.FileWR(week);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

