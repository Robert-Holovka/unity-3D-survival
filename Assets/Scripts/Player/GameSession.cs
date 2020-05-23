using Survival.Enemy;
using Survival.Weaponry;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Survival.Player
{
    internal class GameSession : MonoBehaviour
    {
        [SerializeField] GameObject gameOverPanel = default;
        [SerializeField] GameObject tutorialPanel = default;
        [SerializeField] TextMeshProUGUI enemiesLeftText = default;
        [SerializeField] TextMeshProUGUI fpsText = default;
        [SerializeField] TextMeshProUGUI gameOverText = default;

        // Cached
        private Weapon[] weapons = default;
        private WeaponSelector weaponSelector = default;
        private PlayerHealth playerHealth = default;

        private static bool tutorialShown = false;
        private int enemiesLeft = default;
        private float timer = default;
        private int framesPerSecond = default;
        private bool isPaused = false;

        private void Awake()
        {
            weapons = FindObjectsOfType<Weapon>();
            weaponSelector = FindObjectOfType<WeaponSelector>();
            enemiesLeft = FindObjectsOfType<EnemyHealth>().Length;
            playerHealth = GetComponentInParent<PlayerHealth>();
        }

        private void Start()
        {
            InitUI();
            isPaused = !tutorialShown;
            OnGamePause(isPaused, false);
        }

        private void Update()
        {
            UpdateFPS();
            if (Input.GetKeyDown(KeyCode.Escape)
                && !tutorialPanel.activeInHierarchy
                && !playerHealth.IsDead)
            {
                isPaused = !isPaused;
                OnGamePause(isPaused, isPaused);
            }
        }

        private void InitUI()
        {
            tutorialPanel.SetActive(!tutorialShown);
            gameOverText.text = "";
            UpdateEnemiesLeftText();
        }

        private void OnGamePause(bool isPaused, bool showPauseMenu)
        {
            gameOverPanel.SetActive(showPauseMenu);

            Time.timeScale = isPaused ? 0 : 1;
            Cursor.visible = isPaused;

            foreach (Weapon weapon in weapons)
            {
                weapon.enabled = !isPaused;
            }
            weaponSelector.enabled = !isPaused;
        }

        private void UpdateFPS()
        {
            timer += Time.deltaTime;
            framesPerSecond++;
            if (timer >= 1.0f)
            {
                fpsText.text = $"FPS {Mathf.RoundToInt(framesPerSecond)}";
                timer = 0;
                framesPerSecond = 0;
            }
        }

        public void EnemyKilled()
        {
            enemiesLeft--;
            if (enemiesLeft == 0)
            {
                gameOverText.text = "Well played";
                gameOverText.color = Color.green;
                isPaused = true;
                OnGamePause(isPaused, isPaused);
            }
            UpdateEnemiesLeftText();
        }

        private void UpdateEnemiesLeftText() => enemiesLeftText.text = $"Enemies left {enemiesLeft}";

        internal void HandleDeath()
        {
            gameOverText.text = "Git Gud";
            gameOverText.color = Color.red;
            isPaused = true;
            OnGamePause(isPaused, isPaused);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void OnTutorialFinish()
        {
            isPaused = false;
            OnGamePause(isPaused, isPaused);
            tutorialPanel.SetActive(false);
            tutorialShown = true;
        }
    }
}