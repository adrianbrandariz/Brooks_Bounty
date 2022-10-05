using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Namespace para trabajar cómodamente con elementos de UI
using UnityEngine.UI;
// Namespace para gestionar escenas
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("HUD")]
    // Referencia al campo de texto que mostrará la información por pantalla
    public Text collectableText;
    // Contador del coleccionable recogido
    private int collectableCount = 0;
    // Referencia al CanvasGroup de HUD
    public CanvasGroup hudCanvas;

    [Header("PauseMenu")]
    // Referencia al CanvasGroup del menú de pausa
    public CanvasGroup pauseMenu;

    [Header("EndGameMenu")]
    // Referencia al CanvasGroup del menú de fin de partida
    public CanvasGroup endMenuCanvas;
    // Referencia al campo de texto para mostrar la puntuación final
    public Text finalScoreText;
    // Referencia al campo de texto para mostrar la puntuación final
    public Text totalScoreText;

    [Header("CheatMenu")]
    public CanvasGroup cheatsMenu;

    [Header("Actual Stage")]
    public string actualStage;

    // Patrón de diseño SINGLETON
    public static GameManager instance;
    #endregion

    #region Unity Events
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Se inicializa el texto contador que verá el usuario
        collectableText.text = collectableCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods
    /// <summary>
    /// Incrementa el número de coleccionable recogido
    /// </summary>
    /// <param name="value"></param>
    public void PickUpCollectable(int value)
    {
        // Incrementa el número de coleccionable recogido
        collectableCount += value;
        // Se actualiza la información de coleccionable recogido que verá el usuario
        collectableText.text = collectableCount.ToString();
    }

    /// <summary>
    /// Método que realizará las acciones de fin de partida
    /// </summary>
    public void EndGame()
    {
        // Si se han cogido 100 monedas o más se puede avanzar en niveles
        if (collectableCount >= 50) NextLevelAvailable();
        // Se comprueba si existe el PowerUp de codicia
        int multiplier = DataManager.instance.Load("Greed") == 1 ? 3 : 1;
        // En caso de existir se aumenta la cantidad de doblones recibida
        finalScoreText.text = (collectableCount * multiplier).ToString();
        totalScoreText.text = ((collectableCount * multiplier) + DataManager.instance.Load("Score")).ToString();
        // Se desactiva el HUD
        Utils.ActiveCanvasGroup(hudCanvas, false);
        // Se activa el menú de fin de partida
        Utils.ActiveCanvasGroup(endMenuCanvas, true);
        // Se guarda la puntuación
        DataManager.instance.score = collectableCount + DataManager.instance.Load("Score");
        DataManager.instance.SaveScore();
    }

    /// <summary>
    /// Método que reinicia la escena
    /// </summary>
    public void Restart()
    {
        // Se recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Pausa o reanuda la partida
    /// </summary>
    /// <param name="paused"></param>
    public void Pause(bool paused)
    {
        // Si se pide pausar, se detendrá el contador de tiempo
        Time.timeScale = paused ? 0 : 1;
        // Se activa o se desactiva el menú según se solicita
        Utils.ActiveCanvasGroup(pauseMenu, paused);
    }

    /// <summary>
    /// Cambia el timescale
    /// </summary>
    /// <param name="scale"></param>
    public void ChangeTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    /// <summary>
    /// Alterna entre activar y desactivar el menú de trucos
    /// </summary>
    public void CheatMenuToggle()
    {
        // Cambio el estado de activación del CanvasGroup en relación al valor de su propiedad interactable
        Utils.ActiveCanvasGroup(cheatsMenu, !cheatsMenu.interactable);
    }


    public void NextLevelAvailable()
    {
        DataManager.instance.SaveStageProgression(actualStage, 1);
        Debug.Log($"Se han recogido {collectableCount} doblones. {actualStage} se guarda como progresado.");
    }
    #endregion
}
