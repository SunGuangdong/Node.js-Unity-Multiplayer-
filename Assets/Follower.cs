using UnityEngine;
using System.Collections;

/// <summary>
/// 处理 玩家Player 跟随 目标的运动
/// </summary>
public class Follower : MonoBehaviour {

    public Targeter targeter;

    public float scanFrequency = 0.5f;    // 监视时间（即使你狂点新目标，也会在这个时间之后重新跟随）
	public float stopFollowDistance = 2;  // 停止跟随的距离

	NavMeshAgent agent;

	float lastScanTime = 0;

	void Start() {
		agent = GetComponent<NavMeshAgent> ();
		targeter = GetComponent<Targeter> ();
	}

	void Update () {

        //if (isReadyToScan() && !isInRange())
        //{
        //    Debug.Log("scanning nav path");
        //    agent.SetDestination(targeter.position);
        //}

        if (isReadyToScan() && !targeter.IsInRange(stopFollowDistance))
        {
            Debug.Log("scanning nav path");
            agent.SetDestination(targeter.target.position);
            lastScanTime = Time.time;
        }
    }

    bool isReadyToScan()
    {
        return Time.time - lastScanTime > scanFrequency && targeter.target;
    }

    //bool isInRange()
    //{
    //    return Vector3.Distance(targeter.position, transform.position) <= stopFollowDistance;
    //}
}
