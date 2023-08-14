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
                if(res.status == "Success"){
                    orderDetailHtml += '<td><a class="btn btn-warning" onclick=reviewFormAction('+list[i].id+')>Review</a></td>'
                }
                orderDetailHtml += '</tr>';
            }
            if(res.status === "Success"){
                var reviewHtml = `
                    <form id="reviewForm">
                    <input id="review-fullName" type="hidden" value="${res.user}" />
                    </form>
                `
                $('#reviewProduct').append(reviewHtml)
            }
            $('#orderDetail').html(orderDetailHtml)  
            if(res.status === 'Processing'){
                console.log('alo')
                var saveButton = '';
                saveButton += '<button type="submit" class="btn btn-info form-group mt-3">Save</button>';
                $('#formFooter').append(saveButton)
            }

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

function reviewFormAction(orderDetailId){
    var formReview = `
                        <input id="orderDetailId" type="hidden" value="${orderDetailId}"/>
                        <select id="review-rating" class="form-select  js-choice mt-2">
                            <option value="5" selected="">★★★★★ (5/5)</option>
                            <option value="4">★★★★☆ (4/5)</option>
                            <option value="3">★★★☆☆ (3/5)</option>
                            <option value="2">★★☆☆☆ (2/5)</option>
                            <option value="1">★☆☆☆☆ (1/5)</option>
                        </select>
                        <textarea class="form-control mt-2" id="commentMsg" placeholder="Write your comment" rows="3"></textarea>
                        <button type="submit" id="btnComment" class="btn btn-success mt-2">Submit</button>
                `
                $('#reviewForm').append(formReview)
}

$('#btnComment').click(function(e){
    e.preventDefault();
    var reviewData = {
        userName : $('#review-fullName').val(),
        orderDetailId : $('#orderDetailId').val(),
        commentMsg : $('#commentMsg').val(),
        rating : $('#review-rating').val()
    }
    console.log(reviewData.orderDetailId)
    $.ajax({
        url:baseUrl + '/place-review',
        type:'POST',
        dataType: 'json',
        contentType: "application/JSON",
        data: JSON.stringify(reviewData),
        success:function(){
            alert('review ok')
            $('#commentMsg').val('')
            $('#review-rating').val('')
        },
        error:function(e){
            alert(JSON.stringify(e))
        }
        
    })
})
    
