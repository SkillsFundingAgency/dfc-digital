; $(function () {
    $(".sfFooter").hide();  //hide sitefinity footer as it sometimes ends up in the middle od the page.
    $('.mvc-grid').mvcgrid();   //bind grid

    TableExport($(".mvc-grid table"), {
        headers: true,                              // (Boolean), display table headers (th or td elements) in the <thead>, (default: true)
        footers: true,                              // (Boolean), display table footers (th or td elements) in the <tfoot>, (default: false)
        formats: ['csv', 'txt'],                    // (String[]), filetype(s) for the export, (default: ['xlsx', 'csv', 'txt'])
        filename: 'id',                             // (id, String), filename for the downloaded file, (default: 'id')
        bootstrap: false,                           // (Boolean), style buttons using bootstrap, (default: true)
        exportButtons: true,                        // (Boolean), automatically generate the built-in export buttons for each of the specified formats (default: true)
        position: 'bottom'                          // (top, bottom), position of the caption element relative to table, (default: 'bottom')
    });
});