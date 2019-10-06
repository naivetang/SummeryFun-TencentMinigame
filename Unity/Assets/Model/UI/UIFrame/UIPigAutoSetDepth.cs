using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [RequireComponent(typeof(Image))]
    class UIPigAutoSetDepth : MonoBehaviour
    {
        private Image image;

        private Canvas canvas;

        private BoxCollider2D boxCollider2D;

        private Transform player;

        private string AutoDepthTag = "AutoDepth";


        /// <summary>
        /// 未进入任何其他UI区域，最低
        /// </summary>
        private int depth1 = 201;
        
        /// <summary>
        /// 树 > 猪 > 人
        /// </summary>
        private int depth2 = 205;
        
        /// <summary>
        /// 人 > 猪 > 树
        /// </summary>
        private int depth3 = 220;
        
        /// <summary>
        /// 猪 > 树 > 人
        /// </summary>
        private int depth4 = 245;
        
        private List<Transform> stayTramsList = new List<Transform>();

        void Start()
        {

            this.InitImage();

            this.InitCanvas();

            this.InitCollider();

            this.InitPlayer();
        }

        private void LateUpdate()
        {

        }



        void InitImage()
        {
            this.image = this.GetComponent<Image>();
        }

        void InitCanvas()
        {
            this.canvas = this.gameObject.AddComponent<Canvas>();

            this.canvas.overrideSorting = true;

            this.canvas.sortingOrder = this.depth1;
        }

        void InitCollider()
        {
            this.boxCollider2D = this.gameObject.AddComponent<BoxCollider2D>();

            this.boxCollider2D.isTrigger = true;

            float offectY = (image.rectTransform.pivot.y - 0.5f) * image.rectTransform.sizeDelta.y;

            float offectX = (image.rectTransform.pivot.x - 0.5f) * image.rectTransform.sizeDelta.x;

            this.boxCollider2D.offset = new Vector2(-offectX, -offectY);

            RectTransform rt = this.transform as RectTransform;

            this.boxCollider2D.size = rt.sizeDelta;
        }

        void InitPlayer()
        {
            //this.player = Game.Scene.GetComponent<UnitComponent>().MyUnit.GameObject.transform;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Log.Warning("进入 ： " + this.name );

            Transform colliderTransform = collision.transform;

            string tag = colliderTransform.tag;

            if (tag.Equals("Player"))
            {
                this.player = colliderTransform;

                this.UpdateCanvasDepth();
            }
            else if (tag.Equals(this.AutoDepthTag))
            {
                this.stayTramsList.Add(colliderTransform);

                this.UpdateCanvasDepth();
            }
        }

        /// <summary>
        /// 根据Y值排序进入猪身体的所有UI
        /// </summary>
        int SortStayListByPosy(Transform a, Transform b)
        {
            return (int)(a.position.y - b.position.y);
        }

        void CheckPlayerSort()
        {
            if(this.player != null || this.stayTramsList.Count == 0)
                return;

            foreach (var tran in this.stayTramsList)
            {
                UIAutoSetDepth autoSetDepth = tran.GetComponent<UIAutoSetDepth>();

                if (autoSetDepth.player != null)
                {
                    this.player = autoSetDepth.player;
                    
                    break;
                }
            }
        }

        void UpdateCanvasDepth()
        {
            this.CheckPlayerSort();
            
            
        }


        void UpdateCanvasDepth2()
        {
            this.CheckPlayerSort();
            
            // 猪和其他UI
            if (this.player == null)
            {
                if (this.stayTramsList.Count == 0)
                {
                    this.canvas.sortingOrder = this.depth1;
                }
                else if (this.stayTramsList.Count == 1)
                {
                    if (this.stayTramsList[0].position.y > this.transform.position.y)
                    {
                        this.canvas.sortingOrder = this.depth4;
                    }
                    else
                    {
                        this.canvas.sortingOrder = this.depth1;
                    }
                }
                else if (this.stayTramsList.Count == 2)
                {
                    this.stayTramsList.Sort(this.SortStayListByPosy);

                    int beginSort = this.depth1;
                    
                    foreach (Transform transform1 in this.stayTramsList)
                    {
                        UIAutoSetDepth autoSetDepth = transform1.GetComponent<UIAutoSetDepth>();

                        if (autoSetDepth.EnterPigDepth == 0)
                        {
                            autoSetDepth.EnterPigDepth = autoSetDepth.canvas.sortingOrder;
                        }

                        autoSetDepth.canvas.sortingOrder = beginSort;
                        ++beginSort;
                    }
                }
                else
                {
                    Log.Error("猪进入多个UI Collider");

                    foreach (Transform transform1 in this.stayTramsList)
                    {
                        Log.Error(transform1.name);
                    }
                }
            }
            // 猪和人，或者 猪和人和UI
            else
            {
                if (this.stayTramsList.Count == 0)
                {
                    if (this.player.position.y > this.transform.position.y)
                    {
                        this.canvas.sortingOrder = this.depth3;
                    }
                    else
                    {
                        this.canvas.sortingOrder = this.depth1;
                    }
                }
                else if (this.stayTramsList.Count == 1)
                {
                    // 猪层级大于UI
                    if (this.stayTramsList[0].position.y > this.transform.position.y)
                    {
                        if (this.player.position.y > this.transform.position.y)
                        {
                            // 猪层级 最大
                            this.canvas.sortingOrder = this.depth4;
                        }
                        else
                        {

                            // UI层级 < 猪层级 < 人层级
                            this.canvas.sortingOrder = this.depth2;

                            return;
                        }
                    }
                    // 猪层级小于UI
                    else
                    {
                        if (this.player.position.y > this.transform.position.y)
                        {
                            // 猪层级小于人
                            this.canvas.sortingOrder = this.depth1;
                        }
                        else
                        {
                            // 人层级 < 猪层级 < UI层级
                            this.canvas.sortingOrder = this.depth3;
                        }
                    }
                }
                else
                {
                    Log.Error("猪进入多个UI Collider");

                    foreach (Transform transform1 in this.stayTramsList)
                    {
                        Log.Error(transform1.name);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Log.Warning("离开 ： " + this.name);

            Transform colliderTransform = collision.transform;

            string tag = colliderTransform.tag;

            if (tag.Equals("Player"))
            {
                this.player = null;
                
                this.UpdateCanvasDepth();
            }
            else if (tag.Equals(this.AutoDepthTag))
            {
                this.stayTramsList.Remove(colliderTransform);
                
                this.UpdateCanvasDepth();
            }
            
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
           this.UpdateCanvasDepth();
        }

        // private void OnCollisionEnter2D(Collision2D collision)
        // {
        //     OnTriggerEnter2D(collision.collider);
        // }
        //
        // private void OnCollisionExit2D(Collision2D collision)
        // {
        //     this.OnTriggerExit2D(collision.collider);
        // }
        //
        // private void OnCollisionStay2D(Collision2D collision)
        // {
        //     OnTriggerStay2D(collision.collider);
        // }


    }
}
