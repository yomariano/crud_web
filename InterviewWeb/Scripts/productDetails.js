var productDetails = productDetails || (function () {
    var _args = {};
    var _id = {};
    return {
        init: function (Args) {
            _args = Args;
            _id = _args[0];
        },
        getProduct: function () {
            $.ajax({
                type: 'GET',
                url: 'http://localhost:33942/api/products/' + _id
            }).done(function (data) {
                document.getElementById('identifier').value = data.id;
                document.getElementById('name').value = data.name;
                document.getElementById('internalCode').value = data.internalCode;
                created = new Date(data.dateCreated);
                document.getElementById('dateCreated').value = created.getFullYear() + "/"+ created.getMonth()+  "/" + created.getDate();
                document.getElementById('dateDiscontinued').value = data.dateDiscontinued;
            }).fail(function (err) {
                if (err.status == 410) {
                    alert('Error message: Product has been discontinued.');
                }
                if (err.status == 404) {
                    alert('Error message: Product does not exist.');
                }
                window.location.assign("/Products/Index/");

            });
        }
    };
}());
