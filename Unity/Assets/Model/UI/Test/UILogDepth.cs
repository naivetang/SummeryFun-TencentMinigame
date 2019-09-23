using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETModel
{
    [RequireComponent(typeof (Graphic))]
    public class UILogDepth : UIBehaviour
    {
        void Start()
        {
            //CanvasRenderer renderer =  this.GetComponent<CanvasRenderer>();

            Image image = this.GetComponent<Image>();
            
            Log.Debug(this.name + "image order : "  + image.canvas.sortingOrder); 
            
            //Log.Debug(this.name + " absoluteDepth：" + renderer.absoluteDepth);
            
            //Log.Debug(this.name + " relativeDepth：" + renderer.relativeDepth);
            
            
            

             //Image image;
             //image.
        }
    }
}