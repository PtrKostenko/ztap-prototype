using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharsInfrontTrigger : MonoBehaviour
{
    private ZCharacterController charact;

    // Start is called before the first frame update
    void Start()
    {
        charact = GetComponentInParent<ZCharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            charact.charsInfront.Add(other.GetComponent<ZCharacterController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            charact.charsInfront.Remove(other.GetComponent<ZCharacterController>());
        }
    }
}
