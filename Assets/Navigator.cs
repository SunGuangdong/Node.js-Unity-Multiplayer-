using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour
{
    NavMeshAgent agent;
    Targeter target;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GetComponent<Targeter>();
    }
    public void NavigateTo(Vector3 position)
    {
        agent.SetDestination(position);
        //target.target = null;
        target.ResetTarget();
    }

    void Update()
    {
        GetComponent<Animator>().SetFloat("Distance", agent.remainingDistance);
    }
}
