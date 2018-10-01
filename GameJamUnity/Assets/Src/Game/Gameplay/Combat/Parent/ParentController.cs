using Gameplay.Particles;
using UnityEngine;

public class ParentController : GhostGen.EventDispatcherBehavior, IDamageable
{
    public float health
    {
        get; set;
    }
    public Rigidbody _rigidBody;

    public Rigidbody physbody
    {
        get { return _rigidBody; }
    }

    public bool isDead
    {
        get { return health <= 0; }
    }

    public void Init(PlayerConfig playerConfig)
    {
        health = playerConfig.parentStartHealth;
    }

    public DamageResult TakeDamage(object attacker, float damage)
    {
        DamageResult result = new DamageResult();
        result.prevHealth = health;
        health = Mathf.Max(health - damage, 0.0f);
        result.newHealth = health;
        result.victim = this;
        result.attacker = attacker;

        DispatchEvent(GameplayEventType.DAMAGE_TAKEN, false, result);

        if (isDead && result.prevHealth > 0.0f)
        {
            Singleton.instance.particleGod.GenerateParticle(Particle.Type.BaseExplosion, transform.position);
            gameObject.SetActive(false);
        }
        return result;
    }
}
