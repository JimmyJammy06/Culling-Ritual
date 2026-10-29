using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public int m_score;
    [SerializeField] private TMPro.TextMeshProUGUI m_scoreText;
    [SerializeField] private GameObject m_upgradePanel;
    [SerializeField] private TopDownCharacterController m_player;
    private bool m_upgradePanelActive = false;


    private int m_healthUpgrades = -1;
    //public int m_healthUpgradeCount = 0;
    private int m_healthUpgradeCost = 100;
    [SerializeField] private Image[] m_healthVisualUpgrade;
    [SerializeField] private TMPro.TextMeshProUGUI m_healthText;
    [SerializeField] private TMPro.TextMeshProUGUI m_healthUpgradeCostText;



    private int m_projectileUpgrades = -1;
    //public int m_projectileUpgradeCount = 0;
    private int m_projectileUpgradeCost = 50;
    [SerializeField] private Image[] m_projectileVisualUpgrade;
    [SerializeField] private TMPro.TextMeshProUGUI m_projectileDamageText;
    [SerializeField] private TMPro.TextMeshProUGUI m_projectileDamageUpgradeCostText;



    private int m_meleeUpgrades = -1;
    //private int m_meleeUpgradeCount = 0;
    private int m_meleeUpgradeCost = 50;
    [SerializeField] private Image[] m_meleeVisualUpgrade;
    [SerializeField] private TMPro.TextMeshProUGUI m_meleeDamageText;
    [SerializeField] private TMPro.TextMeshProUGUI m_meleeDamageUpgradeCostText;

    private void Start()
    {
        m_score = 100;
        m_scoreText.text = "Score: " + m_score.ToString();
        m_healthText.text = (m_player.GetHealth()).ToString();
        m_healthUpgradeCostText.text = "Cost: " + m_healthUpgradeCost;
        m_projectileDamageText.text = (m_player.GetProjectileDamage()).ToString();
        m_projectileDamageUpgradeCostText.text = "Cost: " + m_projectileUpgradeCost;
        m_meleeDamageText.text = (m_player.GetMeleeDamage()).ToString();
        m_meleeDamageUpgradeCostText.text = "Cost: " + m_meleeUpgradeCost;
    }


    /// <summary>
    /// Method to add a given amount of score to the player
    /// </summary>
    /// <param name="scoreToAdd">  the amount of score to be added. </param>
    public void OpenUpgradePanel()
    {
        m_upgradePanel.SetActive(true);
    }
    public void CloseUpgradePanel()
    {
        m_upgradePanel.SetActive(false);
    }

    public void PlayerHealthUpgrade()
    {
        if (m_score >= m_healthUpgradeCost)
        {
            m_healthUpgrades++;
            m_player.IncreaseHealth();
            m_score -= m_healthUpgradeCost;
            m_scoreText.text = "Score: " + m_score.ToString();

            m_healthUpgradeCost += 50;
            //Change colour
            m_healthVisualUpgrade[m_healthUpgrades].color = Color.blue;
            // Change Texts
            m_healthText.text = (m_player.GetHealth()).ToString();
            m_healthUpgradeCostText.text = "Cost: " + m_healthUpgradeCost;
            if (m_healthUpgrades != 4)
            {
                m_healthUpgradeCostText.text = "Cost: " + m_healthUpgradeCost;
            }
            else
            {
                m_healthUpgradeCostText.text = "MAXED!!!";
            }
        }
    }
    public void PlayerProjectileUpgrade()
    {
        if (m_score >= m_projectileUpgradeCost)
        {
            m_projectileUpgrades++;
            m_player.IncreaseProjectileDamage();
            
            m_score -= m_projectileUpgradeCost;
            m_scoreText.text = "Score: " + m_score.ToString();

            m_projectileUpgradeCost += 50;
            //Change colour
            m_projectileVisualUpgrade[m_projectileUpgrades].color = Color.blue;
            // Change Texts
            m_projectileDamageText.text = (m_player.GetProjectileDamage()).ToString();
            if (m_projectileUpgrades != 4)
            {
                m_projectileDamageUpgradeCostText.text = "Cost: " + m_projectileUpgradeCost;
            }
            else
            {
                m_projectileDamageUpgradeCostText.text = "MAXED!!!";
            }
        }
    }
    public void PlayerMeleeUpgrade()
    {
        if (m_score >= m_meleeUpgradeCost)
        { 
            m_meleeUpgrades++;
            m_player.IncreaseMeleeDamage();
            m_score -= m_meleeUpgradeCost;
            m_scoreText.text = "Score: " + m_score.ToString();
            m_meleeUpgradeCost += 50;
            //Change colour
            m_meleeVisualUpgrade[m_meleeUpgrades].color = Color.blue;
            // Change Texts
            m_meleeDamageText.text = (m_player.GetMeleeDamage()).ToString();

            if (m_meleeUpgrades != 4)
            {
                m_meleeDamageUpgradeCostText.text = "Cost: " + m_meleeUpgradeCost;
            }
            else
            {
                m_meleeDamageUpgradeCostText.text = "MAXED!!!";
            }
        }
    }

    public void IncreaseScore(int amount)
    {
        m_score += amount;
        m_scoreText.text = "Score: " + m_score.ToString();
    }
}
