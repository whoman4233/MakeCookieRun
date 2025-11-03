using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PetMoving : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float bobSpeed = 3.0f;   // 위아래로 움직이는 속도
    [SerializeField] private float bobHeight = 0.2f;  // 움직이는 높이 (진폭)
    private Vector3 startLocalPosition; // 펫의 원래 로컬 위치

    [Header("Data")]
    private SpriteRenderer spriteRenderer;
    private const string SELECTED_PET_ID_KEY = "SelectedPetID";

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("PetMoving: SpriteRenderer가 없습니다!");
            return;
        }

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "GameScene")
        {
            LoadPetData();
        }

        startLocalPosition = transform.localPosition;
    }

    void LoadPetData()
    {
        PetData[] allPetData = Resources.LoadAll<PetData>("PetData");

        int selectedPetID = PlayerPrefs.GetInt(SELECTED_PET_ID_KEY, 0);
        PetData currentPetData = allPetData.FirstOrDefault(pet => pet.petID == selectedPetID);

        if (currentPetData != null && currentPetData.petID != 0)
        {
            gameObject.SetActive(true);
            spriteRenderer.sprite = currentPetData.petSprite;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        float newYOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        // 삼각함수(Sin)를 사용해 부드러운 위아래 움직임을 형성
        // Time.time * bobSpeed : 시간에 따라 Sin 그래프를 따라 이동
        // Mathf.Sin(...) : -1과 1 사이를 부드럽게 오가는 값을 반환
        // * bobHeight : -bobHeight와 +bobHeight 사이를 오가도록 진폭 조절

        transform.localPosition = new Vector3(
            startLocalPosition.x,
            startLocalPosition.y + newYOffset,
            startLocalPosition.z
        );
        // 펫의 로컬 위치를 (원래 위치 + 계산된 Y 오프셋)으로 설정
    }
    // 고마워 따봉 Gemini야!
}
