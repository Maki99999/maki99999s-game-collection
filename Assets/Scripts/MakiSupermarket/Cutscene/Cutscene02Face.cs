using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MakiSupermarket
{
    public class Cutscene02Face : MonoBehaviour, Rideable, Pausing
    {
        public PlayerController playerController;
        public DialogueManager dialogueManager;
        [Space(10)]
        public Transform playerTextPosition;
        public Transform playerGameLookPosition;
        [Space(10)]
        public Image startText;
        public GameObject lookingAtFace;
        [Space(10)]
        public Transform face;
        public Transform faceIdlePosition;
        public Transform faceByePosition;
        public Transform faceLSidePosition;
        public Animator faceAnim;
        [Space(10)]
        public Transform handL;
        public Transform handWavePos;
        public Animator handWaveAnim;
        public Transform handR;
        public Transform handTopPos;
        public Transform handBotPos;
        public Transform handIdlePos1;
        public Transform handIdlePos2;
        public float handRange = 5f;
        public Transform book;
        public Transform bookEndPos;
        public GameObject bookSafety;
        [Space(5)]
        public Transform thrownObjects;
        public GameObject apple;
        public GameObject banana;
        public GameObject square;
        public GameObject circle;
        public GameObject triangle;
        public GameObject cube;
        public GameObject sphere;
        public GameObject cylinder;
        public GameObject blood;
        public GameObject child;
        [Space(10)]
        public CameraBlurJitterEffect camEffect;
        [Space(5)]
        public AudioSource audioSource;
        public AudioClip bass01;
        public AudioClip bass02;
        public AudioClip bass03;

        bool triggered = false;
        bool camMove = false;

        IEnumerator FaceAnim01()
        {
            playerController.Ride(transform);
            StartCoroutine(playerController.MovePlayer(playerTextPosition, 60));

            for (float f = 0f; f < 1f; f += 1f / 80f)
            {
                startText.color = new Color(1, 1, 1, f);
                yield return new WaitForSeconds(1f / 60f);
            }

            yield return dialogueManager.StartDialogue(Dialogue.OneLineMonologue("\"Look at my face\"\nHmmm..."));

            lookingAtFace.SetActive(true);
            camMove = true;
        }

        public void StartAfterLooking()
        {
            StartCoroutine(FaceAnim02());
        }

        IEnumerator FaceAnim02()
        {
            camMove = false;
            StartCoroutine(TextAway());
            yield return playerController.ForceLookPlayer(face, 30);

            StartCoroutine(Things.PosSLerp(face, faceIdlePosition.position, 120));
            StartCoroutine(ChangeAmbientNoise(bass01));

            for (float f = 0f; f < 1f; f += 1f / 130f)
            {
                yield return playerController.ForceLookPlayer(face, 0);
            }
            yield return dialogueManager.StartDialogue(Dialogue.Cutscene02FaceDialogue());

            StartCoroutine(Things.PosSLerp(face, faceLSidePosition.position, 120));
            StartCoroutine(Things.PosRotSLerp(handL, handIdlePos1, 20));
            StartCoroutine(Things.PosRotSLerp(handR, handIdlePos2, 20));
            yield return playerController.ForceLookPlayer(playerGameLookPosition, 30);
            //Round 1: apples, bananas - how many
            do
            {
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02RoundXReady(1));

                bool apples = Random.value < 0.5f;
                int fruitCount = Random.Range(8, 14);
                int appleCount = 0;
                int bananaCount = 0;

                for (int i = 0; i < fruitCount; i++)
                {
                    yield return RandomHandPosition();

                    if (Random.value < 0.6f)
                    {
                        appleCount++;
                        InstantiateThrowObject(apple, 3f);
                    }
                    else
                    {
                        bananaCount++;
                        InstantiateThrowObject(banana, 3f);
                    }
                }

                yield return new WaitForSeconds(3f);
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02Round1(apples, apples ? appleCount : bananaCount));
            } while (dialogueManager.ending == "Wrong");

            //Round 2: square, circle, triangle - more or less
            do
            {
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02RoundXReady(2));

                int typeA = Random.Range(0, 3);     //0:sq, 1:ci; 2:tr
                int typeB = Random.Range(0, 3);
                typeB = typeA != typeB ? typeB : (typeB + 1) % 3;

                int shapeCount = Random.Range(8, 14);

                int[] counts = new int[] { 0, 0, 0 };
                GameObject[] objects = new GameObject[] { square, circle, triangle };

                for (int i = 0; i < shapeCount; i++)
                {
                    yield return RandomHandPosition();

                    int randomObjectValue = Random.Range(0, 3);
                    InstantiateThrowObject(objects[randomObjectValue], 1f);
                    counts[randomObjectValue]++;
                }

                if (counts[typeA] == counts[typeB])
                {
                    yield return RandomHandPosition();

                    InstantiateThrowObject(objects[typeA], 1f);
                    counts[typeA]++;
                }

                yield return new WaitForSeconds(3f);
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02Round2(NumToStringShapes(typeA), NumToStringShapes(typeB), counts[typeA] > counts[typeB]));

            } while (dialogueManager.ending == "Wrong");

            //Round 3: cube, sphere, cylinder, square, circle, triangle - were there any circles/squares
            do
            {
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02RoundXReady(3));

                bool squares = Random.value < 0.5f;
                bool hasOne = Random.value < 0.5f;

                int shapeCount = Random.Range(12, 18);
                int whenMaybeObject = hasOne ? Random.Range(0, shapeCount) : -1;

                GameObject[] objects = new GameObject[] { squares ? circle : square, triangle, cube, sphere, cylinder };
                GameObject maybeObject = squares ? square : circle;

                for (int i = 0; i < shapeCount; i++)
                {
                    yield return RandomHandPosition();

                    int randomObjectValue = Random.Range(0, objects.Length);
                    InstantiateThrowObject(i == whenMaybeObject ? maybeObject : objects[randomObjectValue], 1f);
                }

                yield return new WaitForSeconds(3f);
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02Round3(squares ? "squares" : "circles", hasOne));

            } while (dialogueManager.ending == "Wrong");

            //Round 4.1: everything normal, one child, blood - whoops, did you see anything weird
            {
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02RoundXReady(4));

                int shapeCount = Random.Range(12, 18);
                int whenChild = Random.Range(11, shapeCount);

                GameObject[] objects = new GameObject[] { apple, banana, circle, square, triangle, cube, sphere, cylinder };

                for (int i = 0; i < shapeCount; i++)
                {
                    yield return RandomHandPosition();

                    int randomObjectValue = Random.Range(0, objects.Length);
                    if (i == whenChild)
                    {
                        InstantiateThrowObject(child, 1.3f);
                        InstantiateThrowObject(blood, 1f);
                        break;
                    }
                    else
                    {
                        InstantiateThrowObject(objects[randomObjectValue], randomObjectValue < 2 ? 3f : 1f);
                    }
                }

                StartCoroutine(ChangeAmbientNoise(bass02));
                faceAnim.SetTrigger("Eyes01");
                camEffect.effectStep = 1;
                yield return new WaitForSeconds(3f);
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02Round4Whoops());

            }

            //Round 4.2: everything normal - how many or more or less or were there any circles
            do
            {
                yield return dialogueManager.StartDialogue(Dialogue.Cutscene02RoundXReady(4));

                int whichGame = Random.Range(0, 3);

                int itemCount = Random.Range(8, 14);
                GameObject[] objects = new GameObject[] { apple, banana, circle, square, triangle, cube, sphere, cylinder };
                int[] counts = new int[objects.Length];

                if (whichGame == 0) //How many
                {
                    int type = Random.Range(0, objects.Length);

                    for (int i = 0; i < itemCount; i++)
                    {
                        yield return RandomHandPosition();

                        int randomObjectValue = Random.Range(0, objects.Length);
                        InstantiateThrowObject(objects[randomObjectValue], 1f);
                        counts[randomObjectValue]++;
                    }

                    yield return new WaitForSeconds(3f);
                    yield return dialogueManager.StartDialogue(Dialogue.Cutscene02Round4A(NumToStringAll(type), counts[type]));
                }
                else if (whichGame == 1)    //More or less
                {
                    int typeA = Random.Range(0, objects.Length);
                    int typeB = Random.Range(0, objects.Length);
                    typeB = typeA != typeB ? typeB : (typeB + 1) % objects.Length;

                    for (int i = 0; i < itemCount; i++)
                    {
                        yield return RandomHandPosition();

                        int randomObjectValue = Random.Range(0, objects.Length);
                        InstantiateThrowObject(objects[randomObjectValue], 1f);
                        counts[randomObjectValue]++;
                    }

                    if (counts[typeA] == counts[typeB])
                    {
                        yield return RandomHandPosition();

                        InstantiateThrowObject(objects[typeA], 1f);
                        counts[typeA]++;
                    }

                    yield return new WaitForSeconds(3f);
                    yield return dialogueManager.StartDialogue(Dialogue.Cutscene02Round4B(NumToStringAll(typeA), NumToStringAll(typeB), counts[typeA] > counts[typeB]));
                }
                else    //Were there any
                {
                    bool hasOne = Random.value < 0.5f;
                    int maybeObject = Random.Range(0, objects.Length);
                    bool maybeObjectInstantiated = false;

                    for (int i = 0; i < itemCount; i++)
                    {
                        yield return RandomHandPosition();

                        int randomObjectValue = Random.Range(0, objects.Length);

                        if (randomObjectValue == maybeObject)
                        {
                            if (hasOne && !maybeObjectInstantiated)
                            {
                                InstantiateThrowObject(objects[randomObjectValue], 1f);
                                maybeObjectInstantiated = true;
                            }
                            else
                            {
                                InstantiateThrowObject(objects[(randomObjectValue + Random.Range(0, objects.Length - 1)) % objects.Length], 1f);
                            }
                        }
                        else
                            InstantiateThrowObject(objects[randomObjectValue], 1f);
                    }

                    yield return new WaitForSeconds(3f);
                    yield return dialogueManager.StartDialogue(Dialogue.Cutscene02Round4C(NumToStringAll(maybeObject), hasOne));
                }

            } while (dialogueManager.ending == "Wrong");

            //Round 5: blood, children - how many children did i kill for this blood
            yield return dialogueManager.StartDialogue(Dialogue.Cutscene02RoundXReady(5));

            for (int i = 0; i < Random.Range(4, 7); i++)
            {
                yield return RandomHandPosition();

                InstantiateThrowObject(child, Random.Range(0.8f, 1.5f));
                InstantiateThrowObject(blood, Random.Range(0.8f, 1.2f));
            }

            StartCoroutine(ChangeAmbientNoise(bass03));
            faceAnim.SetTrigger("Eyes02");
            camEffect.effectStep = 2;
            yield return new WaitForSeconds(3f);
            StartCoroutine(Things.PosSLerp(face, faceIdlePosition.position, 120));
            StartCoroutine(Things.PosRotSLerp(handL, handIdlePos1, 120));
            StartCoroutine(Things.PosRotSLerp(handR, handIdlePos2, 120));

            yield return dialogueManager.StartDialogue(Dialogue.Cutscene02Round5());
            StartCoroutine(Things.PosSLerp(handL, handWavePos.position, 20));

            yield return Wave();

            StartCoroutine(ChangeAmbientNoise(null));
            camEffect.effectStep = 0;
            StartCoroutine(Things.PosSLerp(handL, faceByePosition.position, 20));
            StartCoroutine(Things.PosSLerp(handR, faceByePosition.position, 20));
            yield return Things.PosSLerp(face, faceByePosition.position, 20);
            face.gameObject.SetActive(false);
            handL.gameObject.SetActive(false);
            handR.gameObject.SetActive(false);

            playerController.UnRide();
        }

        IEnumerator Wave()
        {
            handWaveAnim.SetBool("Wave", true);
            yield return new WaitForSeconds(3f);
            StartCoroutine(Book());

            yield return new WaitForSeconds(0.5f);
            handWaveAnim.SetBool("Wave", false);
        }

        IEnumerator Book()
        {
            bookSafety.SetActive(true);
            Transform childFloor = Instantiate(child, bookEndPos.position, Quaternion.Euler(22, 222, 22), book).transform;
            Instantiate(blood, handL.position, Quaternion.Euler(Random.insideUnitSphere * 360), null);

            book.gameObject.SetActive(true);
            book.position = handL.position;

            StartCoroutine(PosRotLerpBook(book, bookEndPos.position, bookEndPos.eulerAngles, 60));

            yield return new WaitForSeconds(0.8f);
            childFloor.SetParent(null);
            bookSafety.SetActive(false);
        }

        IEnumerator PosRotLerpBook(Transform obj, Vector3 toPos, Vector3 toRot, int lengthInFrames)
        {
            Vector3 oldPos = obj.position;
            Quaternion oldRot = obj.rotation;

            float fS;
            for (float f = 0; f < 1f; f += 1f / lengthInFrames)
            {
                fS = LerpHFunc(f);
                obj.position = Vector3.Lerp(oldPos, toPos, fS);
                obj.rotation = Quaternion.Lerp(oldRot, Quaternion.Euler(toRot), fS);
                yield return new WaitForSeconds(1f / 60f);
            }

            obj.position = toPos;
            obj.eulerAngles = toRot;
        }

        IEnumerator ChangeAmbientNoise(AudioClip clipNew)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / 1f;
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();
            audioSource.clip = clipNew;
            if(audioSource.clip != null)
                audioSource.Play();

            while (audioSource.volume < startVolume)
            {
                audioSource.volume += startVolume * Time.deltaTime / 1f;
                yield return null;
            }

            audioSource.volume = startVolume;
        }

        string NumToStringShapes(int shape)
        {
            switch (shape)
            {
                case 0:
                    return "squares";
                case 1:
                    return "circles";
                case 2:
                    return "triangles";
                default:
                    return "";
            }
        }

        string NumToStringAll(int item)
        {
            switch (item)
            {
                case 0:
                    return "apples";
                case 1:
                    return "bananas";
                case 2:
                    return "circles";
                case 3:
                    return "squares";
                case 4:
                    return "triangles";
                case 5:
                    return "cubes";
                case 6:
                    return "spheres";
                case 7:
                    return "cylinders";
                default:
                    return "";
            }
        }

        IEnumerator RandomHandPosition()
        {
            float randHandPos = Random.Range(-handRange, handRange);
            StartCoroutine(LHandPosDelay(2.3f, handBotPos.position + transform.right * randHandPos, new Vector3(-90, 0, 0), 30));
            yield return PosRotLerpH(handR, handTopPos.position + transform.right * randHandPos, new Vector3(90, 0, 0), 30);
        }

        void InstantiateThrowObject(GameObject throwObject, float scale)
        {
            GameObject thrownObject = Instantiate(throwObject, handR.position, Quaternion.Euler(Random.insideUnitSphere * 360), thrownObjects);

            thrownObject.layer = thrownObjects.gameObject.layer;
            foreach (Transform child in thrownObject.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = thrownObjects.gameObject.layer;
            }
            thrownObject.AddComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 100f);
            thrownObject.AddComponent<ThrowObject>().scaleValue = scale;
        }

        IEnumerator LHandPosDelay(float delay, Vector3 toPos, Vector3 toRot, int lengthInFrames)
        {
            yield return new WaitForSeconds(delay);
            yield return PosRotLerpH(handL, toPos, toRot, lengthInFrames);
        }

        IEnumerator PosRotLerpH(Transform obj, Vector3 toPos, Vector3 toRot, int lengthInFrames)
        {
            Vector3 oldPos = obj.position;
            Quaternion oldRot = obj.rotation;

            float fS;
            for (float f = 0; f < 1f; f += 1f / lengthInFrames)
            {
                fS = LerpHFunc(f);
                obj.position = Vector3.Lerp(oldPos, toPos, fS);
                obj.rotation = Quaternion.Lerp(oldRot, Quaternion.Euler(toRot), fS);
                yield return new WaitForSeconds(1f / 60f);
            }

            obj.position = toPos;
            obj.eulerAngles = toRot;
        }

        IEnumerator TextAway()
        {
            for (float f = 1f; f > 0f; f -= 1f / 80f)
            {
                startText.color = new Color(1, 1, 1, f);
                yield return new WaitForSeconds(1f / 60f);
            }
            startText.color = new Color(1, 1, 1, 0);
        }

        float LerpHFunc(float val)
        {
            return (-1 * Mathf.Pow(val - 1, 2) + 1);
        }

        bool Rideable.Move(MoveData inputs)
        {
            return camMove;
        }

        void Pausing.Pause() { }

        void Pausing.UnPause() { }

        void OnTriggerEnter(Collider other)
        {
            if (!triggered && other.CompareTag("Player"))
            {
                triggered = true;
                StartCoroutine(FaceAnim01());
            }
        }
    }
}