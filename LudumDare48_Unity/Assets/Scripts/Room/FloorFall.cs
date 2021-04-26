using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFall : MonoBehaviour
{
    [SerializeField] SO_FloorFall falldata = null;
    float m_time = -1;

    int m_meshStep = 1;

    Transform m_transform = null;
    MeshFilter m_filter = null;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_filter = GetComponent<MeshFilter>();
        GameManager.inst.floorFall += FloorFalling;
    }

    void OnDisable()
    {
        if (GameManager.inst != null)
            GameManager.inst.floorFall -= FloorFalling;
    }

    void FloorFalling()
    {
        StartCoroutine(enumerator());
    }
    IEnumerator enumerator()
    {
        m_time = Time.time + falldata.m_duration;

        Vector3 initAngle = m_transform.eulerAngles;
        float x = Random.Range(-falldata.m_angleRand.x, falldata.m_angleRand.x);
        float y = Random.Range(-falldata.m_angleRand.y, falldata.m_angleRand.y);
        float z = Random.Range(-falldata.m_angleRand.z, falldata.m_angleRand.z);
        Vector3 targetAngle = new Vector3(x, y, z);
        //Vector3 initScale = m_transform.localScale;

        while (m_time > Time.time)
        {
            m_transform.position += Vector3.up * falldata.m_directionY * Time.deltaTime * falldata.m_speed;
            
            if (falldata.changeAngle)
            {
                float timeprog = 1 - (m_time - Time.time) / falldata.m_duration;
                m_transform.eulerAngles = Vector3.Lerp(initAngle, targetAngle, timeprog);
                //m_transform.localScale = Vector3.Lerp(initScale, falldata.m_scale, timeprog);

                if (falldata.changeMesh && m_meshStep < falldata.modelsTime.Length)
                {
                    if (falldata.modelsTime[m_meshStep].time < timeprog)
                    {
                        m_filter.sharedMesh = falldata.modelsTime[m_meshStep].mesh;
                        m_meshStep++;
                    }
                }
            }

            yield return null;
        }

        m_transform.position = new Vector3(m_transform.position.x, falldata.m_startY, m_transform.position.z);
        if (falldata.changeAngle)
        {
            m_transform.eulerAngles = falldata.m_angleStart;
            m_filter.sharedMesh = falldata.modelsTime[0].mesh;
            m_meshStep = 1;
        }
    }
}