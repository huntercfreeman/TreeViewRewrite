using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace TreeViewRewrite.ServerSide.TreeViews;

/// <summary>
/// Every collapse creates a flat list.
/// </summary>
public partial class TreeViewDisplay : ComponentBase
{
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    private readonly List<int> _numberList = [1, 2, 3, 4];
    private Guid _guidId = Guid.NewGuid();
    private string _htmlId = null!;
    private int _index;
    private int _lineHeight = 20;
    private int _caretRowTop = 0;
    private TreeViewMeasurements _treeViewMeasurements;
    
    private bool _isFocused;
    
    private string CaretRowCssClass => _isFocused
        ? "di_common_treeview-caret-row di_active"
        : "di_common_treeview-caret-row";
    
    protected override void OnInitialized()
    {
        _htmlId = $"luth_common_treeview-{_guidId}";
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _treeViewMeasurements = await JsRuntime.InvokeAsync<TreeViewMeasurements>("treeViewRewrite.measureTreeView", _htmlId);
            Console.WriteLine(_treeViewMeasurements);
        }
    }
    
    private async Task HandleOnClick()
    {
        Console.WriteLine("HandleOnClick");
        _treeViewMeasurements = await JsRuntime.InvokeAsync<TreeViewMeasurements>("treeViewRewrite.focusAndMeasureTreeView", _htmlId, false);
        Console.WriteLine(_treeViewMeasurements);
    }
    
    private void HandleOnKeyDown(KeyboardEventArgs keyboardEventArgs)
    {
        switch (keyboardEventArgs.Key)
        {
            case "ArrowDown":
                _index++;
                break;
            case "ArrowUp":
                _index--;
                break;
        }
        
        _caretRowTop = _index * _lineHeight;
    }
    
    private void HandleOnFocus()
    {
        _isFocused = true;
    }
    
    private void HandleOnBlur()
    {
        _isFocused = false;
    }
}
