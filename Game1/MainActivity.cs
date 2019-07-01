
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Game1
{
     /// <summary>
     /// this activity is the startup screen for the application. besides introducing the user to the game
     /// this activity logs in the last user to play and if there wasnt any - loads the login screen
     /// </summary>
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.TitleScr);
            PageController pageController = new PageController(this);
            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.TitleLayout);
            UserDatabaseController databaseController = new UserDatabaseController();
            User lastUser = databaseController.GetLastUser();

            linearLayout.Click += (sender, e) =>
            {
                if (lastUser != null)
                {
                    pageController.GotoPage(typeof(MenuActivity), lastUser.Id, Common.USERID);
                }
                else
                    pageController.GotoPage(typeof(LoginActivity));
            };

        }
    }
}
