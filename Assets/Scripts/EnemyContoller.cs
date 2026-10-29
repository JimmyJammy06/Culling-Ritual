using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
//using Function_Library

public class EnemyContoller : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform m_Player;
    //[SerializeField] private EnemySpawnHandler m_spawner;
    [SerializeField] private Collider2D m_detectionCollider;
    NavMeshAgent m_agent;
    //public float m_speed;
    private float m_meleeRange = 2f;    // Range at which the enemy should stop moving and try to attack. 1.6
    private bool m_PlayerInSight = false;
    private Animator m_animator;
    [SerializeField] private GameObject m_Hitbox;
    private bool m_isAttacking = false;
    SpriteRenderer m_Sprite;

    #endregion

    //SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Player = FindObjectOfType<TopDownCharacterController>().transform;
        m_agent = GetComponent<NavMeshAgent>();
        m_enemyStates = EnemyStates.Idle;
    }

    private void OnEnable()
    {
        //TopDownCharacterController.onClutchMoment.AddListener(PauseBecausePlayerIsAboutToDie);
    }

    void PauseBecausePlayerIsAboutToDie()
    {

    }

    private bool InAttackRange()
    {
        return Vector2.Distance(transform.position, m_Player.position) <= m_meleeRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < m_Player.position.x)
        {
            m_Sprite.flipX = true;
        }
        else 
        {
            m_Sprite.flipX = false;
        }

        switch (m_enemyStates)
        {
            case EnemyStates.Idle:
                {
                    // Idle animtion
                    //Debug.Log("Idle");
                    if (m_PlayerInSight && InAttackRange())
                    {
                        // attack
                        m_enemyStates = EnemyStates.Attack;
                    }
                    else if (m_PlayerInSight)
                    {
                        //move
                        m_animator.SetBool("Walking", true); // Moving animation
                        m_enemyStates = EnemyStates.MoveToPlayer;
                    }
                    break;
                }
            case EnemyStates.MoveToPlayer:
                {
                    if (!m_PlayerInSight)
                    {
                        //back to idle
                        m_animator.SetBool("Walking", false);
                        m_enemyStates = EnemyStates.Idle;
                    }
                    else if (InAttackRange())
                    {
                        //attack
                        m_animator.SetBool("Walking", false);
                        m_enemyStates = EnemyStates.Attack;
                    }
                    m_agent.SetDestination(m_Player.position);
                    break;
                }
            case EnemyStates.Attack:
                {
                    if (m_isAttacking)
                        break;
                    else
                        StartCoroutine(Attack());
                        break;
                }
            default:
                break;
        }
    }

    IEnumerator Attack()
    {
        m_isAttacking = true;
        //Debug.Log("hitting");
        m_animator.SetTrigger("Hitting");
        m_detectionCollider.enabled = false;
        m_Hitbox.SetActive(true);   // Activates a another circle collider that damages players inside
        yield return new WaitForSeconds(1f);
        //Debug.Log("attack on cooldown");
        m_Hitbox.SetActive(false);
        m_detectionCollider.enabled = true;
        yield return new WaitForSeconds(5f);
        m_isAttacking = false;
        m_enemyStates = EnemyStates.Idle;   // Goes back to Idle to wait for next decision
        Debug.Log("Atack end cooldown");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            m_PlayerInSight = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            m_PlayerInSight = false;
    }

    enum EnemyStates
    {
        Idle,
        MoveToPlayer,
        Attack
    };
    EnemyStates m_enemyStates = EnemyStates.Idle;
}
