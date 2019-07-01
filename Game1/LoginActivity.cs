using Android.App;
using Android.Widget;
using Android.OS;
using SQLite;
using System.IO;
using System.Collections.Generic;
using Android.Content;
using System.Threading.Tasks;
using Android.Views;

namespace Game1
{
     /// <summary>
     /// this activity allows for an existing user to log or for a new user to transfer to creating a new
     /// account
     /// </summary>
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Login);

            Button LoginButton = FindViewById<Button>(Resource.Id.LoginButton);
            Button newPlayerButton = FindViewById<Button>(Resource.Id.NewPlayerButton);
            EditText userName = FindViewById<EditText>(Resource.Id.userName);
            EditText password = FindViewById<EditText>(Resource.Id.password);
            TextView userErr = FindViewById<TextView>(Resource.Id.userErr);
            UserDatabaseController databaseController = new UserDatabaseController();
            PageController pageController = new PageController(this.BaseContext);

            LoginButton.Click += (sender, e) =>
            {
                pageController.CloseKeyboard(userName);
                pageController.CloseKeyboard(password);
                //search the db for the user
                int reply = databaseController.checkUserPassword(userName.Text, password.Text);
                if((reply == (int)UserDatabaseController.UserCheckErr.USERNAME_ERR)||
                   (reply == (int)UserDatabaseController.UserCheckErr.PASSWORD_ERR)) //user accepted
                {
                    userErr.Text = ((UserDatabaseController.UserCheckErr)reply).ToString();
                    userErr.SetTextColor(Android.Graphics.Color.Red);
                }
                else
                {
                    pageController.GotoPage(typeof(MenuActivity), reply, Common.USERID);
                }
            };
            newPlayerButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(NewPlayerActivity));
            };
        }
    }
}

