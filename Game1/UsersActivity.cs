
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
    /// this activity is exposed only to admins and allows access to all users' details
    /// from here an admin can go back to the menu screen
    /// </summary>
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UsersActivity : Activity
    {
        const int MAX_ID_LENGTH = 5;
        const int MAX_NAME_LENTGH = 9;
        const int USER_FIELDS = 4;
        const int TABLE_SIZE = 10;

        //members
        int currentUserIndex;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            int UserId = Intent.Extras.GetInt(Common.USERID);
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UsersTable);
            UserDatabaseController databaseController = new UserDatabaseController();
            PageController pageController = new PageController(this);
            TableLayout usersTable = FindViewById<TableLayout>(Resource.Id.usersTable);
            Button backButton = FindViewById<Button>(Resource.Id.backButton);
            backButton.Enabled = false;
            Button nextButton = FindViewById<Button>(Resource.Id.nextButton);
            Button menuButton = FindViewById<Button>(Resource.Id.menuButton);
            List<User> users = databaseController.GetUsers();
            int tableLength = usersTable.ChildCount;
            int tableWidth = ((TableRow)usersTable.GetChildAt(0)).ChildCount;
            int oldIndex = users.Count;
            if(users.Count < tableLength)
            {
                nextButton.Enabled = false;
            }
            currentUserIndex = FillTable();
            //fills the TABLE_SIZE length of the table with users currently viewed
            int FillTable(int lastUserShown = 0)
            {
                TableRow tableRow;
                TextView Id;
                TextView Name;
                TextView Password;
                TextView HighScore;
                TextView Type;
                for (int i = 1; i < tableLength; i++, lastUserShown++)
                {

                    if (users.Count==lastUserShown+1)
                    {
                        fillBlanks(i);
                        break;
                    }
                    tableRow = (TableRow)usersTable.GetChildAt(i);
                    Id = (TextView)(tableRow.GetChildAt(0));
                    Id.Text = users[lastUserShown].Id.ToString();
                    Id.SetTextColor(Android.Graphics.Color.Black);
                    Name = (TextView)(tableRow.GetChildAt(1));
                    Name.Text = users[lastUserShown].UserName;
                    Name.SetTextColor(Android.Graphics.Color.Black);
                    Password = (TextView)(tableRow.GetChildAt(2));
                    Password.Text = users[lastUserShown].Password;
                    Password.SetTextColor(Android.Graphics.Color.Black);
                    HighScore = (TextView)(tableRow.GetChildAt(3));
                    HighScore.Text = users[lastUserShown].HighScore.ToString();
                    HighScore.SetTextColor(Android.Graphics.Color.Black);
                    Type = (TextView)(tableRow.GetChildAt(4));
                    Type.Text = users[lastUserShown].IsAdmin ? "Admin" : "Player";
                    Type.SetTextColor(Android.Graphics.Color.Black);
                }
                return lastUserShown;
            }

            void fillBlanks(int index = 0)
            {
                TableRow tableRow;
                TextView text;
                nextButton.Enabled = false;
                for (int i = index; i < tableLength; i++)
                {
                    tableRow = (TableRow)usersTable.GetChildAt(i);
                    for (int j = 0; j < tableWidth; j++)
                    {
                        text = (TextView)(tableRow.GetChildAt(j));
                        text.Text = "";
                    }
                }
            }
            //move forward through the users table - disable on last page
            nextButton.Click += (sender, e) =>
            {
                currentUserIndex = FillTable(currentUserIndex);
                backButton.Enabled = true;
            };
            //move backward through the users table - diable on first page
            backButton.Click += (sender, e) =>
            {
                nextButton.Enabled = true;
                int page = (int)Math.Floor((double)currentUserIndex / TABLE_SIZE);
                currentUserIndex = (page-1)*TABLE_SIZE;
                if (currentUserIndex == 0)
                    backButton.Enabled = false;
                currentUserIndex = FillTable(currentUserIndex);
            };

            menuButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(MenuActivity), UserId, Common.USERID);
            };


        }
    }
}


