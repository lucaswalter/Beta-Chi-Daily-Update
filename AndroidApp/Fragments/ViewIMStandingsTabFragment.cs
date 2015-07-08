using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;

namespace AndroidApp.Resources.layout
{
    public class ViewIMStandingsTabFragment : Fragment
    {

        private ListView imStandingsImListView;
        private TeamAdapter teamAdapter;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create Fragment View
            var view = inflater.Inflate(Resource.Layout.ViewIMStandingsTabFragment, container, false);

            
            // Initialize Adapter And List View

            return view;
        }
    }
}