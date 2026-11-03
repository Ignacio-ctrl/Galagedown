
using System.Collections;
using UnityEngine;

public class expansiveWave : MonoBehaviour
{
    public float radioMax = 10f;
    public float duracion = 0.5f;
    public float coolDownTime = 10f;

    [SerializeField, Min(1)]
    private int maxCharges = 3;

    private int currentCharges;
    private float rechargeTimer;
    private bool waveIsActive;

    public int CurrentCharges => currentCharges;
    public int MaxCharges => maxCharges;

    private void Awake()
    {
        maxCharges = Mathf.Max(1, maxCharges);
        currentCharges = maxCharges;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        // Recuperar cargas individualmente
        if (currentCharges < maxCharges)
        {
            rechargeTimer += Time.deltaTime;

            float rechargeTime =
                Mathf.Max(0.05f, coolDownTime);

            if (rechargeTimer >= rechargeTime)
            {
                currentCharges++;
                rechargeTimer -= rechargeTime;
            }
        }
        else
        {
            rechargeTimer = 0f;
        }

        // Utilizar una carga
        if (Input.GetKeyDown(KeyCode.Q) &&
            currentCharges > 0 &&
            !waveIsActive)
        {
            currentCharges--;
            StartCoroutine(ExpandWaveFromPlayer());
        }
    }

    public void AddChargeCapacity(int amount)
    {
        if (amount <= 0)
            return;

        maxCharges += amount;
        currentCharges += amount;

        Debug.Log(
            "Ondas disponibles: " +
            currentCharges + "/" + maxCharges
        );
    }

    private IEnumerator ExpandWaveFromPlayer()
    {
        waveIsActive = true;

        Vector2 playerPos = transform.position;

        float timer = 0f;
        float duration = Mathf.Max(0.01f, duracion);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float radius = Mathf.Lerp(
                0f, radioMax, timer / duration
            );

            ClearBulletsFromPosition(playerPos, radius);

            yield return null;
        }

        waveIsActive = false;
    }

    private void ClearBulletsFromPosition(
        Vector2 center,
        float radius)
    {
        GameObject[] bullets =
            GameObject.FindGameObjectsWithTag("EnemieBullet");

        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
            {
                float distance = Vector2.Distance(
                    center,
                    bullet.transform.position
                );

                if (distance <= radius)
                    Destroy(bullet);
            }
        }
    }
}
