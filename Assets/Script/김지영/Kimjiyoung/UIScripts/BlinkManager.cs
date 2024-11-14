using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BlinkManager : MonoBehaviour
{
    public LoopType loopType;
    public TextMeshProUGUI pressText;

    void Start()
    {
        pressText.DOFade(0.0f, 1).SetLoops(-1, loopType);
    }

    void Update()
    {
        
    }
}
