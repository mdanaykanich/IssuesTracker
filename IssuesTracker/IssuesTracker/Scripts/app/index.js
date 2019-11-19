$(function () {
    const selectProject = document.getElementById('project-select');
    const table = $('#result');
    let priorities;
    init(selectProject.options[selectProject.selectedIndex].value);
    function init(projectId) {
        getPriorities();
        eventHandler();
        fillGrid(projectId);
    }
    function eventHandler() {
        selectProject.addEventListener('change', function (event) {
            fillGrid(event.target.value);
        });
        $(document).on('click', '.btn-edit', function () {
            showIssueModal($(this).val());
        });
        $('#issueModal').on('hidden.bs.modal', function () {
            $("#selectType").find('option').remove();
            $("#selectPriority").find('option').remove();
        });
        $('#createIssue').on('click', function () {
            showIssueModal();
        });
        $('#createProject').on('click', function () {
            $('#createPrj').attr('disabled', 'true');
        });
        $('#projectName').on('input', (function (e) {
            let dis = $('#projectName').val().length >= 4;
            $('#createPrj').attr('disabled', !dis);
        }));
        $('#createPrj').on('click', function () {
            createProject();
            fillGrid(selectProject.options[selectProject.selectedIndex].value);
            $('#projectName').val("");
        });
    }
    function createProject() {
        $.ajax({
            type: 'POST',
            url: '/Home/AddProject',
            dataType: 'json',
            data: { projectName: $('#projectName').val() },
            success: function (result) {
                let option = document.createElement('option');
                option.setAttribute('value', result.Id);
                option.text = result.Name;
                selectProject.appendChild(option);
            }
        });
    }
    function getPriorities() {
        $.ajax({
            type: 'GET',
            url: '/Home/GetPriorities',
            dataType: 'json',
            success: function (result) {
                priorities = result;
            }
        });
    }
    function showIssueModal(issueId) {
        let issue = {
            projectId: $('#project-select').val(),
            id: issueId
        };
        $.ajax({
            type: 'POST',
            url: '/Home/IssueModal',
            contentType: 'application/json',
            dataType: 'html',
            data: JSON.stringify(issue),
            success: function (result) {
                $('#issueModal').html(result);
            }
        });
    }
    function fillGrid(projectId) {
        $.ajax({
            type: 'GET',
            url: '/Home/GetGridIssues/' + projectId,
            dataType: 'html',
            success: function (result) {
                table.html(result);
            }
        });
    }
});