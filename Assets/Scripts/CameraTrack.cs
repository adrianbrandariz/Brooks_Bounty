using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{

    #region Variables
    /*
     * Transform del objetivo a seguir, es un valor por referencia por lo que
     * siempre se tendr� acceso a la informaci�n actualizada
     */
    public Transform target;
    // Para indicar si la c�mara seguir� al target horizontalmente
    public bool followX = true;
    [Range(-3, 3)]
    // Desviaci�n de la posici�n en Y para dar margen de visi�n
    public float offsetX = 1;
    // Indica si la c�mara seguir� al target verticalmente
    public bool followY = false;
    [Range(-3, 3)]
    // Desviaci�n de la posici�n en Y para dar margen de visi�n
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
    /// Ajusta el seguimiento del personaje con la c�mara
    /// </summary>
    public void ControlCamera()
    {
        // Se recupera la posici�na actual de la c�mara
        newPos = transform.position;

        // Si est� marcado que siga al objetivo en el eje X
        if (followX)
        {
            // Se recupera la posici�n en X del objetivo y se le suma el offset deseado
            newPos.x = target.position.x + offsetX;
        }
        // Si est� marcado que siga al objetivo en el eje Y
        if (followY)
        {
            // Se recupera la posici�n en Y del objetivo y se le suma el offset deseado
            newPos.y = target.position.y + offsetY;
        }
        // Se modifica el transform de la c�mara para que se posicione en newPos
        transform.position = newPos;
    }
    #endregion
}
