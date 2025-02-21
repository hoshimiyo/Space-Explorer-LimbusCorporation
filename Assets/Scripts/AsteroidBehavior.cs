using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    public float destroyY = -433f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
