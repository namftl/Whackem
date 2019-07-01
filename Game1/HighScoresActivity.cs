
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
    /// This activity compiles and displays a list of the top 5 scores acheived
    /// </summary>
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class HighScoresActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.TopScores);
            UserDatabaseController databaseController = new UserDatabaseController();
            PageController pageController = new PageController(this);
            int UserId = Intent.Extras.GetInt(Common.USERID);
            Button menuButton = FindViewById<Button>(Resource.Id.menuButton);
            TableLayout highScoresTable = FindViewById<TableLayout>(Resource.Id.highScoresTable);
            int tableLength = highScoresTable.ChildCount-1;
            Game[] bestGames = new Game[tableLength];
            List<Game> allGames = databaseController.GetGames();
            fillTable();

            /// <summary>
            /// Fills the table with top5 games by locating and removing the best games one by one from a local 
            /// list containing all the games.
            /// </summary>
            void fillTable()
            {
                TextView Name;
                TextView HighScore;
                TableRow tableRow;

                for (int i = 0; i < tableLength; i++)
                {
                    Game bestGame = findBestGame();
                    if (bestGame.FinalScore == 0)
                        break;
                    allGames.Remove(bestGame);
                    User user = databaseController.GetUser(bestGame.UserId);
                    tableRow = (TableRow)highScoresTable.GetChildAt(i+1);
                    Name = (TextView)(tableRow.GetChildAt(1));
                    Name.Text = user.UserName;
                    Name.SetTextColor(Android.Graphics.Color.Black);
                    HighScore = (TextView)(tableRow.GetChildAt(2));
                    HighScore.Text = bestGame.FinalScore.ToString();
                    HighScore.SetTextColor(Android.Graphics.Color.Black);
                }

            }

            /// <summary>
            /// Finds the best game.
            /// </summary>
            /// <returns>The best game.</returns>
            Game findBestGame()
            {
                Game bestGame = new Game();
                foreach(Game game in allGames)
                {
                    if(game.FinalScore > bestGame.FinalScore)
                    {
                        bestGame = game;
                    }
                }
                return bestGame;
            }

            menuButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(MenuActivity), UserId, Common.USERID);
            };
        }
    }
}
