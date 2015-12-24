$(function () {

    // reset the search form when the page loads
    $("form").each(function () {
        this.reset();
    });

    // wire up the events to the <input> element for search/filter
    $("input").bind("keyup change", function () {
        var searchtxt = this.value.toLowerCase();
        var items = $(".list-group-item");

        // show all speakers that begin with the typed text and hide others
        for (var i = 0; i < items.length; i++) {
            var val = items[i].text.toLowerCase();
            val = val.substring(0, searchtxt.length);
            if (val == searchtxt) {
                $(items[i]).show();
            }
            else {
                $(items[i]).hide();
            }
        }
    });
});