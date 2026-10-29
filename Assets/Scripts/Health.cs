using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Image m_healthBarFiller;
    [SerializeField] private Image m_secondaryHealthBarFiller;
    private TopDownCharacterController m_player;
    private EnemySpawnHandler m_enemySpawner;
    public float m_maxHealth = 100;
    public float m_currentHealth;
    [SerializeField] string m_vulnerableTo = "Player";
    private bool m_healthBarHidden;
    public GameObject m_toBeDestroyed;
    public GameObject m_healthBarCanvas;
    private float m_healthToRegain;
    private bool m_dead = false;
    [SerializeField] private float m_healthRegainSpeed = 0.02f;

    private void Start()
    {
        m_player = FindObjectOfType<TopDownCharacterController>();
        m_enemySpawner = FindObjectOfType<EnemySpawnHandler>();
        if (m_vulnerableTo == "Player")
        {
            m_healthBarHidden = true;
            m_maxHealth = 50; //Enemies go up by 5 health each wave
        }
        else if (m_vulnerableTo == "Enemy")
        {
            m_healthBarHidden = false;
        }
        m_currentHealth = m_maxHealth;
    }

    private void Update()
    {
        //Current_Health -= 2 * Time.deltaTime;

        if (!m_healthBarHidden)
        {
            if (m_vulnerableTo == "Enemy")  //Only the player character needs to worry about this
            {
                if (m_player.GetHealth() != m_maxHealth)   //If the health has changed then also increase current health 
                {
                    m_currentHealth += m_player.GetHealth() - m_maxHealth;
                    m_maxHealth = m_player.GetHealth();
                }
                m_healthBarFiller.fillAmount = m_currentHealth / (m_maxHealth);    //calculates a percentage and uses that for the fill of the health bar
                m_secondaryHealthBarFiller.fillAmount = (m_currentHealth + m_healthToRegain) / m_maxHealth; // shows how much health you will have when all your health to ragin is added
            }
            else
            {
                m_healthBarFiller.fillAmount = m_currentHealth / m_maxHealth;
                // Player doesnt need to worry about the Secondary Health bar as it will never use it (and therefore doesnt have it)
            }
        }
        if (m_healthToRegain >= 0)
        {
            float healthToBeRegainedThisTick = m_healthRegainSpeed * Time.deltaTime;    // temp variable so that Time.deltaTime is consistent from line 42 and 43
            m_currentHealth += healthToBeRegainedThisTick;
            m_healthToRegain -= healthToBeRegainedThisTick;
            if (m_healthToRegain <= 0.2)
            {
                m_currentHealth += m_healthToRegain;
                m_healthToRegain = 0;
            }
        }
        if (m_currentHealth > m_maxHealth)
        {
            m_currentHealth = m_maxHealth;  // Just incase this happens due to Time.delatTimes unpredictable decimal numbers
        }
    }

    public void Damage(float amount, string source)
    {
        if (m_healthBarHidden == true)
        {
            m_healthBarCanvas.SetActive(true);
            m_healthBarHidden = false;
            //Debug.Log("Health Bar reveal");
        }
        m_healthToRegain = 0;
        if (source == "external")
        {
            //Debug.Log(m_maxHealth);
            //Debug.Log(m_currentHealth);
            //Debug.Log(amount);
            m_currentHealth -= amount;
        }
        else
        {
            Debug.Log(m_maxHealth);
            m_currentHealth -= (amount / 100 * m_maxHealth);  //percentage
            m_healthToRegain += amount;
        }
        if (m_currentHealth <= 0 && m_dead == false)
        {
            m_dead = true;
            if (m_vulnerableTo == "Enemy")
            {
                
                SceneManager.LoadScene("Death Scene");
            }
            else
            {
                m_enemySpawner.ConfirmKill();
                Destroy(m_toBeDestroyed, 1);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_vulnerableTo == "Player")
        {
            if (collision.gameObject.CompareTag("PlayerProjectile"))
            {
                Debug.Log("Damage dealt B");
                Damage(m_player.GetProjectileDamage(), "external");
            }
        }
        else if (m_vulnerableTo == "Enemy")
        {
            if (collision.gameObject.CompareTag("EnemyProjectile"))
            {
                Damage((m_enemySpawner.GetWaveCount() * 2 + 5), "external");  //Initial enemy projectile damage is 5, every wave this will increase by 2
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (m_vulnerableTo == "Player")
        {
            if (collision.gameObject.CompareTag("PlayerMelee"))
            {
                Debug.Log("Damage dealt A");
                Damage(m_player.GetMeleeDamage(), "external");
            }
        }
        else if (m_vulnerableTo == "Enemy")
        {
            if (collision.gameObject.CompareTag("EnemyMelee"))
            {
                Damage((m_enemySpawner.GetWaveCount() * 2 + 5), "external"); //Initial enemy melee damage is 10, every wave this will increase by 2
            }
        }
    }
}
