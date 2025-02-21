using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
	public float delay = 2f;
	public GameObject explosionPrefab;
	void Start () {
		Destroy (explosionPrefab, delay); 
	}
}