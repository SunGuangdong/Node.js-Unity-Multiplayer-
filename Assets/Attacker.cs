using UnityEngine;
using System.Collections;

/// <summary>
/// 处理角色的攻击， 也是挂在 Player 上的！
/// </summary>
public class Attacker : MonoBehaviour {

	public float attackDistance = 1;    // 距离目标的距离＜这个值就开始攻击
	public float attackRate = 2;        // 

	float lastAttackTime = 0;

	Targeter targeter;

	void Start () {
		targeter = GetComponent<Targeter> ();
	}

	void Update () {

        if (!isReadyToAttack())
            return;

        if (isTargetDead())
        {
            targeter.ResetTarget();
            return;
        }

        if (targeter.IsInRange(attackDistance))
        {
            Debug.Log("attacking" + targeter.target.name);

            var targetId = targeter.target.GetComponent<NetworkEntity>().id;

            Network.Attack(targetId);

            lastAttackTime = Time.time;
        }
    }

	bool isReadyToAttack()
    {
		return Time.time - lastAttackTime > attackRate && targeter.target;
	}

    bool isTargetDead()
    {
        return targeter.target.GetComponent<Hittable>().IsDead;
    }
}
