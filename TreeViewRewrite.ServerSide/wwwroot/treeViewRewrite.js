// The way JS Interop is done here is a bit outdated see export syntax:
// https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/

window.treeViewRewrite = {
	focusAndMeasureTreeView: function (elementId, preventScroll) {
        let element = document.getElementById(elementId);

        if (!element) {
            return {
                ViewWidth: 0,
                ViewHeight: 0,
                BoundingClientRectLeft: 0,
                BoundingClientRectTop: 0,
            };
        }

		if (preventScroll) {
			element.focus({preventScroll: true});
		}
		else {
			element.focus();
		}
		
		return this.measureTreeView(elementId);
    },
    measureTreeView: function (elementId) {
        let element = document.getElementById(elementId);

        if (!element) {
            return {
                ViewWidth: 0,
                ViewHeight: 0,
                BoundingClientRectLeft: 0,
                BoundingClientRectTop: 0,
            };
        }

		let boundingClientRect = element.getBoundingClientRect();
		
		return {
            ViewWidth: Math.ceil(element.offsetWidth),
            ViewHeight: Math.ceil(element.offsetHeight),
            BoundingClientRectLeft: boundingClientRect.left,
            BoundingClientRectTop: boundingClientRect.top,
        };
    }
}
