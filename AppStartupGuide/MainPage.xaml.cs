using System;
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

        const double OVER_PAN = 80d;
        const double FIXED_TRAVELLED_DISTANCE = 80d;
        const double HEADER_MOVEMENT_FACTOR = -.5;
        const double HEADER_CONTENT_MOVEMENT_FACTOR = -.25;
        static bool _loaded;

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
                UpdateBackgroundBounds();

                _loaded = true;
            };

            SizeChanged += (s, e) =>
            {
                PageHeight = ActualHeight;

                if (_loaded)
                {
                    UpdateBackgroundBounds();
                }
            };

            ScrollingHost.ViewChanged += (s, e) =>
            {
                UpdateBackgroundBounds();

                UpdateHeaders();
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

        void UpdateBackgroundBounds()
        {
            if (ScrollingHost.IsItemVisible(Section3))
            {
                UpdateCurrentAndNext(Section3, Geometry3, Geometry4);
            }
            else if (ScrollingHost.IsItemVisible(Section2))
            {
                UpdateCurrentAndNext(Section2, Geometry2, Geometry3);
            }
            else if (ScrollingHost.IsItemVisible(Section1))
            {
                UpdateCurrentAndNext(Section1, Geometry1, Geometry2);
            }
        }

        void UpdateHeaders()
        {
            UpdateSectionHeader(Section1, Section1Header, Section1HeaderTransform, Section1HeaderContentTransform, 0);
            UpdateSectionHeader(Section2, Section2Header, Section2HeaderTransform, Section2HeaderContentTransform, 1);
            UpdateSectionHeader(Section3, Section3Header, Section3HeaderTransform, Section3HeaderContentTransform, 2);
            UpdateSectionHeader(Section4, Section4Header, Section4HeaderTransform, Section4HeaderContentTransform, 3);
        }

        void UpdateSectionHeader(Panel section, Panel sectionHeader, CompositeTransform headerTransform, CompositeTransform headerContentTransform, double multiplier)
        {
            var travelledDistance = Math.Abs(ScrollingHost.VerticalOffset - ActualHeight * multiplier);
            if (travelledDistance <= FIXED_TRAVELLED_DISTANCE)
            {
                sectionHeader.Opacity = 1;
            }
            else
            {
                var opacity = 1 - travelledDistance / sectionHeader.ActualHeight;
                sectionHeader.Opacity = opacity;
            }

            headerTransform.TranslateY = (ScrollingHost.VerticalOffset - ActualHeight * multiplier) * HEADER_MOVEMENT_FACTOR;
            headerContentTransform.TranslateY = (ScrollingHost.VerticalOffset - ActualHeight * multiplier) * HEADER_CONTENT_MOVEMENT_FACTOR;
        }

        void UpdateCurrentAndNext(Panel section, RectangleGeometry current, RectangleGeometry next)
        {
            var distance = ScrollingHost.DistanceFromTop(section);

            current.Rect = new Rect(0, distance - ActualHeight + OVER_PAN, ActualWidth, ActualHeight);
            next.Rect = new Rect(0, distance + OVER_PAN, ActualWidth, ActualHeight);
        }

        #endregion
    }
}