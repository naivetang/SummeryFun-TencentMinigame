namespace ETModel
{
    [Event(EventIdType.OpenBook)]
    public class UIBookEventHandler : AEvent<int>
    {
        public override async void Run(int index)
        {
            //Game.Scene.GetComponent<UIComponent>().
            
            UIBase com = await UIFactory.Create<UIBookComponent>(ViewLayer.UIPopupLayer, UIType.UIBook);
            
            com.GetComponent<UIBookComponent>().ShowPicture(index);

            
            
        }
    }
}