
using System.Collections;
using UnityEngine;

public class RadialShotWeapon : MonoBehaviour
{
    
    [SerializeField] private RadialShotPattern shotPattern;

    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private bool isShooting = false;

    public bool ShootPattern(float initialAngle = 0f)
    {
        if (!isActiveAndEnabled || isShooting)
            return false;

        if (shotPattern == null || bulletPrefab == null)
            return false;

        if (shotPattern.PatternSettings == null ||
            shotPattern.PatternSettings.Length == 0 ||
            shotPattern.Repetitions <= 0)
            return false;

        if (bulletPrefab.GetComponent<bulletMovement>() == null)
        {
            Debug.LogError(
                "El proyectil necesita bulletMovement", this);
            return false;
        }

        StartCoroutine(ExecutePattern(initialAngle));
        return true;
    }

    private IEnumerator ExecutePattern(float initialAngle)
    {
        isShooting = true;

        if (shotPattern.StarWait > 0f)
            yield return new WaitForSeconds(shotPattern.StarWait);

        for (int lap = 0; lap < shotPattern.Repetitions; lap++)
        {
            float rotation =
                initialAngle +
                shotPattern.AngleOffsetBetweenReps * lap;

            for (int i = 0; i < shotPattern.PatternSettings.Length; i++)
            {
                RadialShotSettings settings =
                    shotPattern.PatternSettings[i];

                if (settings == null ||
                    settings.NumberOfBullets <= 0)
                    continue;

                FireWave(settings, rotation);

                yield return new WaitForSeconds(
                    Mathf.Max(0.05f, settings.CooldownAfterShot));
            }
        }

        if (shotPattern.EndWait > 0f)
            yield return new WaitForSeconds(shotPattern.EndWait);

        isShooting = false;
    }

    private void FireWave(RadialShotSettings settings, float rotation)
    {
        float step = 360f / settings.NumberOfBullets;

        for (int i = 0; i < settings.NumberOfBullets; i++)
        {
            float angle = i * step;

            if (settings.RadialMask &&
                angle > settings.MaskAngle)
                break;

            float finalAngle =
                rotation +
                settings.AngleOffset +
                settings.PhaseOffset * step +
                angle;

            Vector3 position =
                firePoint != null
                ? firePoint.position
                : transform.position;

            //bulletMovement avanza usando transform.up
            Quaternion bulletRotation =
                Quaternion.Euler(0f, 0f, finalAngle - 90f);

            GameObject bullet = BulletPool.Instance.Get(
                bulletPrefab,
                position,
                bulletRotation);

            bulletMovement movement =
                bullet.GetComponent<bulletMovement>();

            //conservamos el daño del prefab
            movement.Configure(
                settings.BulletSpeed,
                movement.damage);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isShooting = false;
    }
}
