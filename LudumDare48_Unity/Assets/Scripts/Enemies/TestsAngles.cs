using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestsAngles : MonoBehaviour
{
    public float AngleA;
    public float AngleB;
    public float ReflextionAngle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float difference = 2* AngleA - AngleB;
        float Sign = Mathf.Sign(difference);
        float minus180 = Mathf.Abs(difference - 180);
        float Resign = minus180 * Sign;
        ReflextionAngle = difference;
    }
}
