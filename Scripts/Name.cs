using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Name : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position + new Vector3(12, 17, 0);
        //transform.rotation = UnityEngine.Quaternion.FreezeRotation;
    }
}
