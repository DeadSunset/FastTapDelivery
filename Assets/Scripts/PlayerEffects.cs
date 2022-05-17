using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField]
    private GenericDeathEffectPlay _deathEffect;
    [SerializeField]
    private ParticleSystem _takeDamageEffect;

    public void DeathEffect()
    {
        _deathEffect.PlayAllParticles();
    }

    public void TakeDamageEffect()
    {
        _takeDamageEffect.Play();
    }
}
