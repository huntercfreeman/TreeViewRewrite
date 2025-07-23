namespace TreeViewRewrite.ServerSide.TreeViews;

/// <summary>All measurements are in pixels.</summary>
public record struct TreeViewMeasurements(
    int ViewWidth,
    int ViewHeight,
    double BoundingClientRectLeft,
    double BoundingClientRectTop);
