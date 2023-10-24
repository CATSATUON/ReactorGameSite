$(function () {
    $("#reorderable-list").sortable({
        update: function (event, ui) {
            // Handle the updated order here, for example, by sending it to the server via AJAX.
            var updatedOrder = $("#reorderable-list").sortable("toArray");
            // You can use updatedOrder to send the new order to the server.
        }
    });
    $("#reorderable-list").disableSelection();
});