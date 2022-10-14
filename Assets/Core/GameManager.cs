using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public List<ObjectPool> pools = new List<ObjectPool>();
    public GameControls _GameControls;
    public UpgradeMenuBehaviour _UpgradeMenu;
    public GameObject _PauseMenu;
    public GameObject _GameEndMenu;
    public float _GameTime;
    public TextMeshProUGUI _CurrentLevelText;
    public TextMeshProUGUI _MoveSpeedText;
    public TextMeshProUGUI _FireSpeedText;
    public TextMeshProUGUI _AttackDamageText;
    public TextMeshProUGUI _TimeLastedText;
    public Slider _HealthBar;
    public Slider _XPBar;

    public GameObject _PauseMenuResumeButton, _UpgradeMenuChoice1Button, _DeathMenuBackButton;

    private void Awake()
    {
        _GameControls = new GameControls();
        _GameControls.Gameplay.Enable();
    }

    void Start()
    {
        for (int i = 0; i < pools.Count; i++)
            pools[i].InitializePool(transform);
    }

    private void FixedUpdate()
    {
        _GameTime += Time.deltaTime;
    }

    public void ResumeGame()
    {
        if (_PauseMenu.activeSelf)
            _PauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
        SetUIState(false);
    }

    public void PauseGame(bool showPauseMenu)
    {
        if (showPauseMenu)
        {
            _PauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_PauseMenuResumeButton);
        }
        AudioListener.pause = true;
        SetUIState(true);
        Time.timeScale = 0;
    }

    public void SetUIState(bool setUIActive)
    {
        if (setUIActive)
        {
            _GameControls.UI.Enable();
            _GameControls.Gameplay.Disable();
        }
        else
        {
            _GameControls.UI.Disable();
            _GameControls.Gameplay.Enable();
        }
    }

    public void ShowUpgradeChoices()
    {
        _UpgradeMenu.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_UpgradeMenuChoice1Button);
        _UpgradeMenu.SetRandomUpgradeChoices();
        PauseGame(false);
    }

    public void GotoMainMenu()
    {
        ResumeGame();

        SceneManager.LoadScene(0);
    }

    internal void ShowGameEndScreen()
    {
        _GameEndMenu.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_DeathMenuBackButton);
        _TimeLastedText.text = "You Survived for\n" + _GameTime.ToString("0.00") + " seconds!";
        PauseGame(false);
    }
}
