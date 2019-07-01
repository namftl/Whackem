
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
    /// this activity is the main screen for a logged user.
    /// from here a user can:
    /// - start a game
    /// - go through a tutorial
    /// - view a list of the high scores
    /// * if its an admin - go to the "players" screen
    /// </summary>
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.MenuScr);

            TextView HelloUser = FindViewById<TextView>(Resource.Id.HelloUser);
            Button StartGameButton = FindViewById<Button>(Resource.Id.StartGameButton);
            Button SwitchUserButton = FindViewById<Button>(Resource.Id.switchUserButton);
            Button TutorialButton = FindViewById<Button>(Resource.Id.tutorialButton);
            Button playersButton = FindViewById<Button>(Resource.Id.playersButton);
            Button highScoresButton = FindViewById<Button>(Resource.Id.highScoresButton);
            PageController pageController = new PageController(this.BaseContext);
            UserDatabaseController databaseController = new UserDatabaseController();

            //display username
            int UserId = Intent.Extras.GetInt(Common.USERID);   
            User user = databaseController.GetUser(UserId);
            HelloUser.Text = "Hello " + user.UserName;  
            //display "players" button if user is an admin   
            if(user.IsAdmin)
            {
                playersButton.Visibility = ViewStates.Visible;
                playersButton.Enabled = true;
            }

            StartGameButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(GameActivity), UserId, Common.USERID);
            };

            TutorialButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(TutorialActivity), UserId, Common.USERID);
            };

            SwitchUserButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(LoginActivity));
            };

            highScoresButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(HighScoresActivity), UserId, Common.USERID);
            };

            playersButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(UsersActivity), UserId, Common.USERID);
            };

        }
    }
}
