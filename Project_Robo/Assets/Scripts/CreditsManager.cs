using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CreditsManager : MonoBehaviour
{
    // -- Declared Variables -- //
    public int currentSlide;
    public RawImage overlay;
    public GameObject nameRoles;
    public GameObject behindTheScenes1;
    public GameObject behindTheScenes2;
    public GameObject behindTheScenes3;

    // Start is called before the first frame update
    void Start()
    {
        /*
        nameRoles.SetActive(true);
        behindTheScenes1.SetActive(false);
        behindTheScenes2.SetActive(false);
        behindTheScenes3.SetActive(false);
        */
        currentSlide = 2;
        StartCoroutine("FadeInAndOut");
        //StartCoroutine("SlideTransition");
        //StartCoroutine("FadeOut");
    }

    // Update is called once per frame
    void Update()
    {
        // added by Adam, an alternate way to exit the game
        // for impatient people
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("The game would close now!");
            Application.Quit();
        }
    }

    void SlideUpdate()
    {
        StartCoroutine("FadeInAndOut");
        //StartCoroutine("SlideTransition");
        //StartCoroutine("FadeOut");
    }

    IEnumerator FadeInAndOut()
    {
        Debug.Log("Fade in...");
        // Make the overlay disappear so the slide 'fades in'
        for (float ft = overlay.color.a; ft >= 0; ft -= 0.1f)
        {
            Color c = overlay.color;
            c.a = ft;
            overlay.color = c;
            //Debug.Log("Fading in: " + overlay.color.a);
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(10f);

        Debug.Log("Fade out... Slide: " + currentSlide);
        // Make the overlay reappear so the slide 'fades out'
        for (float ft = 0f; ft <= 1f; ft += 0.1f)
        {
            Color c = overlay.color;
            c.a = ft;
            overlay.color = c;
            //Debug.Log("Fading out: " + overlay.color.a);
            yield return new WaitForSeconds(.1f);
        }

        Debug.Log("SlideTransition()");
        StartCoroutine("SlideTransition");


        Debug.Log("SlideUpdate()");
        SlideUpdate();
    }

    /*
    IEnumerator FadeOut()
    {
        
        Debug.Log("Fade out... Slide: " + currentSlide);
        // Make the overlay reappear so the slide 'fades out'
        for (float ft = 0f; ft <= 1f; ft += 0.1f)
        {
            Color c = overlay.color;
            c.a = ft;
            overlay.color = c;
            Debug.Log("Fading out: " + overlay.color.a);
            yield return new WaitForSeconds(.3f);
        }
        Debug.Log("SlideUpdate()");
        SlideUpdate();

    }
    */

    IEnumerator SlideTransition()
    {
        Debug.Log("Change slide..." + currentSlide);
        if (currentSlide == 1)
        {
            nameRoles.SetActive(true);
            behindTheScenes1.SetActive(false);
            behindTheScenes2.SetActive(false);
            behindTheScenes3.SetActive(false);
        }
        else if (currentSlide == 2)
        {
            nameRoles.SetActive(false);
            behindTheScenes1.SetActive(true);
            behindTheScenes2.SetActive(false);
            behindTheScenes3.SetActive(false);
        }
        else if (currentSlide == 3)
        {
            nameRoles.SetActive(false);
            behindTheScenes1.SetActive(false);
            behindTheScenes2.SetActive(true);
            behindTheScenes3.SetActive(false);
        }
        else if (currentSlide == 4)
        {
            nameRoles.SetActive(false);
            behindTheScenes1.SetActive(false);
            behindTheScenes2.SetActive(false);
            behindTheScenes3.SetActive(true);
        }
        else if (currentSlide == 5)
        {
            // End of slides
            Debug.Log("Quitting the game...");
            Application.Quit();
        }
        else
        {
            Debug.Log("Not one of the slides avaliable");
        }
        currentSlide++;
        //Debug.Log("Waiting?");
        yield return new WaitForSeconds(1f);
    }
}
