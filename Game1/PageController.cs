using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;

namespace Game1
{
    /// <summary>
    /// this class implements methods for switching between activities to be used by activities
    /// functions:
    /// switching to a new activity with or without parameters(bundle)
    /// close keyboard - closes the keyboard from a given input edittext
    /// </summary>
    public class PageController 
    {
        public Android.Content.Context Context;

        public PageController(Android.Content.Context p_context)
        {
            Context = p_context;
        }

        public void CloseKeyboard(EditText editText)
        {
            //close the keyboard - MAKE GENERAL
            InputMethodManager imm = (InputMethodManager)Context.GetSystemService("input_method"); 
            imm.HideSoftInputFromWindow(editText.WindowToken, 0);
        }

        public void GotoPage(Type activityType)
        {
            Intent intent = new Intent(Context, activityType);
            Context.StartActivity(intent);
        }

        public void GotoPage(Type activityType, int bundle, string bundelDescription)
        {
            Intent intent = new Intent(Context, activityType);
            intent.PutExtra(bundelDescription, bundle); //maybe member this??

            Context.StartActivity(intent);
        }

        public void GotoPage(Type activityType, bool bundle, string bundelDescription)
        {
            Intent intent = new Intent(Context, activityType);
            intent.PutExtra(bundelDescription, bundle); //maybe member this??

            Context.StartActivity(intent);
        }

        public void GotoPage(Type activityType, string bundle, string bundelDescription)
        {
            Intent intent = new Intent(Context, activityType);
            intent.PutExtra(bundelDescription, bundle); //maybe member this??
            Context.StartActivity(intent);
        }

        public void GotoPage(Type activityType, object bundle, string bundelDescription)
        {
            Intent intent = new Intent(Context, activityType);
            if(bundle is string)
                intent.PutExtra(bundelDescription, (Bundle)((string)bundle)); //maybe member this??
            else if (bundle is int)
                intent.PutExtra(bundelDescription, (Bundle)((int)bundle)); //maybe member this??
            Context.StartActivity(intent);
        }

        public void GotoPage(Type activityType, List<KeyValuePair<string ,int>> bundleDescriptions)
        {

            Intent intent = new Intent(Context, activityType);
            foreach(KeyValuePair <string, int> bundesc in bundleDescriptions)
            {
                intent.PutExtra(bundesc.Key, bundesc.Value);
            }

            Context.StartActivity(intent);
        }
    }
}
