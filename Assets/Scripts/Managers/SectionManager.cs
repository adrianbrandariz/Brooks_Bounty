using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionManager : MonoBehaviour
{

    // Listado de las secciones disponibles
    public Section[] sectionPrefabs;
    // Transform que contendr� las secciones instanciadas
    public Transform sectionContainer;
    // La �ltima secci�n generada, ser� tambi�n la secci�n inicial
    public Section currentSection;
    // Secciones iniciales generadas
    public int initialPrewarm = 4;

    /*
     * PATRON SINGLETON
     * Se crea una variable p�blica y est�tica. Al hacerla est�tica se consigue que
     * pertenezca a la clase y no a la instancia que se cree a partir de la clase.
     */
    public static SectionManager instance;

    private void Awake()
    {
        /*
         * Se verifica si la instancia es nula, de ser as� significa que no se ha
         * creado otra antes.
         */
        if (instance == null)
        {
            /*
             * Esta instancia quedar� como est�tica pudiendo acceder a ella desde 
             * cualquier script sin necesidad de referencia.
             */
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Si no se asigna contenedor, el propio manager ser� el contenedor
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
    /// Genera una nueva secci�n en la posici�n adecuada
    /// </summary>
    public void SpawnSection()
    {
        // Se obtiene una nueva secci�n de forma aleatoria
        Section nextSection = sectionPrefabs[Random.Range(0, sectionPrefabs.Length)];
        // Variable para calcular la posici�n de la siguiente secci�n
        Vector3 nextPositionOffset = currentSection.transform.position;
        // Se calcula la posici�n de la siguiente secci�n
        nextPositionOffset.x = currentSection.halfWidth + nextSection.halfWidth;
        // Se instancia una nueva secci�n y se almacena como referencia de la secci�n actual
        currentSection = Instantiate(nextSection,
                                    currentSection.transform.position + nextPositionOffset,
                                    Quaternion.identity,
                                    sectionContainer);
    }
}
