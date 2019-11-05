$(function () {
    $.ajax({
        type: 'GET',
        url: 'http://localhost:54861/Home/GetIssues/' + $('.project-select').val(),
        dataType: 'json',
        success: function (result) {
            let _html = "";
            for (var i = 0; i < result.length; i++) {
                var str = "" + result[i].Id;
                var pad = "00000";
                _html += `<tr><th>${pad.substring(0, pad.length - str.length) + str}</th>`;
                _html += `<td>${result[i].Summary}</td>`;
                _html += `<td>${result[i].Description}</td>`;
                _html += `<td>${result[i].Priority}</td>`;
                _html += `<td>${result[i].Assignee}</td>`;
                _html += `<td><button type="button" id=${result[i].Id} class="btn btn-light " >Edit</button></td></tr>`;
            }
            const res = document.querySelector('.result');
            res.innerHTML = _html;
        }
    });
    const selectElement = document.querySelector('.project-select');
    selectElement.addEventListener('change', (event) => {
        $.ajax({
            type: 'GET',
            url: 'http://localhost:54861/Home/GetIssues/' + event.target.value,
            dataType: 'json',
            success: function (result) {
                let _html = "";
                for (var i = 0; i < result.length; i++) {
                    var str = "" + result[i].Id;
                    var pad = "00000";
                    _html += `<tr><th>${pad.substring(0, pad.length - str.length) + str}</th>`;
                    _html += `<td>${result[i].Summary}</td>`;
                    _html += `<td>${result[i].Description}</td>`;
                    _html += `<td>${result[i].Priority}</td>`;
                    _html += `<td>${result[i].Assignee}</td>`;
                    _html += `<td><button type="button" id=${result[i].Id} class="btn btn-light">Edit</button></td></tr>`;
                }
                const res = document.querySelector('.result');
                res.innerHTML = _html;
                _html = "";
            }
        });
    });
});