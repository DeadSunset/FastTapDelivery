using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDeathEffectPlay : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    public void PlayAllParticles()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
            print("play");
        }
    }
}
