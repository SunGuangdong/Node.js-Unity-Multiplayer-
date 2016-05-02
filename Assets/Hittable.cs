using UnityEngine;
using System.Collections;

/// <summary>
/// 受击的 处理    包括  血量的管理， 死亡判定， 重生
/// </summary>
public class Hittable : MonoBehaviour {

	public float health = 100;
    public float respawnTime = 5;

    public bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnHit() {
		health -= 10;

        if (IsDead)
        {
            animator.SetTrigger("Dead");
            Invoke("Spawn", respawnTime);
        }
    }

    void Spawn()
    {
        Debug.Log("Spawning my player");
        transform.position = Vector3.zero;
        health = 100;
        animator.SetTrigger("Spawn");
    }
}
