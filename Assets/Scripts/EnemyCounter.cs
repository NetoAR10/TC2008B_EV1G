using UnityEngine;
using UnityEngine.UI;

public class EnemyCounterUI : MonoBehaviour
{
    public Text enemyCountText; // Referencia al objeto de texto UI donde se mostrará el contador

    void Update()
    {
        // Contar el número de objetos con la etiqueta "Enemy"
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Actualizar el texto en la UI
        enemyCountText.text = "Enemies: " + enemyCount;
    }
}
