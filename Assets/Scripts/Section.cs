using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    #region Variables
    [Range(2, 100)]
    // Columnas que ocupará la sección
    public int columns;

    [Range(2, 100)]
    // Filas que ocupará la sección
    public int rows;

    [HideInInspector]
    // Referencia al grid de la sección
    public Grid grid;

    // Información de la posición de la cámara
    private Camera camera;

    // Propiedad que devuelve la mitad de la anchura de la sección
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
         * El ortographicSize la mitad del alto de la cámara en unidades
         * screen.width obtendríamos el ancho de la pantalla en píxeles
         * screen.height el alto en píxeles
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
            // Si las columnas y las filas son pares, se pintará el gizmo en verde
            if (columns % 2 == 0 && rows % 2 == 0)
            {
                Gizmos.color = Color.green;
            }
            // En caso contrario se pintará en rojo
            else
            {
                Gizmos.color = Color.red;
            }
            // Se muestra el gizmo con el tamaño seleccionado
            Gizmos.DrawWireCube(transform.position, new Vector3(columns * grid.cellSize.x, rows * grid.cellSize.y, 0f));
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Destruye la sección y genera una nueva con el sistema de spawn
    /// </summary>
    private void DestroySection()
    {
        // Utilizando el SINGLETON de SectionManager, se llama al método de generación de una nueva sección
        SectionManager.instance.SpawnSection();
        // Se destruirá a si mismo
        Destroy(gameObject);
    }
    #endregion



}
