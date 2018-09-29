using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyView : MonoBehaviour
{
    public NavMeshAgent _agent;

    private Transform _target;


    private void Start()
    {
    }

    public NavMeshAgent agent
    {
        get { return _agent; }
    }

    // Update is called once per frame
    void Update ()
    {
        if(_agent != null && _target != null)
        {
            
        }
	}
}
