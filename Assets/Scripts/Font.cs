using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Font : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool isPointEnter = false;
    private int childIndex = -1;

    private void Start()
    {
        childIndex = transform.GetSiblingIndex();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPointEnter) return;
        SoundManager.Inst.SetEsterEggEffectSound(childIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    { 
        isPointEnter = true;

        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointEnter = false;

        transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
    }
}
