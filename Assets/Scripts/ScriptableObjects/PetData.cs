using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPetData", menuName = "ScriptableObjects/Pet Data")]
public class PetData : ScriptableObject
{
    public int petID;                                       // ���� �ĺ��� ���� ID (0�� �� ����)
    public string petName;                                  // �� �̸�
    public Sprite petSprite;                                // ���� �⺻ ��������Ʈ (Static Image)�� �ִ� ���)
    public Vector3 offset;                                  // ĳ���Ϳ��� ����� ��ġ ������ (���� ����)
}