using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "EnemyTypeData", menuName = "Scriptable Objects/EnemyTypeData")]
public class EnemyTypeData : ScriptableObject
{
    public string enemyName;

    [Min(0.1f)] public float speedMultiplier = 1f;
    [Min(0.1f)] public float cadenceMultiplier = 1f;


    [Min(1)] public int firstRound = 1;
}
