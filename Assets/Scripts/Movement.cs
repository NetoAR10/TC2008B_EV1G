using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esta línea para trabajar con la UI

public class Movement : MonoBehaviour
{
    public float speed = 10f; // Velocidad de movimiento
    public float rotationSpeed = 100f; // Velocidad de rotación
    public int maxHealth = 100; // Vida máxima del jugador
    public float damageDistance = 1.0f; // Distancia mínima para considerar que la bala golpeó al jugador
    public Text healthText; // Referencia al objeto de texto UI donde se mostrará la vida del jugador

    private int currentHealth;

    void Start()
    {
        // Establecer la vida inicial
        currentHealth = maxHealth;

        // Actualizar la UI con la vida inicial
        UpdateHealthUI();
    }

    void Update()
    {
        // Modificar la velocidad de movimiento si se presiona Shift
        float adjustedSpeed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? speed / 2f : speed;

        // Movimiento hacia adelante y hacia atrás
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * adjustedSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * adjustedSpeed * Time.deltaTime);
        }

        // Rotación en su propio eje
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // Checar si alguna bala está cerca del jugador
        CheckForBulletProximity();
    }

    void CheckForBulletProximity()
    {
        // Encuentra todas las balas en la escena
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        // Verifica la distancia de cada bala al jugador
        foreach (GameObject bullet in bullets)
        {
            if (Vector3.Distance(transform.position, bullet.transform.position) <= damageDistance)
            {
                // Si la bala está lo suficientemente cerca, el jugador recibe daño
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
        Debug.Log("Jugador recibió daño. Vida actual: " + currentHealth);

        // Actualizar la UI con la nueva vida
        UpdateHealthUI();

        // Si la vida llega a 0, destruye el objeto del jugador
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para manejar la muerte del jugador
    void Die()
    {
        Debug.Log("Jugador ha muerto.");
        Destroy(gameObject);
    }

    // Método para actualizar el texto de la UI con la vida del jugador
    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
    }
}
