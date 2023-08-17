const baseUrl = 'https://localhost:7000';
$(document).ready(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var token = localStorage.getItem('token');
            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });
    displayOrders()
})


function displayOrders() {
    var html = ''
    $.ajax({
        url: baseUrl + '/orders',
        type: 'GET',
        success: function (res) {
            console.log(res)
            for (let item of res.$values) {
                html += '<tr><td>' + item.id + '</td>'
                html += '<td>' + item.userName + '</td>'
                html += '<td>' + item.user + '</td>'
                html += '<td>' + item.phoneNumber + '</td>'
                html += '<td>' + item.address + '</td>'
                html += '<td>' + item.orderDate + '</td>'
                html += '<td>' + item.status + '</td>'
                html += '<td><a href="/admin/orderDetail?id=' + item.id + '" class="btn btn-info">View</a></td>'
            }
            $('#listOrders').html(html)
        }

    })
}

