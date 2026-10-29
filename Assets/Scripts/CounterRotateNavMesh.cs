using UnityEngine;

public class CounterRotateNavMesh : MonoBehaviour
{
    [SerializeField] Transform m_characterPos;

    private void Update()
    {

        Vector2 dirToChar = transform.position - m_characterPos.position;
        float angle = Mathf.Atan2(dirToChar.y, dirToChar.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
