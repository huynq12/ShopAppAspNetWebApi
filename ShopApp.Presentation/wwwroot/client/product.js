const baseUrl = 'https://localhost:7000'
$(document).ready(function () {
    //var cardHtml = ''
    $.ajax({
        url: baseUrl + '/products',
        type: 'GET',
        success: function (res) {
            for (let item of res.data.$values) {
                var cardHtml = `
                    <div class="col-md-4" >
                        <a href="/product?id=${item.id}">
                        <div class="card mb-4" id="btnCard" data-product-id="${item.id}">
                            <input type="hidden" id="productDetailsId" value="${item.id}"/>
                            <img class="card-img-top" src="./img/new-pictures/laptop1.jpg" alt="">
                            <div class="card-body">
                                <h4 class="card-title">${item.name}</h5>
                                <p class="card-text">${item.description}</p>
                                <p class="card-text">${item.price}đ</p>
                            </div>
                        </div>
                       </a>
                    </div>
                `;
                
                $("#product-list").append(cardHtml);
            }
            
        }
    })
})

function viewProduct(id){
    $.ajax({
        url:baseUrl + '/view-product/' +id,
        type:'GET',
        success:function(res){
            console.log(res)
            var productDetail = `
            <h2 class="product-name">${res.name}</h2>
            <div>
                <div class="product-rating">
                    <i class="fa fa-star"></i>
                    <i class="fa fa-star"></i>
                    <i class="fa fa-star"></i>
                    <i class="fa fa-star"></i>
                    <i class="fa fa-star-o"></i>
                </div>
                <a class="review-link" href="#">10 Review(s) | Thêm đánh giá</a>
            </div>
            <div>
                <h3 class="product-price">${res.price} đ <del class="product-old-price">46.600.000 đ</del></h3>
                <span class="product-available">In Stock</span>
            </div>
            <p>Màn hình : ${res.screen}</p>
            <p>CPU : ${res.cpu}</p>
            <p>Tản Nhiệt Khí : HP 800 G1 Tiêu Chuẩn</p>
            <p>RAM : ${res.ram}</p>
            <p>Ổ cứng : ${res.hardDrive}</p>
            <p>Nguồn :${res.power}</p>
            <div class="product-options">
                <label>
                    Kích cỡ
                    <select class="input-select">
                        <option value="0">X</option>
                    </select>
                </label>
                <label>
                    Màu sắc
                    <select class="input-select">
                        <option value="0">Red</option>
                    </select>
                </label>
            </div>

            <div class="add-to-cart">
                <div class="qty-label">
                    Số lượng
                    <div class="input-number">
                        <input type="number">
                        <span class="qty-up">+</span>
                        <span class="qty-down">-</span>
                    </div>
                </div>
                <button class="add-to-cart-btn"><i class="fa fa-shopping-cart"></i>Thêm vào giỏ hàng</button>
            </div>

            <ul class="product-btns">
                <li><a href="#"><i class="fa fa-heart-o"></i> Thêm vào danh sách yêu thích</a></li>
                <li><a href="#"><i class="fa fa-exchange"></i> Thêm vào để so sánh</a></li>
            </ul>

            <ul class="product-links">
                <li>Loại:</li>
                <li><a href="#">TAI NGHE</a></li>
                <li><a href="#">PHỤ KIỆN</a></li>
            </ul>

            <ul class="product-links">
                <li>Chia sẻ:</li>
                <li><a href="#"><i class="fa fa-facebook"></i></a></li>
                <li><a href="#"><i class="fa fa-twitter"></i></a></li>
                <li><a href="#"><i class="fa fa-google-plus"></i></a></li>
                <li><a href="#"><i class="fa fa-envelope"></i></a></li>
            </ul>

            `
            $('#product-details').append(productDetail)
        }
    })
}