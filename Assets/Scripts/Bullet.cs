using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 20f; // Velocidad de la bala
    public float detectionRadius = 0.5f; // Radio para detectar colisiones simuladas con el jugador
    public LayerMask playerLayer; // Capa del jugador para filtrar la detección

    void Update()
    {
        // Mueve la bala hacia adelante
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        // Realiza un raycast para detectar al jugador
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, detectionRadius, out hit, bulletSpeed * Time.deltaTime, playerLayer))
        {
            // Verifica si la bala golpeó al jugador
            if (hit.collider.CompareTag("Player"))
            {
                // Inflige daño al jugador
                hit.collider.GetComponent<Movement>().TakeDamage(10); // Ajusta el valor de daño según sea necesario

                // Destruye la bala
                Destroy(gameObject);
            }
        }

        // Opcional: Destruye la bala después de cierto tiempo
        Destroy(gameObject, 5f);
    }
}
