using UnityEngine;
using System.Collections;

public class StarBehavior : MonoBehaviour
{
    public float spawnScaleTime = 2.5f; // Time to grow to full size
    public float destroyScaleTime = 2.5f; // Time to shrink before disappearing
    public float lifespan = 2f; // Time before the star starts shrinking

    void Start()
    {
        StartCoroutine(GrowEffect());
        Invoke("StartDestruction", lifespan); // Schedule destruction
    }

    IEnumerator GrowEffect()
    {
        float timer = 0;
        Vector3 startScale = Vector3.zero;  // Start at 0 size
        Vector3 targetScale = new Vector3(30,30,100);  // Normal size (1,1,1)

        while (timer < spawnScaleTime)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, timer / spawnScaleTime);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    void StartDestruction()
    {
        StartCoroutine(ShrinkEffect());
    }

    IEnumerator ShrinkEffect()
    {
        float timer = 0;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero; // Shrinking to disappear

        while (timer < destroyScaleTime)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, timer / destroyScaleTime);
            yield return null;
        }

        Destroy(gameObject); // Remove star after shrinking
    }
}
