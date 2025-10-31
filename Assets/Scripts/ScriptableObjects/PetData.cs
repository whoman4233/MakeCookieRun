using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPetData", menuName = "ScriptableObjects/Pet Data")]
public class PetData : ScriptableObject
{
    public int petID;                                       // 펫을 식별할 고유 ID (0은 펫 없음)
    public string petName;                                  // 펫 이름
    public Sprite petSprite;                                // 펫의 기본 스프라이트 (Static Image)
    public RuntimeAnimatorController petAnimatorController; // 펫의 애니메이터 컨트롤러 (애니메이션이 있는 경우)
    public Vector3 offset;                                  // 캐릭터와의 상대적 위치 오프셋 (선택 사항)
}