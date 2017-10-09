using UnityEngine;
using UnityEngine.UI;



class InteractionItemController : MonoBehaviour
{
    [SerializeField]
    private Text label;
    [SerializeField]
    private Image background;

    public void SetLabelText(string text)
    {
        label.text = text;
    }

    public void SetSpyColor()
    {
        background.color = Color.yellow;
    }
}

