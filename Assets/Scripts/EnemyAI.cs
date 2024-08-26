using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esta línea para trabajar con la UI

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float moveSpeed = 5f; // Velocidad de movimiento del enemigo
    public float shootingRange = 15f; // Distancia a la que el enemigo empieza a disparar
    public float stopDistance = 2f; // Distancia mínima para evitar colisiones
    public float fireRate = 1f; // Cadencia de disparo del enemigo (disparos por segundo)
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform firePoint; // Punto de salida de las balas
    public float bulletSpeed = 20f; // Velocidad de las balas
    public float activeTime = 30f; // Tiempo después del cual los enemigos desaparecen
    public int maxHealth = 50; // Vida máxima del enemigo
    public Text healthText; // Referencia al objeto de texto UI donde se mostrará la vida del enemigo
    public float damageDistance = 8.0f; // Distancia mínima para considerar que la bala golpeó al enemigo

    private float nextFireTime = 0f;
    private int currentHealth;

    void Start()
    {
        // Si no se ha asignado un jugador en el inspector, buscarlo en la escena
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Establecer la vida inicial del enemigo
        currentHealth = maxHealth;

        // Actualizar la UI con la vida inicial
        UpdateHealthUI();

        // Destruir este enemigo después del tiempo especificado
        Destroy(gameObject, activeTime);
    }

    void Update()
    {
        FollowPlayer();

        if (IsPlayerInRange())
        {
            ShootAtPlayer();
        }

        // Checar si alguna bala del jugador está cerca del enemigo
        CheckForBulletProximity();
    }

    void FollowPlayer()
    {
        // Calcula la dirección hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Mueve al enemigo hacia el jugador solo si está más allá de la distancia mínima
        if (distanceToPlayer > stopDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // Hace que el enemigo mire hacia el jugador
        transform.LookAt(player);
    }

    bool IsPlayerInRange()
    {
        // Verifica si el jugador está dentro del rango de disparo
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= shootingRange;
    }

    void ShootAtPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            // Instancia la bala y la dispara hacia el jugador
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * bulletSpeed;

            // Establece el próximo tiempo de disparo
            nextFireTime = Time.time + 1f / fireRate;

            // Destruye la bala después de un tiempo (opcional)
            Destroy(bullet, 2f);
        }
    }

    void CheckForBulletProximity()
    {
        // Encuentra todas las balas con la etiqueta "BulletP" en la escena
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletP");

        // Verifica la distancia de cada bala al enemigo
        foreach (GameObject bullet in bullets)
        {
            if (Vector3.Distance(transform.position, bullet.transform.position) <= damageDistance)
            {
                // Si la bala está lo suficientemente cerca, el enemigo recibe daño
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
        Debug.Log("Enemigo recibió daño. Vida actual: " + currentHealth);

        // Actualizar la UI con la nueva vida
        UpdateHealthUI();

        // Si la vida llega a 0, destruye el objeto del enemigo
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para manejar la muerte del enemigo
    void Die()
    {
        Debug.Log("Enemigo ha muerto.");
        Destroy(gameObject);
    }

    // Método para actualizar el texto de la UI con la vida del enemigo
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Enemy Health: " + currentHealth;
        }
    }
}
