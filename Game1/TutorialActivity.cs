
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
    /// this activity allows the user to move through the tutorial presentation or to go back to the main menu
    /// in the end of the presentation there is also an option to start playing
    /// </summary>
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class TutorialActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Tutorial);

            PageController pageController = new PageController(this);
            Button forward = FindViewById <Button>(Resource.Id.forwardButton);
            Button back = FindViewById<Button>(Resource.Id.backButton);
            Button menuButton = FindViewById<Button>(Resource.Id.MenuButton);
            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.tutorialLayout);
            int UserId = Intent.Extras.GetInt(Common.USERID);
            int pagecounter = 1;
            int tutorialPageId = 0;

            //the forward button will allow to go forward thru the presentation and will be disabled on the last page
            forward.Click += (sender, e) =>
            {
                if ((pagecounter >= 1) && (pagecounter < 9))
                {
                    switch (pagecounter)
                    {

                        case 1:
                            pagecounter++;
                            back.Clickable = true;
                            tutorialPageId = Resource.Drawable.tutorial2;
                            break;
                        case 2:
                            pagecounter++;
                            tutorialPageId = Resource.Drawable.tutorial3;
                            break;
                        case 3:
                            pagecounter++;
                            tutorialPageId = Resource.Drawable.tutorial4;
                            break;
                        case 4:
                            pagecounter++;
                            tutorialPageId = Resource.Drawable.tutorial5;
                            break;
                        case 5:
                            pagecounter++;
                            tutorialPageId = Resource.Drawable.tutorial6;
                            break;
                        case 6:
                            pagecounter++;
                            tutorialPageId = Resource.Drawable.tutorial7;
                            break;
                        case 7:
                            pagecounter++;
                            tutorialPageId = Resource.Drawable.tutorial8;
                            break;
                        case 8:
                            pagecounter++;
                            tutorialPageId = Resource.Drawable.tutorial9;
                            break;
                        case 9:
                            pagecounter = 10;
                            break;
                    }
                    ChangeViewBgImg(linearLayout, tutorialPageId);
                }
            };
            //the back button will allow to go back thru the presentation and will be disabled on the first page
            back.Click += (sender, e) =>
            {
                if ((pagecounter> 1)&&(pagecounter<=9))
                {
                    switch (pagecounter)
                    {
                        case 1:
                            pagecounter = 0;
                            break;
                        case 2:
                            pagecounter--;
                            tutorialPageId = Resource.Drawable.tutorial1;
                            break;
                        case 3:
                            pagecounter--;
                            tutorialPageId = Resource.Drawable.tutorial2;
                            break;
                        case 4:
                            pagecounter--;
                            tutorialPageId = Resource.Drawable.tutorial3;
                            break;
                        case 5:
                            pagecounter--;
                            tutorialPageId = Resource.Drawable.tutorial4;
                            break;
                        case 6:
                            pagecounter--;
                            tutorialPageId = Resource.Drawable.tutorial5;
                            break;
                        case 7:
                            pagecounter--;
                            tutorialPageId = Resource.Drawable.tutorial6;
                            break;
                        case 8:
                            pagecounter--;
                            tutorialPageId = Resource.Drawable.tutorial7;
                            break;
                        case 9:
                            pagecounter--;
                            tutorialPageId = Resource.Drawable.tutorial8;
                            break;
                    }
                    ChangeViewBgImg(linearLayout, tutorialPageId);
                }
            };

            //move through to the game if presentation is on last page
            linearLayout.Click += (sender, e) =>
            {
                if (pagecounter == 9)
                    pageController.GotoPage(typeof(GameActivity), UserId, "UserId");
            };

            menuButton.Click += (sender, e) =>
            {
                pageController.GotoPage(typeof(MenuActivity), UserId, "UserId");
            };
        }

        public bool ChangeViewBgImg(View view, int imgId)
        {

            RunOnUiThread(() => view.SetBackgroundResource(imgId));
            return true;
        }
    }
}
