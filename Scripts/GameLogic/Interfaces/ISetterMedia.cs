namespace Pearl
{
    public interface ISetterMedia : IPause
    {
        public float Puntator { get; set; }
        public float Activate { get; }


        public void Stop();

        public void Reset();

        public void ForceFinish();

        public void Play();
    }
}

