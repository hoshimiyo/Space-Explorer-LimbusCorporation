using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    public float destroyY = -433f;
    

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
