$(function () {
    document.addEventListener('change', function (e) {
        if (e.target && e.target.classList.contains('selectProject')) {
            addUserToProject(event.target.value, event.target.dataset.user);
        }
    })
    function addUserToProject(projectId, email) {
        if (projectId === 'none') {
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
