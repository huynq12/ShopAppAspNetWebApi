const baseUrl = "https://localhost:7000"
$(document).ready(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var token = localStorage.getItem('token');

            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });
    var urlParams = new URLSearchParams(window.location.search);
    var orderId = urlParams.get('id');
    viewOrder(orderId)
});
function viewOrder(orderId){
    var orderDetailHtml = ''
    $.ajax({
        url: baseUrl +'/order/'+orderId,
        type:'GET',
        success:function(res){
            //console.log(res)
            $('#order-orderId').val(orderId)
            $('#order-fullName').val(res.userName)
            $('#order-address').val(res.address)
            $('#order-date').val(res.orderDate)
            $('#order-phoneNumber').val(res.phoneNumber)
            $('#order-status').val(res.status)
            $('#order-totalAmount').val(res.totalAmount)
            var list = res.orderDetails.$values
            //console.log(list)
            for(let i = 0; i < list.length;i++){
                orderDetailHtml += '<tr>'
                orderDetailHtml += '<td>'+(i+1)+'</td>'
                orderDetailHtml += '<td>'+list[i].id+'</td>'
                orderDetailHtml += '<td>'+list[i].productName+'</td>'
                orderDetailHtml += '<td>'+list[i].price+'</td>'
                orderDetailHtml += '<td>'+list[i].amount+'</td>'
                orderDetailHtml += '<td>'+list[i].price*list[i].amount+'</td>'
                if(res.status == "Complete"){
                    orderDetailHtml += '<td><a href="/product?id='+list[i].productId+'" class="btn btn-warning">View</a> | <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#reviewModal" onclick=reviewFormAction('+list[i].id+')>Review</button></td>'
                }
                orderDetailHtml += '</tr>';
            }
            $('#orderDetail').html(orderDetailHtml)  
            var submitOrder = '';
            if(res.status == 'New' || res.status == 'Processing'){
                submitOrder += '<a class="btn btn-danger form-group mt-3" onclick=cancelOrder('+orderId+')>Cancel</a>';
            }
            if(res.status === 'Shipped'){
                submitOrder += '<a class="btn btn-info form-group mt-3" onclick=completeOrder('+orderId+')>Complete</a>';
            }
            $('#formFooter').append(submitOrder)
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
            },
            error: function () {
                alert("Error");
            }    
    })
})

function reviewFormAction(orderDetailId){
    $('#review-id').val(orderDetailId)
}
function completeOrder(id){
    var orderData = {
        orderId : id,
        status : 'Complete'
    }
    console.log(orderData)
    if(confirm("Do you want to complete this order?")){
        $.ajax({
            url :baseUrl + '/update-order-user',
            type: 'PUT',
            contentType: "application/JSON",
            data: JSON.stringify(orderData),
            success:function(){
                console.log('success')
                location.reload()
            },
            error: function () {
                alert("Error");
            }
        })
    }
    
}

function cancelOrder(id){
    var orderData = {
        orderId : id,
        status : 'Cancel'
    }
    console.log(orderData)
    if(confirm("Do you want to cancel this order?")){
        $.ajax({
            url :baseUrl + '/update-order-user',
            type: 'PUT',
            contentType: "application/JSON",
            data: JSON.stringify(orderData),
            success:function(){
                console.log('success')
                location.reload()
            },
            error: function () {
                alert("Error");
            }
        })
    }
    
}

function submitReview(){
    var reviewData = {
        orderDetailId : $('#review-id').val(),
        userName : $('#review-userName').val(),
        rating : $('#review-rating').val(),
        commentMsg : $('#review-commentMsg').val(),
    }
    $.ajax({
        url:baseUrl + '/place-review',
        type:'POST',
        //dataType: 'json',
        contentType: "application/JSON",
        data: JSON.stringify(reviewData),
        success:function(){
            $('#reviewModal').modal('hide')
            $('#review-commentMsg').val('')
            $('#review-rating').val('')
            viewOrder(orderId)
        },
        error:function(e){
            alert(JSON.stringify(e))
        }
        
    })
}
    
