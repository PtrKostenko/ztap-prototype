using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;
    private Vector3 startRot;

    private void Awake()
    {
        startRot = transform.rotation.eulerAngles;
    }

    private void LateUpdate()
    {
        Vector3 newrot = transform.rotation.eulerAngles;
        if (freezeX)
            newrot.x = startRot.x;
        if (freezeY)
            newrot.y = startRot.y;
        if (freezeZ)
            newrot.z = startRot.z;
        transform.rotation = Quaternion.Euler(newrot);
    }
}
