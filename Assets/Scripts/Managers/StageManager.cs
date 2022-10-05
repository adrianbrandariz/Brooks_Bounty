using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Namespace para trabajar cómodamente con elementos de UI
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    #region Variables
    [Header("Stage Settings")]
    public Sprite unknownStageImageButton;
    public Sprite unknownStageImage;

    public Button[] stageButton;
    public Image[] stageImage;

    public string[] allStages = { "City", "GloomyJungle", "Jungle", "Beach" };
    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        // TODO: CHECKAVAILABLESTAGE. ERROR CON INSTANCE DATAMANAGER
        //CheckAvailableStage();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods
    /// <summary>
    /// Comprueba si el jugador ha pasado alguna vez el mapa
    /// </summary>
    public void CheckAvailableStage()
    {
        foreach (string stage in allStages)
        {
            // Si es diferente a 1 es que no se ha pasado el nivel
            if (DataManager.instance.Load(stage) != 1)
            {
                switch (stage)
                {
                    case "GloomyJungle":
                        // Se desactiva el siguiente nivel, la ciudad
                        DisableStage("City");
                        break;
                    case "Jungle":
                        // Se desactiva el siguiente nivel, la jungla tenebrosa
                        DisableStage("GloomyJungle");
                        break;
                    case "Beach":
                        // Se desactiva el siguiente nivel, la jungla
                        DisableStage("Jungle");
                        break;
                    default:
                        break;
                }
            }
        }

    }

    /// <summary>
    /// Desactiva los componentes relacionados con el nivel
    /// </summary>
    /// <param name="name"></param>
    public void DisableStage(string name)
    {
        FindButtonByName(name).image.sprite = unknownStageImageButton;
        FindImageByName(name).sprite = unknownStageImage;
        FindButtonByName(name).interactable = false;
    }

    /// <summary>
    /// Método que busca un botón por nombre
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Button FindButtonByName(string name)
    {
        Button button = null;
        foreach (Button loopButton in stageButton)
        {
            if (loopButton.name.Contains(name))
            {
                button = loopButton;
            }
        }
        return button;
    }

    /// <summary>
    /// Método que busca una imagen por nombre
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Image FindImageByName(string name)
    {
        Image image = null;
        foreach (Image loopImage in stageImage)
        {
            if (loopImage.name.Contains(name))
            {
                image = loopImage;
            }
        }
        return image;
    }
    #endregion
}
