
using UnityEngine;

public static class EnemyFactory
{
    public static GameObject CreateEnemy(
        GameObject prefab,
        Vector3 position,
        Quaternion rotation,
        float speedMultiplier,
        float cadenceMultiplier)
    {
        if (prefab == null)
            return null;

        // Creamos el enemigo
        GameObject enemy =
            Object.Instantiate(prefab, position, rotation);

        // Configuramos su velocidad y cadencia
        enemieMovement movement =
            enemy.GetComponent<enemieMovement>();

        if (movement != null)
        {
            movement.ConfigureDifficulty(
                speedMultiplier,
                cadenceMultiplier);
        }

        //enemigo nuevo
        FanEnemy fan = enemy.GetComponent<FanEnemy>();

        if (fan != null)
        {
            fan.ConfigureDifficulty(
                speedMultiplier,
                cadenceMultiplier);
        }

        return enemy;
    }
}

