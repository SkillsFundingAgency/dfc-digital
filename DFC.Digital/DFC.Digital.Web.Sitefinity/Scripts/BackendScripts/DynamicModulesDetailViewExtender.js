var extUrl = "/";
// called by the DetailFormView when it is loaded
function OnDetailViewLoadedCustom(sender, args) {
    // the sender here is DetailFormView
    var currentForm = sender;
    
    $.ajax({
        type: "GET",
        url: "/RestApi/CompUIConfigSettingService/GetFromKey/DFC.Digital.MicroService.HelpPreviewEndPoint",
        contentType: "application/json",
        dataType: "json",
        success: function (result) { extUrl = result },
        error: function (error) { console.log(error); },
        async: false
    });

    DynamicModulesDetailViewExtender.initialize(currentForm);
}


var DynamicModulesDetailViewExtender = (function ($) {

    var extender = function (detailsView) {
        this._detailsView = detailsView;
        this._baseSaveChanges = null;
    };


    extender.prototype = {
        init: function () {
            var element = this._detailsView.get_element();

            this._saveChangesExtendedDelegate = createDelegate(this, this._widgetCommandHandlerExtended);

            if (this._detailsView._widgetBarIds) {
                var wLength = this._detailsView._widgetBarIds.length;
                for (var wCounter = 0; wCounter < wLength; wCounter++) {
                    var widget = $find(this._detailsView._widgetBarIds[wCounter]);
                    if (widget !== null) {
                        if (this._detailsView._widgetCommandDelegate != null) {
                            widget.remove_command(this._detailsView._widgetCommandDelegate);
                        }
                    }
                }
            }

            this._detailsView._widgetCommandDelegate = this._saveChangesExtendedDelegate;

            // subscribe the WidgetBar Events againg with the new _widgetCommandDelegate
            this._detailsView._subscribeToWidgetBarEvents();
        },

        // fired when one of the toolbars fires a command event
        _widgetCommandHandlerExtended: function (sender, args) {
            var commandName = args.get_commandName();
            if (commandName == this._detailsView._previewCommandName) {
                var id = this._detailsView.get_dataItem().Item.OriginalContentId;
                var itemUrl = this._detailsView.get_dataItem().Item.SystemUrl;

                window.open(extUrl + "/job-profiles" + itemUrl);
            }
            else {
                // no need to revert the delegate since we set it through widgetBar command handler
                // call base
                this._detailsView._widgetCommandHandler(sender, args);
            }
        },
    };

    return {
        initialize: function (detailsView) {
            if (!detailsView) {
                throw new Error("detailsView is not defined");
            }

            var detailsViewExtender = new extender(detailsView);
            detailsViewExtender.init();
        }
    };

}(jQuery));

// --- Helpers ---

// From Microsoft.Ajax - Function.createDelegate
function createDelegate(a, b) {
    return function () { return b.apply(a, arguments) }
};