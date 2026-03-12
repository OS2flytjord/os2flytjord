// Jordflytning "namespace"
var Jordflytning = Jordflytning || {};

// Jordflytning.ui "namespace"
Jordflytning.ui = {
	/// Creates a modal window object
	createModalWindow: function(elementId, height, width, title, visible) {
		var win = $("#" + elementId).kendoWindow({
			height: height + "px",
			title: title,
			visible: visible,
			width: width + "px",
			modal: true
		}).data("kendoWindow");

		return win;
	}
};

// Jordflytning validation "namespace"
Jordflytning.val = {
	/// Marks labels for required inputs with *
	displayValidationMarkers: function() {
		$("form input[data-val='true']").each(function() {
			var lbl = $(this).prev("label");
			var reqText = $(this).attr("data-val-required");
			var reqIfText = $(this).attr("data-val-requiredif");
			if (typeof reqText != "undefined" || typeof reqIfText != "undefined") {
				lbl.html(lbl.text() + "<span class=\"requiredField\">*</span>");
			}
		});
	}
};

Jordflytning.url = Jordflytning.url || {
    getParameter: function (name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    },
    setParameter: function(name, value, forceReload) {
        var url = location.href;
        var parameter = this.getParameter(name);
        if (parameter !== value) {
            if (parameter !== '') {
                url = url.replace(name + '=' + parameter, name + '=' + value);
            } else {
                url += ((url.indexOf('?') > -1) ? '&' : '?') + name + '=' + value;
            }
            location.href = url;
        } else if (forceReload) {
            location.reload(true);
        };
    }
};