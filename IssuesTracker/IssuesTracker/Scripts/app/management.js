$(function () {
    const selectProject = document.getElementById('selectProject');
    selectProject.addEventListener('change', function (event) {
        addUserToProject(event.target.value, event.target.dataset.user);
    });
    function addUserToProject(projectId, email) 
    {
        if (projectId == 'none') {
            return;
        }
        $.ajax({
            type: 'POST',
            url: '/Home/AddUserToProject',
            dataType: 'json',
            data: { projectId: projectId, email: email }
        });
    }
});
