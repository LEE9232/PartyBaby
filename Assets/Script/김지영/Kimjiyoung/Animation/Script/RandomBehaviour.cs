using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilRandom;

    [SerializeField]
    private int _numberOfRandomAnumations; // 애니메이션 수에 대해 직렬화 가능한 비공개 int 필드

    private bool _isRandom;
    private float _idleTime; // idle 상태가 얼마나 지속되었는지 저장하는 부동 필드
    private int _randomAnimation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // random 모션이 뜰정도의 지루한 시간이 지났는지?
        if (_isRandom == false)
        {
            // 그렇지 않은 경우 idle 시간 delta 시간만큼 늘린다.
            _idleTime += Time.deltaTime;
            if (_idleTime > _timeUntilRandom && stateInfo.normalizedTime % 1 < 0.02f)
            {
                // 캐릭터가 지루해질 만큼 오래 있었는가?
                // 맞다면 _isRandom 변수에 true
                _isRandom = true;
                // 그 다음으로 재생될 랜덤한 애니메이션 불러오기
                // 최대값은 애니메이션 수에 1을 더한 값으로,
                // 범위에 전달하는 최대값은 포함되지 않으므로 1을 더해야한다.
                _randomAnimation = UnityEngine.Random.Range(1, _numberOfRandomAnumations + 1);
                _randomAnimation = _randomAnimation * 2 - 1;

                animator.SetFloat("RandomAnimation", _randomAnimation - 1);
                
                // Random 애니메이션이 끝난 후 일반 idle 애니메이션으로 다시 전환하기 위해서는 "else if" 사용!
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98) // 애니메이션이 얼마나 진행 되었는지 알 수 있다.
        {
            // 0은 애니메이션의 시작을, 1은 끝을 나타낸다. 루프의 끝부분에 있는지 확인하려면 모듈러스 1을 수행하면 된다.
            // 그러면 값을 1로 나누고 나머지가 반환된다. 나머지가 0.98보다 크다면 Random 애니메이션 루프 중 하나가 거의 끝나가고 있다고 보면 된다.
            ResetIdle();
        }

        // 애니메이터 매게변수의 값
        animator.SetFloat("RandomAnimation", _randomAnimation, 0.2f, Time.deltaTime);
    }
    // 모든것을 재설정 하는 방법.
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
