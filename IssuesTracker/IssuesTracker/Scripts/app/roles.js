$(function () {
    $('#addRole').attr('disabled', 'disabled');
    $('#addRole').on('click', function () {
        $.ajax({
            type: 'POST',
            url: '/Home/AddRole',
            dataType: 'json',
            data: { roleName: $('#rolename').val() },
            success: function (res) {
                $('#roles').append('<li class="list-group-item">' + $('#rolename').val() + '</li>');
                $('#rolename').val('');
            }
        });
    });
    $('#rolename').on('input', (function (e) {
        let dis = $('#rolename').val().length >= 2;
        $('#addRole').attr('disabled', !dis);
    }));
});