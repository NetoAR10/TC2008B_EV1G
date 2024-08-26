using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public int enemyCount = 5; // Número de enemigos a generar
    public float spawnRadius = 100f; // Radio alrededor del spawner donde se generan los enemigos
    public float despawnTime = 30f; // Tiempo después del cual los enemigos se destruirán

    private GameObject[] spawnedEnemies; // Array para guardar las referencias de los enemigos generados

    void Start()
    {
        spawnedEnemies = new GameObject[enemyCount];
        SpawnEnemies();

        // Iniciar el proceso de destrucción de enemigos después del tiempo especificado
        Invoke("DespawnEnemies", despawnTime);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Generar una posición aleatoria alrededor del spawner
            Vector3 spawnPosition = transform.position + (Random.insideUnitSphere * spawnRadius);
            spawnPosition.y = 100; // Mantener la altura en cero (suelo)

            // Instanciar el enemigo
            spawnedEnemies[i] = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void DespawnEnemies()
    {
        for (int i = 0; i < spawnedEnemies.Length; i++)
        {
            if (spawnedEnemies[i] != null)
            {
                Destroy(spawnedEnemies[i]);
            }
        }
    }
}
