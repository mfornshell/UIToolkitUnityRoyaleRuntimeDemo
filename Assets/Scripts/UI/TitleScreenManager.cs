using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityRoyale
{
    public class TitleScreenManager : MonoBehaviour
    {
        static string m_SceneName => "Main";

        [SerializeField] TitleController _title;
        [SerializeField] OptionsController _options;
        [SerializeField] AboutController _about;

        private void Start()
        {
            _title.StartPressed += StartGame;
            _title.OptionsPressed += EnableOptionsScreen;
            _title.AboutPressed += EnableAboutScreen;

            _options.BackPressed += EnableTitleScreen;
            _about.BackPressed += EnableTitleScreen;

            _title.Show();
        }

        void EnableTitleScreen() => _title.Show();
        void EnableOptionsScreen() => _options.Show();
        void EnableAboutScreen() => _about.Show();

        void StartGame() => SceneManager.LoadSceneAsync(m_SceneName);
    }
}
