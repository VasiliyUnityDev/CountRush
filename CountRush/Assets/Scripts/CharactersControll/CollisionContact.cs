using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionContact : MonoBehaviour
{
    public int indexLook;

    public GameObject cap, flipFlopR, fliFlopL;
    public GameObject flippersR, flippersL, glasses;
    public GameObject bootsRight, bootsLeft;

    public static int ammountCharacter;

    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ammountCharacter = 1;
    }

    private void FixedUpdate()
    {
        if(ammountCharacter <= 0)
        {
            UIManager.instance.Lose();
        }

        ChangeSkin();
    }

    private void ChangeSkin()
    {
        if(indexLook == 1)
        {
            flippersR.SetActive(true);
            flippersL.SetActive(true);
            glasses.SetActive(true);
        }
        else if (indexLook == 2)
        {
            cap.SetActive(true);
            flipFlopR.SetActive(true);
            fliFlopL.SetActive(true);
        }
        else if(indexLook == 3)
        {
            bootsRight.SetActive(true);
            bootsLeft.SetActive(true);
        }
        else
        {
            cap.SetActive(false);
            flipFlopR.SetActive(false);
            flippersL.SetActive(false);
            fliFlopL.SetActive(false);
            flippersR.SetActive(false);
            glasses.SetActive(false);
            bootsRight.SetActive(false);
            bootsLeft.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gates")
        {
            anim.SetBool("Spin", true);

            indexLook = other.gameObject.GetComponent<Gates>().indexs;

            Vibration.Vibrate();
            Vibration.Vibrate(120);
        }

        if(other.gameObject.tag == "Clear")
        {
            indexLook = 0;
        }

        if (other.gameObject.tag == "Water")
        {
            if (indexLook == 1)
            {
                anim.SetBool("Swimming", true);
                CharacterMovement.speedMove = 2f;
                
            }
            else
            {
                FollowCamera.instance.TagretFollow();
                Destroy(gameObject);
                ammountCharacter -= 1;
            }
        }

        if(other.gameObject.tag == "Sand")
        {
            if (indexLook == 2)
            {
                anim.SetBool("WalkingSand", true);
                CharacterMovement.speedMove = 2f;
            }
            else
            {
                FollowCamera.instance.TagretFollow();
                Destroy(gameObject);
                ammountCharacter -= 1;
            }
        }

        if (other.gameObject.tag == "Acid")
        {
            if (indexLook == 3)
            {
                anim.SetBool("WalkingAcid", true);
                CharacterMovement.speedMove = 1f;
            }
            else
            {
                FollowCamera.instance.TagretFollow();
                Destroy(gameObject);
                ammountCharacter -= 1;
            }
        }

        if (other.gameObject.tag == "Finish")
        {
            anim.SetBool("Dance", true);
            UIManager.instance.Win();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Vibration.Vibrate();
        Vibration.Vibrate(95);
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("Spin", false);
        anim.SetBool("Swimming", false);
        anim.SetBool("WalkingSand", false);
        anim.SetBool("WalkingAcid", false);

        CharacterMovement.speedMove = 3f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            collision.gameObject.gameObject.tag = "Player";
            collision.gameObject.GetComponent<CharacterMovement>().enabled = true;
            ammountCharacter += 1;
        }
    }
}
