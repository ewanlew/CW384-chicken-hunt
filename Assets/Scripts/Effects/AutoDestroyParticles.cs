using UnityEngine;

public class AutoDestroyPraticles : MonoBehaviour
{
    void Start() {
        ParticleSystem ps = GetComponent<ParticleSystem>(); // grab particle system on object
        if (ps != null) {
            // destroy object after duration + longest lifetime
            Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }
}
