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
$("#replaceSettingsButton").on("click",
    parseAndUpdateSettings.bind(null, true)
);

// Function to append data from file
$("#appendSettingsButton").on("click",
    parseAndUpdateSettings.bind(null, false)
);

function parseAndUpdateSettings(shouldReplace) {
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
            sendNewSettingsToServer(settings, shouldReplace);
        } catch (error) {
            console.log("Error parsing settings file", error);
            alert("Error parsing settings file: " + error.message);
        }
    }

    reader.readAsText(file);
}

// Function to send new settings to the server
function sendNewSettingsToServer(settings, shouldReplace) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var url = shouldReplace ? '/settings?handler=replace' : '/settings?handler=append';

    $.ajax({
        url: url,
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

// Function to add a scenario
$("#addScenarioButton").on("click", function () {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/settings?handler=createScenario',
        method: 'POST',
        headers: {
            'RequestVerificationToken': token
        },
        success: function () {
            window.location.reload();
        },
        error: function (error) {
            console.log("Error adding new scenario", error);
            alert("Error adding new scenario: " + error.responseText);
        }
    });
});

// Function to delete a scenario
$(document).on('click', '.removeScenarioButton', function () {
    var scenarioId = $(this).data('scenarioid');
    console.log("Deleting scenario", scenarioId);
    var token = $('input[name="__RequestVerificationToken"]').val();

    if (confirm('Are you sure you want to delete this scenario?')) {
        $.ajax({
            url: '/settings?handler=deleteScenario&scenarioId=' + scenarioId,
            method: 'POST',
            headers: {
                'RequestVerificationToken': token
            },
            success: function () {
                window.location.reload();
            },
            error: function (error) {
                console.log("Error deleting scenario", error);
                alert("Error deleting scenario: " + error.responseText);
            }
        });
    }
});