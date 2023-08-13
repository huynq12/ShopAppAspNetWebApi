
$(document).ready(function(){
    displayMiniCartItemQuantity()
    displayCart()
    
    
})
function displayMiniCartItemQuantity(){
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    $('#cartItemQuantity').html(cartItems.length)
}
function displayCart() {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    cartItems.sort(function(a, b) {
        return a.productId - b.productId;
    });

    let grandTotal = 0;
    let cartItemsHtml = '';

    function processCartItem(item) {
        return new Promise(function(resolve, reject) {
            if (item.quantity < 1 || item.quantity == null) {
                removeCartItem(item.productId);
                resolve(null);
                return;
            }
            $.ajax({
                url: 'https://localhost:7000/view-product/' + item.productId,
                type: 'GET',
                success: function(product) {
                    var total = product.price * item.quantity;
                    grandTotal += total;

                    var cartItemHtml = '<tr><td><img src="/CustomerContent/img/new-pictures/laptop1.jpg" style="width:100px;height:100px" class="img-sm"></td>'
                    cartItemHtml += '<td>' + product.name + '</td>'
                    cartItemHtml += '<td>' + product.price + '</td>'
                    cartItemHtml += '<td><button class="btn btn-success m-1" onclick=reduceCartItem(' + item.productId + ',1)>-</button><input type="number" value="' + item.quantity + '" style="width:50px" disabled/><button class="btn btn-success m-1" onclick=addToCart(' + item.productId + ',1)>+</button><p><button class="btn btn-danger" onclick=removeCartItemAction(' + item.productId + ')>Remove</button></p></td>'
                    cartItemHtml += '<td>' + total + '</td>'
                    cartItemHtml += '</tr>'

                    resolve(cartItemHtml);
                },
                error: function() {
                    reject();
                }
            });
        });
    }

    const promiseArray = cartItems.map(processCartItem);

    Promise.all(promiseArray).then(function(cartItemHtmlArray) {
        var filteredHtmlArray = cartItemHtmlArray.filter(html => html !== null);
        cartItemsHtml = filteredHtmlArray.join('');
        $('#cartBody').html(cartItemsHtml);

        var grandTotalHtml = '<p>Tổng đơn:' + grandTotal + 'đ</p>';
        $('#grandTotal').html(grandTotalHtml);
    });
}


function addToCart(productId, quantity) {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    let existingItem = cartItems.find(item => item.productId === productId);

    if (existingItem) {
        existingItem.quantity += quantity;
    } else {
        cartItems.push({ productId, quantity });
    } 
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
    displayCart()
    displayMiniCartItemQuantity()
}

function reduceCartItem(productId,quantity) {
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    let existingItem = cartItems.find(item => item.productId === productId);

    if (existingItem & existingItem.quantity==1) {
        removeCartItemAction(existingItem.productId)
    } else {
        existingItem.quantity -= quantity;
        localStorage.setItem('cartItems', JSON.stringify(cartItems));
    }  
    displayCart()
    displayMiniCartItemQuantity()
    
}

function removeCartItem(productId){
    var cartItems = JSON.parse(localStorage.getItem('cartItems')) || [];
    for (var i = cartItems.length - 1; i >= 0; i--) {
        if (cartItems[i].productId === productId) {
            cartItems.splice(i, 1);
            break;
        }
    }
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
}
function removeCartItemAction(productId){
    reduceCartItem(productId)
    location.reload()
    displayCart()
    displayMiniCartItemQuantity()
}



