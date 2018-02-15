(function () {
    var appVirtualPath = document.getElementById('applicationVirtualPath') ? document.getElementById('applicationVirtualPath').value : '';
    var getURLParameter = function (name) {
        return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null
    };
    var redirectToReturnUrl = function () {
        var returnUrl = getURLParameter('ReturnUrl');
        window.location = returnUrl ? returnUrl : location.protocol() + '://' + location.host();
    };
    var getAppStatus = function () {
        var request = new XMLHttpRequest();
        request.open('GET', appVirtualPath + '/appstatus', false);
        request.send();
        if (request.status === 404) {
            redirectToReturnUrl();
        }
    };
    var getAppStatusInterval = function () {
        setInterval(function () {
            getAppStatus();
        }, 3000);
    };
    getAppStatus();
    getAppStatusInterval();
})();