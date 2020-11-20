using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{
    
    private NavMeshAgent m_Nav;
    private FPSController m_Player;
    [Range(0, 360)]
    public float m_viewAngle;
    private float m_ConstRadius;
    public float m_viewRadius;

    public LayerMask m_TargetMask;
    public LayerMask m_ObstacleMask;

    public List<Transform> m_VisibleTargets = new List<Transform>();

    private bool m_BeenSeen = false;
    private Vector3 LastPos;
    [HideInInspector] public Vector3 PlayerPos;

    [HideInInspector] public bool m_SoundPlayed = false;

    void Start()
    {
        m_Player = GameObject.Find("Player").GetComponent<FPSController>();
        m_ConstRadius = m_viewRadius;
       m_Nav = GetComponent<NavMeshAgent>();
        m_BeenSeen = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (m_SoundPlayed)
        {
            MoveToSound(PlayerPos);
        }
        else
        {

        }
       
        FindVisibleTargets();
    }

   
    

    void FindVisibleTargets()
    {
        if (m_Player.isCrouching)
        {
            m_viewRadius = 10;
        }
        else
        {
            m_viewRadius = m_ConstRadius;
        }

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
                    m_BeenSeen = true;
                }
                else if (Physics.Raycast(transform.position, dirToTarget, distToTarget, m_ObstacleMask) && m_BeenSeen)
                {
                    m_Nav.SetDestination(LastPos);
                   
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
       
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public Vector3 MoveToSound(Vector3 PlayerPos)
    {
        m_Nav.SetDestination(PlayerPos);
        return PlayerPos;
       
    }
}
