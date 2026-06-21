window.AppModal = {
    defaultOptions: {
        visible: false,
        modal: true,
        actions: ["Close"],
        appendTo: document.body,
        draggable: true,
        resizable: false
    },

    markAsModal: function (kendoWindow) {
        if (!kendoWindow) {
            return;
        }

        if (kendoWindow.wrapper && kendoWindow.wrapper.addClass) {
            kendoWindow.wrapper.addClass("app-modal");
            return;
        }

        kendoWindow.one("activate", function () {
            if (kendoWindow.wrapper) {
                kendoWindow.wrapper.addClass("app-modal");
            }
        });
    },

    ensure: function (selector, options) {
        var $element = $(selector);
        var kendoWindow = $element.data("kendoWindow");

        if (kendoWindow) {
            return kendoWindow;
        }

        $element.kendoWindow($.extend({}, this.defaultOptions, options || {}));
        kendoWindow = $element.data("kendoWindow");
        this.markAsModal(kendoWindow);

        kendoWindow.bind("activate", function () {
            AppModal.recenter(this);
        });

        return kendoWindow;
    },

    recenter: function (kendoWindow) {
        if (!kendoWindow || !kendoWindow.center) {
            return;
        }

        kendoWindow.center();
        setTimeout(function () {
            kendoWindow.center();
        }, 0);
        setTimeout(function () {
            kendoWindow.center();
        }, 150);
    },

    open: function (kendoWindow, afterOpenFn) {
        if (!kendoWindow) {
            return;
        }

        kendoWindow.open();

        if (afterOpenFn) {
            afterOpenFn();
        }

        this.recenter(kendoWindow);
    }
};
