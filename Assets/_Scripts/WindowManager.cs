using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �����, ������������� �� ������������ ����
/// </summary>
public class WindowManager : MonoBehaviour
{
    // ������� �������� -- � ������ ���� ������������ ������
    public static WindowManager Instance { get; private set; }

    /// <summary>
    /// ����, ������������ ��� ������
    /// </summary>
    [SerializeField] private GameObject winWindow;
    
    /// <summary>
    /// ����, ������������ ��� ���������
    /// </summary>
    [SerializeField] private GameObject looseWindow;

    /// <summary>
    /// ����������, ������� ����� ��������� ��� �������� ����
    /// </summary>
    [SerializeField] private MonoBehaviour[] objectToDisable;

    private void Awake()
    {
        // ������� ��������
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
    /// ����� ��� ������ ���� ������ ����� 1 �������
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
    /// ����� ��� ������ ���� ��������� ����� 1 �������
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
    /// ����� ��� ������ ���� ������
    /// </summary>
    private void OpenWinWindow()
    {
        Time.timeScale = 0f;
        winWindow.SetActive(true);
    }

    /// <summary>
    /// ����� ��� ������ ���� ���������
    /// </summary>
    private void OpenLooseWindow()
    {
        Time.timeScale = 0f;
        looseWindow.SetActive(true);
    }


    /// <summary>
    /// ����� ������������ ������
    /// </summary>
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void OnDestroy()
    {
        // ������� ��������
        if (Instance == this)
        {
            Instance = null;
        }
    }


}
