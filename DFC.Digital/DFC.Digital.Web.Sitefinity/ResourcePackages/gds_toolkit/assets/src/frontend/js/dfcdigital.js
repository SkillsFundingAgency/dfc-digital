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

$(document).ready(function () {
    $(".js-search-focus").ready(function () { dfc.digital.addFocus(".js-search-focus"); }).focus(function () { dfc.digital.addFocus(this) }).blur(function () { dfc.digital.addFocus(this) });

    //Survey Show Hide / Cookie Functionality

    $(".js-survey_open").click(function () {
        $("#js-survey-form").removeClass("js-hidden");
        $(".js-survey-intro").addClass("js-hidden");
    });

    $(".js-survey_close").click(function () {
        GOVUK.cookie('survey', 'dismissed', { days: 31 });
        $(".survey_container").removeClass("visible");
    });

    if (GOVUK.cookie('survey') !== "dismissed") {
        $(".survey_container").addClass("visible");
    };

    $(".js-survey-button").click(function (e) {
        sendEmail(e);
    });

    $(".survey_link").click(function () {
        GOVUK.cookie('survey', 'dismissed', { days: 31 });
    });

    //Filters Non Applicable functinality
    $(".filter-na").change(function () {
        if (this.checked) {
            $('input:checked').not(".filter-na").removeAttr('checked');
            this.change
        }
    });

    $('input:checkbox').not(".filter-na").change(function () {
        if ($(".filter-na").prop('checked')) {
            $(".filter-na").removeAttr('checked');
        }
    });

});

function sendEmail(e) {
    var emailAddress = $('input[name=EmailAddress]').val();

    if (emailAddress.length && $('.survey_form')[0].checkValidity()) {
        e.preventDefault();
        $("#js-survey-form").addClass("js-hidden");
        $.post('/govnotifyemail',
            { emailaddress: emailAddress },
            function (result) {
                DisplayResponseMessage(result);
            });
    }
}
function DisplayResponseMessage(result) {
    if (result === true) {
        GOVUK.cookie('survey', 'dismissed', { days: 31 });
        $(".js-survey-complete").removeClass("js-hidden");
    } else {
        $(".js-survey-not-complete").removeClass("js-hidden");
    }
}

$.extend($.ui.autocomplete.prototype, {
    _resizeMenu: function () {
        this.menu.element.outerWidth(this.element[0].clientWidth);
    }
});

//get all input boxes with class "autocomplete"
$('.js-autocomplete').each(function () {
    $(this).autocomplete({
        source: $(this).data("autocomplete-source") + '?maxNumberDisplyed=' + $(this).data("autocomplete-maxnumberdisplyed") + '&fuzzySearch=' + $(this).data('autocomplete-fuzzysearch'),
        minLength: $(this).data('autocomplete-minlength')
    });
});