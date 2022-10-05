using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    #region Variables
    [Range(2, 100)]
    // Columnas que ocupar� la secci�n
    public int columns;

    [Range(2, 100)]
    // Filas que ocupar� la secci�n
    public int rows;

    [HideInInspector]
    // Referencia al grid de la secci�n
    public Grid grid;

    // Informaci�n de la posici�n de la c�mara
    private Camera camera;

    // Propiedad que devuelve la mitad de la anchura de la secci�n
    public float halfWidth { get { return ((columns / 2) * grid.cellSize.x); } }
    public float width { get { return (columns * grid.cellSize.x); } }
    #endregion

    #region UnityEvents
    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        /*
         * Calcular el lado izquierdo de la pantalla en el mundo.
         * El ortographicSize la mitad del alto de la c�mara en unidades
         * screen.width obtendr�amos el ancho de la pantalla en p�xeles
         * screen.height el alto en p�xeles
         * alturaUnidades -- anchuraUnidades
         *         height -- width
         * anchuraUnidades = (alturaUnidades * width)/height
         */
        float leftSideOfScreen = camera.transform.position.x - (camera.orthographicSize * Screen.width) / Screen.height;
        if (transform.position.x < (leftSideOfScreen - halfWidth))
        {
            DestroySection();
        }
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
        {
            grid = GetComponentInChildren<Grid>();
        }
        if (grid != null)
        {
            // Si las columnas y las filas son pares, se pintar� el gizmo en verde
            if (columns % 2 == 0 && rows % 2 == 0)
            {
                Gizmos.color = Color.green;
            }
            // En caso contrario se pintar� en rojo
            else
            {
                Gizmos.color = Color.red;
            }
            // Se muestra el gizmo con el tama�o seleccionado
            Gizmos.DrawWireCube(transform.position, new Vector3(columns * grid.cellSize.x, rows * grid.cellSize.y, 0f));
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Destruye la secci�n y genera una nueva con el sistema de spawn
    /// </summary>
    private void DestroySection()
    {
        // Utilizando el SINGLETON de SectionManager, se llama al m�todo de generaci�n de una nueva secci�n
        SectionManager.instance.SpawnSection();
        // Se destruir� a si mismo
        Destroy(gameObject);
    }
    #endregion



}
