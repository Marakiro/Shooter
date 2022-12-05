using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject Menu;
    [SerializeField]
    GameObject Info;
    public void goToMenu()
    {
        Menu.SetActive(true);
        Info.SetActive(false);
    }
    public void goToINFO()
    {
        Menu.SetActive(false);
        Info.SetActive(true);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void SelectLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
