using System.Xml.Serialization;
using UnityEngine;

public class PlayerCommandInput : MonoBehaviour
{
    private IPlayerCommand attackCommand;
    private IPlayerCommand waveCommand;
    private IPlayerCommand previousWeaponCommand;

    private IPlayerCommand[] weaponCommands;

    private void Awake()
    {
        PlayerWeaponController weapons = GetComponent<PlayerWeaponController>();

        expansiveWave wave = GetComponent<expansiveWave>();

        //se crean los comandos
        attackCommand = new AttackCommand(weapons);
        waveCommand = new WaveCommand(wave);
        previousWeaponCommand = new PreviousWeaponCommand(weapons);

        weaponCommands = new IPlayerCommand[]
        {
            new ChangeWeaponCommand(weapons,Weapon.WeaponType.Normal),
            new ChangeWeaponCommand(weapons,Weapon.WeaponType.Escopeta),
            new ChangeWeaponCommand(weapons,Weapon.WeaponType.Circular),
            new ChangeWeaponCommand(weapons,Weapon.WeaponType.EscopetaDoble)
        };
    }

    
   private void Update()
   {
        if (Time.timeScale == 0f)
        {
            return;
        }

        //dispara
        if (Input.GetButton("Fire1"))
        {
            attackCommand.Execute();
        }
        //onda expansiva
        if (Input.GetKeyDown(KeyCode.Q))
        {
            waveCommand.Execute();
        }
        //cambiar armas
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponCommands[0].Execute();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponCommands[1].Execute();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponCommands[2].Execute();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weaponCommands[3].Execute();
        }
        //volver a arma anterior
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            previousWeaponCommand.Execute();
        }

    }
}
