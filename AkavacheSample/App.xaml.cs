using Akavache;
using Xamarin.Forms;

namespace AkavacheSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            BlobCache.ApplicationName = "AkavacheSample";

            MainPage = new NavigationPage(new AkavacheSamplePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
