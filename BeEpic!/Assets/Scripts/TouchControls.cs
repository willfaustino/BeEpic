using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchControls : MonoBehaviour
{
    //hold touch
    //private float holdTime = 3f; //or whatever
    //private float acumTime = 0;

    //double tap
    //int TapCount = 0;
    //float MaxDubbleTapTime = 0.8f;
    //float NewTime;

    //private AudioSource musica;

    //void Start ()
    //{
        //musica = GetComponent<AudioSource>();
    //}

    //void Update()
    //{
      //  if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject())
//        {
  //          acumTime += Input.GetTouch(0).deltaTime;
    //    
      //      if (acumTime >= holdTime)
        //    {
          //      Destroy(gameObject);
//            }

  //          if (Input.GetTouch(0).phase == TouchPhase.Ended) 
  //          {
    //            acumTime = 0;
      //      }
        //}

        //if (Input.touchCount == 1)
        //{
          //  Touch touch = Input.GetTouch(0);

            //if (touch.phase == TouchPhase.Ended)
            //{
              //  TapCount += 1;
            //}

//            if (TapCount == 1)
  //          {

    //            NewTime = Time.time + MaxDubbleTapTime;
//            }
  //          else if (TapCount == 2 && Time.time <= NewTime)
         //   {
    //
      //          if (!musica.loop)
        //        {
          //          musica.loop = true;
            //    }
              //  else
                //{
                  //  musica.loop = false;
                //}

//                TapCount = 0;
  //          }

    //    }
      //  if (Time.time > NewTime)
        //{
          //  TapCount = 0;
        //}
    //}

}
