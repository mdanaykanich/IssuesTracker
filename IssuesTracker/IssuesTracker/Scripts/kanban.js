$(function () {
    const selectProject = document.getElementById('project-select');
    const ulNewIssues = document.getElementById('newIssues');
    const ulInprogressIssues = document.getElementById('inprogressIssues');
    const ulDoneIssues = document.getElementById('doneIssues');
    init($('#project-select').val());

    function init(projectId) {
        getIssues(projectId);
        eventHandler();
    }
    function eventHandler() {
        selectProject.addEventListener('change', function (event) {
            getIssues(event.target.value);
        });
    }
    function getIssues(projectId) {
        ulNewIssues.innerHTML = '';
        ulInprogressIssues.innerHTML = '';
        ulDoneIssues.innerHTML = '';
        $.ajax({
            type: 'GET',
            url: '/Home/GetIssues/' + projectId,
            dataType: 'json',
            success: function (result) {
                for (let i = 0; i < result.length; i++) {
                    var liIssue = create_liIssue(result[i]);
                    switch (result[i].Type) {
                        case 'New': {
                            ulNewIssues.appendChild(liIssue);
                            break;
                        }
                        case 'InProgress': {
                            ulInprogressIssues.appendChild(liIssue);
                            break;
                        }
                        case 'Done': {
                            ulDoneIssues.appendChild(liIssue);
                            break;
                        }
                    }
                }
            }
        });
    }
    function create_liIssue(issue) {
        let li = document.createElement('li');
        li.setAttribute('class', 'list-group-item kanban-li');

        let divr1 = document.createElement('div');
        divr1.setAttribute('class', 'row');
        let divcl1 = document.createElement('div');
        divcl1.setAttribute('class', 'col');
        let divr2 = document.createElement('div');
        divr2.setAttribute('class', 'row');
        let divcl2 = document.createElement('div');
        divcl2.setAttribute('class', 'col');
        let divr3 = document.createElement('div');
        divr3.setAttribute('class', 'row');
        let divcl3 = document.createElement('div');
        divcl3.setAttribute('class', 'col');

        let spanr1 = document.createElement('span');
        spanr1.setAttribute('style', 'margin-left: 1em; display: block');
        let spanSummary = document.createElement('span');
        spanSummary.setAttribute('class', 'kanban-span');
        let spanr2 = document.createElement('span');
        spanr2.setAttribute('style', 'margin-left: 1em');
        let spanPriority = document.createElement('span');
        spanPriority.setAttribute('class', 'kanban-span');
        let spanr3 = document.createElement('span');
        spanr3.setAttribute('style', 'margin-left: 1em');
        let spanAssignee = document.createElement('span');
        spanAssignee.setAttribute('class', 'kanban-span');

        spanSummary.innerText = 'Summary: ';
        spanPriority.innerText = 'Priority: ';
        spanAssignee.innerText = 'Assignee: ';

        spanr1.appendChild(spanSummary);
        spanr1.innerHTML += issue.Summary;
        divcl1.appendChild(spanr1);
        divr1.appendChild(divcl1);
        li.appendChild(divr1);

        spanr2.appendChild(spanPriority);
        spanr2.innerHTML += issue.Priority;
        divcl2.appendChild(spanr2);
        divr2.appendChild(divcl2);
        li.appendChild(divr2);

        spanr3.appendChild(spanAssignee);
        spanr3.innerHTML += issue.Assignee;
        divcl3.appendChild(spanr3);
        divr3.appendChild(divcl3);
        li.appendChild(divr3);

        return li;
    }
});