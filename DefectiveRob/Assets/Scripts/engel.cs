using UnityEngine;

public class engel : MonoBehaviour
{
    public GameObject engel1;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            engel1.SetActive(true);
        }
    }
}
