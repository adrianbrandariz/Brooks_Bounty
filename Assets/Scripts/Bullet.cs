using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables

    [Header("Bullet Settings")]
    public float maxSpeed = 1f;
    public float acceleration = 1f;
    public float bulletMaxRange = 5f;

    private float initialPosition;
    private float actualPosition;
    private int powerUpDistance;

    // Variable que almacena la referencia al Rigidbody2D
    private Rigidbody2D rigidbody;

    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        // Se recupera la referencia al Rigidbody2D
        rigidbody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position.x;
        actualPosition = transform.position.x;
        powerUpDistance = DataManager.instance.Load("Sniper") == 1 ? 2 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        ShootBullet();
        CheckBulletOutOfRange();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag.ToString())
        {
            case "Enemy":
                DestroyBullet();
                break;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Destruye la bala
    /// </summary>
    public void DestroyBullet()
    {
        // Se destruirá a si mismo
        Destroy(gameObject);
    }

    /// <summary>
    /// Dispara la bala
    /// </summary>
    public void ShootBullet()
    {
        // Se almacena la velocidad actual del Rigidbody en una variable
        Vector2 tempVelocity = rigidbody.velocity;
        /*
         * Se modifica el valor x (horizontal) para aproximarlo al valor de la velocidad deseada
         * mediante una aceleración progresiva. Habrá que multiplicarlo por delta time, para que
         * se aplique la aceleración correspondiente a la fracción de tiempo transcurrido desde
         * el último frame. De esta forma la aceleración será igual independientemente de la tasa
         * de frames por segundo.
         */
        tempVelocity.x = Mathf.MoveTowards(tempVelocity.x, maxSpeed, acceleration * Time.deltaTime);
        // Se asigna la velocidad calculada
        rigidbody.velocity = tempVelocity;
    }

    /// <summary>
    /// Comprueba la distancia que recorre la bala
    /// </summary>
    public void CheckBulletOutOfRange()
    {
        actualPosition = transform.position.x;
        if (actualPosition > initialPosition + (bulletMaxRange * powerUpDistance))
        {
            DestroyBullet();
        }
    }
    #endregion
}
