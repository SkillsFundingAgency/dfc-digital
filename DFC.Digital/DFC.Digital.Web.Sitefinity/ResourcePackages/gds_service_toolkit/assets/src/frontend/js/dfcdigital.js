var dfc = (dfc || {});
dfc.digital = {
    addFocus: function (identifier) {
        var elm = $(identifier);
        if (elm && elm.val() && elm.val().length > 0) {
            elm.addClass("focus");
        } else {
            elm.removeClass("focus");
        }
    }
};
