$(function () {
    var _products = [];
    var _productTemplate = $("#product-item-template").html();
    Mustache.parse(_productTemplate);

    var Product = function (id, name) {
        this.id = id;
        this.name = name;
    };

    function displayProducts() {
        $.each(_products,
            function(idx, product) {
                $("#productList").append(Mustache.render(_productTemplate, product));
            });
    }

    $.ajax({
        type: 'GET',
        url: 'api/products',
        beforeSend: function(xhr) {
        }
    }).done(function (response) {
        $.each(response,
            function(idx, product) {
                _products.push(new Product(product.id, product.name));
            });        
        displayProducts();
    }).fail(function() {
        console.log("ERROR!");
    });
});