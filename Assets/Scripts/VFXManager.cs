using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject m_explosionPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static VFXManager s_instance;

    private void Awake()
    {
        // Set the instance to this if its null.
        if (s_instance == null)
            s_instance = this;
    }
    /// <summary>
    /// Creates an explosion at a given point and destroy it after a certain time has passed.
    /// </summary>
    /// <param name="position">The position at which the explosion should be spawned.</param>
    /// <param name= "destroyAfter"> The number of secondas to pass before the destroying of the explosion. </param>
    /// <returns> A reference to the explosion GameObject that has been spawned</returns>

    public static GameObject CreateExplosion(Vector3 position, float destroyAfter = 0.5f)
    {
        //Instance not set? Return null but show an error.
        if (s_instance == null)
        {
            //Debug.LogError("Tried to spawn an explosion but the instance hasn't been set");
            return null;
        }

        //Otherwise, spawn the explosion.
        GameObject explosion = Instantiate(s_instance.m_explosionPrefab, position, Quaternion.identity);

        //Destroy the explosion after a certain amount of time.
        Destroy(explosion, destroyAfter);

        //Return a reference to the explosion.
        return explosion;
    }
}
