using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using Player.Config;

namespace GameCore.Gameplay {
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform gameOverScreen;
        [SerializeField] private TextMeshProUGUI gameOverText;

        private GameObject player;
        private int score;
        private bool EndOfGame;

        public int Score => score;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
            score = 0;
            EndOfGame = false;
            gameOverScreen.gameObject.SetActive(false);
        }

        void Update()
        {
            if (!EndOfGame && player.transform.GetChild(0).GetComponent<PlayerStats>().IsDead())
            {
                EndOfGame = true;
                gameOverScreen.gameObject.SetActive(true);
                StartCoroutine(finishGame());
            }
        }

        public void AddScore(int score)
        {
            this.score += score;
        }

        IEnumerator finishGame()
        {
            Color backgroundColor = gameOverScreen.GetComponent<Image>().color;
            Color textColor = gameOverText.color;
            while (backgroundColor.a < 1.0f || textColor.a < 1.0f)
            {
                backgroundColor.a = Mathf.Min(backgroundColor.a + 0.02f, 1.0f);
                textColor.a = Mathf.Min(textColor.a + 0.02f, 1.0f);
                gameOverText.color = textColor;
                gameOverScreen.GetComponent<Image>().color = backgroundColor;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("TitleScene");
        }
    }
}
