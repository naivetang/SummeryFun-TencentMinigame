namespace ETModel
{
    [Event(EventIdType.OpenBook)]
    public class UIBookEventHandler : AEvent<int>
    {
        public override void Run(int index)
        {
            //Game.Scene.GetComponent<UIComponent>().
            
            UIBase com = UIFactory.Create<UIBookComponent>(ViewLayer.UIPopupLayer, UIType.UIBook).Result;
            
            com.GetComponent<UIBookComponent>().ShowPicture(index);

            
            
        }
    }
}