using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ModelLibrary;

namespace Calcul
{
    class MenuOfDay
    {
        public string date;
        public List<Recipe> breakfast;
        public List<Recipe> lunch;
        public List<Recipe> dinner;
    }
    class MenuData : SerializableObject
    {
        public string data_update;
        public MenuOfDay[] menu_of_week;
        public MenuOfDay[] menu_of_week_add;

        private Recipe error_;
        private string path = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "weekmenu_file.txt");
        private string uri_ = "http://4aee6b9146f2.ngrok.io/";

        public MenuData()
        {
            data_update = DateTime.Now.ToString("dd.MM.yyyy");
            menu_of_week = new MenuOfDay[7];
            string[] date_of_week = Week();
            menu_of_week_add = new MenuOfDay[7];
            for (int i = 0; i < 7; i++)
            {
                menu_of_week[i] = new MenuOfDay();
                menu_of_week[i].date = date_of_week[i];
                menu_of_week[i].breakfast = new List<Recipe>();
                menu_of_week[i].lunch = new List<Recipe>();
                menu_of_week[i].dinner = new List<Recipe>();
                menu_of_week_add[i] = new MenuOfDay();
                menu_of_week_add[i].date = date_of_week[i];
                menu_of_week_add[i].breakfast = new List<Recipe>();
                menu_of_week_add[i].lunch = new List<Recipe>();
                menu_of_week_add[i].dinner = new List<Recipe>();
            }
            error_ = new Recipe();
            error_.Name = "На сегодня нет меню";
        }
       /* ~MenuData()
        {
            FileInfo ff = new FileInfo(path);
            if (ff.Exists)
            {
                FileWR(this);
            }
        }*/
        public void FileWR(MenuData us)
        {
            FileInfo flw = new FileInfo(path);
            flw.Delete();
            using (FileStream filew = new FileStream(path, FileMode.OpenOrCreate))
            {
                data_update = DateTime.Now.ToString("dd.MM.yyyy");
                string textin = Serialization<MenuData>.Write(us);
                byte[] array = System.Text.Encoding.Default.GetBytes(textin);
                filew.Write(array, 0, array.Length);
                filew.Close();
            }
        }
        public MenuData FileRD()
        {
            using (FileStream filer = File.OpenRead(path))
            {
                byte[] array = new byte[filer.Length];
                filer.Read(array, 0, array.Length);
                string textout = System.Text.Encoding.Default.GetString(array);
                filer.Close();
                return Serialization<MenuData>.Read(textout);
            }
        }
        public string[] Week()
        {
            string[] date_of_week = new string[7];
            DateTime date_ = DateTime.Now.AddDays(-((double)(DateTime.Now.DayOfWeek - 1) % 7));
            date_of_week[0] = date_.ToString("dd.MM.yyyy");
            for (int i = 1; i < 7; i++) date_of_week[i] = date_.AddDays(i).ToString("dd.MM.yyyy");
            return date_of_week;
        }
        public void RequestMenu()
        {
            List<DayMenu> week;
            List<DayMenu> week_number = new List<DayMenu>();
            for (int i = 0; i < 7; i++) week_number.Add(null);
            WebClient client = new WebClient();
            try
            {
                Uri add = new Uri(uri_ + "/recipes/weekmenu");
                week = Serialization<DayMenu>.ReadList(client.DownloadString(add));
                for (int j = 0; j < week.Count; j++) for (int i = 0; i < 7; i++) if (week[j].Date == menu_of_week[i].date) week_number[i] = week[j];
                for (int i = 0; i < 7; i++)
                {
                    if (week_number[i] != null)
                    {
                        for (int j = 0; j < week_number[i].BreakfastRecipes.Count; j++)
                            menu_of_week[i].breakfast.Add(RequestFood(week_number[i].BreakfastRecipes[j]));

                        for (int j = 0; j < week_number[i].LaunchRecipes.Count; j++)
                            menu_of_week[i].lunch.Add(RequestFood(week_number[i].LaunchRecipes[j]));

                        for (int j = 0; j < week_number[i].DinnerRecipes.Count; j++)
                            menu_of_week[i].dinner.Add(RequestFood(week_number[i].DinnerRecipes[j]));
                    }
                }
            }
            catch { };
        }
        /*public void RequestTodayMenu()
        {
            DayMenu today;
            WebClient client = new WebClient();
            if (client.IsBusy)
            {
                Uri add = new Uri(uri_ + "/recipes/todaymenu");
                today = Serialization<DayMenu>.Read(client.DownloadString(add));
                if (today != null)
                {
                    for (int j = 0; j < today.BreakfastRecipes.Count; j++)
                        menu_of_day.breakfast.Add(RequestFood(today.BreakfastRecipes[j]));

                    for (int j = 0; j < today.LaunchRecipes.Count; j++)
                        menu_of_day.lunch.Add(RequestFood(today.LaunchRecipes[j]));

                    for (int j = 0; j < today.DinnerRecipes.Count; j++)
                        menu_of_day.dinner.Add(RequestFood(today.DinnerRecipes[j]));
                }
            }
        }*/
        public void RequestCalcul(int calories)
        {
            int b = (int)(calories * 0.25);
            int l = (int)(calories * 0.50);
            int d = (int)(calories * 0.25);
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < menu_of_week[i].breakfast.Count; j++)
                {
                    if (menu_of_week[i].breakfast[j].Colories <= (int)(b * 0.75))
                    {
                        menu_of_week_add[i].breakfast.Add(menu_of_week[i].breakfast[j]);
                        b = b - menu_of_week[i].breakfast[j].Colories;
                    }
                }
                for (int j = 0; j < menu_of_week[i].lunch.Count; j++) ;
                for (int j = 0; j < menu_of_week[i].dinner.Count; j++) ;
            }

        }
        private Recipe RequestFood(long id_food)
        {
            WebClient client = new WebClient();
            Uri add = new Uri(uri_ + "/recipes/details/" + id_food.ToString());
            return (Serialization<Recipe>.Read(client.DownloadString(add)));
        }
        public Bitmap RequestIntakeImage(long id_food)
        {
            //чтение картинки
            WebClient client = new WebClient();
            Bitmap bb = null;
            try
            {
                byte[] s = client.DownloadData(uri_ + "/recipes/image/" + id_food.ToString());
                bb = BitmapFactory.DecodeByteArray(s, 0, s.Length);
            }
            catch { };
            return bb;
        }
    }
}