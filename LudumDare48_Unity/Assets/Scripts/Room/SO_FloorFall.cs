using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallData", menuName = "Scriptable Objects/Room/FallData")]
public class SO_FloorFall : ScriptableObject
{
    public float m_speed = 1;
    public float m_duration = 5;
    
    public float m_startY = 0;
    public float m_directionY = -1;

    public bool changeAngle = true;
    public Vector3 m_angleRand = Vector3.one;
    public Vector3 m_angleStart = Vector3.zero;
    public Vector3 m_scale = Vector3.one * .1f;

    public bool changeMesh = true;

    public Mesh[] variations;
    public ModelTime[] modelsTime;

    [System.Serializable]
    public struct ModelTime
    {
        public float time;
        public Mesh mesh;
    }
}
