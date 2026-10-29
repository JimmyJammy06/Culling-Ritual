using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] m_transforms;
    [SerializeField] private GameObject m_coinPrefab;
    private int m_coinsActive = 0;
    private bool m_coinsSpawning = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (m_coinsSpawning == false && m_coinsActive == 0)
        {
            StartCoroutine(CoinSpawn());
        }
    }

    IEnumerator CoinSpawn()
    {
        m_coinsSpawning = true;
        for (int pos = 0; pos < m_transforms.Length - 1; pos++)
        {
            Instantiate(m_coinPrefab, m_transforms[pos].position, Quaternion.identity);      //Spawn a coin at one of our positions
            m_coinsActive++;
            yield return new WaitForSeconds(10);   //We wait for a while so theres not too many coins being given out
        }
        m_coinsSpawning =false;
    }
}
