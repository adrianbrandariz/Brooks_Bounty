using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    #region Variables
    [Range(0, 0.5f)]
    // Velocidad del fondo basada en la velocidad de la c�mara
    public float speedFactor = 0.066f;
    // Posici�n para control del offset de la textura
    private Vector2 pos = Vector2.zero;
    // Referencia a la c�mara principal
    private Camera cam;
    // Posici�n anterior de la c�mara
    private Vector2 camOldPos;
    // Referencia al Renderer del background
    private Renderer rend;
    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        // Se recupera la referencia a la c�mara principal
        cam = Camera.main;
        // Se inicializa la posici�n anterior para que sea la posici�n actual
        camOldPos = cam.transform.position;
        // Se recupera la referencia al componente Renderer
        rend = GetComponent<Renderer>();
        /*
         * El ortographicSize LA MITAD del alto de la c�mara en unidades
         * screen.width obtendr�amos el ancho de la pantalla en p�xeles
         * screen.height el alto en p�xeles
         * alturaUnidades -- anchuraUnidades
         *         height -- width
         * anchuraUnidades = (alturaUnidades * width)/height
         * Se averigua la mitad del tama�o de la pantalla en unidades.
         */
        Vector2 backgroundHalfSize = new Vector2((cam.orthographicSize * Screen.width) / Screen.height,
                                                cam.orthographicSize);
        // Se ajusta la escala del fondo para que se ajuste al tama�o de la pantalla
        transform.localScale = new Vector3(backgroundHalfSize.x * 2,
                                        backgroundHalfSize.y * 2,
                                        transform.localScale.z);
        // Se ajusta el tilling para que la textura se proporcione de forma correcta a la escala
        //rend.material.SetTextureScale("_MainTex", backgroundHalfSize);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyParallax();
    }
    #endregion

    #region Methods
    private void ApplyParallax()
    {
        // Se calcula el desplazamiento de la c�mara respecto al frame anterior
        Vector2 camVar = (Vector2)cam.transform.position - camOldPos;
        /*
         * Se modifica el offset que ser� aplicado a la textura del background.
         * Se atenua la velocidad mediante el factor
         */
        pos.Set(pos.x + camVar.x * speedFactor, 0);
        // Se aplica el offset a la textura principal
        rend.material.SetTextureOffset("_MainTex", pos);
        /*
         * Se almacena la posici�n de la c�mara actual que pasar� a ser la posici�n 
         * antigua para el siguiente frame.
         */
        camOldPos = cam.transform.position;
    }
    #endregion

}
