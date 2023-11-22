using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackInitializer : MonoBehaviourPunCallbacks
{
    Animator animator;
    MusicSFXManager soundEffect;

    public GameObject attack;
    public GameObject attackSpawn;

    private TurnBasedCombatManager tbc;

    void Start()
    {
        tbc = TurnBasedCombatManager.Instance;
        animator = GetComponent<Animator>();
        soundEffect = MusicSFXManager.Instance;
    }

    public void StartAttackAnimation()
    {
        tbc.canvas.SetActive(false);
        animator.SetBool("Atacando", true);
    }

    public void EndAnimation()
    {
        animator.SetBool("Atacando", false);
    }

    public void SpawnAttack()
    {
        PhotonNetwork.Instantiate(attack.name, attackSpawn.transform.position, Quaternion.identity);
    }
}
