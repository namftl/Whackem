
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
    /// this class introduces the player to his final score and chekcs whether or not its a new high score
    /// from here a player can replay or go back to the menu
    /// </summary>
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PostGameActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.PostGame);
            PageController pageController = new PageController(this);
            UserDatabaseController databaseController = new UserDatabaseController();
            TextView newHighScore = FindViewById<TextView>(Resource.Id.newHighScore);

            int UserId = Intent.Extras.GetInt(Common.USERID);
            int FinalScore = Intent.Extras.GetInt(Common.SCORE);
            User currentUser = databaseController.GetUser(UserId);
            if(FinalScore > currentUser.HighScore)
            {
                currentUser.HighScore = FinalScore;
                databaseController.UpdateHighScore(UserId, FinalScore);
                RunOnUiThread(() => newHighScore.Text = "NEW HIGH SCORE!!!");
            }

            TextView scoreText = FindViewById<TextView>(Resource.Id.urFinalScore);
            RunOnUiThread(() => scoreText.SetTextColor(Android.Graphics.Color.Black));
            RunOnUiThread(() => scoreText.Text = String.Format("U Whacked {0} Moles!", FinalScore));
            Button replayButton = FindViewById<Button>(Resource.Id.replayButton);
            Button menuButton = FindViewById<Button>(Resource.Id.menuButton);

            replayButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(GameActivity), UserId, Common.USERID);
            };

            menuButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(MenuActivity), UserId, Common.USERID);
            };
            // Create your application here
        }
    }
}
