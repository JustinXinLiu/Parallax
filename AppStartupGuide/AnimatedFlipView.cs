using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
namespace AppStartupGuide
{
    public sealed class AnimatedFlipView : FlipView
    {
        public AnimatedFlipView()
        {
            this.DefaultStyleKey = typeof(AnimatedFlipView);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new AnimatedFlipViewItem();
        }
    }
}
