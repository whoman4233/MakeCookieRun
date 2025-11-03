using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPetData", menuName = "ScriptableObjects/Pet Data")]
public class PetData : ScriptableObject
{
    public int petID;                                       // 펫을 식별할 고유 ID (0은 펫 없음)
    public string petName;                                  // 펫 이름
    public Sprite petSprite;                                // 펫의 기본 스프라이트 (Static Image)이 있는 경우)
}