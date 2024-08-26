using UnityEngine;
using UnityEngine.UI;

public class BulletCounterUI : MonoBehaviour
{
    public Text bulletCountText; // Referencia al objeto de texto UI donde se mostrará el contador

    void Update()
    {
        // Contar el número de objetos con la etiqueta "Bullet"
        int bulletCount = GameObject.FindGameObjectsWithTag("Bullet").Length;

        // Actualizar el texto en la UI
        bulletCountText.text = "Bullets: " + bulletCount;
    }
}
