namespace Background
{
    public interface IJumpUppable
    {
        public void Init(IBackgroundAdjuster adjuster);

        public void Move();

        public void JumpUp();
    }
}
