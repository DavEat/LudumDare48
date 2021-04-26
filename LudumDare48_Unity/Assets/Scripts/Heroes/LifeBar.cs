using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    Transform m_transform = null;
    Image m_image = null;
    [SerializeField] Image m_image_arrow = null;

    public Gradient gradient;

    Vector3 m_defaultAngle;

    void Start()
    {
        m_transform = GetComponent<Transform>();
        m_image = GetComponent<Image>();

        m_defaultAngle = m_transform.eulerAngles;
        UpdateLife(1);
    }

    void Update()
    {
        m_transform.eulerAngles = m_defaultAngle + new Vector3(0,0,1) * 180 * m_image.fillAmount;
    }
    public void UpdateLife(float percent)
    {
        m_image.fillAmount = percent;
        m_image.color = gradient.Evaluate(1 - percent);
        m_image_arrow.color = m_image.color;
    }
}