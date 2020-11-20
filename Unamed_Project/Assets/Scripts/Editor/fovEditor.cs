using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (EnemyAi))]
public class fovEditor : Editor
{
    void OnScreenHUI()
    {
        EnemyAi fov = (EnemyAi)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.m_viewAngle);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.m_viewAngle / 2);
        Vector3 viewAngleB = fov.DirFromAngle(fov.m_viewAngle / 2);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.m_viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.m_viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.m_VisibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }

    }
}
