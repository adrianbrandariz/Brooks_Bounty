using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    #region Variables
    public int score;

    string[] allPowerUpNames = { "Sniper", "Assault", "Greed", "Broadside", "Scimitar" };
    string[] allStages = { "Beach", "Jungle", "GloomyJungle", "City" };

    public static DataManager instance;
    #endregion

    #region Unity Events
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        score = Load("Score");
        Debug.Log($"La puntuación guardada es {score}");
    }
    #endregion

    #region Methods
    /// <summary>
    /// Método que realizará el guardado de datos
    /// </summary>
    public void SaveScore()
    {
        /*
         * Si la cantidad a guardar es inferior a 999999 se almacena.
         * En caso contrario se almacena la cantidad máxima permitida.
         */
        score = (score <= 999999) ? score : 999999;
        PlayerPrefs.SetInt("Score", score);
    }

    /// <summary>
    /// Método que realizará el guardado de datos
    /// </summary>
    public void SaveScore(int scoreCheat)
    {
        PlayerPrefs.SetInt("Score", scoreCheat);
        Debug.Log($"La puntuación seteada es {scoreCheat}");
    }

    /// <summary>
    /// Guarda el estado de un powerup
    /// </summary>
    /// <param name="powerUp"></param>
    /// <param name="value"></param>
    public void SavePowerUp(string powerUp, int value)
    {
        // Se marca con el valor 1 para indicar que se ha comprado
        PlayerPrefs.SetInt(powerUp, value);
    }

    /// <summary>
    /// Guarda la progresión de nivel
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="value"></param>
    public void SaveStageProgression(string stage, int value)
    {
        // Se marca con el valor 1 para indicar que se ha comprado
        PlayerPrefs.SetInt(stage, value);
    }

    /// <summary>
    /// Método que realizará la recuperación de datos
    /// </summary>
    public int Load(string param)
    {
        int result = 0;
        if (PlayerPrefs.HasKey(param))
        {
            result = PlayerPrefs.GetInt(param);
        }
        return result;
    }

    /// <summary>
    /// Reinicia todas las características almacenadas
    /// </summary>
    public void ResetDatabase()
    {
        ResetPowerUps();
        ResetDoubloons();
        ResetProgression();
    }

    /// <summary>
    /// Reinicia los doblones
    /// </summary>
    public void ResetDoubloons()
    {
        SaveScore(0);
    }

    /// <summary>
    /// Reinicia la progresión del jugador
    /// </summary>
    public void ResetProgression()
    {
        foreach (string stage in allStages) PlayerPrefs.SetInt(stage, 0);
        Debug.Log("Se ha reiniciado el progreso.");
    }

    /// <summary>
    /// Reinicia los powerups
    /// </summary>
    public void ResetPowerUps()
    {

        foreach (string powerUpName in allPowerUpNames)
        {
            SavePowerUp(powerUpName, 0);
        }

        Debug.Log("Todos los PowerUp han sido reiniciados.");
    }

    /// <summary>
    /// Le asigna el progreso completo, todos los powerups y 9999 monedas
    /// </summary>
    public void FullProgress()
    {
        foreach (string stage in allStages) PlayerPrefs.SetInt(stage, 1);
        foreach (string powerUpName in allPowerUpNames) SavePowerUp(powerUpName, 1);

        SaveScore(9999);
    }
    #endregion



}
