using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс объектов, при нажатии на которые игрок сделает ошибочный выстрел
/// </summary>
public class WrongShootChoice : MonoBehaviour
{
    private void OnMouseDown()
    {
        Player.Instance.WrongShoot();
    }

}
