using UnityEngine;

public class BloodDecal : MonoBehaviour
{
    public float growTime = 1.0f; // Time in seconds for the decal to reach full size
    public float randomFactor = 0.5f;
    public float particleSizeWeight = 1.0f; // New parameter to adjust target size based on particle size
    private Vector3 targetScale;
    private float timer;

    void Start()
    {
        // Optionally, you can adjust the targetScale here based on particleSizeWeight
    }

    public void InitializeWithParticleSize(float particleSize)
    {
        // Adjust the target scale based on the particle size and particleSizeWeight
        targetScale = transform.localScale * Random.Range(1 - randomFactor, 1 + randomFactor) * particleSize * particleSizeWeight;
        transform.localScale = Vector3.zero; // Start from zero scale
        timer = 0;
    }

    void Update()
    {
        if (timer < growTime)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(0, 1, timer / growTime);
            transform.localScale = scale * targetScale;
        }
    }
}
