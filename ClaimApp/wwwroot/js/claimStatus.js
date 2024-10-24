"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/claimStatusHub").build();

// Disable the button until the connection is established
document.getElementById("statusUpdateButton").disabled = true;

connection.on("ReceiveClaimStatusUpdate", function (claimId, status) {
    // Find the claim element and update its status
    var claimElement = document.getElementById("claim-status-" + claimId);
    if (claimElement) {
        claimElement.innerText = status; // Update status text
    }
});

// Start the connection and enable the button once connected
connection.start().then(function () {
    document.getElementById("statusUpdateButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

// If you're using a button to manually trigger updates
document.getElementById("statusUpdateButton").addEventListener("click", function (event) {
    var claimId = document.getElementById("claimId").value;
    var status = document.getElementById("claimStatus").value;

    connection.invoke("UpdateClaimStatus", claimId, status).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
