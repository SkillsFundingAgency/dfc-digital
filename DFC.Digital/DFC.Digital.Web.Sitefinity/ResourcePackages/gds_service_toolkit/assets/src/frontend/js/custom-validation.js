$(document).ready(function() {
    $("#formSubmit").click(function () {
      
        $('#error-validation-summary .govuk-error-summary__body ul').empty();

        var validator = $('#userform').validate();
        if ($('#userform').valid()) {
            $("#userform").submit(e);
        } else {
            $('#error-validation-summary').show();


            for (var i = 0; i < validator.errorList.length; i++) {
                var linkElement = validator.errorList[i].element.name.split('.').join('_');
                $('#error-validation-summary .govuk-error-summary__body ul').append('<li><a href="#' +
                    linkElement +
                    '">' +
                    validator.errorList[i].message +
                    '</a></li>');
            }

            $('html,body').animate({
                    scrollTop: $("#error-validation-summary").offset().top
                },
                2000);

            return false;
        }
    });
});

$.validator.setDefaults({
    highlight: function (element) {
        $(element).closest(".govuk-form-group").addClass("govuk-form-group--error");
    },
    unhighlight: function (element) {
        $(element).closest(".govuk-form-group").removeClass("govuk-form-group--error");
    }
});
