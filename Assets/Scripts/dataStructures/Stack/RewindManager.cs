using UnityEngine;

public class RewindManager
{
    private SimpleArrayStack<ISimpleSet<PowerUpType>> stateHistory =
        new SimpleArrayStack<ISimpleSet<PowerUpType>>();

    public void SaveState(ISimpleSet<PowerUpType> current)
    {
        // Guardamos una COPIA, no la referencia
        stateHistory.Push(new SimpleArraySet<PowerUpType>(current));
    }

    public ISimpleSet<PowerUpType> Rewind()
    {
        if (stateHistory.IsEmpty()) return null;
        return stateHistory.Pop();
    }
}