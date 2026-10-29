using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>
/// A class to control the top-down character.
/// Implements the player controls for moving and shooting.
/// Updates the player animator so the character animates based on input.
/// </summary>
public class TopDownCharacterController : MonoBehaviour
{
    #region Framework Variables

    //The inputs that we need to retrieve from the input system.
    private InputAction m_moveAction;
    private InputAction m_attackAction;

    //The components that we need to edit to make the player move smoothly.
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;

    // The direction that the player is moving in.
    private Vector2 m_playerDirection;


    [Header("Movement parameters")]
    //The speed at which the player moves
    [SerializeField] private float m_playerSpeed = 200f;
    //The maximum speed the player can move
    [SerializeField] private float m_playerMaxSpeed = 1000f;

    #endregion

    // Players Health Script
    [SerializeField] private Health m_healthCode;

    [Header("Projectile parameters")]
    // Reference to out projectile prefab object
    [SerializeField] private GameObject m_projectilePrefab;
    //The location where we want to spawn the porjectile
    [SerializeField] private Transform m_firePoint;
    //How fast the projectile will travel.
    [SerializeField] private float m_projectileSpeed;
    //How many bullets will be spawned in a range of time
    [SerializeField] private float m_fireRate;
    // Time of the last butllet being fired
    private float m_timeOfLastFire = 0;
    //The amount of time after firing it takes for an object to be deleted
    private float m_fireTimeout = 0;
    //Bool to check if weapon should be firing
    private bool m_firing = false;
    // Scrpit to get the 2d vector of mouse x,y
    private Mouse_Position m_mousePosition;

    // How many waves the player has survived
    private int m_waves;

    private int m_projectileDamage = 5;

    [SerializeField] private ScoreSystem m_scoreSystem;

    private Vector2 m_lastDirection;    //The last direction the player was moving

    [Header("Melee parameters")]
    // The input used to attack
    private InputAction m_meleeAttackAction;
    // The box collider for that represents the melee attacks range
    [SerializeField] private GameObject m_meleeHitbox;
    // the time you must wait before using a melee attack again
    private float m_meleeCooldown = 2;       // private variable, public setter
    // Time since you last used the melee divided by the m_meleeCooldown (When the timer is at 1, that means one full cooldown has passed)
    private float m_meleeTimer = 0;
    // Checks whether the player cna use their melee or whether its still on cooldown
    private bool m_canMeleeAttack = true;
    // A gameobject that represents how long it takes for your melee to recharge
    [SerializeField] private GameObject m_meleeRechargeBox;
    // A coloured bar inside the recharge box
    [SerializeField] private Image m_meleeBarFiller;
    // is the melee bar visible
    private bool m_meleeBarVisible = false;
    private int m_meleeDamage = 10;

    /// <summary>
    /// When the script first initialises this gets called.
    /// Use this for grabbing components and setting up input bindings.
    /// </summary>
    private void Awake()
    {
        //bind movement inputs to variables
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_attackAction = InputSystem.actions.FindAction("Attack");
        m_meleeAttackAction = InputSystem.actions.FindAction("Secondary Attack");


        //get components from Character game object so that we can use them later.
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Called after Awake(), and is used to initialize variables e.g. set values on the player
    /// </summary>
    void Start()
    {
        //not currently used - left here for demonstration purposes.
    }

    /// <summary>
    /// When a fixed update loop is called, it runs at a constant rate, regardless of pc performance.
    /// This ensures that physics are calculated properly.
    /// </summary>
    private void FixedUpdate()
    {
        //clamp the speed to the maximum speed for if the speed has been changed in code.
        float speed = m_playerSpeed > m_playerMaxSpeed ? m_playerMaxSpeed : m_playerSpeed;

        //apply the movement to the character using the clamped speed value.
        m_rigidbody.linearVelocity = m_playerDirection * (speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// When the update loop is called, it runs every frame.
    /// Therefore, this will run more or less frequently depending on performance.
    /// Used to catch changes in variables or input.
    /// </summary>
    void Update()
    {
        // store any movement inputs into m_playerDirection - this will be used in FixedUpdate to move the player.
        m_playerDirection = m_moveAction.ReadValue<Vector2>();

        // ~~ handle animator ~~
        // Update the animator speed to ensure that we revert to idle if the player doesn't move.
        m_animator.SetFloat("Speed", m_playerDirection.magnitude);

        // If there is movement, set the directional values to ensure the character is facing the way they are moving.
        if (m_playerDirection.magnitude > 0)
        {
            m_animator.SetFloat("Horizontal", m_playerDirection.x);
            m_animator.SetFloat("Vertical", m_playerDirection.y);

            //Set the last facing direction for projectile shooting
            m_lastDirection = m_playerDirection;
        }
        //if (m_attackAction.IsPressed())
        //{
        //    m_firing = true;
        //}
        //else
        //{
        //    m_firing = false;
        //}
        if (m_meleeBarVisible)
        {
            if (m_meleeTimer < 1)
            {
                m_meleeTimer += ((1 / m_meleeCooldown) * Time.deltaTime);
                m_meleeBarFiller.fillAmount = m_meleeTimer;
            }
            else if (m_meleeTimer >= 1)
            {
                m_canMeleeAttack = true;
                m_meleeRechargeBox.SetActive(false);
                m_meleeBarVisible = false;
            }
        }
    }

    private void Fire()
    {
        Debug.Log("Fire");
        //Vector2 fireDirection = m_lastDirection;
        //fireDirection.x = m_mousePosition.mouse_position2D.x;
        //fireDirection.y = m_mousePosition.mouse_position2D.x;
        //if (fireDirection == Vector2.zero)
        //{
        //    fireDirection = Vector2.down;
        //}

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 dir = mousePos - transform.position;

        GameObject spawnedProjectile = Instantiate(m_projectilePrefab, m_firePoint.position, Quaternion.identity);

        Rigidbody2D projectileRB = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (projectileRB != null)
        {
            //Debug.Log("Called");
            projectileRB.AddForce(dir.normalized * m_projectileSpeed, ForceMode2D.Impulse);
        }
    }

    private void Melee()
    {
        if (m_canMeleeAttack)
        {
            m_canMeleeAttack = false;

            float m_lastDirectionRadians = Mathf.Atan2(m_lastDirection.y, m_lastDirection.x);

            // Move the hitbox to the correct position based on the players last direction
            m_meleeHitbox.transform.rotation = Quaternion.Euler(0, 0, m_lastDirectionRadians * Mathf.Rad2Deg);

            // Activate the melee hitbox (it should be disbaled initially)
            StartCoroutine(MeleeAttack());

            m_healthCode.Damage(5, "Internal");

        }
    }

    IEnumerator MeleeAttack()
    {
        m_meleeHitbox.SetActive(true);
        m_meleeRechargeBox.SetActive(true);
        m_meleeBarVisible = true;
        m_meleeTimer = 0;
        m_meleeBarFiller.fillAmount = 0;
        yield return new WaitForSeconds(0.5f);
        m_meleeHitbox.SetActive(false);
        //yield return new WaitForSeconds(5f);
        //canAttack = true;
    }

    public void HandleAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && (Time.time - m_timeOfLastFire) >= m_fireRate)
        {
            Fire();
            m_timeOfLastFire = Time.time;
        }

        if (ctx.performed && Time.time > m_fireTimeout && m_firing == true)
        {
            m_fireTimeout = Time.time + m_fireRate;

        }
    }

    public void HandleMeleeAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && m_canMeleeAttack)
        {
            Melee();
        }
    }

    public void HandleMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //m_playerDirection = ctx;
            onClutchMoment?.Invoke();
        }
    }

    public static UnityEvent onClutchMoment;

    public int GetWaves()
    {
        return m_waves;
    }
    public void PassWave()
    {
        m_waves++;
    }

    public int GetMeleeDamage()
    {
        return m_meleeDamage;
    }
    public int GetProjectileDamage()
    {
        return m_projectileDamage;
    }
    public void IncreaseMeleeDamage()
    {
        m_meleeDamage += 10;
    }
    public void IncreaseProjectileDamage()
    {
        m_projectileDamage += 5;
    }
    public float GetHealth()
    {
        return m_healthCode.m_maxHealth;
    }
    public void IncreaseHealth()
    {
        m_healthCode.m_currentHealth += 10;
        m_healthCode.m_maxHealth += 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Touch");
        if (collision.gameObject.CompareTag("coin"))
        {
            Debug.Log("Touch confirmed");
            m_scoreSystem.IncreaseScore(100);
            Destroy(collision.gameObject);    //Destroy the coin after you collect it
        }
    }
}
