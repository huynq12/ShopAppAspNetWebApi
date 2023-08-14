const baseUrl = 'https://localhost:7000'
$(document).ready(function(){
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
    displayCategories()
})

function displayCategories(){
    var str = ''
    $.ajax({
        url:baseUrl + '/categories',
        type:'GET',
        success:function(res){
            console.log(res)
            for(let item of res.$values){
                str += `
                    <tr>
                        <td>${item.id}</td>
                        <td>${item.name}</td>
                        <td>${item.description}</td>
                        <td><a href="#" class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#category-edit" onclick=getCategory(${item.id})>Edit</a> | <a class="btn btn-danger" onclick=deleteCategory(${item.id})>Delete</a></td>
                    </tr>
                `
            } 
            $('#listCategory').html(str)  

        }
    })
}

function addNewCategory(){
    var categoryData = {
        name : $('#category-create-name').val(),
        description : $('#category-create-description').val()
    }
    $.ajax({
        url:baseUrl + '/create-category',
        type:'POST',
        contentType : 'application/json',
        data:JSON.stringify(categoryData),
        success:function(){
            $('#category-create').modal('hide')
            displayCategories()
        }
    })
}
function getCategory(id){
    $.ajax({
        url:baseUrl+'/category/'+id,
        type:'GET',
        success:function(res){
            $('#category-edit-id').val(res.id)
            $('#category-edit-name').val(res.name)
            $('#category-edit-description').val(res.description)
        }
    })
}
function updateCategory(){
    categoryData = {
        id: $('#category-edit-id').val(),
        name : $('#category-edit-name').val(),
        description : $('#category-edit-description').val()
    }
    $.ajax({
        url:baseUrl + '/update-category',
        type:'PUT',
        contentType:'application/json',
        data:JSON.stringify(categoryData),
        success:function(){
            //alert('update successfully')
            $('#category-edit').modal('hide')
            displayCategories()
        },
        error:function(error){
            console.log(JSON.stringify(error))
        }
    })
}

function deleteCategory(id){
    if(confirm("Do you want to delete this category?")){
        $.ajax({
            url : baseUrl + '/delete-category/' + id,
            type:'DELETE',
            dataType:'json',
            success:function(){
                displayCategories()
                alert('Delete successfully')
            },
            error:function(error){
                console.log(JSON.stringify(error))
            }
        })
    }
}