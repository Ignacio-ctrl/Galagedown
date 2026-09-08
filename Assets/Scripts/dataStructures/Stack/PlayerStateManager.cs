using UnityEditor;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public bool HasSpeedBoost = false;
    public bool HasShield = false;
    public bool HasDamageBoost = false;

    private RewindManager rewindManager = new RewindManager();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivatePowerUp(ref HasSpeedBoost, "Speed Boost");
            Debug.Log("Speed activado");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivatePowerUp(ref HasDamageBoost, "Damage Boost");
            Debug.Log("Damage activado");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivatePowerUp(ref HasShield, "Shield");
            Debug.Log("Shield activado");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RewindOneStep();
        }
    }

    void ActivatePowerUp(ref bool flag, string nombre)
    {
        PlayerPowerUpState ultimoEstado = new PlayerPowerUpState(HasSpeedBoost, HasDamageBoost, HasShield);
        rewindManager.SaveState(ultimoEstado);

        flag = true;
    }

    void RewindOneStep()
    {
        PlayerPowerUpState previous = rewindManager.Rewind();
        if (previous == null)    
        {
            Debug.Log("No hay estado activado");
            return;
        }
        HasSpeedBoost = previous.SpeedBoost;
        HasShield = previous.Shield;
        HasDamageBoost = previous.DamageBoost;
    }
}
