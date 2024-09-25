document.getElementById("allowSkip").addEventListener("change", function () {
    var json = JSON.stringify({ AllowSkip: this.checked });

    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch('/videos?handler=ToggleSkip', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: json
    })
        .then(response => response.json())
    .catch(error => console.error("Error", error));
});