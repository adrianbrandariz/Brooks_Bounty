using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Namespace para trabajar c�modamente con elementos de UI
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    #region Variables

    [Header("ShopMenu")]
    // Referencia al campo de texto para mostrar los doblones disponibles
    public Text actualDoubloons;
    public string[] powerUpNames;
    public Text[] powerUpValues;
    public Button[] powerUpButtons;
    #endregion

    #region Unity Events

    // Start is called before the first frame update
    void Start()
    {
        actualDoubloons.text = DataManager.instance.Load("Score").ToString();
        CheckPowerUpPurchased();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods
    /// <summary>
    /// M�todo que compra un PowerUp
    /// </summary>
    /// <param name="powerUp"></param>
    public void PurchasePowerUp(string powerUp)
    {
        // Se recupera el valor del PowerUp
        int powerUpPurchasedValue = FindPowerUpValue(powerUp);
        // Si se tienen los doblones necesarios para la compra
        if (int.Parse(actualDoubloons.text) >= powerUpPurchasedValue)
        {
            // Se retiran los doblones necesarios
            DataManager.instance.SaveScore(int.Parse(actualDoubloons.text) - powerUpPurchasedValue);
            // Se almacena la compra
            Debug.Log($"Se ha comprado el PowerUp {powerUp}");
            // Se asignar� con valor 1 (COMPRADO)
            DataManager.instance.SavePowerUp(powerUp, 1);
            // Finalmente se actualizan los doblones restantes
            actualDoubloons.text = DataManager.instance.Load("Score").ToString();
            CheckPowerUpPurchased();
        }

    }

    /// <summary>
    /// Marca un PowerUp como comprado
    /// </summary>
    public void CheckPowerUpPurchased()
    {
        // Se recorre la colecci�n que almacena los nombres de los PowerUp
        foreach (string powerUp in powerUpNames)
        {
            // Se guarda el valor de si el powerUp est� comprado (1 = COMPRADO)
            bool purchased = DataManager.instance.Load(powerUp) == 1 ? true : false;
            // Si se encuentra el bot�n se activa o desactiva en funci�n al valor anterior
            if (FindPowerUpButton(powerUp) != null)
            {
                FindPowerUpButton(powerUp).interactable = !purchased;
                if (purchased) FindPowerUpText(powerUp).text = "X";
            }

        }
    }

    /// <summary>
    /// Busca el boton del powerUp por su nombre
    /// </summary>
    /// <param name="powerUpName"></param>
    /// <returns></returns>
    public Button FindPowerUpButton(string powerUpName)
    {
        Button powerUpButton = null;
        // Se busca en la colecci�n de botones
        foreach (Button button in powerUpButtons)
        {
            // Si el nombre proporcionado est� contenido en alguno de los nombres se asigna
            if (button.name.Contains(powerUpName)) powerUpButton = button;
        }
        return powerUpButton;
    }

    /// <summary>
    /// Busca el label del powerUp por su nombre
    /// </summary>
    /// <param name="powerUpName"></param>
    /// <returns></returns>
    public Text FindPowerUpText(string powerUpName)
    {
        Text powerUpText = null;
        // Se busca en la colecci�n de botones
        foreach (Text text in powerUpValues)
        {
            // Si el nombre proporcionado est� contenido en alguno de los nombres se asigna
            if (text.name.Contains(powerUpName)) powerUpText = text;
        }
        return powerUpText;
    }

    /// <summary>
    /// B�sca el precio del powerup por su nombre
    /// </summary>
    /// <param name="powerUpName"></param>
    /// <returns></returns>
    public int FindPowerUpValue(string powerUpName)
    {
        int value = 0;
        foreach (Text textValue in powerUpValues)
        {
            if (textValue.name.Contains(powerUpName)) value = int.Parse(textValue.text);
        }
        return value;
    }
    #endregion



}
