sabio.services.blogPosts = sabio.services.blogPosts || {};
sabio.services.blogPosts.adminApiPrefix = "/api/v2/admin/blogs/";
sabio.services.blogPosts.consumerApiPrefix = "/api/blogs/";

sabio.services.blogPosts.insert = function (data, onSuccess, onError) {
    var url = sabio.services.blogPosts.adminApiPrefix;

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

sabio.services.blogPosts.update = function (id, data, onSuccess, onError) {
    var url = sabio.services.blogPosts.adminApiPrefix + id;

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

sabio.services.blogPosts.get = function (onAjaxSuccess, onAjaxError) {
    var url = "/api/blog-posts/";

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

sabio.services.blogPosts.getById = function (id, onSuccess, onError) {
    var url = sabio.services.blogPosts.consumerApiPrefix + "posts/" + id;

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

sabio.services.blogPosts.getByDateAndTitle = function (year, month, day, title, onSuccess, onError) {
    var url = sabio.services.blogPosts.consumerApiPrefix + "posts/" + year + "/" + month + "/" +day + "/" + title;

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

sabio.services.blogPosts.getMetaTags = function (year, month, day, title, onSuccess, onError) {
    var url = sabio.services.blogPosts.consumerApiPrefix + year + "/" + month + "/" + day + "/" + title + "/metaTags";

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

sabio.services.blogPosts.getTop = function (onSuccess, onError) {
    var url = sabio.services.blogPosts.consumerApiPrefix + "posts/top";

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

sabio.services.blogPosts.getForConsumer = function (pageIndex, itemsPerPage, onSuccess, onError) {
    var url = sabio.services.blogPosts.consumerApiPrefix + pageIndex + "/" + itemsPerPage;

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

sabio.services.blogPosts.getForAdmin = function (pageIndex, itemsPerPage, onSuccess, onError) {
    var url = sabio.services.blogPosts.adminApiPrefix + pageIndex + "/" + itemsPerPage;

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

sabio.services.blogPosts.getByTag = function (blogTag, pageIndex, itemsPerPage, onSuccess, onError) {
    var url = sabio.services.blogPosts.consumerApiPrefix + blogTag + "/" +pageIndex + "/" + itemsPerPage;

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

sabio.services.blogPosts.commentsByBlog = function (id, onSuccess, onError) {

    var url = sabio.services.blogPosts.adminApiPrefix +"comments/" + id;
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

sabio.services.blogPosts.delete = function (blog, onSuccess, onError) {
    var url = sabio.services.blogPosts.adminApiPrefix + blog.id;

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: function (data) {
            onSuccess(data, blog);
        }
        , error: onError
        , type: "DELETE"
    };

    $.ajax(url, settings);
};

sabio.services.blogPosts.restore = function (blog, onSuccess, onError) {
    var url = sabio.services.blogPosts.adminApiPrefix + "restore/" + blog.id;

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: function (data) {
            onSuccess(data, blog);
        }
        , error: onError
        , type: "DELETE"
    };

    $.ajax(url, settings);
};

sabio.services.blogPosts.getLocal = function (name, onAjaxSuccess, onAjaxError) {
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

(function () {
    if (angular) {
        angular.module(APPNAME).factory("blogPostsService", blogPostsService); //should have service at end of name
        blogPostsService.$inject = ["$baseService", "$sabio"];
        function blogPostsService($baseService, $sabio) {
            var serviceObject = sabio.services.blogPosts;
            var service = $baseService.merge(true, {}, serviceObject, $baseService);
            return service;
        }
    }
})();