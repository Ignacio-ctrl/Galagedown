using UnityEngine;

public class WaveCommand : IPlayerCommand
{
    private expansiveWave wave;

    public WaveCommand(expansiveWave wave)
    {
        this.wave = wave;
    }

    public void Execute()
    {
        if (wave != null)
            wave.TryActivateWave();
    }
}
