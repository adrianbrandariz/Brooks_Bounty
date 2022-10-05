using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawnPoint : MonoBehaviour
{

    #region Variables

    public GameObject prefab;
    // Ratio de aparición del prefab
    [Tooltip("1 aparece siempre, 0 no aparece")]
    [Range(0, 1)]
    public float spawnRatio = 1f;
    #endregion

    #region Unity Events


    // Start is called before the first frame update
    private void Start()
    {
        /*
         * Si no hay un prefab definido, no se hace nada.
         * Adicionalmente se podría poner un mensaje de warning.
         */
        if (prefab == null)
        {
            Debug.LogWarning("Falta definir prefab en objeto " + name);
            return;
        }

        // Si el ratio es inferior o igual a la tirada aleatoria se instancia el prefab     
        if (Random.Range(0f, 1f) <= spawnRatio)
        {
            /*
             * Se instancia el prefab como hijo del propio spawn para asegurarse que se
             * destruirá junto con la sección
             */
            Instantiate(prefab, transform.position, Quaternion.identity, transform.parent);
        }
    }
    #endregion

    #region Methods

    #endregion
}
