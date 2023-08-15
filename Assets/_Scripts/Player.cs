using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;

/// <summary>
/// Класс, описывающий поведение и анимацию игрока
/// </summary>
public class Player : MonoBehaviour
{
    // Паттерн одиночки -- у класса один единственный объект
    public static Player Instance { get; private set; }

    /// <summary>
    /// Компонент скелетной анимации у игрока
    /// </summary>
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    /// <summary>
    /// Эффект выстрела
    /// </summary>
    [SerializeField] private ParticleSystem ShootEffect;


    /// <summary>
    /// Событие смерти игрока
    /// </summary>
    [SerializeField] private UnityEvent OnDieEvent;

    /// <summary>
    /// Камера останавливается после достижения игроком этой точки по оси X
    /// </summary>
    [SerializeField] private float changeCameraPointX;
    /// <summary>
    /// Камера, не следующая за игроком
    /// </summary>
    [SerializeField] private GameObject cinemamachineIdleCamera;

    /// <summary>
    /// Событие выстрела на анимации игрока
    /// </summary>
    [SpineEvent] public string fireEventName;

    // Псоледний враг, в которого выстрелил игрок
    [HideInInspector] public Enemy EnemyToDestroy;

    // Переменная для состояния анимации
    private Spine.AnimationState _animationState;
    // Переменная для данных о событиях в анимации
    private Spine.EventData _eventData;


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

    void Start()
    {
        // Сохраняем состояние анимации
        _animationState = skeletonAnimation.AnimationState;
        // Сохраняем данные события выстрела
        _eventData = skeletonAnimation.Skeleton.Data.FindEvent(fireEventName);
        // Добавляем обработчик события анимации выстрела
        _animationState.Event += HandleAnimShootEvent;
    }

    private void Update()
    {
        if (transform.position.x >= changeCameraPointX)
        {
            cinemamachineIdleCamera.SetActive(true);
        }
    }

    // При столкновении с триггером-врагом проигрываем, с победным триггером -- побеждаем
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Loose();
        }
        else if (collision.CompareTag("Win"))
        {
            Win();
        }
    }

    /// <summary>
    /// Метод выстрела игрока
    /// </summary>
    public void Shoot()
    {
        PlayOneAnim("shoot");
        WalkAnim();
    }

    /// <summary>
    /// Метод ошибочного выстрела игрока
    /// </summary>
    public void WrongShoot()
    {
        PlayOneAnim("shoot_fail");
        WalkAnim();
    }

    // Метод для проигрывания анимации ходьбы на 1 слое
    private void WalkAnim()
    {
        _animationState.AddAnimation(1, "walk", true, 0);
    }

    // Метод для поигрывания одной анимации на 0 слое
    private void PlayOneAnim(string animationName)
    {
        // Добавляем пустые анимации, тк на 0 слое нет других анимаций
        _animationState.AddEmptyAnimation(0, 0.2f, 0);
        _animationState.SetAnimation(0, animationName, false);
        _animationState.AddEmptyAnimation(0, 0.2f, 0);
    }

    // Метод поражения игрока
    private void Loose()
    {
        OnDieEvent.Invoke();
        PlayOneAnim("loose");

        WindowManager.Instance.Loose();
    }

    // Метод победы игрока
    private void Win()
    {
        _animationState.SetAnimation(1, "idle", false);
        WindowManager.Instance.Win();
    }

    private void OnDestroy()
    {
        // паттерн одиночки
        if (Instance == this)
        {
            Instance = null;
        }
    }


    // Обработчик события выстрела
    private void HandleAnimShootEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data == _eventData)
        {
            ShootEffect.gameObject.SetActive(true);
            ShootEffect.Play();
            // Если существует враг, в которого выстрелили, уничтожаем его
            // (чтобы враги уничтожались во время выстрела, а не по клику)
            if (EnemyToDestroy)
            {
                EnemyToDestroy.Die();
                EnemyToDestroy = null;
            }
        }
    }
}
