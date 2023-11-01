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
$('#downloadSettingsButton').on('click', function downloadSettings() {
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
        error: function (error) {
            console.log("Error attempting to fetch settings", error);
        }
    });
});

// Function to upload data from a file
$("#replaceSettingsButton").on("click", function () {
    const fileInput = document.getElementById('uploadSettingsFile');
    const file = fileInput.files[0];
    if (!file) {
        alert("Please select a file to upload first")
        return;
    }

    const reader = new FileReader();
    reader.onload = function (event) {
        const contents = event.target.result;
        try {
            const settings = JSON.parse(contents);
            sendNewSettingsToServer(settings);
        } catch (error) {
            console.log("Error parsing settings file", error);
            alert("Error parsing settings file: " + error.message);
        }
    }

    reader.readAsText(file);
});

// Function to send new settings to the server
function sendNewSettingsToServer(settings) {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/settings?handler=replace',
        method: 'POST',
        data: JSON.stringify(settings),
        contentType: 'application/json; charset=utf-8',
        headers: {
            'RequestVerificationToken': token
        },
        success: function () {
            window.location.reload();
        },
        error: function (error) {
            console.log("Error attempting to replace settings", error);
            alert("Error attempting to replace settings: " + error.responseText)
        }
    });
}