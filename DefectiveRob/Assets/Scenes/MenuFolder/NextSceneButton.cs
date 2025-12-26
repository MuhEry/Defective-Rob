using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButton : MonoBehaviour
{
    public string sonrakiSahne;
    public void NextScene()
    {
        SceneManager.LoadScene(sonrakiSahne);
    }
}
