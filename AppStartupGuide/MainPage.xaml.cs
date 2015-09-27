using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AppStartupGuide
{
    public sealed partial class MainPage : Page
    {
        #region private members

        private const double OVER_PAN = 80d;
        private static bool _loaded;

        #endregion

        #region constructors 
        public MainPage()
        {
            InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(360, 640);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(360, 640));

            Loaded += (s, e) =>
            {
                UpdateBounds();

                _loaded = true;
            };

            SizeChanged += (s, e) =>
            {
                PageHeight = ActualHeight;

                if (_loaded)
                {
                    UpdateBounds();
                }
            };

            ScrollingHost.ViewChanged += (s, e) =>
            {
                UpdateBounds();
            };
        }

        #endregion

        #region properties

        public double PageHeight
        {
            get { return (double)GetValue(PageHeightProperty); }
            set { SetValue(PageHeightProperty, value); }
        }

        public static readonly DependencyProperty PageHeightProperty =
            DependencyProperty.Register("PageHeight", typeof(double), typeof(MainPage), new PropertyMetadata(0d));

        #endregion

        #region methods

        private void UpdateBounds()
        {
            if (ScrollingHost.IsItemVisible(Section3))
            {
                UpdateCurrentAndNext(this.Section3, Geometry3, Geometry4);
            }
            else if (this.ScrollingHost.IsItemVisible(this.Section2))
            {
                UpdateCurrentAndNext(this.Section2, Geometry2, Geometry3);
            }
            else if (this.ScrollingHost.IsItemVisible(this.Section1))
            {
                UpdateCurrentAndNext(this.Section1, Geometry1, Geometry2);
            }
        }

        private void UpdateCurrentAndNext(Panel section, RectangleGeometry current, RectangleGeometry next)
        {
            var distance = ScrollingHost.DistanceFromTop(section);

            current.Rect = new Rect(0, distance - ActualHeight + OVER_PAN, ActualWidth, ActualHeight);
            next.Rect = new Rect(0, distance + OVER_PAN, ActualWidth, ActualHeight);
        }

        #endregion
    }
}