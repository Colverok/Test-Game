using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;

/// <summary>
/// Класс, описывающий поведение и анимацию врага
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Компонент скелетной анимации у игрока
    /// </summary>
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    /// <summary>
    /// Эффект взрыва
    /// </summary>
    [SerializeField] private ParticleSystem explosionEffect;
    /// <summary>
    /// Событие остановки врага
    /// </summary>
    [SerializeField] private UnityEvent onStopEvent;


    
    // Объект игрока
    private GameObject player;
    // Состояние анимации
    private Spine.AnimationState _animationState;

    void Start()
    {
        player = Player.Instance.gameObject;
        _animationState = skeletonAnimation.AnimationState;

        // Если враг со скриптом движения, добавляем остановку это скрипта в событие остановки врага 
        if (GetComponent<MoveToPointX>() is MoveToPointX script)
        {
            onStopEvent.AddListener(() => script.enabled = false); 
        }
    }

    private void OnMouseDown()
    {
        // При нажатии на врага игрок выстреливает
        Player.Instance.Shoot();
        // Коллайдер отключаем, чтобы не было поражения
        GetComponent<Collider2D>().enabled = false;
        // Этого врага нужно уничтожить во время события выстрела
        Player.Instance.EnemyToDestroy = this;
    }

    /// <summary>
    /// Метод для уничтожения врага
    /// </summary>
    public void Die()
    {
        explosionEffect.gameObject.SetActive(true);
        explosionEffect.transform.parent = null;
        explosionEffect.Play();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Когда враг сталкивается с игроком, он останавливается и побеждает
        onStopEvent.Invoke();
        WinAnimate();
    }

    // Метод для анимации победы врага над игроком
    private void WinAnimate()
    {
        PlayOneAnim("win");
    }

    // Метод для проигрывания 1 анимации на 0 слое
    private void PlayOneAnim(string animationName)
    {
        _animationState.SetAnimation(0, animationName, false);
    }

}
