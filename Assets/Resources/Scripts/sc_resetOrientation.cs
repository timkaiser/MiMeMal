using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_resetOrientation : MonoBehaviour
{
    public GameObject resetee;

    public void resetOrientation() {
        resetee.transform.rotation = Quaternion.identity;
    }
}
