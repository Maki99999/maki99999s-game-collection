using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace artgame
{
    public class CreateStairs : MonoBehaviour
    {
        public int initStairCount;
        public GameObject firstStair;
        public float stairSizeX;
        public float stairSizeY;

        List<GameObject> stairs = new List<GameObject>();
        int activated = 0;
        float nextStairX;
        float nextStairY;

        bool back = true;

        void Start()
        {
            stairs.Add(firstStair);
            nextStairX = stairSizeX;
            nextStairY = stairSizeY;
            ChangeStairCount(initStairCount);
        }

        void OnEnable()
        {
            StartCoroutine(ChangeStairs());
        }

        IEnumerator ChangeStairs()
        {
            while (enabled)
            {
                if(activated == 100 || activated == 0)
                    back = !back;
                    ChangeStairCount(activated + (back ? 0 : 2));
                yield return new WaitForSeconds(Random.Range(0f, 0.1f));
            }
        }

        public void ChangeStairCount(int newStairCount)
        {
            while (stairs.Count < newStairCount)
            {
                stairs.Add(Instantiate(firstStair, transform));
                stairs[stairs.Count - 1].transform.localPosition = new Vector3(0f, nextStairY, nextStairX);
                nextStairX += stairSizeX;
                nextStairY += stairSizeY;
            }

            while (activated + 1 > newStairCount)
            {
                stairs[activated--].SetActive(false);
            }

            while (activated + 1 < newStairCount)
            {
                stairs[++activated].SetActive(true);
            }
        }
    }
}