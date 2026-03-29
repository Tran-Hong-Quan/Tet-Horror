using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Room1SelectGift : MonoBehaviour
{
    public GiftData[] giftDatas;
    public Text itemNameText;
    public Image itemImage;

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

    [System.Serializable]
    public class GiftData
    {
        public Sprite sprite;
        public LocalizedString localizeName;
    }

}
