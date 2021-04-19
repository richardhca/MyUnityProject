using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Player.Config;

namespace GameCore.GameMenu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Transform options;

        private KeyCode UP = KeyCode.UpArrow;
        private KeyCode DOWN = KeyCode.DownArrow;
        private KeyCode SPACE = KeyCode.Space;
        private KeyCode ESC = KeyCode.Escape;

        private int currentSelectedIndex;

        private GameObject player;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
            currentSelectedIndex = 0;
            options.GetChild(currentSelectedIndex).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }

        void Update()
        {
            if (Input.GetKeyDown(UP))
            {
                var currentOption = options.GetChild(currentSelectedIndex);
                currentSelectedIndex = (currentSelectedIndex == 0) ? options.childCount - 1 : currentSelectedIndex - 1;
                currentOption.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                options.GetChild(currentSelectedIndex).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            }
            else if (Input.GetKeyDown(DOWN))
            {
                var currentOption = options.GetChild(currentSelectedIndex);
                currentSelectedIndex = (currentSelectedIndex == options.childCount - 1) ? 0 : currentSelectedIndex + 1;
                currentOption.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                options.GetChild(currentSelectedIndex).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            }
            else if (Input.GetKeyDown(SPACE))
            {
                Time.timeScale = 1;
                player.transform.GetChild(0).GetComponent<PlayerControl>().GamePaused = false;
                switch (options.GetChild(currentSelectedIndex).name)
                {
                    case "Resume":
                        gameObject.SetActive(false);
                        break;
                    case "Restart":
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        break;
                    case "Exit":
                        SceneManager.LoadScene("TitleScene");
                        break;
                }
            }
            else if (Input.GetKeyDown(ESC))
            {
                var currentOption = options.GetChild(currentSelectedIndex);
                currentSelectedIndex = 0;
                currentOption.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                options.GetChild(currentSelectedIndex).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
                Time.timeScale = 1;
                player.transform.GetChild(0).GetComponent<PlayerControl>().GamePaused = false;
                gameObject.SetActive(false);
            }
        }
    }
}
