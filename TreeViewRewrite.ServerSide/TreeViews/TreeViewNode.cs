namespace TreeViewRewrite.ServerSide.TreeViews;

public struct TreeViewNode<TItem>
{
    public TreeViewNode(TItem item, int depth)
    {
        Item = item;
        Depth = depth;
    }

    public TItem Item { get; set; }
    public int Depth { get; set; }
}
