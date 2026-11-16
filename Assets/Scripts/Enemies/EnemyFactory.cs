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

        // Dificultad segun ronda
        float finalSpeed = speedMultiplier;
        float finalCadence = cadenceMultiplier;

        // Type Object: los multiplicadores propios del tipo de enemigo
        EnemyTypeHolder holder = enemy.GetComponent<EnemyTypeHolder>();

        if (holder != null && holder.data != null)
        {
            finalSpeed *= holder.data.speedMultiplier;
            finalCadence *= holder.data.cadenceMultiplier;
        }

        // Configura a cualquier componente que cumpla el contrato,
        // sin conocer qué enemigo concreto es
        IDifficultyConfigurable[] configurables =
            enemy.GetComponents<IDifficultyConfigurable>();

        for (int i = 0; i < configurables.Length; i++)
        {
            configurables[i].ConfigureDifficulty(finalSpeed, finalCadence);
        }

        return enemy;
    }

    // Para enemigos que aparecen fuera de las rondas (por ejemplo la horda)
    public static GameObject CreateEnemy(
        GameObject prefab,
        Vector3 position,
        Quaternion rotation)
    {
        return CreateEnemy(prefab, position, rotation, 1f, 1f);
    }
}