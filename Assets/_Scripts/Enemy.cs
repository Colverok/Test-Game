using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;

/// <summary>
/// �����, ����������� ��������� � �������� �����
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// ��������� ��������� �������� � ������
    /// </summary>
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    /// <summary>
    /// ������ ������
    /// </summary>
    [SerializeField] private ParticleSystem explosionEffect;
    /// <summary>
    /// ������� ��������� �����
    /// </summary>
    [SerializeField] private UnityEvent onStopEvent;


    
    // ������ ������
    private GameObject player;
    // ��������� ��������
    private Spine.AnimationState _animationState;

    void Start()
    {
        player = Player.Instance.gameObject;
        _animationState = skeletonAnimation.AnimationState;

        // ���� ���� �� �������� ��������, ��������� ��������� ��� ������� � ������� ��������� ����� 
        if (GetComponent<MoveToPointX>() is MoveToPointX script)
        {
            onStopEvent.AddListener(() => script.enabled = false); 
        }
    }

    private void OnMouseDown()
    {
        // ��� ������� �� ����� ����� ������������
        Player.Instance.Shoot();
        // ��������� ���������, ����� �� ���� ���������
        GetComponent<Collider2D>().enabled = false;
        // ����� ����� ����� ���������� �� ����� ������� ��������
        Player.Instance.EnemyToDestroy = this;
    }

    /// <summary>
    /// ����� ��� ����������� �����
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
        // ����� ���� ������������ � �������, �� ��������������� � ���������
        onStopEvent.Invoke();
        WinAnimate();
    }

    // ����� ��� �������� ������ ����� ��� �������
    private void WinAnimate()
    {
        PlayOneAnim("win");
    }

    // ����� ��� ������������ 1 �������� �� 0 ����
    private void PlayOneAnim(string animationName)
    {
        _animationState.SetAnimation(0, animationName, false);
    }

}
