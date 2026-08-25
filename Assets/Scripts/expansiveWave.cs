using System.Collections;

using UnityEngine;

public class expansiveWave : MonoBehaviour
{
    public float radioMax = 10f;
    public float duracion = 0.5f;

    public float coolDownTime = 10f;

    private bool waveIsActive = false;
    private bool onCoolDown = false;
    private float radioActual = 0f;
    private float coolDownTimer = 0f;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (onCoolDown)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer <= 0f)
            {
                onCoolDown = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !waveIsActive && !onCoolDown)
        {
            StartCoroutine(ExpandWaveFromPlayer());
        }
    }

    IEnumerator ExpandWaveFromPlayer()
    {
        waveIsActive = true;
        onCoolDown = true;  
        coolDownTimer = coolDownTime;
        radioActual = 0f;
        Vector2 playerPos = transform.position;

        float timer = 0f;
        while (timer < duracion)
        {
            timer += Time.deltaTime;
            radioActual = Mathf.Lerp(0f, radioMax, timer / duracion);

            // Destruir balas desde la posición del jugador
            ClearBulletsFromPosition(playerPos, radioActual);

            yield return null;
        }

        waveIsActive = false;
    }
    void ClearBulletsFromPosition(Vector2 center, float radius)
    {
        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("EnemieBullet");

        foreach (GameObject bullet in allBullets)
        {
            if (bullet != null)
            {
                float distance = Vector2.Distance(center, bullet.transform.position);
                if (distance <= radius)
                {
                    Destroy(bullet);
                }
            }
        }
    }
}
