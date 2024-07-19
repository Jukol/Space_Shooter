namespace Infrastructure.Signals
{
    public class WaveCompleted
    {
        public int WaveNumber;
        
        public WaveCompleted(int waveNumber)
        {
            WaveNumber = waveNumber;
        }
    }
}