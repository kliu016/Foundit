
sabio.services.productCategoryKeywords = sabio.services.productCategoryKeywords || {};


sabio.services.productCategoryKeywords.insert = function (data, onSuccess, onError) {
    var url = "/api/product-category-keywords";
    
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

sabio.services.productCategoryKeywords.update = function (id, data, onSuccess, onError) {
    var url = "/api/product-category-keywords/" + id;
    
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

sabio.services.productCategoryKeywords.get = function (onAjaxSuccess, onAjaxError) {
    var url = "/api/product-category-keywords";
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

sabio.services.productCategoryKeywords.getById = function (id, onSuccess, onError) {
    var url = "/api/product-category-keywords/" + id;
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

sabio.services.productCategoryKeywords.getKeywordsByCategoryId = function (id, onSuccess, onError) {
    var url = "/api/product-category-keywords/" + id + "/categories";
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


sabio.services.productCategoryKeywords.delete = function (id, onSuccess, onError) {
    var url = "/api/product-category-keywords/" + id;
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


(function () {
    if (angular) {
        angular.module(APPNAME).factory("productCategoryKeywordsServices", productCategoryKeywordsServices); //should have service at end of name
        productCategoryKeywordsServices.$inject = ["$baseService", "$sabio"];
        function productCategoryKeywordsServices($baseService, $sabio) {
            var serviceObject = sabio.services.productCategoryKeywords;
            var service = $baseService.merge(true, {}, serviceObject, $baseService);
            return service;
        }

    }
})();