using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace proj3ct
{
    public class LevelController : MonoBehaviour
    {
        public Sprite levelImage;
        public Animator fadeBlack;

        [Space(20)]

        public GameObject playerPrefab;
        public GameObject blockPrefab;
        public GameObject finishPrefab;
        public float spriteSize = 1.6f;

        [Space(20)]

        public Sprite spriteGrass;
        public Sprite spriteStone;
        public Sprite spriteStoneBack;
        public Sprite spriteFinish;
        public Sprite spriteWood;
        public Sprite spriteSky;
        public Sprite spriteCloud;

        Color32 colorGrass = new Color32(45, 163, 45, 255);
        Color32 colorStone = new Color32(128, 128, 128, 255);
        Color32 colorStoneBack = new Color32(255, 128, 0, 255);
        Color32 colorFinish = new Color32(255, 0, 0, 255);
        Color32 colorWood = new Color32(255, 255, 0, 255);
        Color32 colorSky = new Color32(0, 255, 255, 255);
        Color32 colorCloud = new Color32(255, 255, 255, 255);

        void Start()
        {
            CreateLevel();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Q))
            {
                StartCoroutine(LoadScene("Proj3ctMenu"));
            }
        }

        void CreateLevel()
        {
            Texture2D image = levelImage.texture;

            for (int i = 0; i < image.width; i++)
            {
                for (int j = 0; j < image.height; j++)
                {
                    Color32 col = image.GetPixel(i, j);
                    GameObject newBlock = Instantiate(blockPrefab, new Vector3(i * spriteSize, j * spriteSize, 0f), Quaternion.identity, transform);
                    SpriteRenderer newBlockSpriteRenderer = newBlock.GetComponent<SpriteRenderer>();
                    Collider2D newBlockColider = newBlock.GetComponent<Collider2D>();

                    if (similarColor(col, colorStone))
                    {
                        newBlockSpriteRenderer.sprite = spriteStone;
                    }
                    else if (similarColor(col, colorStoneBack))
                    {
                        newBlockSpriteRenderer.sprite = spriteStoneBack;
                        newBlockColider.enabled = false;
                    }
                    else if (similarColor(col, colorFinish))
                    {
                        Instantiate(finishPrefab, new Vector3(i * spriteSize, j * spriteSize, 0f), Quaternion.identity, transform);
                        newBlockSpriteRenderer.sprite = spriteFinish;
                        newBlockColider.enabled = false;
                    }
                    else if (similarColor(col, colorWood))
                    {
                        newBlockSpriteRenderer.sprite = spriteWood;
                    }
                    else if (similarColor(col, colorSky))
                    {
                        newBlockSpriteRenderer.sprite = spriteSky;
                        newBlockColider.enabled = false;
                    }
                    else if (similarColor(col, colorGrass))
                    {
                        newBlockSpriteRenderer.sprite = spriteGrass;
                    }
                    else if (similarColor(col, colorCloud))
                    {
                        newBlockSpriteRenderer.sprite = spriteCloud;
                        newBlockColider.enabled = false;
                    }
                    else
                    {
                        newBlockSpriteRenderer.sprite = spriteStone;
                    }
                }
            }

            Instantiate(playerPrefab, new Vector3(20 * spriteSize, (image.height - 10) * spriteSize, 0f), Quaternion.identity);
        }

        bool similarColor(Color color1, Color color2, float acceptance = 0.1f)
        {
            return (color1.r < color2.r + acceptance && color1.r > color2.r - acceptance) &&
                (color1.g < color2.g + acceptance && color1.g > color2.g - acceptance) &&
                (color1.b < color2.b + acceptance && color1.b > color2.b - acceptance) &&
                (color1.a < color2.a + acceptance && color1.a > color2.a - acceptance);
        }

        public void EndLevel()
        {
            StartCoroutine(LoadScene("Proj3ctMenu"));
        }

        IEnumerator LoadScene(string name)
        {
            fadeBlack.SetTrigger("Out");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(name);
        }
    }
}