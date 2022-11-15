namespace Pearl.Tweens
{
    public interface ITween : IPause
    {
        public void Stop();

        public void ResetTween();

        public void ForceFinish();

        public void Kill();

        public void Force(float percent);

        public void Play(bool setInit = false);
    }
}
