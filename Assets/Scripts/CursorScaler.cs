using System.Collections;
using UnityEngine;

public class CursorScaler : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture;
    [SerializeField] float referenceDPI = 96f;  // Standard DPI for most desktop screens
    [SerializeField] float cursorSizeInches = 0.3f;  // Desired physical size of the cursor in inches

    private void Start()
    {
        StartCoroutine(_RefreshCursorResolution());
    }

    IEnumerator _RefreshCursorResolution()
    {
        while (isActiveAndEnabled)
        {
            ScaleCursor();
            yield return new WaitForSeconds(2f);
        }
    }

    private void ScaleCursor()
    {
        float dpi = Screen.dpi;

        // If DPI is not available (e.g., some older devices), fall back to reference DPI
        if (dpi == 0)
        {
            dpi = referenceDPI;
        }

        // Calculate the scale based on the DPI
        float scale = dpi / referenceDPI;

        // Calculate the cursor size in pixels based on the desired physical size (in inches)
        float cursorSizePixels = cursorSizeInches * dpi;

        // Set the cursor with the calculated size
        Vector2 cursorSize = Vector2.one * cursorSizePixels;
        Cursor.SetCursor(cursorTexture, cursorSize / 2, CursorMode.Auto);
    }
}
