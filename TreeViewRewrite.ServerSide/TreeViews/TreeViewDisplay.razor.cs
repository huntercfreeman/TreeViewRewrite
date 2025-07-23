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
    
    protected override void OnInitialized()
    {
        _htmlId = $"luth_common_treeview-{_guidId}";
    }
    
    private async Task HandleOnClick()
    {
        Console.WriteLine("HandleOnClick");
        await JsRuntime.InvokeVoidAsync("treeViewRewrite.focusHtmlElementById", _htmlId, false);
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
}
