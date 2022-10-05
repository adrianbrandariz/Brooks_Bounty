using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableTypes { Basic, PowerUp }

public class Collectable : MonoBehaviour
{
    #region Variables
    public CollectableTypes type;

    [Header("Value")]
    // Se asigna el valor del coleccionable
    public int collectableValue = 1;

    [Header("Feedback")]
    public Color flashColor = Color.white;
    // Duración del flash de color
    public float flashTime = 0.4f;
    // Clip de audio que será reproducido al recoger el coleccionable
    public AudioClip audioClip;

    private AudioSource audioSource;

    public Collider2D collider;
    public SpriteRenderer spriteRenderer;
    #endregion

    #region Unity Events
    private void Start()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        // No se reproducirá el sonido al inicio
        audioSource.playOnAwake = false;
        // No se reproducirá en bucle
        audioSource.loop = false;
        // Se asigna el clip de audio que será reproducido
        audioSource.clip = audioClip;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Se aplica el feedback teniendo la referencia del player colisionado
            Feedback(collision.gameObject);
            // Al ser recogido, se desactiva su renderer y su collider
            DeactivateCollectable();

            switch (type)
            {
                case CollectableTypes.Basic:
                    // Se indica al GameManager que se incremente el valor del contador
                    GameManager.instance.PickUpCollectable(collectableValue);
                    break;
            }

            // Se destroye el objeto pasado el tiempo de duración del sonido
            Destroy(gameObject, audioClip.length);
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Devuelve feedback al jugador
    /// </summary>
    /// <param name="other"></param>
    private void Feedback(GameObject other)
    {
        
        // Se reproduce el clip de audio configurado
        audioSource.Play();
    }

    /// <summary>
    /// Desactiva el collider y la visibilidad del coleccionable
    /// </summary>
    private void DeactivateCollectable()
    {
        // Desactiva el componente collider
        collider.enabled = false;
        // Desactiva el componente SpriteRenderer
        spriteRenderer.enabled = false;
    }
    #endregion
}
