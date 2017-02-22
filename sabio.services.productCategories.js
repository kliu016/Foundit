sabio.services.productCategories = sabio.services.productCategories || {};

sabio.services.productCategories.insert = function (data, onSuccess, onError) {
    var url = "/api/product-categories/";

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: data
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    };
    $.ajax(url, settings);


};

sabio.services.productCategories.getByPageIndex = function (pageIndex, itemsPerPage, onSuccess, onError) {
    var url = "/api/product-categories/pageIndex/" + pageIndex + "/itemsPerPage/" + itemsPerPage;

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "GET"
    };

    $.ajax(url, settings);
};

sabio.services.productCategories.update = function (id, data, onSuccess, onError) {

    var url = "/api/product-categories/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , data: data
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "PUT"
    };

    $.ajax(url, settings);

};

sabio.services.productCategories.get = function (onAjaxSuccess, onAjaxError) {
    var url = "/api/product-categories/";
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onAjaxSuccess
            , error: onAjaxError
            , type: "GET"
    };

    $.ajax(url, settings);

};

sabio.services.productCategories.getById = function (id, onSuccess, onError) {

    var url = "/api/product-categories/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);

};

sabio.services.productCategories.getTopLevel = function (onSuccess, onError) {

    var url = "/api/product-categories/toplevel";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);

};

sabio.services.productCategories.GetCategoriesByDisplayName = function (onSuccess, onError) {

    var url = "/api/product-categories/displayNames";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);

};

sabio.services.productCategories.getSubCat= function (parentId, onSuccess, onError) {

    var url = "/api/product-categories/subcategory/" + parentId;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);

};

sabio.services.productCategories.delete = function (id, onSuccess, onError) {


    var url = "/api/product-categories/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "DELETE"
    };

    $.ajax(url, settings);

};

sabio.services.productCategories.getLocal = function (name, onAjaxSuccess, onAjaxError) {
    var url = "/json/" + (name || "simple") + ".json";
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onAjaxSuccess
            , error: onAjaxError
            , type: "GET"
    };

    $.ajax(url, settings);

};

sabio.services.productCategories.getSubCatsByRequest = function (id, onSuccess, onError) {
    var url = "/api/product-categories/request/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);
};

(function () {
    if (angular) {
        angular.module(APPNAME).factory("productCategories", productCategories);

        productCategories.$inject = ["$baseService", "$sabio"];
        function productCategories($baseService, $sabio) {
            var serviceObject = sabio.services.productCategories;
            var service = $baseService.merge(true, {}, serviceObject, $baseService);
            return service;
        }
    }
})();