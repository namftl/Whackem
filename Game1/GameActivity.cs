
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Game1
{
    /*
     * functions:
     *  countdown -> game timer -> each second a mole is raffled and progress bar updated
     *  pause button
     *  menu button
     * params:
     *  current user Id - "UserId"
     */
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class GameActivity : Activity
    {

        const int COUNTDOWN_TIME = 3;
        const int GAME_TIME = 15;
        const int MILI = 1000;
        const int MIN_EASY_MILI = 900;
        const int MAX_EASY_MILI = 1200;
        const int MIN_MED_MILI = 500;
        const int MAX_MED_MILI = 800; 
        const int MIN_HARD_MILI = 400;
        const int MAX_HARD_MILI = 400;
        //members being used all through the class
        private int GameSecondsLeft;
        private int WaitingMin;
        private int WaitingMax;
        private int Score;
        private int UserId;
        private System.Timers.Timer GameTimer;
        private TextView TimerText;
        private TextView ScoreTest;
        private List<Button> AllButtons;
        private PageController PageCtrl;
        private int Countdown;
        private TextView CountdownButton;
        private ProgressBar GameProgressBar;
        private int MolePic;
        private int MoleSuccess;
        private int PauseButtonPic;
        private int PlayButtonPic;
        private int CountDownGoPic;
        private int CountDown1Pic;
        private int CountDown2Pic;
        private int CountDown3Pic;
        private int GameOverPic;

        /// <summary>
        /// Ons the create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Game);

            //getting elements from layout
            TextView currHighScore = FindViewById<TextView>(Resource.Id.scoreText13);
            LinearLayout GameLayout = FindViewById<LinearLayout>(Resource.Id.GameLayout);
            Button pauseButton = FindViewById<Button>(Resource.Id.pauseButton11);
            Button menuButton = FindViewById<Button>(Resource.Id.menuButton);
            CountdownButton = FindViewById<TextView>(Resource.Id.countdownText22);
            TimerText = FindViewById<TextView>(Resource.Id.timerText);
            ScoreTest = FindViewById<TextView>(Resource.Id.scoreText2);
            GameProgressBar = FindViewById<ProgressBar>(Resource.Id.gameProgressBar);
            AllButtons = GetButtonList(Resource.Id.button1, Resource.Id.button2, Resource.Id.button3,
                                       Resource.Id.button4, Resource.Id.button5, Resource.Id.button6);
            //initing params
            PageCtrl = new PageController(this);
            WaitingMin = MIN_EASY_MILI;
            WaitingMax = MAX_EASY_MILI;
            UserId = Intent.Extras.GetInt(Common.USERID); 
            UserDatabaseController databaseController = new UserDatabaseController();
            User currUser = databaseController.GetUser(UserId);
            InitGameButtons(AllButtons);
            bool gameOver = false;
            bool isPaused = false;
            Countdown = COUNTDOWN_TIME;
            double timerInterval = 1;
            GameTimer = new System.Timers.Timer(timerInterval*MILI);
            GameTimer.Elapsed += GameTimerEvent;
            GameSecondsLeft = GAME_TIME;

            RunOnUiThread(() => MoleSuccess = Resource.Drawable.coolMoleOrange);
            RunOnUiThread(() => MolePic = Resource.Drawable.coolMoleGreen);
            RunOnUiThread(() => PauseButtonPic = Resource.Drawable.pauseButton);
            RunOnUiThread(() => PlayButtonPic = Resource.Drawable.playButton);
            RunOnUiThread(() => currHighScore.Text = "HighScore:" + currUser.HighScore.ToString());
            RunOnUiThread(() => CountDownGoPic = Resource.Drawable.countDownGo);
            RunOnUiThread(() => CountDown1Pic = Resource.Drawable.countDown1);
            RunOnUiThread(() => CountDown2Pic = Resource.Drawable.countDown2);
            RunOnUiThread(() => CountDown3Pic = Resource.Drawable.countDown3);
            RunOnUiThread(() => GameOverPic = Resource.Drawable.gameOver);
            RunOnUiThread(() => GameProgressBar.Max = GAME_TIME);
            RunOnUiThread(() => GameProgressBar.SetProgress(GAME_TIME, true));
            RunOnUiThread(() => GameTimer.Enabled = true);
            RunOnUiThread(() => ScoreTest.Text = Score.ToString());

            //the event for text changed is used for monitoring time left
            TimerText.TextChanged += (sender, e) =>
            {       
                if (GameSecondsLeft == 0)
                {
                    GameTimer.Stop();
                    ChangeViewBgImg(CountdownButton, GameOverPic);
                    RunOnUiThread(() => CountdownButton.Visibility = ViewStates.Visible);
                    gameOver = true;
                }
            };

            GameLayout.Click += (sender, e) =>
            {
                if(gameOver)
                {
                    List<KeyValuePair<string, int>> postGameArgs = new List<KeyValuePair<string, int>>
                    {
                        new KeyValuePair<string, int>(Common.SCORE, Score),
                        new KeyValuePair<string, int>(Common.USERID, UserId)
                    };
                    databaseController.InsertGame(UserId, Score);
                    PageCtrl.GotoPage(typeof(PostGameActivity), postGameArgs);
                }
            };

            pauseButton.Click += (sender, e) =>
            {
                if(isPaused)
                {
                    RunOnUiThread(() =>  menuButton.Visibility = ViewStates.Invisible);
                    GameTimer.Start();
                    ChangeViewBgImg(pauseButton, PauseButtonPic);
                    isPaused = false;
                }
                else
                {
                    RunOnUiThread(() => menuButton.Visibility = ViewStates.Visible);
                    GameTimer.Stop();
                    ChangeViewBgImg(pauseButton, PlayButtonPic);
                    isPaused = true;
                }
            };

            menuButton.Click += (sender, e) =>
            {
                PageCtrl.GotoPage(typeof(MenuActivity), UserId, Common.USERID);
            };
        }

        private void GameTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            //initial countdown, game time hasnt started
            if (Countdown >= 0)
            {
                switch(Countdown)
                {
                    case 3:
                        ChangeViewBgImg(CountdownButton, CountDown3Pic);
                        break;
                    case 2:
                        ChangeViewBgImg(CountdownButton, CountDown2Pic);
                        break;
                    case 1:
                        ChangeViewBgImg(CountdownButton, CountDown1Pic);
                        break;
                    case 0:
                        ChangeViewBgImg(CountdownButton, CountDownGoPic);
                        break;
                }

                Countdown--;
            }
            else  //game timer started
            {
                RunOnUiThread(() => CountdownButton.Visibility = ViewStates.Invisible);
                GameSecondsLeft--;
                GameProgressBar.Progress -= 1;

                if (GameSecondsLeft == 6)
                {
                    WaitingMin = MIN_MED_MILI;
                    WaitingMax = MAX_MED_MILI;
                    RunOnUiThread(() => GameProgressBar.ProgressDrawable.SetColorFilter(Android.Graphics.Color.Pink, Android.Graphics.PorterDuff.Mode.Multiply));
                }
                if (GameSecondsLeft == 3)
                {
                    WaitingMin = MIN_HARD_MILI;
                    WaitingMax = MAX_HARD_MILI;
                    RunOnUiThread(() => GameProgressBar.ProgressDrawable.SetColorFilter(Android.Graphics.Color.Red, Android.Graphics.PorterDuff.Mode.Multiply));
                }
                //Update visual representation here
                RunOnUiThread(() => TimerText.Text = GameSecondsLeft.ToString());
                Random random = new Random();
                int randomButton = random.Next(0, 5);
                Task<bool> lightTask = LightButtonTask(AllButtons[randomButton]);
                lightTask.Start();
            }
        }

        /// <summary>
        /// makes the button visible for a random time
        /// </summary>
        /// <returns>The button task.</returns>
        /// <param name="button">Button.</param>
        async Task<bool> LightButtonTask(Button button)
        {
            RunOnUiThread(() => button.Visibility = ViewStates.Visible);
            Random random = new Random();
            int randomWait = random.Next(WaitingMin, WaitingMax);
            await Task.Delay(randomWait);
            RunOnUiThread(() => button.Visibility = ViewStates.Invisible);
            await Task<bool>.Factory.StartNew(() => ChangeViewBgImg(button, MolePic));
            return true;
        }

        public List<Button> GetButtonList(params int[] ButtonIds)
        {
            List<Button> buttons = new List<Button>();
            foreach(int buttonId in ButtonIds)
            {
                Button button = FindViewById<Button>(buttonId);
                buttons.Add(button);
            }
            return buttons;
        }

        /// <summary>
        /// initing all game buttons to be invisible and add click action
        /// </summary>
        /// <param name="buttons">Buttons.</param>
        public void InitGameButtons(List<Button> buttons)
        {
            foreach(Button button in buttons)
            {
                RunOnUiThread(() => button.Visibility = ViewStates.Invisible);
                button.Click += ButtonClick;
            }
        }

        public bool ChangeViewBgImg(View view, int imgId)
        {
            RunOnUiThread(() => view.SetBackgroundResource(imgId));
            return true;
        }

        /// <summary>
        /// event for clicking one of the 6 game buttons
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void ButtonClick(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Visibility == ViewStates.Visible)
            {
                Score += 1;
                ChangeViewBgImg(button, MoleSuccess);
            }
            RunOnUiThread(() => ScoreTest.Text = Score.ToString());
        }
    }
}
