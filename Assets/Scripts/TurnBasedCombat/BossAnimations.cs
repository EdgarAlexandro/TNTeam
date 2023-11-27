using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimations : MonoBehaviour
{
    Animator animatorController;
    MusicSFXManager soundEffect;
    TurnBasedCombatTargetHandler tbcTH;

    // Start is called before the first frame update
    void Start()
    {
        animatorController = GetComponent<Animator>();
        soundEffect = MusicSFXManager.Instance;
        tbcTH = TurnBasedCombatTargetHandler.Instance;
    }

    public void Attack()
    {
        animatorController.SetBool("Atacando", true);
        StartCoroutine("ColorChange");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ColorChange()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 5; i++)
        {
            sprite.color = Color.gray;
            yield return new WaitForSeconds(0.15f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitWhile(() => animatorController.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        animatorController.SetBool("Atacando", false);
    }
    public void SpawnAttack()
    {
        tbcTH.SpawnAttack();
    }

    public void SoundEffect(AudioClip clip)
    {
        soundEffect.PlaySFX(clip);
    }
}
