using LaVida.Controls;
using LaVida.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]
namespace LaVida.iOS.Renderers
{
    public class ExtendedListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.AllowsSelection = true;
                    Control.AlwaysBounceVertical = false;
                    Control.Bounces = true;
                }
            }
        }
    }
}
