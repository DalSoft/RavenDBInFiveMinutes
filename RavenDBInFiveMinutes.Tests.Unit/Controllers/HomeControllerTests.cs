using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;
using RavenDBInFiveMinutes.Website.Controllers;
using RavenDBInFiveMinutes.Website.Models;

namespace RavenDBInFiveMinutes.Tests.Unit.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private readonly IDocumentStore _documentStore;
        private readonly IDocumentSession _documentSession;

        public HomeControllerTests()
        {
            _documentStore = new EmbeddableDocumentStore { RunInMemory = true }
                .Initialize();

            _documentSession = _documentStore.OpenSession();
        }

        [Test(Description = "GET: /Home/")]
        public void ListAction_Always_ReturnsViewByConvention()
        {
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.List(null);
            Assert.That(result.ViewName, Is.Empty);
        }

        [Test(Description = "GET: /Home/")]
        public void ListAction_SuppliedMessage_MessageIsAddedToViewBag()
        {
            const string message = "Test Messaage";
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.List(message);
            Assert.That(result.ViewBag.Message, Is.EqualTo(message));
        }

        [Test(Description = "GET: /Home/")]
        public void ListAction_SuppliedNullMessage_MessageIsAddedToViewBagNoErrorIsThrown()
        {
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.List(null);
            Assert.That((string)result.ViewBag.Message, Is.Null);
        }

        [Test(Description = "GET: /Home/")]
        public void ListAction_Always_ViewModelIsAListOfMovies()
        {
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.List(null);
            Assert.IsInstanceOf<List<Movie>>(result.Model);
        }

        [Test(Description = "GET: /Home/Details/5")]
        public void DetailsAction_IdFound_ReturnsViewByConvention()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.Details(movie.Id);
            Assert.That(result.ViewName, Is.Empty);
        }


        [Test(Description = "GET: /Home/Details/5")]
        public void DetailsAction_IdFound_ViewModelIsMovie()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.Details(movie.Id);
            Assert.IsInstanceOf<Movie>(result.Model);
        }

        [Test(Description = "GET: /Home/Details/5")]
        public void DetailsAction_IdNotFound_RedirectsToIndexAction()
        {
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Details(0);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test(Description = "GET: /Home/Details/5")]
        public void DetailsAction_IdNotFound_RedirectsToIndexActionSettingMessage()
        {
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Details(0);
            Assert.That(result.RouteValues["message"], Is.EqualTo(string.Format("Movie {0} not found", 0)));
        }


        [Test(Description = "GET: /Home/Create")]
        public void CreateAction_Always_ReturnsViewByConvention()
        {
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.Create();
            Assert.That(result.ViewName, Is.Empty);
        }

        [Test(Description = "POST: /Home/Create")]
        public void CreateAction_FailsModelValidation_ReturnsViewByConvention()
        {
            var homeController = new HomeController(_documentSession);
            homeController.ModelState.AddModelError("Title", "The Title field is required.");
            var result = (ViewResult)homeController.Create(new Movie().NewInvalid());
            Assert.That(result.ViewName, Is.Empty);
        }

        [Test(Description = "POST: /Home/Create")]
        public void CreateAction_PassesModelValidation_RedirectsToIndexAction()
        {
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Create(new Movie().NewValid());
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test(Description = "POST: /Home/Create")]
        public void CreateAction_PassesModelValidation_RedirectsToIndexActionSettingMessage()
        {
            var movie = new Movie().NewValid();
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Create(movie);
            Assert.That(result.RouteValues["message"], Is.EqualTo(string.Format("Created Movie {0}", movie.Title)));
        }

        [Test(Description = "GET: /Home/Edit/5")]
        public void EditAction_IdFound_ReturnsViewByConvention()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.Edit(movie.Id);
            Assert.That(result.ViewName, Is.Empty);
        }

        [Test(Description = "GET: /Home/Edit/5")]
        public void EditAction_IdFound_ViewModelIsMovie()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.Edit(movie.Id);
            Assert.IsInstanceOf<Movie>(result.Model);
        }

        [Test(Description = "GET: /Home/Edit/5")]
        public void EditAction_IdNotFound_RedirectsToIndexAction()
        {
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Edit(0);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test(Description = "GET: /Home/Edit/5")]
        public void EditAction_IdNotFound_RedirectsToIndexActionSettingMessage()
        {
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Edit(0);
            Assert.That(result.RouteValues["message"], Is.EqualTo(string.Format("Movie {0} not found", 0)));
        }

        [Test(Description = "POST: /Home/Edit")]
        public void EditAction_FailsModelValidation_ReturnsViewByConvention()
        {
            var homeController = new HomeController(_documentSession);
            homeController.ModelState.AddModelError("Title", "The Title field is required.");
            var result = (ViewResult)homeController.Edit(new Movie().NewInvalid());
            Assert.That(result.ViewName, Is.Empty);
        }

        [Test(Description = "POST: /Home/Edit")]
        public void EditAction_PassesModelValidation_RedirectsToIndexAction()
        {
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Edit(new Movie().NewValid());
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test(Description = "POST: /Home/Edit")]
        public void EditAction_PassesModelValidation_RedirectsToIndexActionSettingMessage()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Edit(movie);
            Assert.That(result.RouteValues["message"], Is.EqualTo(string.Format("Saved changes to Movie {0}", movie.Title)));
        }
        
        [Test(Description = "GET: /Home/Delete/5")]
        public void ConfirmDeleteAction_IdFound_ReturnsViewByConvention()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.ConfirmDelete(movie.Id);
            Assert.That(result.ViewName, Is.Empty);
        }

        [Test(Description = "GET: /Home/Delete/5")]
        public void ConfirmDeleteAction_IdFound_ViewModelIsMovie()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (ViewResult)homeController.ConfirmDelete(movie.Id);
            Assert.IsInstanceOf<Movie>(result.Model);
        }

        [Test(Description = "GET: /Home/Delete/5")]
        public void ConfirmDeleteAction_IdNotFound_RedirectsToIndexAction()
        {
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.ConfirmDelete(0);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test(Description = "GET: /Home/Delete/5")]
        public void ConfirmDeleteAction_IdNotFound_RedirectsToIndexActionSettingMessage()
        {
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.ConfirmDelete(0);
            Assert.That(result.RouteValues["message"], Is.EqualTo(string.Format("Movie {0} not found", 0)));
        }

        [Test(Description = "POST: /Home/Delete")]
        public void DeleteAction_Always_RedirectsToIndexAction()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Delete(movie.Id);
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test(Description = "POST: /Home/Delete")]
        public void DeleteAction_Always_RedirectsToIndexActionSettingMessage()
        {
            var movie = _documentSession.SaveNewMovieToRavenDB(new Movie().NewValid());
            var homeController = new HomeController(_documentSession);
            var result = (RedirectToRouteResult)homeController.Delete(movie.Id);
            Assert.That(result.RouteValues["message"], Is.EqualTo(string.Format("Deleted Movie with the Id {0}", movie.Id)));
        }


    }
}
