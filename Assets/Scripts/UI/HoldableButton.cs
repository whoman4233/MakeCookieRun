using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("이벤트 설정")]
    public UnityEvent onPress;     // 누를 때 실행
    public UnityEvent onRelease;   // 뗄 때 실행

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Btn Pressed");
        onPress?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Btn Released");
        onRelease?.Invoke();
    }
}