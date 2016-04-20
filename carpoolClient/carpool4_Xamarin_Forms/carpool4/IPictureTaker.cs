
namespace Carpool
{
    public interface IPictureTaker
    {
        void SnapPic();

        void SelectPic();

        byte[] Rotate(byte[] image, int g);
    }
}
