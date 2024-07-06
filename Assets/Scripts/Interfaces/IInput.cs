namespace Interfaces
{
    public interface IInput : IService
    {
        public float Horizontal { get; }
        public float Vertical { get; }
        public bool IsFire { get; }

        public void UserInput();
    }
}
