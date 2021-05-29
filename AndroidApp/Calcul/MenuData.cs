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
        public List<Recipe> BreakfastRecipes;
        public List<Recipe> LaunchRecipes;
        public List<Recipe> DinnerRecipes;
    }
    class MenuData : SerializableObject
    {
        public string data_update;
        public DayMenu[] menu_of_week;
        public DayMenu[] menu_of_week_add;

        private Recipe error_;
        private string path = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "weekmenu_file.txt");
        private string uri_ = "http://4aee6b9146f2.ngrok.io";

        public MenuData()
        {
            data_update = DateTime.Now.ToString("dd.MM.yyyy");
            menu_of_week = new DayMenu[7];
            string[] date_of_week = Week();
            menu_of_week_add = new DayMenu[7];
            for (int i = 0; i < 7; i++)
            {
                menu_of_week[i] = new DayMenu();
                menu_of_week[i].Date = date_of_week[i];
                menu_of_week[i].BreakfastRecipes = new List<Recipe>();
                menu_of_week[i].LaunchRecipes = new List<Recipe>();
                menu_of_week[i].DinnerRecipes = new List<Recipe>();
                menu_of_week_add[i] = new DayMenu();
                menu_of_week_add[i].Date = date_of_week[i];
                menu_of_week_add[i].BreakfastRecipes = new List<Recipe>();
                menu_of_week_add[i].LaunchRecipes = new List<Recipe>();
                menu_of_week_add[i].DinnerRecipes = new List<Recipe>();
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
        public void RequestMenu(int cal)
        {
            List<DayMenu> week;
            List<DayMenu> week_number = new List<DayMenu>();
            for (int i = 0; i < 7; i++) week_number.Add(null);
            WebClient client = new WebClient();
            try
            {
                Uri add = new Uri(uri_ + "/recipes/weekmenu");
                week = Serialization<DayMenu>.ReadList(client.DownloadString(add));
                for (int j = 0; j < week.Count; j++) for (int i = 0; i < 7; i++) if (week[j].Date == menu_of_week[i].Date) menu_of_week[i] = week[j];
                RequestCalcul(cal);
            }
            catch { };
        }
        public void RequestCalcul(int calories)
        {
            for (int i = 0; i < 7; i++)
            {
                int b = (int)(calories * 0.25);
                int l = (int)(calories * 0.50);
                int d = (int)(calories * 0.25);
                List<Recipe>[] br = new List<Recipe>[2];
                for (int j = 0; j < 2; j++) br[j] = new List<Recipe>();
                List<Recipe>[] lu = new List<Recipe>[5];
                for (int j = 0; j < 5; j++) lu[j] = new List<Recipe>();
                List<Recipe>[] di = new List<Recipe>[4];
                for (int j = 0; j < 4; j++) di[j] = new List<Recipe>();
                for (int j = 0; j < menu_of_week[i].BreakfastRecipes.Count; j++)
                {
                    if ((menu_of_week[i].BreakfastRecipes[j].Type != RecipeType.Drink) & (menu_of_week[i].BreakfastRecipes[j].Colories <= (int)(b * 0.9)))
                        br[0].Add(menu_of_week[i].BreakfastRecipes[j]);
                    if ((menu_of_week[i].BreakfastRecipes[j].Type == RecipeType.Drink) & (menu_of_week[i].BreakfastRecipes[j].Colories <= (int)(b * 0.1)))
                        br[1].Add(menu_of_week[i].BreakfastRecipes[j]);
                }
                for (int j = 0; j < 2; j++)
                {
                    if (br[j].Count != 0)
                    {
                        Recipe max = br[j][0];
                        for (int k = 1; k < br[j].Count; k++) if (max.Colories <= br[j][k].Colories) max = br[j][k];
                        menu_of_week_add[i].BreakfastRecipes.Add(max);
                    }
                }


                for (int j = 0; j < menu_of_week[i].LaunchRecipes.Count; j++)
                {
                    if ((menu_of_week[i].LaunchRecipes[j].Type == RecipeType.Primary) & (menu_of_week[i].LaunchRecipes[j].Colories <= (int)(l * 0.2)))
                        lu[0].Add(menu_of_week[i].LaunchRecipes[j]);
                    if ((menu_of_week[i].LaunchRecipes[j].Type == RecipeType.Secondary) & (menu_of_week[i].LaunchRecipes[j].Colories <= (int)(l * 0.4)))
                        lu[1].Add(menu_of_week[i].LaunchRecipes[j]);
                    if ((menu_of_week[i].LaunchRecipes[j].Type == RecipeType.Garnish) & (menu_of_week[i].LaunchRecipes[j].Colories <= (int)(l * 0.3)))
                        lu[2].Add(menu_of_week[i].LaunchRecipes[j]);
                    if ((menu_of_week[i].LaunchRecipes[j].Type == RecipeType.Drink) & (menu_of_week[i].LaunchRecipes[j].Colories <= (int)(l * 0.1)))
                        lu[3].Add(menu_of_week[i].LaunchRecipes[j]);
                }
                for (int j = 0; j < 4; j++)
                {
                    if (lu[j].Count != 0)
                    {
                        Recipe max = lu[j][0];
                        for (int k = 1; k < lu[j].Count; k++) if (max.Colories <= lu[j][k].Colories) max = lu[j][k];
                        menu_of_week_add[i].LaunchRecipes.Add(max);
                    }
                }


                for (int j = 0; j < menu_of_week[i].DinnerRecipes.Count; j++)
                {
                    if ((menu_of_week[i].DinnerRecipes[j].Type == RecipeType.Secondary) & (menu_of_week[i].DinnerRecipes[j].Colories <= (int)(d * 0.45)))
                        di[0].Add(menu_of_week[i].DinnerRecipes[j]);
                    if ((menu_of_week[i].DinnerRecipes[j].Type == RecipeType.Garnish) & (menu_of_week[i].DinnerRecipes[j].Colories <= (int)(d * 0.35)))
                        di[1].Add(menu_of_week[i].DinnerRecipes[j]);
                    if ((menu_of_week[i].DinnerRecipes[j].Type == RecipeType.Drink) & (menu_of_week[i].DinnerRecipes[j].Colories <= (int)(d * 0.2)))
                        di[2].Add(menu_of_week[i].DinnerRecipes[j]);
                    /*if ((menu_of_week[i].DinnerRecipes[j].Type == RecipeType.Drink) & (menu_of_week[i].DinnerRecipes[j].Colories <= (int)(d * 0.1)))
                        di[3].Add(menu_of_week[i].DinnerRecipes[j]);*/
                }
                for (int j = 0; j < 3; j++)
                {
                    if (di[j].Count != 0)
                    {
                        Recipe max = di[j][0];
                        for (int k = 1; k < di[j].Count; k++) if (max.Colories <= di[j][k].Colories) max = di[j][k];
                        menu_of_week_add[i].DinnerRecipes.Add(max);
                    }
                }
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