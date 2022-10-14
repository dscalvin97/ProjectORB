using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuBehavior : MonoBehaviour
{
    private List<GameObject> _menus = new List<GameObject>();

    public GameObject _BackgroundMusic;
    public GameObject _PlayButton, _AboutBackButton;

    private void Start()
    {
        BackgroundMusic bgm = FindObjectOfType<BackgroundMusic>();
        if (bgm == null)
            Instantiate(_BackgroundMusic);

        for (int i = 0; i < transform.childCount; i++)
            _menus.Add(transform.GetChild(i).gameObject);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_PlayButton);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SwitchMenu(int switchToMenu)
    {
        for (var i = 0; i < _menus.Count; i++)
        {
            _menus[i].SetActive(i == switchToMenu);
        }
        switch (switchToMenu)
        {
            case 0:
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(_PlayButton);
                }
                break;
            case 1:
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(_AboutBackButton);
                }
                break;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
