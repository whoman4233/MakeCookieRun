using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("�̺�Ʈ ����")]
    public UnityEvent onPress;     // ���� �� ����
    public UnityEvent onRelease;   // �� �� ����

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