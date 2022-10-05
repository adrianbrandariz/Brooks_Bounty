using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{

    #region Variables
    /*
     * Transform del objetivo a seguir, es un valor por referencia por lo que
     * siempre se tendrá acceso a la información actualizada
     */
    public Transform target;
    // Para indicar si la cámara seguirá al target horizontalmente
    public bool followX = true;
    [Range(-3, 3)]
    // Desviación de la posición en Y para dar margen de visión
    public float offsetX = 1;
    // Indica si la cámara seguirá al target verticalmente
    public bool followY = false;
    [Range(-3, 3)]
    // Desviación de la posición en Y para dar margen de visión
    public float offsetY = 0;

    private Vector3 newPos;
    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ControlCamera();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Ajusta el seguimiento del personaje con la cámara
    /// </summary>
    public void ControlCamera()
    {
        // Se recupera la posicióna actual de la cámara
        newPos = transform.position;

        // Si está marcado que siga al objetivo en el eje X
        if (followX)
        {
            // Se recupera la posición en X del objetivo y se le suma el offset deseado
            newPos.x = target.position.x + offsetX;
        }
        // Si está marcado que siga al objetivo en el eje Y
        if (followY)
        {
            // Se recupera la posición en Y del objetivo y se le suma el offset deseado
            newPos.y = target.position.y + offsetY;
        }
        // Se modifica el transform de la cámara para que se posicione en newPos
        transform.position = newPos;
    }
    #endregion
}
