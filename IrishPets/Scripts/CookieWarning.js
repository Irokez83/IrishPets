function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}


function displayNotification() {

    // this sets the page background to semi-transparent black should work with all browsers
    var message = "<div id='cookiewarning' class='help-block alert alert-success alert-dismissable'>";

    // message = message + "<div class='container alert' style='font-size:120%;width:100%;background-color:#ffffff;margin:0px auto;text-align:center;padding:10px 0px;'>";

    // this is the message displayed to the user.
    message = message + "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>"
    message = message + "Note: <strong>This website uses cookies</strong>, some of which are essential for this site to operate.";
    message = message + "You can read how we use them in our privacy policy <input class='btn' type='button' value='Accept' onClick='JavaScript:setCookie(\"jsCookieCheck\",null,365);' />";
    message = message + "</div>";

    document.writeln(message);
}

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);

    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;

    // set cookiewarning to hidden.
    var cw = document.getElementById("cookiewarning");

    if (null != cw)
        cw.innerHTML = "";
}

function checkCookie() {

    var cookieName = "jsCookieCheck";

    var cookieChk = getCookie(cookieName);

    if (null != cookieChk && "" != cookieChk) {
        // the jsCookieCheck cookie exists so we can assume the person has read the notification within the last year
        setCookie(cookieName, cookieChk, 365);  // set the cookie to expire in a year.
    }
    else {
        // No cookie exists, so display the lightbox effect notification.
        displayNotification();
    }
}

checkCookie();