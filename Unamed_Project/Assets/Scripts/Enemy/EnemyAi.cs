using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{
    
    private NavMeshAgent m_Nav;

    [Range(0, 360)]
    public float m_viewAngle;
    public float m_viewRadius;

    public LayerMask m_TargetMask;
    public LayerMask m_ObstacleMask;

    public List<Transform> m_VisibleTargets = new List<Transform>();

    private Vector3 LastPos;

    void Start()
    {
        m_Nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
       
        FindVisibleTargets();
    }

   
    

    void FindVisibleTargets()
    {
        m_VisibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, m_viewRadius, m_TargetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < m_viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, m_ObstacleMask))
                {
                    m_Nav.SetDestination(target.position);
                    LastPos = target.position;
                    m_VisibleTargets.Add(target);
                }
                else if (Physics.Raycast(transform.position, dirToTarget, distToTarget, m_ObstacleMask))
                {
                    m_Nav.SetDestination(LastPos);
                   
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool IsGlobal)
    {
       
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
