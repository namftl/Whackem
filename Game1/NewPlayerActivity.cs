
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using SQLite;

namespace Game1
{
    /*
     * functions:
     *  registering new player -> passing directly to pre game screen with new user logged
     * menu button - to login screen
     */
     /// <summary>
     /// this activity creates a registration process for a new player
     /// </summary>
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class NewPlayerActivity : Activity
    {
        private UserDatabaseController databaseController;
        const string ADMIN_PASSWORD = "admin";
        const int MIN_PASSWORD_LENGTH = 5;
        const int MAX_PASSWORD_LENGTH = 8;
        const int MIN_USERNAME_LENGTH = 3;
        const int MAX_USERNAME_LENGTH = 8;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.NewPlayer);

            //
            databaseController = new UserDatabaseController();
            EditText userNameInput = FindViewById<EditText>(Resource.Id.newUserName);
            EditText userPassword = FindViewById<EditText>(Resource.Id.userPassword);
            Button checkUserNameButton = FindViewById<Button>(Resource.Id.checkUserNameButton);
            Button toMenuButton = FindViewById<Button>(Resource.Id.toMenuButton);
            Button registerButton = FindViewById<Button>(Resource.Id.registerButton);
            Button beginButton = FindViewById<Button>(Resource.Id.beginButton);
            TextView illegalUserName = FindViewById<TextView>(Resource.Id.illegalUsername);
            CheckBox adminBox = FindViewById<CheckBox>(Resource.Id.adminBox);
            EditText adminPassword = FindViewById<EditText>(Resource.Id.adminPassword);
            PageController pageController = new PageController(this.BaseContext);
            bool isAdmin = false;
            int newUserId = 0;

            //check if username is legal and unique
            checkUserNameButton.Click += (sender, e) =>
            {
                pageController.CloseKeyboard(userNameInput);
                if (databaseController.checkUsername(userNameInput.Text))
                {
                    illegalUserName.Text = "Name exists - try a different one";
                    illegalUserName.SetTextColor(Android.Graphics.Color.Red);
                }
                else if((userNameInput.Length() > MAX_USERNAME_LENGTH)||(userNameInput.Length() < MIN_USERNAME_LENGTH))
                {
                    illegalUserName.Text = string.Format("Name must be between {0}-{1} characters", MIN_USERNAME_LENGTH, MAX_USERNAME_LENGTH);
                    illegalUserName.SetTextColor(Android.Graphics.Color.Red);
                }
                else
                {
                    illegalUserName.Text = "GOOD!";
                    illegalUserName.SetTextColor(Android.Graphics.Color.Green);
                    userPassword.Enabled = true;
                }
            };

            //opens the admin password dialog if the admin checkbox is clicked
            adminBox.Click += (sender, e) =>
            {
                if (adminBox.Checked)
                    adminPassword.Visibility = ViewStates.Visible;
                else
                    adminPassword.Visibility = ViewStates.Invisible;
            };

            //try to insert a new player and informs of the outcome
            registerButton.Click += (sender, e) =>
            {
                pageController.CloseKeyboard(userPassword);
                if ((!adminBox.Checked) ||
                   (adminPassword.Text == ADMIN_PASSWORD))
                {
                    isAdmin = adminBox.Checked;
                    if ((userPassword.Length() > MAX_PASSWORD_LENGTH) || (userPassword.Length() < MIN_PASSWORD_LENGTH))
                    {
                        illegalUserName.Text = string.Format("Password must be between {0}-{1} characters",MIN_PASSWORD_LENGTH, MAX_PASSWORD_LENGTH);
                        illegalUserName.SetTextColor(Android.Graphics.Color.Red);
                    }
                    else
                    {
                        newUserId = databaseController.saveUser(userNameInput.Text, userPassword.Text, isAdmin);
                        if (newUserId != 0)
                            beginButton.Enabled = true;
                        else
                        {
                            illegalUserName.Text = "registration failed";
                            illegalUserName.SetTextColor(Android.Graphics.Color.Red);
                        }
                    }
                }
                else
                {
                    illegalUserName.Text = "admin password error";
                    illegalUserName.SetTextColor(Android.Graphics.Color.Red);
                }
            };

            beginButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(MenuActivity), newUserId, Common.USERID);
            };

            toMenuButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            };
        
        }

    }
    


}
