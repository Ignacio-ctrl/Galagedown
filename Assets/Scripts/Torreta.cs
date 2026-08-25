using UnityEngine;

public class Torreta : MonoBehaviour
{
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float interval = 5f;      
        [SerializeField] private int bulletsPerWave = 12;  
        [SerializeField] private float bulletSpeed = 4f;   
        void Start()
        {
            InvokeRepeating("ShootCircularWave", 1f, interval);
        }

        void ShootCircularWave()
        {
            float angleStep = 360f / bulletsPerWave;

            // Crear todas las balas
            for (int i = 0; i < bulletsPerWave; i++)
            {
                // Calcular ángulo de esta bala
                float angle = i * angleStep;

                // Convertir ángulo a dirección (matemáticas simples)
                Vector2 direction = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),  // X
                    Mathf.Sin(angle * Mathf.Deg2Rad)   // Y
                );

                // Crear la bala
                CreateBullet(direction);
            }

        
        }

        void CreateBullet(Vector2 direction)
        {
            if (bulletPrefab == null) return;

            // Instanciar bala en posición de la torreta
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * bulletSpeed;
            }
            else
            {
                bullet.transform.Translate(direction.normalized * bulletSpeed * Time.deltaTime);
            }

            // Rotar bala para que mire en su dirección
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }