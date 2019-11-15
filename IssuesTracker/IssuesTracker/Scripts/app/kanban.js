$(function () {
    const selectProject = document.getElementById('project-select');
    init($('#project-select').val());
    function init(projectId) {
        renderCards(projectId);
        eventHandler();
    }
    function renderCards(projectId) {
        $.ajax({
            type: 'GET',
            url: '/Home/KanbanCards/' + projectId,
            dataType: 'html',
            success: function (result) {
                $('#kanban-result').html(result);
            }
        });
    }
    function eventHandler() {
        selectProject.addEventListener('change', function (event) {
            renderCards(event.target.value);
        });
    }
});