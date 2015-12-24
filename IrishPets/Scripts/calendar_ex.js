$("#sandbox-container input")
    .datepicker({
        format: "dd.mm.yyyy",
        weekStart: 1,
        autoclose: true,
        orientation: "bottom auto",
        multidateSeparator: ".",
        todayHighlight: true,
        toggleActive: true,
        todayBtn: "linked",
        clearBtn: true,
        calendarWeeks: true
    });
