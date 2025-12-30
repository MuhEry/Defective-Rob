using UnityEngine;
using UnityEngine.SceneManagement;

public class easteregg : MonoBehaviour
{
public string sahneadi;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(sahneadi);
        }
    }
}
