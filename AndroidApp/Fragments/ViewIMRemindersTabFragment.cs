using Android.App;
using Android.OS;
using Android.Views;

namespace AndroidApp.Resources.layout
{
    public class ViewIMRemindersTabFragment : Fragment
    {
        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create Fragment View
            var view = inflater.Inflate(Resource.Layout.ViewIMRemindersTabFragment, container, false);

            // TODO: Set Everything

            return view;
        }
    }
}