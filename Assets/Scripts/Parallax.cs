using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    #region Variables
    [Range(0, 0.5f)]
    // Velocidad del fondo basada en la velocidad de la cámara
    public float speedFactor = 0.066f;
    // Posición para control del offset de la textura
    private Vector2 pos = Vector2.zero;
    // Referencia a la cámara principal
    private Camera cam;
    // Posición anterior de la cámara
    private Vector2 camOldPos;
    // Referencia al Renderer del background
    private Renderer rend;
    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        // Se recupera la referencia a la cámara principal
        cam = Camera.main;
        // Se inicializa la posición anterior para que sea la posición actual
        camOldPos = cam.transform.position;
        // Se recupera la referencia al componente Renderer
        rend = GetComponent<Renderer>();
        /*
         * El ortographicSize LA MITAD del alto de la cámara en unidades
         * screen.width obtendríamos el ancho de la pantalla en píxeles
         * screen.height el alto en píxeles
         * alturaUnidades -- anchuraUnidades
         *         height -- width
         * anchuraUnidades = (alturaUnidades * width)/height
         * Se averigua la mitad del tamaño de la pantalla en unidades.
         */
        Vector2 backgroundHalfSize = new Vector2((cam.orthographicSize * Screen.width) / Screen.height,
                                                cam.orthographicSize);
        // Se ajusta la escala del fondo para que se ajuste al tamaño de la pantalla
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
        // Se calcula el desplazamiento de la cámara respecto al frame anterior
        Vector2 camVar = (Vector2)cam.transform.position - camOldPos;
        /*
         * Se modifica el offset que será aplicado a la textura del background.
         * Se atenua la velocidad mediante el factor
         */
        pos.Set(pos.x + camVar.x * speedFactor, 0);
        // Se aplica el offset a la textura principal
        rend.material.SetTextureOffset("_MainTex", pos);
        /*
         * Se almacena la posición de la cámara actual que pasará a ser la posición 
         * antigua para el siguiente frame.
         */
        camOldPos = cam.transform.position;
    }
    #endregion

}
