$(function () {
    const selectProject = document.getElementById('project-select');
    const table = document.getElementById('result');
    let priorities;
    init(selectProject.options[selectProject.selectedIndex].value, table);

    function init(projectId, table) {
        getPriorities();
        getIssues(projectId, table);
        eventHandler();
    }
    function eventHandler() {
        selectProject.addEventListener('change', function (event) {
            getIssues(event.target.value, table);
        });
        $(document).on('click', '.btn-edit', function () {
            getIssue($(this).val());
        });
        $('#btnSave').on('click', function () {
            let flag;
            let action;
            let issue = {
                Id: parseInt($('#inpId').val()),
                Summary: $('#inpSummary').val(),
                Description: $('#txtareaDescr').val(),
                Type: $('#selectType').val(),
                Assignee: $('#inpAssignee').val(),
                Priority: $('#selectPriority').val(),
                ProjectName: $('#inpProjectName').val(),
                ProjectId: $('#lblProjectId').text()
            }
            if (issue.Id)
                action = 'EditIssue';
            else {
                action = 'AddIssue';
                issue.Id = '';
            }
            $.ajax({
                type: 'POST',
                url: `/Home/${action}`,
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(issue),
                success: function (result) {                  
                    $('#IssueModal').modal('hide');
                    getIssues(parseInt(issue.ProjectId), table);
                },
                error: function (error) {
                    alert('Wrong data entry');
                }
            });
        });
        $('#btnCreateIssue').on('click', function () {
            createIssue();
        });
        $('#IssueModal').on('hidden.bs.modal', function () {
            $("#selectType option").innerHTML='';
            $("#selectPriority option").innerHTML='';
        });
        $('#inpSummary').on('input', (function (e) {
            if ($('#inpSummary').val().length >= 4) {
                $('#btnSave').attr('disabled', false);
            }
            else {
                $('#btnSave').attr('disabled', true);
            }
        }));
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
    function createIssue() {
        $('#inpProjectName').val($('#project-select option:selected').html());
        $('#selectType').append($('<option selected></option>').attr('value', 'New').text('New'));
        $('#lblProjectId').text($('#project-select').val());
        $('#inpId').val('');
        $('#inpId').prop('disabled', true);
        $('#inpProjectName').prop('disabled', true);
        $('#selectType').prop('disabled', 'disabled');
        $('#inpSummary').val('');
        $('#txtareaDescr').val('');
        $('#inpAssignee').val();
        $.each(priorities, function (key, value) {
            if (value === 'Trivial')
                $('#selectPriority').append($('<option selected></option>').attr("value", key).text(value));
            else
                $('#selectPriority').append($('<option></option>').attr("value", key).text(value));
        });
    }
    function getIssue(issueId) {
        let strId;
        $.ajax({
            type: 'GET',
            url: '/Home/GetIssue/' + issueId,
            dataType: 'json',
            success: function (result) {
                $('#txtareaDescr').val('');
                $('#selectType').prop('disabled', false);
                $('#inpProjectName').val(result.ProjectName);
                $('#inpProjectName').prop('disabled', true);
                strId = '' + result.Id;
                $('#inpId').val('000000'.substring(0, '000000'.length - strId.length) + strId);
                $('#inpId').prop('disabled', true);
                $('#inpSummary').val(result.Summary);
                $('#lblProjectId').text(result.ProjectId);
                if ($('#inpSummary').val().length >= 4)
                    $('#btnSave').attr('disabled', false);
                if (result.Description === null)
                    $('#txtareaDescr').val('');
                else
                    $('#txtareaDescr').val(result.Description);
                $('#inpAssignee').val(result.Assignee);
                $.each(result.Types, function (key, value) {
                    if (value === result.Type)
                        $('#selectType').append($('<option selected></option>').attr("value", key).text(value));
                    else
                        $('#selectType').append($('<option ></option>').attr("value", key).text(value));
                });
                $.each(result.Priorities, function (key, value) {
                    if (value === result.Priority)
                        $('#selectPriority').append($('<option selected></option>').attr("value", key).text(value));
                    else
                        $('#selectPriority').append($('<option></option>').attr("value", key).text(value));
                });
            }
        });
    }
    function getIssues(projectId, table) {
        $.ajax({
            type: 'GET',
            url: '/Home/GetIssues/' + projectId,
            dataType: 'json',
            success: function (result) {
                table.innerHTML = '';
                for (let i = 0; i < result.length; i++) {
                    let row = document.createElement('tr');
                    let Id = document.createElement('th');
                    let Summary = document.createElement('td');
                    let Description = document.createElement('td');
                    let Priority = document.createElement('td');
                    let Assignee = document.createElement('td');
                    let tdEdit = document.createElement('td');
                    let btnEdit = document.createElement('button');
                    strId = '' + result[i].Id;
                    Id.innerText = `${'000000'.substring(0, '000000'.length - strId.length) + strId}`;
                    Summary.innerText = `${result[i].Summary}`;

                    if (result[i].Description === null)
                        Description.innerText = '';
                    else Description.innerText = `${result[i].Description}`;

                    Priority.innerText = `${result[i].Priority}`;
                    Assignee.innerText = `${result[i].Assignee}`;

                    btnEdit.setAttribute('class', 'btn btn-light btn-edit');
                    btnEdit.setAttribute('value', `${result[i].Id}`);
                    btnEdit.setAttribute('data-toggle', 'modal');
                    btnEdit.setAttribute('data-target', '#IssueModal');
                    btnEdit.innerText = 'Edit';
                    tdEdit.appendChild(btnEdit);

                    row.appendChild(Id);
                    row.appendChild(Summary);
                    row.appendChild(Description);
                    row.appendChild(Priority);
                    row.appendChild(Assignee);
                    row.appendChild(tdEdit);

                    table.appendChild(row);
                }
            }
        });
    }
});