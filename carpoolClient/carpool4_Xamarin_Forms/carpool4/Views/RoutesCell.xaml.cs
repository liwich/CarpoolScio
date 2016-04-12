using Xamarin.Forms;

namespace Carpool
{
    public partial class RoutesCell : ViewCell
    {
        public RoutesCell()
        {
            InitializeComponent();

            fromLabel.SetBinding(Label.TextProperty, new Binding("From"));
            toLabel.SetBinding(Label.TextProperty, new Binding("To"));

        }
    }
}