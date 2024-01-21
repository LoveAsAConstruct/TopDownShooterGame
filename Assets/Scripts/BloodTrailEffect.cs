using UnityEngine;

public class BloodTrailEffect : MonoBehaviour
{
    public GameObject bloodDecalPrefab; // Assign your blood decal prefab in the inspector
    public int chanceToNotSpawn = 100;
    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }

    void Update()
    {
        int numParticlesAlive = particleSystem.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            if (Random.Range(0, chanceToNotSpawn) <= 1)
            {
                // Instantiate a blood decal at each particle's position
                InstantiateBloodDecal(particles[i].position, particles[i].GetCurrentSize(particleSystem));
            }
            // Additional logic for pooling can be added here, e.g., checking particle velocity
        }
    }

    public void InstantiateBloodDecal(Vector3 position, float particleSize)
    {
        GameObject decal = Instantiate(bloodDecalPrefab, position, Quaternion.identity);
        BloodDecal bloodDecalScript = decal.GetComponent<BloodDecal>();
        if (bloodDecalScript != null)
        {
            bloodDecalScript.InitializeWithParticleSize(particleSize);
        }
    }

}
