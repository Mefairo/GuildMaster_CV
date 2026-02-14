using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image _panelMenu;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private List<GameObject> _gameObjectsOpen;

    private void Start()
    {
        _panelMenu.gameObject.SetActive(true);

        _playButton?.onClick.RemoveAllListeners();
        _exitButton?.onClick.RemoveAllListeners();

        _playButton?.onClick.AddListener(PlayGame);
        _exitButton?.onClick.AddListener(ExitGame);

        _volumeSlider.maxValue = 1;
        _volumeSlider.value = _audioManager.TargetVolume;

        _volumeSlider.onValueChanged.AddListener(HandleVolumeChange);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_gameObjectsOpen.Any(obj => obj.activeInHierarchy) && !_panelMenu.gameObject.activeInHierarchy)
            {
                _panelMenu.gameObject.SetActive(true);
            }

            else
            {
                _panelMenu.gameObject.SetActive(false);
            }
        }
    }

    private void PlayGame()
    {
        _panelMenu.gameObject.SetActive(false);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void HandleVolumeChange(float newVolume)
    {
        // Передаём новое значение громкости в AudioManager
        _audioManager.SetVolume(newVolume);
    }
}
