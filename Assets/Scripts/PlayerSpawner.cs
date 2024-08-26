using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab del jugador que se va a generar
    public bool respawnOnDeath = false; // Determina si el jugador debe reaparecer al morir
    public float respawnDelay = 3f; // Tiempo de espera antes de reaparecer

    private GameObject currentPlayerInstance;
    private Transform spawnPoint;

    void Start()
    {
        // Encuentra el objeto "playerSpawner" en la escena
        spawnPoint = GameObject.Find("PlayerSpawner").transform;

        if (spawnPoint == null)
        {
            Debug.LogError("No se encontró un objeto llamado 'playerSpawner' en la escena.");
            return;
        }

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Instanciar el jugador en la posición y rotación del objeto "playerSpawner"
        currentPlayerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void RespawnPlayer()
    {
        if (respawnOnDeath)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Destruye la instancia anterior del jugador si existe
        if (currentPlayerInstance != null)
        {
            Destroy(currentPlayerInstance);
        }

        // Genera una nueva instancia del jugador
        SpawnPlayer();
    }
}
