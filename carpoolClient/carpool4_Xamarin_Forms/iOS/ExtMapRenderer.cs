using MapKit;
using Carpool;
using Carpool.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtMap), typeof(ExtMapRenderer))]
namespace Carpool.iOS.CustomRenderers
{
    /// <summary>
    /// http://kampeki-factory.blogspot.mx/2015/08/xamarinformsmaps-tap-to-get-position-on.html
    /// Renderer for the xamarin ios map control
    /// </summary>
    public class ExtMapRenderer : MapRenderer
    {
        private readonly UITapGestureRecognizer _tapRecogniser;

        public ExtMapRenderer()
        {
            _tapRecogniser = new UITapGestureRecognizer(OnTap)
            {
                NumberOfTapsRequired = 1,
                NumberOfTouchesRequired = 1
            };
        }

        private void OnTap(UITapGestureRecognizer recognizer)
        {
            var cgPoint = recognizer.LocationInView(Control);

            var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);

            ((ExtMap)Element).OnTap(new Position(location.Latitude, location.Longitude));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            if (Control != null)
                Control.RemoveGestureRecognizer(_tapRecogniser);

            base.OnElementChanged(e);

            if (Control != null)
                Control.AddGestureRecognizer(_tapRecogniser);
        }
    }
}
