using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public AudioClip  Aud;
    public void PlayerDamage()
    {
        transform.GetComponentInParent<AIEnemy>().DamagePlayer();
    }

    public void Play_sound()
    {
        AudioSource.PlayClipAtPoint(Aud, transform.position);
    }
}
