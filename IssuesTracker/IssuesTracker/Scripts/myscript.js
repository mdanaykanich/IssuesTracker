$(function () {
    const selectProject = document.getElementById('project-select');
    const table = document.getElementById('result');
    const priorities = ['Trivial', 'Low', 'Medium', 'High', 'Critical'];
    init(selectProject.options[selectProject.selectedIndex].value, table);

    function init(projectId, table) {

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
        $('#btnSaveEdit').on('click', function () {
            let issue = {
                Id: $('#lblId').text(),
                Summary: $('#inpSummary').val(),
                Description: $('#txtareaDescr').val(),
                Type: $('#selectType').val(),
                Assignee: $('#lblAssignee').text(),
                Priority: $('#selectPriority').val(),
                ProjectName: $('#lblProjectName').text(),
                ProjectId: $('#lblProjectId').text()
            }
            $.ajax({
                type: 'POST',
                url: '/Home/EditIssue',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(issue),
                success: function (result) {
                    console.log(result);
                    console.log(issue);
                    $('#editIssueModal').modal('hide');
                    getIssues(parseInt(issue.ProjectId), table);
                }
            });
        });
        $('#btnCreateIssue').on('click', function () {
            createIssue();
        });
        $('#btnSaveCreate').on('click', function () {
            let issue = {
                Id: '',
                Summary: $('#inpSummaryCreate').val(),
                Description: $('#txtareaDescrCreate').val(),
                Type: $('#selectTypeCreate').val(),
                Assignee: $('#inpAssigneeCreate').val(),
                Priority: $('#selectPriorityCreate option:selected').html(),
                ProjectName: $('#lblProjectNameCreate').text(),
                ProjectId: $('#lblProjectIdCreate').text()
            }
           
            $.ajax({
                type: 'POST',
                url: '/Home/AddIssue',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(issue),
                success: function (result) {
                    console.log(result);
                    console.log(issue);
                    $('#createIssueModal').modal('hide');
                    getIssues(parseInt(issue.ProjectId), table);
                },
                error: function (error) {
                    alert('Incorrect data');
                }
            });
        })
    }
    function createIssue() {
        let strId;
        $('#selectType').innerHTML = '';
        $('#lblProjectNameCreate').text($('#project-select option:selected').html());
        $('#selectTypeCreate').append($('<option selected></option>').attr('value', 'New').text('New'));
        $('#lblProjectIdCreate').text($('#project-select').val());
        $.each(priorities, function (key, value) {
            if(value==='Trivial')
                $('#selectPriorityCreate').append($('<option selected></option>').attr("value", key).text(value));
            else
                $('#selectPriorityCreate').append($('<option></option>').attr("value", key).text(value));
        });
    }
    function getIssue(issueId) {
        let strId;
        $("#selectType option").each(function () {
            $(this).remove();
        });
        $("#selectPriority option").each(function () {
            $(this).remove();
        });
        $.ajax({
            type: 'GET',
            url: '/Home/GetIssue/' + issueId,
            dataType: 'json',
            success: function (result) {        
                $('#lblProjectName').text(result.ProjectName);
                strId = '' + result.Id;
                $('#lblId').text('000000'.substring(0, '000000'.length - strId.length) + strId);
                $('#inpSummary').val(result.Summary);
                $('#lblProjectId').text(result.ProjectId);
                if (result.Description === null)
                    $('#txtareaDescr').text('');
                else
                    $('#txtareaDescr').text(result.Description);
                $('#lblAssignee').text(result.Assignee);            
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
        let Id, Summary, Description, Priority, Assignee, tdEdit, btnEdit, strId, row;
        Id = document.createElement('th');
        Summary = document.createElement('td');
        Description = document.createElement('td');
        Priority = document.createElement('td');
        Assignee = document.createElement('td');
        tdEdit = document.createElement('td');
        btnEdit = document.createElement('button');
        row = document.createElement('tr');
        $.ajax({
            type: 'GET',
            url: '/Home/GetIssues/' + projectId,
            dataType: 'json',
            success: function (result) {
                table.innerHTML = '';
                for (let i = 0; i < result.length; i++) {
                    row = document.createElement('tr');
                    strId = '' + result[i].Id;
                    Id.innerHTML = `${'000000'.substring(0, '000000'.length - strId.length) + strId}`;
                    Summary.innerHTML = `${result[i].Summary}`;

                    if (result[i].Description === null) {
                        Description.innerHTML = '';
                    }
                    else Description.innerHTML = `${result[i].Description}`;

                    Priority.innerHTML = `${result[i].Priority}`;
                    Assignee.innerHTML = `${result[i].Assignee}`;

                    btnEdit.setAttribute('class', 'btn btn-light btn-edit');
                    btnEdit.setAttribute('value', `${result[i].Id}`);
                    btnEdit.setAttribute('data-toggle', 'modal');
                    btnEdit.setAttribute('data-target', '#editIssueModal');
                    btnEdit.innerHTML = 'Edit';
                    tdEdit.appendChild(btnEdit);

                    row.appendChild(Id);
                    row.appendChild(Summary);
                    row.appendChild(Description);
                    row.appendChild(Priority);
                    row.appendChild(Assignee);
                    row.appendChild(tdEdit);

                    table.innerHTML += row.innerHTML;
                }
            }
        });
    }
});