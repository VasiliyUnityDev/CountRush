 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float cameraDistance = 3f;
    public float cameraHeight = 6f;
    public float cameraFollowSpeed = 15f;
    float cameraZ;

    [SerializeField]
    GameObject target;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, cameraHeight, target.transform.position.z - cameraDistance), cameraFollowSpeed * Time.deltaTime);
    }
}
