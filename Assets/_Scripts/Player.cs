using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;

/// <summary>
/// �����, ����������� ��������� � �������� ������
/// </summary>
public class Player : MonoBehaviour
{
    // ������� �������� -- � ������ ���� ������������ ������
    public static Player Instance { get; private set; }

    /// <summary>
    /// ��������� ��������� �������� � ������
    /// </summary>
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    /// <summary>
    /// ������ ��������
    /// </summary>
    [SerializeField] private ParticleSystem ShootEffect;


    /// <summary>
    /// ������� ������ ������
    /// </summary>
    [SerializeField] private UnityEvent OnDieEvent;

    /// <summary>
    /// ������ ��������������� ����� ���������� ������� ���� ����� �� ��� X
    /// </summary>
    [SerializeField] private float changeCameraPointX;
    /// <summary>
    /// ������, �� ��������� �� �������
    /// </summary>
    [SerializeField] private GameObject cinemamachineIdleCamera;

    /// <summary>
    /// ������� �������� �� �������� ������
    /// </summary>
    [SpineEvent] public string fireEventName;

    // ��������� ����, � �������� ��������� �����
    [HideInInspector] public Enemy EnemyToDestroy;

    // ���������� ��� ��������� ��������
    private Spine.AnimationState _animationState;
    // ���������� ��� ������ � �������� � ��������
    private Spine.EventData _eventData;


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

    void Start()
    {
        // ��������� ��������� ��������
        _animationState = skeletonAnimation.AnimationState;
        // ��������� ������ ������� ��������
        _eventData = skeletonAnimation.Skeleton.Data.FindEvent(fireEventName);
        // ��������� ���������� ������� �������� ��������
        _animationState.Event += HandleAnimShootEvent;
    }

    private void Update()
    {
        if (transform.position.x >= changeCameraPointX)
        {
            cinemamachineIdleCamera.SetActive(true);
        }
    }

    // ��� ������������ � ���������-������ �����������, � �������� ��������� -- ���������
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
    /// ����� �������� ������
    /// </summary>
    public void Shoot()
    {
        PlayOneAnim("shoot");
        WalkAnim();
    }

    /// <summary>
    /// ����� ���������� �������� ������
    /// </summary>
    public void WrongShoot()
    {
        PlayOneAnim("shoot_fail");
        WalkAnim();
    }

    // ����� ��� ������������ �������� ������ �� 1 ����
    private void WalkAnim()
    {
        _animationState.AddAnimation(1, "walk", true, 0);
    }

    // ����� ��� ����������� ����� �������� �� 0 ����
    private void PlayOneAnim(string animationName)
    {
        // ��������� ������ ��������, �� �� 0 ���� ��� ������ ��������
        _animationState.AddEmptyAnimation(0, 0.2f, 0);
        _animationState.SetAnimation(0, animationName, false);
        _animationState.AddEmptyAnimation(0, 0.2f, 0);
    }

    // ����� ��������� ������
    private void Loose()
    {
        OnDieEvent.Invoke();
        PlayOneAnim("loose");

        WindowManager.Instance.Loose();
    }

    // ����� ������ ������
    private void Win()
    {
        _animationState.SetAnimation(1, "idle", false);
        WindowManager.Instance.Win();
    }

    private void OnDestroy()
    {
        // ������� ��������
        if (Instance == this)
        {
            Instance = null;
        }
    }


    // ���������� ������� ��������
    private void HandleAnimShootEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data == _eventData)
        {
            ShootEffect.gameObject.SetActive(true);
            ShootEffect.Play();
            // ���� ���������� ����, � �������� ����������, ���������� ���
            // (����� ����� ������������ �� ����� ��������, � �� �� �����)
            if (EnemyToDestroy)
            {
                EnemyToDestroy.Die();
                EnemyToDestroy = null;
            }
        }
    }
}
