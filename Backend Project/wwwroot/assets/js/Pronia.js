$(document).ready(function () {

    $(document).on("click", ".category", function (e) {
        e.preventDefault();
        let categoryId = $(this).attr("data-id");
        let parent = $(".product-list");

        $.ajax({
            url: `shop/GetProductByCategory?id=${categoryId}`,
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })



    $(document).on("click", ".all-product", function (e) {
        e.preventDefault();
        let parent = $(".product-list");

        $.ajax({
            url: "shop/GetAllProduct",
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })


    $(document).on("click", ".product-color", function (e) {
        e.preventDefault();
        let colorId = $(this).attr("data-id")
        let parent = $(".product-list");

        $.ajax({
            url: `shop/GetProductByColor?id=${colorId}`,
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })


    $(document).on("click", ".all-color", function (e) {
        e.preventDefault();
        let parent = $(".product-list");

        $.ajax({
            url: "shop/GetAllProductByColor",
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })


    $(document).on("click", ".select", function (e) {
        e.preventDefault();
        let colorId = $(this).attr("data-id")
        let parent = $(".product-list");

        $.ajax({
            url: `shop/GetProductFilteredByPrice?id=${colorId}`,
            type: "Get",
            success: function (res) {
                $(parent).html(res);

            }
        })
    })

    
    
})