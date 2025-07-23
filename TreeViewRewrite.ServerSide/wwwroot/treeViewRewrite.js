// The way JS Interop is done here is a bit outdated see export syntax:
// https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/

window.treeViewRewrite = {
	focusHtmlElementById: function (elementId, preventScroll) {
        let element = document.getElementById(elementId);

        if (!element) {
            return;
        }

		if (preventScroll) {
			element.focus({preventScroll: true});
		}
		else {
			element.focus();
		}
    }
}
