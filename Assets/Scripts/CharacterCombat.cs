using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{

    public bool InCombat { get; private set; }
    public event System.Action OnAttack;
    public float attackSpeed = 1f;
    public float attackDelay = .6f;
    const float combatCooldown = 5;

    CharacterStats myStats;
    CharacterStats opponentStats;
    float attackCooldown = 0f;
    float lastAttackTime;


    private void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (Time.time - lastAttackTime > combatCooldown)
        {
            InCombat = false;
        }
    }

    public void Attack(CharacterStats targetStats)
    {
        if (attackCooldown <= 0f)
        {
            opponentStats = targetStats;
            if (OnAttack != null)
            {
                OnAttack();
            }

            attackCooldown = 1f / attackSpeed;
            InCombat = true;
            lastAttackTime = Time.time;
        }
    }   

    public void AttackHit_AnimationEvent()
    {
        opponentStats.TakeDamage(myStats.damage.GetValue());

        if (opponentStats.currentHealth <= 0)
        {
            InCombat = false;
        }
    }

}
