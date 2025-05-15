using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrientationBackgroundSwitcher : MonoBehaviour
{
    public Sprite portraitBackground;
    public Sprite landscapeBackground;

    private Image backgroundImage;
    private ScreenOrientation lastOrientation;

    void Awake()
    {
        Debug.Log("OrientationBackgroundSwitcher: Awake - Script started."); // Log khi script bắt đầu

        // Lấy tham chiếu đến Image component trên cùng GameObject này
        backgroundImage = GetComponent<Image>();

        if (backgroundImage == null)
        {
            Debug.LogError("OrientationBackgroundSwitcher: Image component not found on this GameObject!");
            enabled = false; // Tắt script nếu không tìm thấy Image
            return; // Thoát khỏi Awake nếu không tìm thấy Image
        }
        Debug.Log("OrientationBackgroundSwitcher: Found Image component."); // Log khi tìm thấy Image

        // Kiểm tra xem Sprites đã được gán chưa trong Inspector
        if (portraitBackground == null) Debug.LogWarning("OrientationBackgroundSwitcher: Portrait Background Sprite is NOT assigned in Inspector.");
        else Debug.Log("OrientationBackgroundSwitcher: Portrait Background Sprite is assigned.");

        if (landscapeBackground == null) Debug.LogWarning("OrientationBackgroundSwitcher: Landscape Background Sprite is NOT assigned in Inspector.");
        else Debug.Log("OrientationBackgroundSwitcher: Landscape Background Sprite is assigned.");


        lastOrientation = Screen.orientation;
        Debug.Log("OrientationBackgroundSwitcher: Initial Orientation is " + lastOrientation); // Log orientation ban đầu

        // Cập nhật ảnh nền dựa trên orientation hiện tại ngay lúc đầu
        UpdateBackground(lastOrientation);
    }

    void Update()
    {
        // Kiểm tra nếu orientation của màn hình đã thay đổi
        if (Screen.orientation != lastOrientation)
        {
            lastOrientation = Screen.orientation;
            Debug.Log("OrientationBackgroundSwitcher: Orientation Changed to " + lastOrientation); // Log khi orientation thay đổi
            // Cập nhật ảnh nền
            UpdateBackground(lastOrientation);
        }
    }

    void UpdateBackground(ScreenOrientation currentOrientation)
    {
        Debug.Log("OrientationBackgroundSwitcher: Calling UpdateBackground for orientation: " + currentOrientation); // Log khi hàm update được gọi

        if (backgroundImage == null)
        {
            Debug.LogError("OrientationBackgroundSwitcher: UpdateBackground called but Image component is null!");
            return;
        }

        // Kiểm tra orientation và gán ảnh nền tương ứng
        if (currentOrientation == ScreenOrientation.Portrait ||
            currentOrientation == ScreenOrientation.PortraitUpsideDown)
        {
            Debug.Log("OrientationBackgroundSwitcher: Setting Portrait Background Sprite."); // Log khi set ảnh dọc
            backgroundImage.sprite = portraitBackground;
        }
        else if (currentOrientation == ScreenOrientation.LandscapeLeft ||
                 currentOrientation == ScreenOrientation.LandscapeRight)
        {
            Debug.Log("OrientationBackgroundSwitcher: Setting Landscape Background Sprite."); // Log khi set ảnh ngang
            backgroundImage.sprite = landscapeBackground;
        }
        // Các trường hợp khác có thể không cần xử lý cho background

        // Kiểm tra xem Sprite đã được gán vào Image component thành công chưa
        if (backgroundImage.sprite == null)
        {
             Debug.LogWarning("OrientationBackgroundSwitcher: Image sprite is NULL after UpdateBackground attempt.");
        } else {
             Debug.Log("OrientationBackgroundSwitcher: Image sprite successfully assigned: " + backgroundImage.sprite.name);
        }
    }
}