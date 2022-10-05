using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Namespace para realizar cambios de escena
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    #region Variables

    #endregion

    #region Unity Events

    #endregion

    #region Methods

    /// <summary>
    /// Cambia a la escana cuyo nombre recibe como parámetro
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Sale de la aplicación.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}
