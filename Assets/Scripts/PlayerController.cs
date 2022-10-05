using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Jump Settings")]
    public float jumpForce = 0.8f;

    [Header("Ground Check")]
    // Referencia al transform de groundCheck
    public Transform groundCheck;
    // Layers en los que el player se considera estar grounded
    public LayerMask groundLayer;
    // Se define el área de verificación del contacto con el suelo
    public Vector2 sizeGroundCheck = new Vector2(0.16f, 0.03f);
    // Boolean para indicar si el personaje está tocando el suelo
    public bool grounded = false;
    // Límite mortal de altura
    public float deadLimit = -5.8f;

    [Header("Movement Settings")]
    public bool automove = true;
    public float maxSpeed = 1f;
    public float acceleration = 20f;
    // El tiempo que tardará el personaje en comenzar a moverse
    public float delayMovement = 4.5f;

    // Variable que almacena la referencia al Rigidbody2D
    private Rigidbody2D rigidbody;
    // Variable que almacena la referencia al animator
    private Animator animator;

    // Variable que almacena el tiempo de ejecución de la partida
    private float timer;
    private bool dead = false;

    // Variable que almacena un temporizador desde que el personaje ataca
    private float attackTimer;
    // Variable que almacena un temporizador desde que el personaje dispara
    private float shootTimer;
    // Variable que almacena un temporizador desde que el personaje se desliza
    private float slideTimer;

    [Header("Combat Settings")]
    // Sección de cadencia de tiro
    public float fireRate = 2f;
    // Variable que almacena un temporizador desde que controla la cadencia de tiro
    private float fireRateTimer;
    // Variable que almacena si el personaje posee el powerup de mejora de cadencia de tiro
    private int fireRatePowerUp;

    private BoxCollider2D swordCollider;

    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        // Se recupera la referencia al Rigidbody2D
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        // Se inicializa el timer
        timer = Time.deltaTime;
        fireRateTimer = Time.deltaTime;
        fireRatePowerUp = DataManager.instance.Load("Broadside") == 1 ? 2 : 1;
        fireRate = fireRate / fireRatePowerUp;

        swordCollider = transform.Find("Sword").GetComponent<BoxCollider2D>();
        if (DataManager.instance.Load("Scimitar") == 1)
        {
            swordCollider.size = new Vector2(swordCollider.size.x * 2, swordCollider.size.y);
            swordCollider.offset = new Vector2(swordCollider.offset.x + 1.5f, swordCollider.offset.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // En cada frame se verifica si se está tocando el suelo
        EvaluateGrounded();
        // Se actualiza el temporizador del juego
        UpdateTimer();
        // Se aplica el movimiento
        Movement();
        // Se alimenta el valor de los parámetros para el animator
        FeedAnimations();
        // Se verifica que el jugador se mantenga por encima del límite de supervivencia
        CheckDeadLimit();
    }

    // Creo un gizmo en este evento para visualizar lo que abarca el OverlapBox
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, sizeGroundCheck);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            // La muerte del jugador
            Dead();
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Método para llamar a la función del salto
    /// </summary>
    /// <param name="context"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        // Cuando se identifica que se ha pulsado el control de saltar
        if (context.started) Jump();

    }

    /// <summary>
    /// Aplica la fuerza para el salto.
    /// </summary>
    public void Jump()
    {
        // El personaje no saltará hasta que no pueda caminar
        if (timer > delayMovement)
        {
            // Si está en el suelo
            if (grounded)
            {
                // Se aplica la fuerza de salto vertical hacia arriba y de tipo impulso
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    /// <summary>
    /// Método para llamar a la función de deslizarse
    /// </summary>
    /// <param name="context"></param>
    public void OnSlide(InputAction.CallbackContext context)
    {
        // Cuando se identifica que se ha pulsado el control de deslizarse
        if (context.started) Slide();

    }

    /// <summary>
    /// El personaje se desliza.
    /// </summary>
    public void Slide()
    {
        // El personaje no deslizará hasta que no pueda caminar
        if (timer > delayMovement)
        {
            // Si está en el suelo
            if (grounded)
            {
                // TODO: Lógica para deslizarse, habrá que modificar el collider
                // Se informa al animator que el personaje se desliza
                animator.SetBool("Slide", true);
                // Se inicia el contador de deslizamiento
                slideTimer = Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Verifica si se está tocando el suelo.
    /// </summary>
    private void EvaluateGrounded()
    {
        /*
         * Se comprueba el contacto con el suelo usando un OverlapBox.
         * Se indica su posición y tamaño, así como el layer de interacción.
         */
        grounded = Physics2D.OverlapBox(groundCheck.position, sizeGroundCheck, 0f, groundLayer);
    }

    /// <summary>
    /// Aplica el movimiento al jugador.
    /// </summary>
    private void Movement()
    {
        /*
         * Si no está activo el automove o no se tiene contacto con el suelo y, además, estamos 
         * en caída no se aplica movimiento alguno o no ha superado el tiempo mínimo.
         */
        if (!automove || (!grounded && rigidbody.velocity.y < 0) || timer <= delayMovement) return;
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
    /// Método para llamar a la función del ataque
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Cuando se identifica que se ha pulsado el control de atacar
        if (context.started) Attack();

    }

    /// <summary>
    /// EL personaje ataca
    /// </summary>
    public void Attack()
    {
        // El personaje no atacará hasta que no pueda caminar
        if (timer > delayMovement)
        {
            // Si está en el suelo y no está atacando
            if (grounded && !animator.GetBool("Attack"))
            {
                swordCollider.enabled = true;
                StartCoroutine(WaitForSec(0.5f));
                // Se informa al animator que el personaje ataca
                animator.SetBool("Attack", true);
                // Se marca el momento en el que el personaje ataca
                attackTimer = Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Método para llamar a la función del disparo
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        // Cuando se identifica que se ha pulsado el control de disparar
        if (context.started) Shoot();

    }

    /// <summary>
    /// El personaje dispara.
    /// </summary>
    public void Shoot()
    {
        // El personaje no atacará hasta que no pueda caminar
        if (timer > delayMovement)
        {
            // Si está en el suelo y no está atacando
            if (grounded && !animator.GetBool("Shoot") && fireRateTimer >= fireRate)
            {
                // TODO: LOGICA DEL DISPARO
                // Se informa al animator que el personaje ataca
                animator.SetBool("Shoot", true);
                // Se marca el momento en el que el personaje ataca
                shootTimer = Time.deltaTime;
                fireRateTimer = Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Actualiza el valor de las variables del animator que requieren actualización continua
    /// </summary>
    private void FeedAnimations()
    {
        // Se transmite el valor absoluto de la velocidad en el eje X al parámetro Velocity del animator
        animator.SetFloat("Velocity", Mathf.Abs(rigidbody.velocity.x));
        // Se informa al animator cuando el player toca tierra
        animator.SetBool("Grounded", grounded);
        // Se informa al animator que el juego ha comenzado
        animator.SetBool("Walking", timer > delayMovement);
        CheckAttackTimer();
        CheckShootTimer();
        CheckSlideTimer();
        UpdateFireRateTimer();
    }



    /// <summary>
    /// Marca al personaje como muerto e invoca al método de final de partida
    /// </summary>
    public void Dead()
    {
        dead = true;
        // Se impide que se mueva
        automove = false;
        // Se elimina la velocidad residual
        rigidbody.velocity = Vector2.zero;
        // Se indica al animator que el jugador ha muerto
        animator.SetBool("Dead", true);
        // Se informa al animator que el personaje ya no camina
        animator.SetBool("Walking", false);
        // Al morir el jugador se pide al GameManager que finalice la partida
        Invoke("InvokeEndGame", 3f);
    }

    /// <summary>
    /// Al morir el jugador se pide al GameManager que finalice la partida
    /// </summary>
    private void InvokeEndGame()
    {
        GameManager.instance.EndGame();
    }

    /// <summary>
    /// Verifica si el jugador cae por debajo del límite mortal de altura
    /// </summary>
    private void CheckDeadLimit()
    {
        if (transform.position.y <= deadLimit && !dead) Dead();
    }

    /// <summary>
    /// Método que se encargará de ir actualizando el temporizador del juego
    /// </summary>
    private void UpdateTimer()
    {
        // Se actualiza el timer
        timer += Time.deltaTime;
    }

    /// <summary>
    /// Método que comprueba el timer del ataque
    /// </summary>
    private void CheckAttackTimer()
    {
        attackTimer += Time.deltaTime;
        // Cuando el personaje alcanza tiempo especificado se avisa al animator que el personaje ya no está atacando
        if (attackTimer > 0.4f)
        {
            animator.SetBool("Attack", false);
        }
    }

    /// <summary>
    /// Método que comprueba el timer del disparo
    /// </summary>
    private void CheckShootTimer()
    {
        shootTimer += Time.deltaTime;
        // Cuando el personaje alcanza tiempo especificado se avisa al animator que el personaje ya no está atacando
        if (shootTimer > 0.4f)
        {
            animator.SetBool("Shoot", false);
        }
    }

    /// <summary>
    /// Método que comprueba el timer de deslizarse
    /// </summary>
    private void CheckSlideTimer()
    {
        slideTimer += Time.deltaTime;
        // Cuando el personaje alcanza tiempo especificado se avisa al animator que el personaje ya no está deslizándose
        if (slideTimer > 0.8f)
        {
            animator.SetBool("Slide", false);
        }
    }

    /// <summary>
    /// Método que actualiza el timer del fire rate
    /// </summary>
    private void UpdateFireRateTimer()
    {
        fireRateTimer += Time.deltaTime;
    }

    /// <summary>
    /// Método que ralentiza el uso de la espada
    /// </summary>
    private IEnumerator WaitForSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        swordCollider.enabled = false;
    }

    #endregion
}
