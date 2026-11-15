using UnityEngine;

public class EnemyTypeHolder : MonoBehaviour
{
    [SerializeField] private EnemyTypeData enemyType;

    public EnemyTypeData data => enemyType;
}
