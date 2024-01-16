using NinjutsuGames.StateMachine.Editor;

public class CustomToolbarView : ToolbarView
{
    private readonly MiniMapView miniMapView;

    public CustomToolbarView(BaseGraphView graphView, MiniMapView miniMapView) : base(graphView)
    {
        this.miniMapView = miniMapView;
    }

    protected override void AddButtons()
    {
        // Add the hello world button on the left of the toolbar
        // AddButton("Hello !", () => Debug.Log("Hello World"), left: false);

        // add the default buttons (center, show processor and show in project)
        base.AddButtons();

        AddToggle("Minimap", miniMapView.visible, v => miniMapView.visible = v, false);
    }
}