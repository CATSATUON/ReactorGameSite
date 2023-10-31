$(function () {
    $("#reorderable-list").sortable({
        update: function (event, ui) {
            var updatedOrder = $("#reorderable-list").sortable("toArray");
            // TODO: update the order
        }
    });
    $("#reorderable-list").disableSelection();
});

// Function to download data to a file
function downloadSettings() {
    $.ajax({
        url: '/settings?handler=Json',
        method: 'GET',
        success: function (data) {
            const filename = (data.name || "settings") + ".json";
            const blob = new Blob([JSON.stringify(data, null, 4)], { type: 'application/json' });
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        },
        error: function(error) {
            console.log("Error attempting to fetch settings", error);
        }
    });
}

// Connect the button to the download function
$('#downloadSettingsButton').on('click', downloadSettings);