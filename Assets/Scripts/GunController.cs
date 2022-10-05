using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    #region Variables
    public Transform firePoint;
    public Bullet bulletPrefab;
    public Transform gun;
    private Animator animator;

    // Transform que contendr� las secciones instanciadas
    public Transform sectionContainer;

    // Variable que almacena el tiempo de ejecuci�n de la partida
    private float timer;
    // El tiempo que tardar� el personaje en comenzar a moverse
    public float delayShooting = 4.5f;

    public float fireRate = 2f;
    private float fireRateTimer;
    private int fireRatePowerUp;
    #endregion

    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        // Si no se asigna contenedor, el propio manager ser� el contenedor
        if (!sectionContainer)
        {
            sectionContainer = transform;
        }
        // Se inicializa el timer
        timer = Time.deltaTime;
        animator = GetComponentInChildren<Animator>();
        fireRateTimer = Time.deltaTime;

        fireRatePowerUp = DataManager.instance.Load("Broadside") == 1 ? 2 : 1;
        fireRate = fireRate / fireRatePowerUp;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= delayShooting) UpdateTimer();
        UpdateFireRateTimer();
    }
    #endregion

    #region Methods
    /// <summary>
    /// M�todo que llama a la funci�n del disparo
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started) Shoot();
    }

    /// <summary>
    /// Dispara una bala
    /// </summary>
    public void Shoot()
    {
        if (timer > delayShooting && fireRateTimer >= fireRate && animator.GetBool("Grounded"))
        {
            fireRateTimer = Time.deltaTime;
            // Se obtiene una nueva secci�n de forma aleatoria
            Bullet nextBullet = bulletPrefab;
            // Se instancia una nueva bala y se almacena como referencia de la secci�n actual
            Instantiate(nextBullet,
                        gun.transform.position,
                        Quaternion.identity,
                        sectionContainer);
        }
    }

    /// <summary>
    /// Actualiza el timer
    /// </summary>
    private void UpdateTimer()
    {
        timer += Time.deltaTime;
    }
    
    /// <summary>
    /// Actualiza el timer del fire rating
    /// </summary>
    private void UpdateFireRateTimer()
    {
        fireRateTimer += Time.deltaTime;
    }
    #endregion




}
