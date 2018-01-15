

function ConfirmDelete() {
    var confirmedDelete = document.createElement("INPUT");
    confirmedDelete.name = "confirmedDelete";
    if (confirm("Do you want to delete the record?")) {
        confirmedDelete.value = "yes";
    }
    else {
        confirmedDelete.value = "no";
    }
    document.forms[0].appendChild(confirmedDelete);
}