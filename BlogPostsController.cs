using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sabio.Web.Models.ViewModels;

namespace Sabio.Web.Controllers
{
    [RoutePrefix("blog-posts")]
    public class BlogPostsController : BaseController
    {
        [Route("Create")]
        public ActionResult Create()
        {
            return View("Create2");
        }

        [Route]
        public ActionResult Index()
        {
            return View("AngularIndex");
        }

        [Route("~/metatags")]
        public ActionResult Make()
        {
            return View();
        }

        [Route("{id:int}/edit")]
        public ActionResult Edit(int id)
        {
            ItemViewModel<int> model = new ItemViewModel<int>();
            model.Item = id;
            return View("Create2", model);
        }
    }
}