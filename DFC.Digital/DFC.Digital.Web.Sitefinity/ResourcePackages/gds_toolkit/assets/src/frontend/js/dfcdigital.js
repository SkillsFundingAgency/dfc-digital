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
            $('input:checked').not(".filter-na").prop('checked', false);
            this.change;
        }
    });

    $('input:checkbox').not(".filter-na").change(function () {
        if ($(".filter-na").prop('checked')) {
            $(".filter-na").prop('checked', false);
        }
    });

    //JP Thumbs Up / Down
    if (GOVUK.cookie('JPsurvey') != "dismissed") {
        $(".job-profile-feedback").addClass("js-visible");
    };

    $("#jp-feedback-yes").click(function () {
        GOVUK.cookie('JPsurvey', 'dismissed', { days: 31 });
        $(".job-profile-feedback-start").addClass("hidden");
        $(".job-profile-feedback-end-yes").removeClass("hidden");
    });

    $("#jp-feedback-no").click(function () {
        GOVUK.cookie('JPsurvey', 'dismissed', { days: 31 });
        $(".job-profile-feedback-start").addClass("hidden");
        $(".job-profile-feedback-end-no").removeClass("hidden");

    });

    $("#job-profile-feedback-survey").click(function (e) {
        e.preventDefault();
        var profileURL = window.location.href;
        sessionStorage.setItem("profileURL", profileURL);
        window.location = $(this).attr('href');
    });

    $("#job-profile-feedback-survey-finish").click(function (e) {
        e.preventDefault();
        var profileURL = sessionStorage.getItem("profileURL");
        if (sessionStorage.getItem("profileURL") !== null) {
            window.location = profileURL;
        }
        else {
            window.location = $(this).attr('href');
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
        source: function (term, response) {
            var searchTerm = term.term;
            var maxnumberCharacters = $('.js-autocomplete').data("autocomplete-maxlength");

            if (searchTerm.length > maxnumberCharacters) {
                return;
            }
            //else : fetch your data, and call the 'response' callback
            $.ajax({
                url: $('.js-autocomplete').data("autocomplete-source"),
                dataType: 'json',
                data: { 'term': searchTerm, 'accessibleSearch': 'False', 'maxNumberDisplayed' : $('.js-autocomplete').data("autocomplete-maxnumberdisplyed"), 'fuzzySearch' : $('.js-autocomplete').data('autocomplete-fuzzysearch') },
                success: function (data) {
                    response(data);
                }
            });
        }, 
        minLength: $(this).data('autocomplete-minlength')
    });
});


var element = document.querySelector('#my-autocomplete-container');
var id = 'my-autocomplete';
accessibleAutocomplete({
    element: element,
    id: id,
    source: suggest
});

function suggest(term, populateResults) {
    $.ajax({
        url: $('.js-autocomplete').data("autocomplete-source"),
        dataType: 'json',
        data: { 'term': term, 'accessibleSearch': 'True', 'maxNumberDisplayed': $('.js-autocomplete').data("autocomplete-maxnumberdisplyed"), 'fuzzySearch': $('.js-autocomplete').data('autocomplete-fuzzysearch') },
        success: function (data) {
            populateResults(data);
        }
    });
}