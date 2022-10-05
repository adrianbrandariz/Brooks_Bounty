using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{

    // Listado de las secciones disponibles
    public Section[] sectionPrefabs;
    // Transform que contendrá las secciones instanciadas
    public Transform sectionContainer;
    // La última sección generada, será también la sección inicial
    public Section currentSection;
    // Secciones iniciales generadas
    public int initialPrewarm = 4;

    /*
     * PATRON SINGLETON
     * Se crea una variable pública y estática. Al hacerla estática se consigue que
     * pertenezca a la clase y no a la instancia que se cree a partir de la clase.
     */
    public static SectionManager instance;

    private void Awake()
    {
        /*
         * Se verifica si la instancia es nula, de ser así significa que no se ha
         * creado otra antes.
         */
        if (instance == null)
        {
            /*
             * Esta instancia quedará como estática pudiendo acceder a ella desde 
             * cualquier script sin necesidad de referencia.
             */
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Si no se asigna contenedor, el propio manager será el contenedor
        if (!sectionContainer)
        {
            sectionContainer = transform;
        }
        // Se crean tantas plataformas como se haya indicado inicialmente
        for (int i = 0; i < initialPrewarm; i++)
        {
            SpawnSection();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Genera una nueva sección en la posición adecuada
    /// </summary>
    public void SpawnSection()
    {
        // Se obtiene una nueva sección de forma aleatoria
        Section nextSection = sectionPrefabs[Random.Range(0, sectionPrefabs.Length)];
        // Variable para calcular la posición de la siguiente sección
        Vector3 nextPositionOffset = currentSection.transform.position;
        // Se calcula la posición de la siguiente sección
        nextPositionOffset.x = currentSection.halfWidth + nextSection.halfWidth;
        // Se instancia una nueva sección y se almacena como referencia de la sección actual
        currentSection = Instantiate(nextSection,
                                    currentSection.transform.position + nextPositionOffset,
                                    Quaternion.identity,
                                    sectionContainer);
    }
}
