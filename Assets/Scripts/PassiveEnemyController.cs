using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemyController : MonoBehaviour
{
    #region Variables

    // Variable que almacena la referencia al animator
    private Animator animator;
    // Variable que almacena la referencia al BoxCollider2D
    private BoxCollider2D collider;

    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        // Se recupera la referencia al animator
        animator = GetComponentInChildren<Animator>();
        // Se recupera la referencia al BoxCollider2D
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag.ToString())
        {
            case "Sword":
                Dead();
                break;
            case "Bullet":
                Dead();
                break;

        }
    }
    #endregion

    #region Methods

    /// <summary>
    /// Método que mata al enemigo
    /// </summary>
    public void Dead()
    {
        // Se indica al animator que el jugador ha muerto
        animator.SetBool("Dead", true);
        collider.enabled = false;
        if (DataManager.instance.Load("Assault") == 1) GameManager.instance.PickUpCollectable(Random.Range(0, 2));
    }
    #endregion
}
