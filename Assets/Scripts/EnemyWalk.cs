using UnityEngine;
using System.Collections; // Přidáno pro IEnumerator

public class EnemyWalk : MonoBehaviour
{
    private Animator animator;
    private RuntimeAnimatorController originalController;
    private RuntimeAnimatorController attackController;
    private float attackAnimationLength = 2f; // Délka animaci "Attack" v sekundách

    void Start()
    {
        animator = GetComponent<Animator>();
        originalController = animator.runtimeAnimatorController; // Uložení původního kontroleru
        attackController = Resources.Load<RuntimeAnimatorController>("Assets/FantasyMonsters/Animations/Bunny/Attack"); // Cesta k vašemu kontroleru s animaci "Attack"
    }

    public void TriggerAttackAnimation()
    {
        StartCoroutine(AttackAnimationSequence());
    }

    private IEnumerator AttackAnimationSequence()
    {
        animator.runtimeAnimatorController = attackController; // Nastavení kontroleru na animaci "Attack"
        yield return new WaitForSeconds(attackAnimationLength); // Čekání na dokončení animaci "Attack"
        animator.runtimeAnimatorController = originalController; // Návrat na původní kontroler
    }
}