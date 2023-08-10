const baseUrl = "https://localhost:7000"
let cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];

$(document).ready(function(){
    displayCart()
    
    
})
function displayCart (){
    let cartItemsHtml = '';
    let total=0
    let grandTotal = 0
    for (let i = 0;i < cartItems.length; i++) {
        let item = cartItems[i]
        $.ajax({
            url:baseUrl+'/view-product/'+item.productId,
            type:'GET',
            success:function(product){
                total = product.price*item.quantity
                cartItemsHtml += '<tr><td><img src="/CustomerContent/img/new-pictures/laptop1.jpg" style="width:100px;height:100px" class="img-sm"></td>'
                cartItemsHtml += '<td>'+product.name+'</td>'
                cartItemsHtml += '<td>'+product.price+'</td>'
                cartItemsHtml += '<td><button onclick=reduceCartItem('+item.productId+',1)>-</button><input type="number" id="itemQuantity" value="'+item.quantity+'"/><button onclick=addToCart('+item.productId+',1)>+</button></td>'
                cartItemsHtml += '<td>'+total+'</td>'
                cartItemsHtml += '</tr>'
                $('#cartBody').html(cartItemsHtml)
                grandTotal += total
                if(i==cartItems.length-1){
                    $('#grandTotal').html('Tổng đơn: '+grandTotal +'đ')
                }
            }
        })
    }
}



function addToCart(productId, quantity) {
    let existingItem = cartItems.find(item => item.productId === productId);

    if (existingItem) {
        existingItem.quantity += quantity;
    } else {
        cartItems.push({ productId, quantity });
    }
    location.reload()   
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
}

function reduceCartItem(productId,quantity) {
    let existingItem = cartItems.find(item => item.productId === productId);

    if (existingItem & existingItem.quantity==1) {
        cartItems.splice(index, 1);
    } else {
        existingItem.quantity -= quantity;
    }
    location.reload()   
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
}



