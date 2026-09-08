using UnityEngine;

public class RewindManager
{
    private SimpleArrayStack<PlayerPowerUpState> stateHistory = new SimpleArrayStack<PlayerPowerUpState>();

    public void SaveState(PlayerPowerUpState currentState)
    {
        stateHistory.Push(currentState);
    }

    public PlayerPowerUpState Rewind()
    {
        if (stateHistory.IsEmpty())
            return null;

        return stateHistory.Pop();
    }
}
