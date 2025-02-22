 using UnityEngine;
 using UnityEngine.SceneManagement;
 using System.Collections;


public class PlayAgainButton : MonoBehaviour
{
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}