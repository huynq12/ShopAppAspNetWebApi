const baseUrl = 'https://localhost:7000'
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
    var empData = []
    $.ajax({
        url: baseUrl + '/products',
        type: 'GET',
        success: function (res) {
            console.log(res)
            for (let item of res.data.$values) {
                empData.push({ id: item.id, name: item.name, quantity: item.quantity, price: item.price, description: item.description })
            }
            console.log(empData)
            $('#listProduct').DataTable({
                data: empData,
                columnDefs: [{
                    targets: 0,
                    data: 'id',
                    name: 'Id',
                    orderable: false,
                    searchable: false,
                },
                {
                    targets: 1,
                    data: 'name',
                    name: 'Name',

                },
                {
                    targets: 2,
                    data: 'quantity',
                    name: 'Quantity',
                },
                {
                    targets: 3,
                    data: 'price',
                    name: 'Price',
                },
                {
                    targets: 4,
                    data: 'description',
                    name: 'Description',
                },
                {
                    targets: 5,
                    name: 'Actions',
                    render: function (data, type, row) {
                        return '<a href="#" class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#product-edit" onclick=editProduct("' + row.id + '");>Edit</a> | <a href="#" class="btn btn-danger" onclick=deleteProduct("' + row.id + '");>Delete</a>'
                    },
                    orderable: false,
                    searchable: false,
                },
                ]
            })
        }
    })
    getCategoryOptions()
})

// $('#btnCreateProduct').click(function(){
//     $('#product-create').modal('show')
// })

function addNewProduct() {
    const objData = {
        name: $('#product-create-name').val(),
        quantity: $('#product-create-quantity').val(),
        price: $('#product-create-price').val(),
        description: $('#product-create-description').val(),
        cpu: $('#product-create-cpu').val(),
        ram: $('#product-create-ram').val(),
        hardDrive: $('#product-create-hardDrive').val(),
        screen: $('#product-create-screen').val(),
        power: $('#product-create-power').val(),
        image: $('#product-create-image').val(),
        categoryIds:$('#product-create-categories').val()
    }
    console.log(objData)
    console.log(objData.tagIds)
    setTimeout(500)
    $.ajax({
        url: baseUrl + '/create-product',
        type: 'POST',
        dataType: 'JSON',
        data: JSON.stringify(objData),
        contentType: 'application/JSON;charset=utf-8',
        success: function () {
            $('#product-create').modal('hide')
            location.reload();
            setTimeout(500)
            alert('thanh cong')
        },
        error: function (error) {
            alert(JSON.stringify(error));
            console.error(JSON.stringify(error));
        }
    });

}

function getCategoryOptions() {
    $.ajax({
        url: baseUrl + '/categories',
        type: 'GET',
        dataType: 'JSON',
        success: function (data) {
            const categorySelects = $('#product-create-categories');
            //categorySelects.empty();
            var list = data.$values
            for (var i = 0; i < list.length; i++) {
                categorySelects.append($('<option>', {
                    value: list[i].id,
                    text: list[i].name
                }));
            };
        },
        error: function (error) {
            console.error(JSON.stringify(error));
        }
    });
}

function getProductById(id){
    $.get(baseUrl + "/view-product/" + id, function (data) {

        if (data && typeof data === "object") {
            console.log(data)
            $('#product-edit-id').val(data.id)
            $("#product-edit-name").val(data.name)
            $("#product-edit-quantity").val(data.quantity)
            $("#product-edit-price").val(data.price)
            $("#product-edit-description").val(data.description)
            $("#product-edit-cpu").val(data.cpu)
            $("#product-edit-ram").val(data.ram)
            $("#product-edit-hardDrive").val(data.hardDrive)
            $("#product-edit-screen").val(data.screen)
            $("#product-edit-power").val(data.power)
            $("#product-edit-image").val(data.image)
            $("#product-edit-categories").val(data.categoryIds)
        } else {
            console.error('data error');
        }
    }).fail(function () {
        console.error("call api fail");
    });
}

function getCategoryByProductId(id){
    var categorySelects = $("#product-edit-categories");
    $.get(baseUrl + '/categories/', function (data) {
        categorySelects.empty();
        var categories = data.$values
        for (var i = 0; i < categories.length; i++) {
            categorySelects.append("<option value='" + categories[i].id + "'>" + categories[i].name + "</option>");
        };
    }).fail(function () {
        console.error("call api fail");
    });
    setTimeout(500)
    $.get(baseUrl + '/categories-by/' + id, function (res) {
        var categories = res.$values
        //console.log(categories.length)
        for (var i = 0; i < categories.length; i++) {
            categorySelects.find("option[value='" + categories[i].id + "']").prop("selected", true);
        };
    }).fail(function () {
        console.error("call api fail");
    });
}

function editProduct(id){
    getProductById(id)
    getCategoryByProductId(id)
}

function updateProduct(){
    var data = {
        id: $('#product-edit-id').val(),
        name: $('#product-edit-name').val(),
        quantity: $('#product-edit-quantity').val(),
        price: $('#product-edit-price').val(),
        description: $('#product-edit-description').val(),
        cpu: $('#product-edit-cpu').val(),
        ram: $('#product-edit-ram').val(),
        hardDrive: $('#product-edit-hardDrive').val(),
        screen: $('#product-edit-screen').val(),
        power: $('#product-edit-power').val(),
        image: $('#product-edit-image').val(),
        categoryIds:$('#product-edit-categories').val()
    }
    console.log(data.id)
    $.ajax({
        url: baseUrl + '/edit-product',
        type: 'PUT',
        contentType: "application/JSON",
        data: JSON.stringify(data),
        success: function () {
            $('#product-edit').modal('hide')
            location.reload();
            setTimeout(500)
            alert('thanh cong')
        },
        error: function (e) {
            alert(JSON.stringify(e))
        }
    })
}

function deleteProduct(id) {
    if (confirm("Do you want to delete this product?")) {
        $.ajax({
            url: baseUrl + '/delete-product/' + id,
            type: 'DELETE',
            dataType: 'JSON',
            success: function () {
                location.reload()
                setTimeout(500)
                alert("Delete successfully");
            },
            error: function () {

                alert("Error");
            }
        })
    }
}
