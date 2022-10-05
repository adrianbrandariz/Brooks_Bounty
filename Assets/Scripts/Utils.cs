using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    #region Variables

    #endregion

    #region Unity Events

    #endregion

    #region Methods

    /// <summary>
    /// Activa un CanvasGroup y lo hace visible.
    /// </summary>
    /// <param name="group"></param>
    /// <param name="enable"></param>
    public static void ActiveCanvasGroup(CanvasGroup group, bool enable)
    {
        // En función de si está activo o inactivo, se pone visible o invisible
        group.alpha = enable ? 1 : 0;
        // Activa o desactiva la interacción del CanvasGroup
        group.interactable = enable;
        // Activa o desactiva el bloqueo de raycast del CanvasGroup
        group.blocksRaycasts = enable;
    }
    #endregion
}
