using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.GameMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject keyConfig;

        private KeyCode UP = KeyCode.UpArrow;
        private KeyCode DOWN = KeyCode.DownArrow;
        private KeyCode SPACE = KeyCode.Space;

        private int currentSelectedIndex;
        private int newSelectedIndex;

        void Start()
        {
            keyConfig.SetActive(false);
            currentSelectedIndex = 0;
            newSelectedIndex = 0;
            selectNewIndex(true);
        }

        void Update()
        {
            if (Input.GetKeyDown(UP))
            {
                newSelectedIndex = Math.Max(currentSelectedIndex - 1, 0);
                selectNewIndex(false);
            }

            if (Input.GetKeyDown(DOWN))
            {
                newSelectedIndex = Math.Min(currentSelectedIndex + 1, transform.childCount - 1);
                selectNewIndex(false);
            }

            if (Input.GetKeyDown(SPACE))
            {
                var option = transform.GetChild(currentSelectedIndex);
                switch (option.GetComponent<TextMeshProUGUI>().text)
                {
                    case "PLAY":
                        SceneManager.LoadScene("GameScene");
                        break;
                    case "KEY CONFIG":
                        keyConfig.SetActive(true);
                        gameObject.SetActive(false);
                        break;
                    case "QUIT":
                        //if (Application.isEditor)
                        //    UnityEditor.EditorApplication.isPlaying = false;
                        //else
                            Application.Quit();
                        break;
                }
            }
        }

        private void selectNewIndex(bool initialize)
        {
            if (currentSelectedIndex != newSelectedIndex || initialize)
            {
                if (!initialize)
                {
                    var prev = transform.GetChild(currentSelectedIndex).GetComponent<TextMeshProUGUI>();
                    prev.fontSize = 74;
                }
                var curr = transform.GetChild(newSelectedIndex).GetComponent<TextMeshProUGUI>();
                curr.fontSize = 90;
                currentSelectedIndex = newSelectedIndex;
            }
        }
    }
}
