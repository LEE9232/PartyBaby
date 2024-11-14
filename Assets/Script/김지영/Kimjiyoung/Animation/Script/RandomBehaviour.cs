using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilRandom;

    [SerializeField]
    private int _numberOfRandomAnumations; // �ִϸ��̼� ���� ���� ����ȭ ������ ����� int �ʵ�

    private bool _isRandom;
    private float _idleTime; // idle ���°� �󸶳� ���ӵǾ����� �����ϴ� �ε� �ʵ�
    private int _randomAnimation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // random ����� �������� ������ �ð��� ��������?
        if (_isRandom == false)
        {
            // �׷��� ���� ��� idle �ð� delta �ð���ŭ �ø���.
            _idleTime += Time.deltaTime;
            if (_idleTime > _timeUntilRandom && stateInfo.normalizedTime % 1 < 0.02f)
            {
                // ĳ���Ͱ� �������� ��ŭ ���� �־��°�?
                // �´ٸ� _isRandom ������ true
                _isRandom = true;
                // �� �������� ����� ������ �ִϸ��̼� �ҷ�����
                // �ִ밪�� �ִϸ��̼� ���� 1�� ���� ������,
                // ������ �����ϴ� �ִ밪�� ���Ե��� �����Ƿ� 1�� ���ؾ��Ѵ�.
                _randomAnimation = UnityEngine.Random.Range(1, _numberOfRandomAnumations + 1);
                _randomAnimation = _randomAnimation * 2 - 1;

                animator.SetFloat("RandomAnimation", _randomAnimation - 1);
                
                // Random �ִϸ��̼��� ���� �� �Ϲ� idle �ִϸ��̼����� �ٽ� ��ȯ�ϱ� ���ؼ��� "else if" ���!
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98) // �ִϸ��̼��� �󸶳� ���� �Ǿ����� �� �� �ִ�.
        {
            // 0�� �ִϸ��̼��� ������, 1�� ���� ��Ÿ����. ������ ���κп� �ִ��� Ȯ���Ϸ��� ��ⷯ�� 1�� �����ϸ� �ȴ�.
            // �׷��� ���� 1�� ������ �������� ��ȯ�ȴ�. �������� 0.98���� ũ�ٸ� Random �ִϸ��̼� ���� �� �ϳ��� ���� �������� �ִٰ� ���� �ȴ�.
            ResetIdle();
        }

        // �ִϸ����� �ŰԺ����� ��
        animator.SetFloat("RandomAnimation", _randomAnimation, 0.2f, Time.deltaTime);
    }
    // ������ �缳�� �ϴ� ���.
    private void ResetIdle()
    {
        if (_isRandom)
        {
            _randomAnimation--;
        }
        _isRandom = false;
        _idleTime = 0;     
    }

}
