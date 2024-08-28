using MyScreen;

namespace Interfaces
{
    public interface IMovable
    {
        public void Init(CurrentScreen currentScreen, IGetSizeable gameObjectSize);

        public void Move();
    }
}
