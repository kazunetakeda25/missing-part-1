using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MatFade : MonoBehaviour 
{
    public Renderer myRenderer;
    public UIButton myButton;
    public float minAlphaVal;
    public float maxAlphaVal;
    public float timeForAlpha;
    public string colorToChange;

    private float progress;
    private bool contracting;
    private bool paused = false;

    private void Update()
    {
        UpdatePauseState();

        if(paused == true)
            return;

        Glow();
    }

    private void UpdatePauseState()
    {
        if(paused == false) {
            if(myButton.controlState == UIButton.CONTROL_STATE.OVER) {
                paused = true;
                progress = minAlphaVal;
                contracting = false;
                myRenderer.material.color = new Color(myRenderer.material.color.r, myRenderer.material.color.g, myRenderer.material.color.b, minAlphaVal);
            }
        } else {
            if(myButton.controlState == UIButton.CONTROL_STATE.NORMAL) {
                paused = false;
            }
        }
    }

    private void Glow()
    {
        float start = contracting ? minAlphaVal : maxAlphaVal;
        float end = contracting ? maxAlphaVal : minAlphaVal;
        progress += Time.deltaTime;

        if(progress >= timeForAlpha) {
            contracting = !contracting;
            progress = 0.0f;
            return;
        }

        float lerpVal = progress / timeForAlpha;
        
        float alphaVal = Mathf.Lerp(start, end, lerpVal);
        myRenderer.material.color = new Color(myRenderer.material.color.r, myRenderer.material.color.g, myRenderer.material.color.b, alphaVal);
    }
}
