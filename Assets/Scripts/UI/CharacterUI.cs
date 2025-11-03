using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterUI : BaseUI
{
    [SerializeField] private Button toTitleBtn;

    [Header("Pet Selection")]
    [SerializeField] private List<Button> petSelectButtons;
    [SerializeField] private List<PetData> availablePets;

    private SpriteRenderer previewPetSpriteRenderer;
    private bool arePreviewComponentsFound = false;

    private const string SELECTED_PET_ID_KEY = "SelectedPetID";

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        toTitleBtn.onClick.AddListener(OnClickTotitleBtn);

        for (int i = 0; i < petSelectButtons.Count; i++)
        {
            int petIndex = i;
            petSelectButtons[i].onClick.AddListener(() => OnClickPetSelectButton(petIndex));
        }
    }

    private void OnEnable()
    {
        if (!arePreviewComponentsFound)
        {
            FindPreviewPetComponents();
        }

        if (arePreviewComponentsFound)
        {
            HighlightSelectedPetButton();
            LoadAndApplyCurrentPet();
        }
    }

    private void OnDisable()
    {
        arePreviewComponentsFound = false;
        previewPetSpriteRenderer = null;
    }

    private void FindPreviewPetComponents()
    {
        GameObject previewPetObject = GameObject.FindWithTag("PreviewPet");

        if (previewPetObject != null)
        {
            previewPetSpriteRenderer = previewPetObject.GetComponent<SpriteRenderer>();

            if (previewPetSpriteRenderer != null)
            {
                arePreviewComponentsFound = true; // 성공
            }
            else
            {
                Debug.LogError("PreviewPet 오브젝트에서 SpriteRenderer를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("CharacterScene에서 'PreviewPet' 태그를 가진 오브젝트를 찾을 수 없습니다!");
        }
    }

    public void OnClickTotitleBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void OnClickPetSelectButton(int petIndex) // 선택한 펫을 펫으로 설정하는 메서드
    {
        if (petIndex >= 0 && petIndex < availablePets.Count)
        {
            PetData selectedPet = availablePets[petIndex];                  // 선택된 펫의 ID를 PlayerPrefs에 저장
            PlayerPrefs.SetInt(SELECTED_PET_ID_KEY, selectedPet.petID);
            PlayerPrefs.Save();                                             // 변경사항 즉시 저장
            Debug.Log($"Selected Pet: {selectedPet.petName} (ID: {selectedPet.petID})");
            HighlightSelectedPetButton();
            UpdatePreviewPet(selectedPet);
        }
    }

    private void HighlightSelectedPetButton() // 선택한 펫을 강조하는 메서드
    {
        if (!arePreviewComponentsFound) return;

        int currentPetID = PlayerPrefs.GetInt(SELECTED_PET_ID_KEY, 0);  // 기본값 0 (펫 없음)
        for (int i = 0; i < availablePets.Count; i++)
        {
            Button petButton = petSelectButtons[i];                     // 여기에서 버튼의 색상, 이미지, 테두리 등을 변경하여 선택 여부를 표시
            if (petButton != null)
            {
                Image buttonImage = petButton.GetComponent<Image>();
                if (buttonImage != null)
                {
                    if (availablePets[i].petID == currentPetID)
                    {
                        buttonImage.color = Color.yellow; // 선택됨
                    }
                    else
                    {
                        buttonImage.color = Color.white; // 선택 안 됨 (기본 색상)
                    }
                }
            }
        }
    }

    private void LoadAndApplyCurrentPet()
    {
        if (!arePreviewComponentsFound) return;

        int currentPetID = PlayerPrefs.GetInt(SELECTED_PET_ID_KEY, 0);              // 기본값 0 (펫 없음)
        PetData currentPet = availablePets.Find(pet => pet.petID == currentPetID);  // availablePets 리스트에서 저장된 ID와 일치하는 PetData를 찾음

        if (currentPet != null)
        {
            UpdatePreviewPet(currentPet); // 찾은 펫 데이터로 미리보기 업데이트
        }
        else // 만약 저장된 ID가 리스트에 없다면 (데이터가 변경되었거나 오류)
        {
            PetData noPetData = availablePets.Find(pet => pet.petID == 0);
            if (noPetData != null)
            {
                UpdatePreviewPet(noPetData);
            }
            else
            {
                Debug.LogError("PetData 리스트에서 ID 0 (펫 없음) 데이터를 찾을 수 없습니다!");
            }
        }
    }

    private void UpdatePreviewPet(PetData petData)
    {
        if (petData.petID == 0) // 펫이 없음을 선택한 경우
        {
            previewPetSpriteRenderer.enabled = false;               // 스프라이트 렌더러를 끔 (투명하게 만듦)
        }
        else // 펫이 선택된 경우
        {
            previewPetSpriteRenderer.enabled = true;                // 스프라이트 렌더러를 켬
            previewPetSpriteRenderer.sprite = petData.petSprite;    // 펫 데이터의 스프라이트로 변경
        }
    }

    protected override UIState GetUIState()
    {
        return UIState.Character;
    }
}
