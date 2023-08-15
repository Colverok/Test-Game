using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ��� ��������, ����������� � ����� �� ��� X
/// </summary>
public class MoveToPointX : MonoBehaviour
{
    // �������� ��������
    [SerializeField] private float speed = 1f;

    // �������� ����� �� ��� X
    [SerializeField] private float endXPoint = 0;

    // ������� �������
    private float timer;

    // ��������� ����� ��������
    private float time;

    private Vector2 startPosition;
    void Start()
    {      
        startPosition = transform.position;
        StartCoroutine(MoveToPoint());
    }

    /// <summary>
    /// �������� ��� ����������� �������
    /// </summary>
    private IEnumerator MoveToPoint()
    {
        timer = 0;
        // ����� �������� ������������ � ������ �������� � ����������
        time = Mathf.Abs(startPosition.x - endXPoint) / speed;
        while (timer < time)
        {
            transform.position = Vector2.Lerp(startPosition, new Vector2(endXPoint, transform.position.y), timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        // �� ��������� ���� ������������ ������� � ��������, �� Lerp �� ������ �������� ������� �� ��������� �����
        transform.position = new Vector2(endXPoint, transform.position.y);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


}
