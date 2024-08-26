using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance; // Singleton para fácil acceso
    public TextMeshProUGUI bulletCountText; // Referencia al texto UI para mostrar el contador

    private int bulletCount = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBullet()
    {
        bulletCount++;
        UpdateBulletCountUI();
    }

    public void RemoveBullet()
    {
        bulletCount--;
        UpdateBulletCountUI();
    }

    void UpdateBulletCountUI()
    {
        bulletCountText.text = "Bullets: " + bulletCount;
    }
}
