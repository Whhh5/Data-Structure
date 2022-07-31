using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBase : MonoBehaviour
{
    [SerializeField] Vector3 point;
    public Vector3 GetPoint()
    {
        return point;
    }
    public void SetPoint(Vector3 point)
    {
        this.point = point;
    }
}
