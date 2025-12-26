using UnityEngine;

public class EyeLineTrigger : MonoBehaviour
{
    public DroneAI drone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            drone.SetPlayerInSight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            drone.SetPlayerInSight(false);
        }
    }
}
