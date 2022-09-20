using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Font : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool isPointEnter = false;
    private int childIndex = -1;
    public bool isCharacter = false;

    private void Start()
    {
        Destroy(this);
        return;
        childIndex = transform.GetSiblingIndex();
        StartCoroutine(SwingFont());
    }

    private IEnumerator SwingFont()
    {
        yield return new WaitForSecondsRealtime(childIndex * 0.1f);

        while (true)
        {
            transform.DORotate(new Vector3(0f, 0f, 15f), 1.5f).SetUpdate(true); 
            yield return new WaitForSecondsRealtime(1.5f);

            transform.DORotate(new Vector3(0f, 0f, -15f), 1.5f).SetUpdate(true); 

            yield return new WaitForSecondsRealtime(1.5f);

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isPointEnter) return;
        if (isCharacter) return;
        SoundManager.Inst.SetEsterEggEffectSound(childIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    { 
        isPointEnter = true;

        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.25f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointEnter = false;

        transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f).SetUpdate(true);
    }
}
