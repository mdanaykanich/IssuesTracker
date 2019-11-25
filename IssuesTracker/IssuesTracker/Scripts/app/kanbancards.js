$('.kanban-ul').sortable({
    connectWith: '.connectedSortable'
}).disableSelection();
$('#newIssues').sortable({
    receive: function (ev, ui) {
        if (ui.item[0].dataset.type == 'InProgress' || ui.item[0].dataset.type == 'Done') {
            ui.sender.sortable('cancel');
        }
    }
});
$('#inprogressIssues').sortable({
    receive: function (ev, ui) {
        if (ui.item[0].dataset.type == 'Done') {
            ui.sender.sortable('cancel');
        }
        else {
            ui.item[0].dataset.type = 'InProgress';
            let issue = {
                issueId: ui.item[0].id,
                issueType: ui.item[0].dataset.type
            };
            changeType(issue);
        }
    }
});
$('#doneIssues').sortable({
    receive: function (ev, ui) {
        ui.item[0].dataset.type = 'Done';
        let issue = {
            issueId: ui.item[0].id,
            issueType: ui.item[0].dataset.type
        };
        changeType(issue);
    }
});
function changeType(issue) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: '/Home/UpdateType',
        dataType: 'json',
        data: JSON.stringify(issue),
        success: function (result) {
        }
    });
}
