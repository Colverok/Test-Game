using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для объектов, двигающихся к точке по оси X
/// </summary>
public class MoveToPointX : MonoBehaviour
{
    // Скорость движения
    [SerializeField] private float speed = 1f;

    // Конечная точка по оси X
    [SerializeField] private float endXPoint = 0;

    // Счетчик времени
    private float timer;

    // Суммарное время движения
    private float time;

    private Vector2 startPosition;
    void Start()
    {      
        startPosition = transform.position;
        StartCoroutine(MoveToPoint());
    }

    /// <summary>
    /// Корутина для перемещения объекта
    /// </summary>
    private IEnumerator MoveToPoint()
    {
        timer = 0;
        // Время движения подсчитываем с учетом скорости и расстояния
        time = Mathf.Abs(startPosition.x - endXPoint) / speed;
        while (timer < time)
        {
            transform.position = Vector2.Lerp(startPosition, new Vector2(endXPoint, transform.position.y), timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        // На последнем шаге приравниваем позицию к конечной, тк Lerp не всегда успевает довести до последней точки
        transform.position = new Vector2(endXPoint, transform.position.y);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


}
