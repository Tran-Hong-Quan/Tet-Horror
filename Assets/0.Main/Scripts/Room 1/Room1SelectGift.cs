using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Room1SelectGift : MonoBehaviour
{
    public GiftData[] giftDatas;

    public Text itemNameText;
    public Image itemImage;
    public GameObject selectGiftUIRoot;

    public int knifeGiftIndex = 2;
    public int vibratorGiftIndex = 3;

    public PlayableDirector knifeTimeline;
    public PlayableDirector vibratorTimeline;
    public PlayableDirector commonTimeline;

    int selectedGiftIndex = 0;
    int giftCount = 0;
    void Start()
    {
        giftCount = giftDatas.Length;
        UpdateGiftUI();
    }

    public void ChangeGift(int delta)
    {
        selectedGiftIndex += delta;
        if(selectedGiftIndex >= giftCount)
        {
            selectedGiftIndex = 0;
        }
        else if(selectedGiftIndex < 0)
        {
            selectedGiftIndex = giftCount - 1;
        }
        UpdateGiftUI();
    }

    void UpdateGiftUI()
    {
        if (selectedGiftIndex < 0 || selectedGiftIndex >= giftCount)
        {
            return;
        }
        itemImage.sprite = giftDatas[selectedGiftIndex].sprite;
        itemNameText.text = giftDatas[selectedGiftIndex].localizeName.GetLocalizedString();
    }

    public void SelectGift()
    {
        selectGiftUIRoot.SetActive(false);
        if(selectedGiftIndex == knifeGiftIndex)
        {
            knifeTimeline.gameObject.SetActive(true);
            knifeTimeline.Play();
        }
        else if(selectedGiftIndex == vibratorGiftIndex)
        {
            vibratorTimeline.gameObject.SetActive(true);
            vibratorTimeline.Play();
        }
        else
        {
            commonTimeline.gameObject.SetActive(true);
            commonTimeline.Play();
        }
    }

    [System.Serializable]
    public class GiftData
    {
        public Sprite sprite;
        public LocalizedString localizeName;
    }

}
