using UnityEngine;

public class AutoDestroyPraticles : MonoBehaviour
{
    void Start() {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) {
            Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }
}


