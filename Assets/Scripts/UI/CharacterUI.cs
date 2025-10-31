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
    private Animator previewPetAnimator;
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

    private void FindPreviewPetComponents()
    {
        // [����] transform.Find ��� GameObject.FindWithTag�� ����ϵ��� �ǵ����ϴ�.
        GameObject previewPetObject = GameObject.FindWithTag("PreviewPet");

        if (previewPetObject != null)
        {
            // ã�� ������Ʈ���� ������Ʈ�� �����ɴϴ�.
            previewPetSpriteRenderer = previewPetObject.GetComponent<SpriteRenderer>();
            previewPetAnimator = previewPetObject.GetComponent<Animator>();

            if (previewPetSpriteRenderer != null && previewPetAnimator != null)
            {
                arePreviewComponentsFound = true; // ����
            }
            else
            {
                Debug.LogError("PreviewPet ������Ʈ���� SpriteRenderer �Ǵ� Animator�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            // �� ������ ��ٸ� 2�ܰ� (Tag ����)�� �ٽ� Ȯ���ϼ���.
            Debug.LogError("CharacterScene���� 'PreviewPet' �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    public void OnClickTotitleBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void OnClickPetSelectButton(int petIndex) // ������ ���� ������ �����ϴ� �޼���
    {
        if (petIndex >= 0 && petIndex < availablePets.Count)
        {
            PetData selectedPet = availablePets[petIndex];                  // ���õ� ���� ID�� PlayerPrefs�� ����
            PlayerPrefs.SetInt(SELECTED_PET_ID_KEY, selectedPet.petID);
            PlayerPrefs.Save();                                             // ������� ��� ����
            Debug.Log($"Selected Pet: {selectedPet.petName} (ID: {selectedPet.petID})");
            HighlightSelectedPetButton();
            UpdatePreviewPet(selectedPet);
        }
    }

    private void HighlightSelectedPetButton() // ������ ���� �����ϴ� �޼���
    {
        if (!arePreviewComponentsFound) return;

        int currentPetID = PlayerPrefs.GetInt(SELECTED_PET_ID_KEY, 0);  // �⺻�� 0 (�� ����)
        for (int i = 0; i < availablePets.Count; i++)
        {
            Button petButton = petSelectButtons[i];                     // ���⿡�� ��ư�� ����, �̹���, �׵θ� ���� �����Ͽ� ���� ���θ� ǥ��
            if (petButton != null)
            {
                Image buttonImage = petButton.GetComponent<Image>();
                if (buttonImage != null)
                {
                    if (availablePets[i].petID == currentPetID)
                    {
                        buttonImage.color = Color.yellow; // ���õ�
                    }
                    else
                    {
                        buttonImage.color = Color.white; // ���� �� �� (�⺻ ����)
                    }
                }
            }
        }
    }

    private void LoadAndApplyCurrentPet()
    {
        if (!arePreviewComponentsFound) return;

        int currentPetID = PlayerPrefs.GetInt(SELECTED_PET_ID_KEY, 0);              // �⺻�� 0 (�� ����)
        PetData currentPet = availablePets.Find(pet => pet.petID == currentPetID);  // availablePets ����Ʈ���� ����� ID�� ��ġ�ϴ� PetData�� ã��

        if (currentPet != null)
        {
            UpdatePreviewPet(currentPet); // ã�� �� �����ͷ� �̸����� ������Ʈ
        }
        else // ���� ����� ID�� ����Ʈ�� ���ٸ� (�����Ͱ� ����Ǿ��ų� ����)
        {
            PetData noPetData = availablePets.Find(pet => pet.petID == 0);
            if (noPetData != null)
            {
                UpdatePreviewPet(noPetData);
            }
            else
            {
                Debug.LogError("PetData ����Ʈ���� ID 0 (�� ����) �����͸� ã�� �� �����ϴ�!");
            }
        }
    }

    private void UpdatePreviewPet(PetData petData)
    {
        if (petData.petID == 0) // ���� ������ ������ ���
        {
            previewPetSpriteRenderer.enabled = false;               // ��������Ʈ �������� �� (�����ϰ� ����)
            previewPetAnimator.enabled = false;                     // �ִϸ����͸� ��
        }
        else // ���� ���õ� ���
        {
            previewPetSpriteRenderer.enabled = true;                // ��������Ʈ �������� ��
            previewPetSpriteRenderer.sprite = petData.petSprite;    // �� �������� ��������Ʈ�� ����

            if (petData.petAnimatorController != null)
            {
                previewPetAnimator.runtimeAnimatorController = petData.petAnimatorController; // �ִϸ����� ��ü
                previewPetAnimator.enabled = true; // �ִϸ����͸� ��
            }
            else // �ִϸ����� ��Ʈ�ѷ��� ���ٸ�(�۾� �� �߰ų� ���� �����߰ų�)
            {
                previewPetAnimator.runtimeAnimatorController = null;
                previewPetAnimator.enabled = false; // �ִϸ����͸� ��
            }
        }
    }

    protected override UIState GetUIState()
    {
        return UIState.Character;
    }
}
