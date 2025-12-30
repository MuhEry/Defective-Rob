using UnityEngine;

public class Music : MonoBehaviour
{
    private static Music instance;

    void Awake()
    {
        Time.timeScale = 1f;
        // Eğer sahnede zaten bir müzik çalar varsa, ben fazlalığım, beni yok et.
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Eğer ilk defa oluşuyorsam, beni kaydet ve sahne değişince yok etme.
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}