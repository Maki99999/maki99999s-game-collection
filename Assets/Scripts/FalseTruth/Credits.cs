using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth
{
    public class Credits : MonoBehaviour
    {
        public Animator doubleDoor01;
        public Animator doubleDoor02;
        public Animator occultDoor;

        public Animator unityChan;
        public GameObject ghost;
        public Animator creditRoll;

        public GameController gameController;

        void Start()
        {
            StartCoroutine(Animation());
        }

        IEnumerator Animation() {
            yield return new WaitForSeconds(12.5f);
            doubleDoor01.SetBool("Open", true);

            yield return new WaitForSeconds(1f);    //13.5
            doubleDoor01.SetBool("Open", false);

            yield return new WaitForSeconds(8f);    //21.5
            doubleDoor02.SetBool("Open", true);
            
            yield return new WaitForSeconds(3f);    //24.5
            unityChan.SetTrigger("Go");
            
            yield return new WaitForSeconds(7f);    //31.5
            ghost.SetActive(false);

            yield return new WaitForSeconds(18.75f);//50.25
            occultDoor.SetTrigger("Secret");
            
            yield return new WaitForSeconds(1.6f);  //51.85
            creditRoll.SetTrigger("Start");

            GameController.UnlockMouse();
        }

        public void BackToMM() {
            gameController.EndScene("FalseTruthMainMenu");
        }
    }
}