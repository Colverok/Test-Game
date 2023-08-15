using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Класс, ответственный за переключение окон
/// </summary>
public class WindowManager : MonoBehaviour
{
    // Паттерн одиночки -- у класса один единственный объект
    public static WindowManager Instance { get; private set; }

    /// <summary>
    /// Окно, вызывающееся при победе
    /// </summary>
    [SerializeField] private GameObject winWindow;
    
    /// <summary>
    /// Окно, вызывающееся при поражении
    /// </summary>
    [SerializeField] private GameObject looseWindow;

    /// <summary>
    /// Компоненты, которые нужно отключить при открытии окон
    /// </summary>
    [SerializeField] private MonoBehaviour[] objectToDisable;

    private void Awake()
    {
        // паттерн одиночки
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Метод для вызова окна победы через 1 секунду
    /// </summary>
    public void Win()
    {
        Invoke(nameof(OpenWinWindow), 1f);
        for (int i = 0; i < objectToDisable.Length; i++)
        {
            if (objectToDisable[i])
            {
                objectToDisable[i].enabled = false;
            }
        }
    }


    /// <summary>
    /// Метод для вызова окна поражения через 1 секунду
    /// </summary>
    public void Loose()
    {
        Invoke(nameof(OpenLooseWindow), 1f);

        for (int i = 0; i < objectToDisable.Length; i++)
        {
            if (objectToDisable[i])
            {
                objectToDisable[i].enabled = false;
            }
        }
    }


    /// <summary>
    /// Метод для вызова окна победы
    /// </summary>
    private void OpenWinWindow()
    {
        Time.timeScale = 0f;
        winWindow.SetActive(true);
    }

    /// <summary>
    /// Метод для вызова окна поражения
    /// </summary>
    private void OpenLooseWindow()
    {
        Time.timeScale = 0f;
        looseWindow.SetActive(true);
    }


    /// <summary>
    /// Метод перезагрузки уровня
    /// </summary>
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void OnDestroy()
    {
        // паттерн одиночки
        if (Instance == this)
        {
            Instance = null;
        }
    }


}
