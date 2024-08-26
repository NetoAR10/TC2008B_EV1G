using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab de la bala
    public Transform bulletSpawnPoint;  // Punto de aparición de la bala
    public float bulletSpeed = 20f;  // Velocidad de la bala

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instancia la bala en el punto de aparición
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Añade velocidad a la bala
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        // Opcional: Destruir la bala después de un tiempo para evitar saturar la escena
        Destroy(bullet, 2.0f);
    }
}
