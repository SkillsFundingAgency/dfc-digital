$('#DateOfBirth').removeAttr("data-val-date");
$(document).ready(function () {
    $("button[type='submit']").click(function () {
        $('#error-validation-summary').hide();
        $('#error-validation-summary .govuk-error-summary__body ul').empty();
        if ($('#DateOfBirth'))
        {
	        PopulateDateOfBirth();
        }

        var validator = $('form').validate();
        if ($('form').valid()) {
            $("form").submit();
        } else {
            $('#error-validation-summary').show();

            for (var i = 0; i < validator.errorList.length; i++) {
                var linkElement = validator.errorList[i].element.name.split('.').join('_');
                $('#error-validation-summary .govuk-error-summary__body ul')
                    .append('<li><a href="#' + linkElement + '">' + validator.errorList[i].message + '</a></li>');
            }

            $('html,body').animate({
                scrollTop: $("#main-content").offset().top
            }, 0);

            return false;
        }
    });

    $("#DateOfBirthDay").change(function () {
        PopulateDateOfBirth();
    });

    $("#DateOfBirthMonth").change(function () {
        PopulateDateOfBirth();
    });

    $("#DateOfBirthYear").change(function () {
        PopulateDateOfBirth();
    });

    var validator = $("form").validate();
    if(validator)
    {
       validator.settings.ignore = [];
    }
});

$.validator.setDefaults({
    highlight: function (element) {
        if ($(element).attr('id') === "DateOfBirthDay" ||
            $(element).attr('id') === "DateOfBirthMonth" ||
            $(element).attr('id') === "DateOfBirth" ||
            $(element).attr('id') === "DateOfBirthYear") {
            $('#dobDiv').addClass('govuk-form-group--error');
        }
        else {
            $(element).closest(".govuk-form-group").addClass("govuk-form-group--error");
        }
    },
    unhighlight: function (element) {
        if ($(element).attr('id') === "DateOfBirthDay" ||
            $(element).attr('id') === "DateOfBirthMonth" ||
            $(element).attr('id') === "DateOfBirth" ||
            $(element).attr('id') === "DateOfBirthYear") {
            var otherValidationErrors = $("#dobDiv").find(".field-validation-error");

            if (otherValidationErrors.length === 0) {
                $('#dobDiv').removeClass('govuk-form-group--error');
            }
        } else {
            $(element).closest(".govuk-form-group").removeClass("govuk-form-group--error");
        }
    }
});

//// *** Date of Birth Validation ***

function PopulateDateOfBirth() {
    var dobDay = $('#DateOfBirthDay').val();
    var dobMonth = $('#DateOfBirthMonth').val();
    var dobDayYear = $('#DateOfBirthYear').val();

    if (dobDay !== "" && dobMonth !== "" && dobDayYear !== "") {
        var dateOfBirth = "";
        if (dobDay || dobMonth || dobDayYear) {
            dateOfBirth = dobDay + '/' + dobMonth + '/' + dobDayYear;
        }

        $('#DateOfBirth').val(dateOfBirth);
        var validator = $("form").validate();
        validator.element("#DateOfBirth");
    }
}

//// *** Custom Attributes area ***
jQuery.validator.addMethod("enforcetrue", function (value, element, param) {
    return element.checked;
});

jQuery.validator.unobtrusive.adapters.addBool("enforcetrue");

jQuery.validator.addMethod("agerange", function (value, element, param) {
    if (value === "" || value === null || value === undefined) {
        return true;
    }

    var dates = param["dates"];
    var errormessages = param["errormessages"];
    var failedDatesErrorMessages = new Array();

    var minAge = parseInt(dates[0]);
    var maxAge = parseInt(dates[1]);

    var minAgeErrorMessage = errormessages[0];
    var maxAgeErrorMessage = errormessages[1];
    var invalidAgeErrorMessage = errormessages[2];

    var dateParts = value.split('/');
    var entryDay = parseInt(dateParts[0]);
    var entryMonth = parseInt(dateParts[1]) - 1;
    var entryYear = parseInt(dateParts[2]);
    var entryDate = new Date(entryYear, entryMonth, entryDay);

    if (entryDate.getFullYear() === entryYear && entryDate.getMonth() === entryMonth && entryDate.getDate() === entryDay) {
        if (Object.prototype.toString.call(entryDate) === "[object Date]") {
            // it is a date type
            if (isNaN(entryDate.getTime())) {  // entryDate.valueOf() could also work
                // date is not valid
                failedDatesErrorMessages[0] = invalidAgeErrorMessage;
            }
            else {
                // date is valid
                var minYear = entryYear + minAge;
                var minAgeDate = new Date(minYear, entryMonth, entryDay);

                var maxYear = entryYear + maxAge;
                var maxAgeDate = new Date(maxYear, entryMonth, entryDay);

                var today = new Date();
                today.setHours(0, 0, 0, 0);

                if (minAgeDate > today) {
                    failedDatesErrorMessages[0] = minAgeErrorMessage;
                }
                else if (maxAgeDate < today) {
                    failedDatesErrorMessages[0] = maxAgeErrorMessage;
                }
                else {
                    return true;
                }
            }
        }
        else {
            // not a date
            failedDatesErrorMessages[0] = invalidAgeErrorMessage;
        }
    }
    else {
        // not valid date - day, month, year don't match before and after Date parsing
        failedDatesErrorMessages[0] = invalidAgeErrorMessage;
    }
    if (failedDatesErrorMessages.length > 0) {
        $.validator.messages.agerange = failedDatesErrorMessages.toString();
        return false;
    }
});

$.validator.unobtrusive.adapters.add('agerange', ['dates', 'errormessages'], function (options) {
    options.rules['agerange'] = {
        dates: options.params['dates'].split(' '),
        errormessages: options.params['errormessages'].split(',')
    };
});

jQuery.validator.addMethod("daterange", function (value, element, param) {
    if (value === "" || value === null || value === undefined) {
        return true;
    }

    var dates = param["dates"];
    var errormessages = param["errormessages"];
    var failedDatesErrorMessages = new Array();

    var startDateDay = parseInt(dates[0]);
    var endDateDay = parseInt(dates[1]);

    var dateRangeErrorMessage = errormessages[0];
    var invalidErrorMessage = errormessages[1];

    var dateParts = value.split('-');
    var entryYear = parseInt(dateParts[0]);
    var entryMonth = parseInt(dateParts[1]) - 1;
    var entryDay = parseInt(dateParts[2]);
    var entryDate = new Date(entryYear, entryMonth, entryDay);

    if (entryDate.getFullYear() === entryYear && entryDate.getMonth() === entryMonth && entryDate.getDate() === entryDay) {
        if (Object.prototype.toString.call(entryDate) === "[object Date]") {
            // it is a date type
            if (isNaN(entryDate.getTime())) {  // entryDate.valueOf() could also work
                // date is not valid
                failedDatesErrorMessages[0] = invalidErrorMessage;
            }
            else {
                // date is valid
                var validStartDate = new Date();
                validStartDate.setDate(validStartDate.getDate() - startDateDay - 1);
                var validEndDate = new Date();
                validEndDate.setDate(validEndDate.getDate() + endDateDay);

                if (entryDate >= validStartDate && entryDate <= validEndDate) {
                    return true;
                }
                else {
                    failedDatesErrorMessages[0] = dateRangeErrorMessage;
                }
            }
        }
        else {
            // not a date
            failedDatesErrorMessages[0] = invalidErrorMessage;
        }
    }
    else {
        // not valid date - day, month, year don't match before and after Date parsing
        failedDatesErrorMessages[0] = invalidErrorMessage;
    }

    if (failedDatesErrorMessages.length > 0) {
        $.validator.messages.daterange = failedDatesErrorMessages.toString();
        return false;
    }
});

$.validator.unobtrusive.adapters.add('daterange', ['dates', 'errormessages'], function (options) {
    options.rules['daterange'] = {
        dates: options.params['dates'].split(' '),
        errormessages: options.params['errormessages'].split(',')
    };
});

jQuery.validator.addMethod("doubleregex", function (value, element, param) {
    var firstRegex = param["firstregex"];
    var secondRegex = param["secondregex"];
    var drErrorMessage = param["drerrormessage"];
    var isAndOperator = param["isandoperator"];
    var isDrRequired = param["isdrrequired"];

    var failedPatternErrorMessages = new Array();

    if (isDrRequired === "False" && (value === "" || value === null || value === undefined)) {
        return true;
    }
    else {
        try {
            var firstMatch = new RegExp(firstRegex).exec(value);
            var secondMatch = new RegExp(secondRegex).exec(value);

            if (isAndOperator === "True") {

                if (firstMatch && secondMatch) {
                    return true;
                }
                else {
                    failedPatternErrorMessages[0] = drErrorMessage;
                }
            }
            else {

                if (firstMatch || secondMatch) {
                    return true;
                }
                else {
                    failedPatternErrorMessages[0] = drErrorMessage;
                }
            }
        }
        catch (err) {
            //console.log(err);
            return true;
        }
    }

    if (failedPatternErrorMessages.length > 0) {
        $.validator.messages.doubleregex = failedPatternErrorMessages.toString();
        return false;
    }
});

$.validator.unobtrusive.adapters.add('doubleregex', ['firstregex', 'secondregex', 'drerrormessage', 'isandoperator', 'isdrrequired'], function (options) {
    options.rules['doubleregex'] = {
        firstregex: options.params.firstregex,
        secondregex: options.params.secondregex,
        drerrormessage: options.params.drerrormessage,
        isandoperator: options.params.isandoperator,
        isdrrequired: options.params.isdrrequired
    };
});

$(function () {
    // Replace the builtin US date validation with UK date validation
    $.validator.addMethod(
        "date",
        function (value, element) {
            
            var bits = value.match(/([0-9]+)/gi), str;
            if (!bits)
                return this.optional(element) || false;
            str = bits[1] + '/' + bits[0] + '/' + bits[2];
            return this.optional(element) || !/Invalid|NaN/.test(new Date(str));
        },
        ""
    );
});