using UnityEngine;
using TMPro;                             

public class TMP_PageAdvance : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject panel;

    int currentPage = 1;

    void Awake() {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
        if (panel == null) panel = text.transform.root.gameObject;

        text.ForceMeshUpdate();              
        currentPage = 1;
        text.pageToDisplay = currentPage;  // always start on page 1
    }

    public void AdvanceOrClose() {
        // how many pages does the text currently have?
        int totalPages = text.textInfo.pageCount;

        if (currentPage < totalPages)          // still more pages? Go to next page
        {
            currentPage++;
            text.pageToDisplay = currentPage;
        } else                                   // last page already shown, close panel
          {
            panel.SetActive(false);         
        }
    }
}
