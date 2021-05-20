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
        [SerializeField] KeyConfigData keyConfig;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI remainderText;
        [SerializeField] private TextMeshProUGUI eliminatedText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Transform youWinScreen;
        [SerializeField] private TextMeshProUGUI youWinText;
        [SerializeField] private Transform gameOverScreen;
        [SerializeField] private TextMeshProUGUI gameOverText;
        [SerializeField] private Transform enemyContainer;
        [SerializeField] private GameObject PauseMenu;

        private const int maxLevel = 5;

        private GameObject player;
        private int level;
        private int[] enemiesEachLvl;
        private int score;
        private bool EndOfGame;

        private int enemiesEliminated;
        private int enemiesRemain;

        private bool startingNextLevel;

        public int Score => score;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
            level = 1;
            enemiesEachLvl = new int[maxLevel];
            for (int i=0; i<maxLevel; i++)
                enemiesEachLvl[i] = 10 + (5*i);
            score = 0;
            EndOfGame = false;
            youWinScreen.gameObject.SetActive(false);
            gameOverScreen.gameObject.SetActive(false);
            enemiesEliminated = 0;
            enemiesRemain = 0;
            startingNextLevel = false;
            startLevel();
        }

        void Update()
        {
            levelText.text = "Level " + level;
            remainderText.text = "Remainder: " + enemiesRemain;
            eliminatedText.text = "Eliminated: " + enemiesEliminated;
            scoreText.text = "Score: " + score;

            if (EndOfGame) return;

            if (Input.GetKeyDown(keyConfig.Pause))
            {
                Time.timeScale = 0;
                player.transform.GetChild(0).GetComponent<PlayerControl>().GamePaused = true;
                PauseMenu.SetActive(true);
            }

            if (player.transform.GetChild(0).GetComponent<PlayerStats>().IsDead())
            {
                EndOfGame = true;
                gameOverScreen.gameObject.SetActive(true);
                StartCoroutine(finishGame(false));
            }

            if (enemyContainer.transform.childCount == 0 && !startingNextLevel)
            {
                if (level == maxLevel)
                {
                    EndOfGame = true;
                    youWinScreen.gameObject.SetActive(true);
                    StartCoroutine(finishGame(true));
                }
                else
                {
                    startingNextLevel = true;
                    StartCoroutine(toNextLevel());
                }
            }
        }

        private void startLevel()
        {
            enemiesRemain = enemiesEachLvl[level - 1];
            enemyContainer.GetComponent<EnemyGenerator>().GenerateEnemies(enemiesEachLvl[level - 1]);
        }

        public void EnemyKilled()
        {
            enemiesEliminated++;
            enemiesRemain--;
            AddScore(100);
        }

        public void AddScore(int score)
        {
            this.score += score;
        }

        IEnumerator toNextLevel()
        {
            enemyContainer.GetComponent<EnemyGenerator>().ClearEnemies();
            yield return new WaitForSeconds(3.0f);
            level++;
            startLevel();
            yield return new WaitForSeconds(2.0f);
            startingNextLevel = false;
        }

        IEnumerator finishGame(bool win)
        {
            Color color = Color.white;
            color.a = 0.0f;
            while (color.a < 1.0f)
            {
                color.a = Mathf.Min(color.a + 0.02f, 1.0f);
                if (win)
                {
                    youWinText.color = color;
                    youWinScreen.GetComponent<Image>().color = color;
                }
                else
                {
                    gameOverText.color = color;
                    gameOverScreen.GetComponent<Image>().color = color;
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("TitleScene");
        }
    }
}
