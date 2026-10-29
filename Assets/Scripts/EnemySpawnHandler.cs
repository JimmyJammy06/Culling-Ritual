using System.Collections;
using UnityEngine;

public class EnemySpawnHandler : MonoBehaviour
{
    #region Variable Declarations
    [SerializeField] GameObject m_enemyPrefab;
    //[SerializeField] private GameObject m_enemy;
    //[SerializeField] private int m_possiblePositions;
    [SerializeField] private Transform[] m_transforms;
    private Transform m_Player;
    [SerializeField] private ScoreSystem m_scoreKeeper;
    //[SerializeField] private TopDownCharacterController m_characterController;
    //public Array m_spawnPoints;
    private int m_spawnCount;
    private int m_enemiesRemaining = 0;
    private int m_waveCount;
    //[SerializeField] private Array m_enemyArray;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Player = FindObjectOfType<TopDownCharacterController>().transform;
        m_enemiesRemaining = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_enemiesRemaining == 0)    //If theres no enemies remaining start next wave
        {
            m_waveCount++;
            if (m_waveCount > 1)
            {
                m_spawnCount = m_waveCount + 4;     //second wave spawns 5 enemies and then increases 1 enemy per wave past that point
                m_enemiesRemaining = m_spawnCount;
                StartCoroutine(EnemySpawn(m_spawnCount));
            }
            else
            {
                m_enemiesRemaining = 1;
                StartCoroutine(EnemySpawn(1)); // Start with spawning 1 enemy to introduce player
            }
        }
    }

    IEnumerator EnemySpawn(int amount)
    {
        int amountLeftToSpawn = amount;
        while (amountLeftToSpawn > 0)
        {
            for (int pos = 0; pos < m_transforms.Length - 1; pos++)
            {
                yield return new WaitForSeconds(0.5f + (m_waveCount / 10f));   //We add a 0.1 of a second to the gap inbetween so as the number of enemies increases,it also slightly slows spawns 
                Instantiate(m_enemyPrefab, m_transforms[pos].position, Quaternion.identity);      //Spawn an enemy at one of our positions
                amountLeftToSpawn--;
                if (amountLeftToSpawn == 0)     //If it reaches the amount of enemies to spawn mid loops,just break out
                {
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f);

        }
    }

    public void ConfirmKill()
    {
        m_enemiesRemaining--;
        m_scoreKeeper.IncreaseScore(50);
    }
    public int GetWaveCount()
    {
        return m_waveCount;
    }
}

// https://gamedevbeginner.com/how-to-get-a-variable-from-another-script-in-unity-the-right-way/

//Make an enemy spanwer
//as enemy dies, send a noti to collider
//collider box at door
//spawner turns on collider box
