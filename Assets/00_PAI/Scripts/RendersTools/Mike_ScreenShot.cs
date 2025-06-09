using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class Mike_ScreenShot : MonoBehaviour
{
   public SO_SreenShoot_Configuration SreenShootConfiguration;
    
   public GameObject[] color_Objects;
    
   public Camera camera;
    

   [OnValueChanged("SetColor")]
    
   public SO_SreenShoot_Configuration.TipoColor tipoColor;
    
   public string nombre;
   public string path;
   
   public void SetColor()
   {
      foreach (var obj in color_Objects)
      {
         obj.GetComponent<Renderer>().material.color = SreenShootConfiguration.colors[(int)tipoColor];
      }
   }
    

   [Button]
   public void Add_POV()
   {
      SreenShootConfiguration.DataPozos.Add(new DataPozoScreenShoot(nombre,camera.transform.position,camera.transform.localEulerAngles));
   }
    
   [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
   public void TakeScreenshot( )
   { 
      StartCoroutine(ScreenShootCoroutine());
   }

   [Button]
   public void SetCameraPOV(int index)
   {
      camera.transform.position = SreenShootConfiguration.DataPozos[index].positions;
      camera.transform.localEulerAngles = SreenShootConfiguration.DataPozos[index].localEulerAngles;
   }

   IEnumerator ScreenShootCoroutine()
   {
      string fileName = " ";
      for (int i = 0; i < SreenShootConfiguration.DataPozos.Count; i++)
      {
         camera.transform.position = SreenShootConfiguration.DataPozos[i].positions;
         camera.transform.localEulerAngles = SreenShootConfiguration.DataPozos[i].localEulerAngles;
         var fileName0 = SreenShootConfiguration.DataPozos[i].names + "_" ;

         var colores = Enum.GetValues(typeof(SO_SreenShoot_Configuration.TipoColor));
         
         yield return new WaitForSeconds(0.5f);
         
         Debug.Log(SreenShootConfiguration.DataPozos[i].names);
         foreach (SO_SreenShoot_Configuration.TipoColor _color in colores)
         {
            Debug.Log(_color.ToString());
            fileName = fileName0 + _color.ToString();
            tipoColor = _color;
            SetColor();
            ScreenCapture.CaptureScreenshot(path+fileName + ".png");
            //yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.5f);
         }
        
      }
        
   }
}
