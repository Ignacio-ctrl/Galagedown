using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private ISimpleSet<PowerUpType> active = new SimpleArraySet<PowerUpType>();
    private RewindManager rewindManager = new RewindManager();

    // Mantienen compatibilidad si otros scripts leen estas variables
    public bool HasSpeedBoost => active.Contains(PowerUpType.SpeedBoost);
    public bool HasShield => active.Contains(PowerUpType.Shield);
    public bool HasDamageBoost => active.Contains(PowerUpType.DamageBoost);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ActivatePowerUp(PowerUpType.SpeedBoost);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ActivatePowerUp(PowerUpType.Shield);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ActivatePowerUp(PowerUpType.DamageBoost);
        if (Input.GetKeyDown(KeyCode.R)) RewindOneStep();
    }

    void ActivatePowerUp(PowerUpType type)
    {
        if (active.Contains(type))
        {
            Debug.Log($"{type} ya estaba activo");
            return;
        }

        rewindManager.SaveState(active); // guardamos ANTES de agregar
        active.Add(type);
        Debug.Log($"{type} activado");
    }

    void RewindOneStep()
    {
        ISimpleSet<PowerUpType> previous = rewindManager.Rewind();
        if (previous == null)
        {
            Debug.Log("No hay estado para volver");
            return;
        }

        // Lo que estaba activo ahora y NO estaba antes = lo que se apaga
        foreach (PowerUpType t in active.DifferenceWith(previous).ToArray())
            Debug.Log($"{t} desactivado por rewind");

        active = previous;
    }
}