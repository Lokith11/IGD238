using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowItem : item
{
    // Start is called before the first frame update
    public float slowAmount = 0.6f; //%
    public float slowDuration = 2f; 

    public override void itemEffect(GameObject target)
    {
        damageAmount = 5;
        base.itemEffect(target);
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Slow(slowAmount, slowDuration);
        }
    }
}
