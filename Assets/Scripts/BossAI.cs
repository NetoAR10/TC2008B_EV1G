using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esta línea para trabajar con la UI

public class BossAI : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float moveSpeed = 3f; // Velocidad de movimiento del jefe
    public float stopDistance = 5f; // Distancia mínima para evitar colisiones
    public Transform firePoint; // Punto de salida de los proyectiles
    public GameObject bulletPrefab; // Prefab de las balas
    public float bulletSpeed = 10f; // Velocidad de las balas
    public float fireRate = 0.1f; // Cadencia de disparo en algunos patrones
    public float spiralFireRate = 0.02f; // Cadencia de disparo específica para PatternTwo
    public float invisibilityDuration = 30f; // Duración de la invisibilidad al inicio
    public int maxHealth = 200; // Vida máxima del jefe
    public Text healthText; // Referencia al objeto de texto UI donde se mostrará la vida del jefe
    public float damageDistance = 20f; // Distancia mínima para considerar que la bala golpeó al jefe

    private int currentHealth;
    private int currentPattern = 0; // Patrón actual
    private float nextPatternTime = 0f;
    private float nextFireTime = 0f;
    private float nextSpiralFireTime = 0f; // Tiempo de disparo específico para PatternTwo
    private float spiralAngle = 0f; // Ángulo para la espiral
    private bool isVisible = false; // Estado de visibilidad del jefe

    void Start()
    {
        // Establecer la vida inicial del jefe
        currentHealth = maxHealth;

        // Actualizar la UI con la vida inicial
        UpdateHealthUI();

        // Configura el tiempo para el primer cambio de patrón
        nextPatternTime = Time.time + 10f;

        // Iniciar la invisibilidad del jefe
        StartCoroutine(InvisibilityCoroutine());
    }

    void Update()
    {
        if (isVisible)
        {
            FollowPlayer();

            if (Time.time >= nextPatternTime)
            {
                // Cambia al siguiente patrón de disparo
                currentPattern = (currentPattern + 1) % 3;
                nextPatternTime = Time.time + 10f;
            }

            ShootPattern();
        }

        // Checar si alguna bala del jugador está cerca del jefe
        CheckForBulletProximity();
    }

    void FollowPlayer()
    {
        // Calcula la dirección hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Mueve al jefe hacia el jugador solo si está más allá de la distancia mínima
        if (distanceToPlayer > stopDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // Hace que el jefe mire hacia el jugador
        transform.LookAt(player);
    }

    void ShootPattern()
    {
        switch (currentPattern)
        {
            case 0:
                PatternOne();
                break;
            case 1:
                PatternTwo();
                break;
            case 2:
                PatternThree();
                break;
        }
    }

    void PatternOne()
    {
        if (Time.time >= nextFireTime)
        {
            int numberOfBullets = 20; // Número de balas para formar el círculo

            for (int i = 0; i < numberOfBullets; i++)
            {
                // Calcula el ángulo para cada bala
                float angle = i * (360f / numberOfBullets);
                // Convierte el ángulo en radianes
                float radianAngle = angle * Mathf.Deg2Rad;
                // Calcula la dirección de la bala usando seno y coseno
                Vector3 direction = new Vector3(Mathf.Cos(radianAngle), 0, Mathf.Sin(radianAngle));

                // Dispara la bala en la dirección calculada
                FireBullet(direction);
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    void PatternTwo()
    {
        if (Time.time >= nextSpiralFireTime)
        {
            // Incrementa el ángulo para la espiral
            spiralAngle += 10f; // Ajusta este valor para controlar la velocidad de la espiral

            // Calcula la dirección de disparo basado en el ángulo de la espiral
            float radianAngle = spiralAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(radianAngle), 0, Mathf.Sin(radianAngle));

            // Dispara la bala en la dirección calculada
            FireBullet(direction);

            // Actualiza el tiempo para el próximo disparo en PatternTwo
            nextSpiralFireTime = Time.time + spiralFireRate;
        }
    }

    void PatternThree()
    {
        if (Time.time >= nextFireTime)
        {
            int numberOfBullets = 10; // Número de picos de la estrella (debe ser par para formar una estrella simétrica)

            for (int i = 0; i < numberOfBullets; i++)
            {
                // Calcula el ángulo para cada pico de la estrella
                float angle = i * (360f / numberOfBullets) * 2; // *2 para alternar los picos largos y cortos
                // Convierte el ángulo en radianes
                float radianAngle = angle * Mathf.Deg2Rad;
                // Calcula la dirección de la bala usando seno y coseno
                Vector3 direction = new Vector3(Mathf.Cos(radianAngle), 0, Mathf.Sin(radianAngle));

                // Dispara la bala en la dirección calculada
                FireBullet(direction);
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    void FireBullet(Vector3 direction)
    {
        // Instancia la bala en la posición del firePoint y la dispara en la dirección indicada
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * bulletSpeed;

        // Destruye la bala después de un tiempo
        Destroy(bullet, 5f);
    }

    void CheckForBulletProximity()
    {
        // Encuentra todas las balas con la etiqueta "BulletP" en la escena
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletP");

        // Verifica la distancia de cada bala al jefe
        foreach (GameObject bullet in bullets)
        {
            if (Vector3.Distance(transform.position, bullet.transform.position) <= damageDistance)
            {
                // Si la bala está lo suficientemente cerca, el jefe recibe daño
                TakeDamage(10); // Ajusta el valor de daño según sea necesario

                // Destruye la bala
                Destroy(bullet);
            }
        }
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Jefe recibió daño. Vida actual: " + currentHealth);

        // Actualizar la UI con la nueva vida
        UpdateHealthUI();

        // Si la vida llega a 0, destruye el objeto del jefe
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para manejar la muerte del jefe
    void Die()
    {
        Debug.Log("Jefe ha muerto.");
        Destroy(gameObject);
    }

    // Método para actualizar el texto de la UI con la vida del jefe
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Boss Health: " + currentHealth;
        }
    }

    IEnumerator InvisibilityCoroutine()
    {
        // Desactivar todos los renderizadores del jefe y sus hijos
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        // Desactivar otros componentes si es necesario, por ejemplo:
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Espera el tiempo de invisibilidad
        yield return new WaitForSeconds(invisibilityDuration);

        // Reactivar todos los renderizadores para hacer visible el jefe
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        // Reactivar los colliders o cualquier otro componente
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        // Permitir que el jefe comience a actuar
        isVisible = true;
    }
}
