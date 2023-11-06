using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GhostTrailEffect : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool makeGhost = false;

    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }
    void Update()
    {
        if(makeGhost){
            if(ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.transform.localScale = this.transform.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = ghostDelay;

                // Use DoTween to smoothly fade out the ghost and destroy it after the animation
                SpriteRenderer ghostRenderer = currentGhost.GetComponent<SpriteRenderer>();
                ghostRenderer.DOFade(0, 0.3f).OnComplete(() => Destroy(currentGhost));

            }
        }
    }
}
