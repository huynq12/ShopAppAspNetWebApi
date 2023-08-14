const baseUrl = "https://localhost:7000"
$(document).ready(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            // Get token from localStorage
            var token = localStorage.getItem('token');

            if (token) {
                // Attach token to the Authorization header
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });
    var urlParams = new URLSearchParams(window.location.search);
    var orderId = urlParams.get('id');
    viewOrder(orderId)
});
function viewOrder(id){
    var orderDetailHtml = ''
    $.ajax({
        url: baseUrl +'/order/'+id,
        type:'GET',
        success:function(res){
            console.log(res)
            $('#order-orderId').val(id)
            $('#order-fullName').val(res.userName)
            $('#order-address').val(res.address)
            $('#order-date').val(res.orderDate)
            $('#order-phoneNumber').val(res.phoneNumber)
            $('#order-status').val(res.status)
            $('#order-totalAmount').val(res.totalAmount)
            var list = res.orderDetails.$values
            console.log(list)
            for(let i = 0; i < list.length;i++){
                orderDetailHtml += `
                    <tr>
                        <td>${i+1}</td>
                        <td>${list[i].productId}</td>
                        <td>${list[i].productName}</td>
                        <td>${list[i].price}</td>
                        <td>${list[i].amount}</td>
                        <td>${list[i].price*list[i].amount}</td>
                    </tr>    

                `
            }
            $('#orderDetail').html(orderDetailHtml)
            
        }
    })
}

$('#order-edit-form').submit(function (e) {
    e.preventDefault();
    
    let orderData = {
        orderId : $('#order-orderId').val(),
        userName : $('#order-fullName').val(),
        address : $('#order-address').val(),
        phoneNumber : $('#order-phoneNumber').val(),
        status : 'processing'
    }
    $.ajax({
        url:baseUrl + '/update-order',
        type:'PUT',
        contentType: "application/JSON",
            data: JSON.stringify(orderData),
            success:function(){
                console.log('success')
                window.location = '/user'
                alert('edit successfully')
            },
            error: function () {
                alert("Error");
            }
    })
})
