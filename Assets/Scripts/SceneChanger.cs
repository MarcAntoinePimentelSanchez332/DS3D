using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // M�thode pour charger une sc�ne sp�cifique
    public void ChangeScene(string sceneName)
    {
        Debug.Log("Changement de sc�ne pour : " + sceneName);
        SceneManager.LoadScene(sceneName);
        Debug.Log("Changement de sc�ne r�ussi");
    }
    public void QuitApplication()
    {
        Debug.Log("Quitter l'application");
        Application.Quit();
        Debug.Log("Application quittée");
    }
}
