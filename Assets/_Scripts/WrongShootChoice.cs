using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ��������, ��� ������� �� ������� ����� ������� ��������� �������
/// </summary>
public class WrongShootChoice : MonoBehaviour
{
    private void OnMouseDown()
    {
        Player.Instance.WrongShoot();
    }

}
