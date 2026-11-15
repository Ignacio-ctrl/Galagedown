
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

        //busca el tipo de enemigo
        EnemyTypeHolder holder = enemy.GetComponent<EnemyTypeHolder>();

        //dificultad segun ronda
        float finalSpeed = speedMultiplier;
        float finalCadence = cadenceMultiplier;

        //aplica estadisticas
        if(holder != null && holder.data != null)
        {
            finalSpeed *= holder.data.speedMultiplier;
            finalCadence *= holder.data.cadenceMultiplier;
        }

        // Configuramos su velocidad y cadencia enem normal
        enemieMovement movement =
            enemy.GetComponent<enemieMovement>();

        if (movement != null)
        {
            movement.ConfigureDifficulty(
                finalSpeed,
                finalCadence);
        }

        //enemigo nuevo
        FanEnemy fan = enemy.GetComponent<FanEnemy>();

        if (fan != null)
        {
            fan.ConfigureDifficulty(
                finalSpeed,
                finalCadence);
        }

        return enemy;
    }
}

