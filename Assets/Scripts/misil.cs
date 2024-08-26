using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missilePrefab; // Prefab del misil
    public Transform target; // El objeto hacia el que se dirigirá el misil
    public float missileSpeed = 20f; // Velocidad del misil
    public float missileLifetime = 5f; // Tiempo de vida del misil antes de destruirse

    void Update()
    {
        // Dispara el misil cuando se presiona la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchMissile();
        }
    }

    void LaunchMissile()
    {
        // Instancia el misil en la posición y rotación del objeto actual
        GameObject missile = Instantiate(missilePrefab, transform.position, transform.rotation);

        // Configura la dirección del misil hacia el objetivo
        Vector3 direction = (target.position - transform.position).normalized;
        missile.GetComponent<Rigidbody>().velocity = direction * missileSpeed;

        // Destruye el misil después de un tiempo
        Destroy(missile, missileLifetime);
    }
}
