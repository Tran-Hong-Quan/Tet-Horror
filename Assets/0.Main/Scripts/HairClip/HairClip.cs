using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using SFB;

public class HairClip : InteractableObject
{
    [Header("Target Material")]
    public Material targetMaterial; // Material cần thay đổi texture
    public string texturePropertyName = "_MainTex"; // Tên property texture trong shader


    [Header("UI References")]
    public GameObject rootUI;
    public List<Button> imageButtons; // Danh sách các button ảnh
    public Button selectImageButton;
    public RawImage displayImage;

    public string saveFileName = "hairclipicon.png";
    private string savePath;
    private Texture2D currentTexture;

    const string IS_USING_EXTERNAL_IMAGE_KEY = "IS_USING_EXTERNAL_IMAGE_FOR_HAIRCLIP";
    const string CURRENT_IMAGE_INDEX_KEY = "CURRENT_HAIR_CLIP_IMAGE_INDEX_HAIRCLIP";

    protected override void Start()
    {
        base.Start();
        // Kiểm tra material đã được gán chưa
        if (targetMaterial == null)
        {
            Debug.LogError("Vui lòng gán Target Material trong Inspector!");
            return;
        }

        // Gán sự kiện cho từng button
        AssignButtonEvents();


        // Gán sự kiện cho button
        if (selectImageButton != null)
            selectImageButton.onClick.AddListener(OpenFilePicker);

        // Tạo đường dẫn lưu file
        savePath = Path.Combine(Application.persistentDataPath, saveFileName);
        if (PlayerPrefs.GetInt(IS_USING_EXTERNAL_IMAGE_KEY, 0) == 1)
        {
            // Load ảnh đã lưu nếu có
            LoadSavedImage();
        }
        else
        {
            //Load icon truoc do
            LoadLastSelectedIcon();
        }
    }

    void LoadLastSelectedIcon()
    {
        if (imageButtons == null || imageButtons.Count == 0)
        {
            Debug.LogWarning("Không có button nào trong danh sách imageButtons!");
            return;
        }
        int index = Mathf.Clamp(PlayerPrefs.GetInt(CURRENT_IMAGE_INDEX_KEY, 0), 0, imageButtons.Count);
        ChangeMaterialFromButton(index);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        LoadLastSelectedIcon();
    }

    PlayerInteract currentPlayerInteract;

    override public void Interact(CharacterInteract characterInteract)
    {
        base.Interact(characterInteract);
        OpenChangeIconBoard(characterInteract as PlayerInteract);
    }

    public void OpenChangeIconBoard(PlayerInteract playerInteract)
    {
        if (playerInteract == null)
        {
            Debug.LogError("PlayerInteract invalid!");
            return;
        }
        this.currentPlayerInteract = playerInteract;
        playerInteract.DisableControlPlayer();
        rootUI.SetActive(true);
    }

    public void CloseChageIconBoard()
    {
        rootUI.SetActive(false);
        if (currentPlayerInteract != null)
        {
            currentPlayerInteract.EnableControlPlayer();
            currentPlayerInteract = null;
        }
    }

    // Hàm gán sự kiện cho các button
    void AssignButtonEvents()
    {
        for (int i = 0; i < imageButtons.Count; i++)
        {
            if (imageButtons[i] != null)
            {
                int index = i; // Capture index cho lambda
                imageButtons[i].onClick.AddListener(() => ChangeMaterialFromButton(index));   
            }
            else
            {
                Debug.LogWarning($"Button tại vị trí {i} chưa được gán!");
            }
        }
    }

    // Hàm thay đổi material từ button
    public void ChangeMaterialFromButton(int index)
    {
        Button clickedButton = null;
        if(index >= 0 && index < imageButtons.Count)
        {
            clickedButton = imageButtons[index];
        }
        else
        {
            Debug.LogError("Index button không hợp lệ: " + index);
            return;
        }
        PlayerPrefs.SetInt(CURRENT_IMAGE_INDEX_KEY, index); // Lưu index đã chọn
        if (clickedButton == null)
        {
            Debug.LogError("Button không hợp lệ!");
            return;
        }

        // Lấy Image component từ button
        Image buttonImage = clickedButton.image;

        if (buttonImage == null)
        {
            Debug.LogError("Button không có Image component!");
            return;
        }

        // Lấy sprite từ Image
        Sprite buttonSprite = buttonImage.sprite;

        if (buttonSprite == null)
        {
            Debug.LogError("Button Image không có sprite!");
            return;
        }

        // Lấy texture từ sprite và gán cho material
        Texture2D buttonTexture = buttonSprite.texture;

        if (buttonTexture != null)
        {
            SetHairClipTexture(buttonTexture);
            Debug.Log($"Đã chuyển sang ảnh: {buttonSprite.name}");
        }
        else
        {
            Debug.LogError("Không thể lấy texture từ sprite!");
        }
    }

    public void SetHairClipTexture(Texture2D texture)
    {
        displayImage.texture = texture;
        targetMaterial.SetTexture(texturePropertyName, texture);
    }

    void OpenFilePicker()
    {
#if UNITY_EDITOR
        // Trong Editor: dùng file browser giả lập
        StartCoroutine(PickImageInEditor());
#elif UNITY_ANDROID || UNITY_IOS
        // Trên mobile: dùng Native Gallery
        PickImageFromGallery();
#elif UNITY_WEBGL
        // Trên WebGL: dùng input file HTML
        PickImageInWebGL();
#else
        // Trên Windows/Mac/Linux: dùng System.Windows.Forms
        PickImageFromDisk();
#endif
    }

    #region Platform-Specific Methods

#if UNITY_EDITOR
    IEnumerator PickImageInEditor()
    {
        UpdateStatus("Selecting image...");

        // Mở file picker trong Editor
        string filePath = UnityEditor.EditorUtility.OpenFilePanel(
            "Select an image",
            "",
            "png,jpg,jpeg,bmp");

        if (!string.IsNullOrEmpty(filePath))
        {
            yield return StartCoroutine(LoadImageFromDisk(filePath));
        }
        else
        {
            UpdateStatus("No image selected");
        }
    }
#endif

    void PickImageFromDisk()
    {
        //#if !UNITY_EDITOR && (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX)
        UpdateStatus("Selecting image...");

        var paths = StandaloneFileBrowser.OpenFilePanel(
            "Select Image",
            "",
            new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "bmp") },
            false
        );

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            StartCoroutine(LoadImageFromDisk(paths[0]));
        }
        else
        {
            UpdateStatus("No image selected");
        }

        //#endif
    }

    void PickImageFromGallery()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        UpdateStatus("Opening gallery...");
        
        // Sử dụng Native Gallery plugin
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                StartCoroutine(LoadImageFromDisk(path));
            }
            else
            {
                UpdateStatus("No image selected");
            }
        });
        
        if (permission == NativeGallery.Permission.Denied)
        {
            UpdateStatus("Permission denied to access gallery");
        }
#endif
    }

    void PickImageInWebGL()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        UpdateStatus("Selecting image...");
        
        // Tạo input file tạm thời
        CreateWebGLFileInput();
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    void CreateWebGLFileInput()
    {
        // Inject JavaScript code để tạo file input
        Application.ExternalEval(@"
            var input = document.createElement('input');
            input.type = 'file';
            input.accept = 'image/*';
            input.onchange = function(e) {
                var file = e.target.files[0];
                var reader = new FileReader();
                reader.onload = function(readerEvent) {
                    var imageData = readerEvent.target.result;
                    // Gửi dữ liệu về Unity
                    unityInstance.SendMessage('" + gameObject.name + @"', 'OnWebGLImageSelected', imageData);
                };
                reader.readAsDataURL(file);
            };
            input.click();
        ");
    }
    
    void OnWebGLImageSelected(string imageData)
    {
        StartCoroutine(LoadImageFromWebGL(imageData));
    }
    
    IEnumerator LoadImageFromWebGL(string imageData)
    {
        // Xử lý dữ liệu base64 từ WebGL
        string base64Data = imageData.Substring(imageData.IndexOf(",") + 1);
        byte[] imageBytes = System.Convert.FromBase64String(base64Data);
        
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);
        
        ProcessLoadedTexture(texture);
    }
#endif

    #endregion

    IEnumerator LoadImageFromDisk(string filePath)
    {
        UpdateStatus("Loading image...");

        // Đọc file thành byte array
        byte[] imageBytes = File.ReadAllBytes(filePath);

        // Tạo texture từ bytes
        Texture2D texture = new(2, 2);
        texture.LoadImage(imageBytes);

        yield return null; // Đợi 1 frame

        ProcessLoadedTexture(texture);
    }

    void ProcessLoadedTexture(Texture2D texture)
    {
        if (texture != null)
        {
            currentTexture = texture;

            // Hiển thị ảnh 
            SetHairClipTexture(texture);

            UpdateStatus("Image loaded successfully!");

            // Tự động lưu ảnh
            SaveImage(texture);

            PlayerPrefs.SetInt(IS_USING_EXTERNAL_IMAGE_KEY, 1);
        }
        else
        {
            UpdateStatus("Failed to load image");
        }
    }

    public void SaveImage(Texture2D texture)
    {
        if (texture == null)
        {
            UpdateStatus("No image to save");
            return;
        }

        try
        {
            // Chuyển texture thành PNG
            byte[] pngData = texture.EncodeToPNG();

            // Lưu file
            File.WriteAllBytes(savePath, pngData);

            UpdateStatus("Image saved to: " + savePath);
            Debug.Log("Image saved to: " + savePath);
        }
        catch (System.Exception e)
        {
            UpdateStatus("Error saving image: " + e.Message);
            Debug.LogError("Error saving image: " + e.Message);
        }
    }

    public void LoadSavedImage()
    {
        if (File.Exists(savePath))
        {
            StartCoroutine(LoadImageFromDisk(savePath));
        }
        else
        {
            UpdateStatus("No saved image found");
            LoadLastSelectedIcon();
        }
    }

    void UpdateStatus(string message)
    {
        Debug.Log("[ImageUploader] " + message);
    }

    // Hàm public để gọi từ các nút khác
    public void DeleteSavedImage()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            currentTexture = null;

            if (displayImage != null)
                displayImage.texture = null;

            UpdateStatus("Saved image deleted");
        }
    }
}
