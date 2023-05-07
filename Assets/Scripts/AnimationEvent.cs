using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public void PlayerDamage()
    {
        transform.GetComponentInParent<AIEnemy>().DamagePlayer();
    }
}
