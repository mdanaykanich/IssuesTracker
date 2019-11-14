$('#summary').on('input', (function (e) {
    let dis = $('#summary').val().length >= 4;
    $('#save').attr('disabled', !dis);
}));
$('#save').on('click', function () {
    let action;
    let issue = {
        Id: $('#issueId').val(),
        Summary: $('#summary').val(),
        Description: $('#description').val(),
        Type: $('#selectType').val(),
        Assignee: $('#assignee').val(),
        Priority: $('#selectPriority').val(),
        ProjectName: $('#projectName').val(),
        ProjectId: $('#projectId').text()
    }
    if (issue.Id) {
        action = 'EditIssue';
    }
    else {
        action = 'AddIssue';
        issue.Id = '';
    }
    $.ajax({
        type: 'POST',
        url: '/Home/' + action,
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(issue),
        success: function (result) {
            $('#issueModal').modal('hide');
            fillGrid(issue.ProjectId);
        },
        error: function (error) {
            alert('Wrong data entry');
        }
    });
});
function fillGrid(projectId) {
    $.ajax({
        type: 'GET',
        url: '/Home/GetGridIssues/' + projectId,
        dataType: 'html',
        success: function (result) {
            $('#result').html(result);
        }
    });
}