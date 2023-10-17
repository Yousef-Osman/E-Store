function getData(selector, url, columns, params = [], dir = null) {

    var table = $(selector).DataTable({
        scrollX: true,
        scrollCollapse: true,
        fixedHeader: {
            header: true,
            footer: true
        },
        processing: true,
        serverSide: true,
        searching: true,
        lengthChange: true,
        info: true,
        ordering: true,
        filter: true,
        order: [],
        autoWidth: true,
        ajax: {
            url: url,
            async: true,
            global: false,
            type: "Post",
            data: {
                parameters: params
            },
            dataSrc: function (json) {
                return json.data;
            }
        },
        columns: columns
    });

    return table;
}