using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace TreeViewRewrite.ServerSide.TreeViews;

/// <summary>
/// Every collapse creates a flat list.
/// </summary>
public partial class TreeViewDisplay<TItem> : ComponentBase
{
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;
    
    /// <summary>
    /// You cannot modify the list that you provide here.
    /// Enumeration was modified exceptions are possible if you do modify it.
    /// </summary>
    [Parameter, EditorRequired]
    public IReadOnlyList<TItem> ItemList { get; set; }
    /// <summary>
    /// Every node has a set height (<see cref="LineHeight"/>). If you exceed this, your content will get cut off.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<TItem> NodeRenderFragment { get; set; } = null!;
    
    /// <summary>
    /// If non-null, this will replace the default browser's context menu.
    /// If null, then the default browser's context menu will be used.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ContextMenuRenderFragment { get; set; }

    private Guid _guidId = Guid.NewGuid();
    private string _htmlId = null!;
    private int _caretRowTop = 0;
    private TreeViewMeasurements _treeViewMeasurements;
    
    private bool _isFocused;
    
    private string CaretRowCssClass => _isFocused
        ? "di_common_treeview-caret-row di_active"
        : "di_common_treeview-caret-row";
        
    private int _lineHeight = 20;
    
    private int LineHeight
    {
        get => _lineHeight;
        set
        {
            // Avoid divide by 0 exceptions
            if (value == 0)
                value = 1;
            
            _lineHeight = value;
            CalculateCaretRowTop();
        }
    }
    
    private int _index;
    
    private int Index
    {
        get => _index;
        set
        {
            // It is presumed that all resulting index math is correct and won't be less than 0.
            // This is because some actions might "wrap around" or other special ways to handle the validation.
        
            _index = value;
            CalculateCaretRowTop();
        }
    }
    
    private bool _showContextMenu;
    private TItem _contextMenuTarget;
    
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
    
    private async Task HandleOnClick(MouseEventArgs mouseEventArgs)
    {
        _treeViewMeasurements = await JsRuntime.InvokeAsync<TreeViewMeasurements>("treeViewRewrite.focusAndMeasureTreeView", _htmlId, false);
        
        var relativeY = mouseEventArgs.ClientY - _treeViewMeasurements.BoundingClientRectTop;
        relativeY = Math.Max(0, relativeY);
        
        var indexLocal = (int)(relativeY / LineHeight);
        
        Index = IndexBasicValidation(indexLocal);
        
        Console.WriteLine(_treeViewMeasurements);
    }
    
    private void HandleOnKeyDown(KeyboardEventArgs keyboardEventArgs)
    {
        int indexLocal;
    
        switch (keyboardEventArgs.Key)
        {
            case "ArrowDown":
                indexLocal = Index + 1;
                break;
            case "ArrowUp":
                indexLocal = Index - 1;
                break;
            default:
                indexLocal = Index;
                break;
        }
        
        Index = IndexBasicValidation(indexLocal);
    }
    
    private void HandleOnFocus()
    {
        _isFocused = true;
    }
    
    private void HandleOnBlur()
    {
        _isFocused = false;
    }
    
    private void HandleOnContextMenu()
    {
        ContextMenuLogic(clientY: null);
    }
    
    /// <summary>
    /// 'contextmenu' and 'onmousedown' share this logic.
    ///
    /// HandleOnContextMenu(...)
    /// - will invoke this method with 'clientY: null'.
    ///
    /// HandleOnMouseDown(...)
    /// - will invoke this method with 'clientY: mouseEventArgs.ClientY'.
    /// </summary>
    private void ContextMenuLogic(double? clientY)
    {
        _showContextMenu = true;
        
        if (clientY is null)
        {
            // _contextMenuTarget = ;
        }
        else
        {
            // _contextMenuTarget = ;
        }
    }
    
    private void CalculateCaretRowTop()
    {
        _caretRowTop = Index * LineHeight;
    }
    
    private int IndexBasicValidation(int indexLocal)
    {
        if (indexLocal < 0)
            return 0;
        else if (indexLocal >= ItemList.Count)
            return ItemList.Count - 1;
        
        return indexLocal;
    }
}
