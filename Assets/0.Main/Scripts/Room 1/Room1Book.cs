using UnityEngine;
using UnityEngine.UI;

public class Room1Book : InteractableObject
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject[] albumPages;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;

    int currentPageIndex = 0;
    int pageCount;
    PlayerInteract playerInteract;

    protected override void Start()
    {
        base.Start();
        pageCount = albumPages.Length;
        nextButton.onClick.AddListener(() => ChangePage(1));
        prevButton.onClick.AddListener(() => ChangePage(-1));
    }

    public void ChangePage(int alpha)
    {
        albumPages[currentPageIndex].SetActive(false);
        currentPageIndex += alpha;
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pageCount - 1);
        albumPages[currentPageIndex].SetActive(true);
    }

    protected override void OnInteract(CharacterInteract characterInteract)
    {
        base.OnInteract(characterInteract);
        if (characterInteract is not PlayerInteract)
        {
            return;
        }
        this.playerInteract = (PlayerInteract)characterInteract;
        canvas.SetActive(true);
        playerInteract.DisableControlPlayer();
    }

    public void CloseAlbum()
    {
        canvas.SetActive(false);
        playerInteract.EnableControlPlayer();
    }
}
