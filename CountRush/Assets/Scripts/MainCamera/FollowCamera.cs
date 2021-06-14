using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public static FollowCamera instance;
    GameObject player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (CollisionContact.ammountCharacter >= 1)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            player = gameObject;
        }
    }

    public void TagretFollow()
    {
        transform.parent = null;
        Invoke("NewTargetFollow", 0.01f);
    }

    private void NewTargetFollow()
    {
        transform.parent = player.transform;
    }
}
