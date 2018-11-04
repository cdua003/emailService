using System;
using System.Web.Http;
using eMailService.ViewModels;
using eMailService.Repository;

namespace eMailService.Controllers
{
    public class EmailServiceController : ApiController
    {
        private readonly EmailViewModel emailViewModel;

        public EmailServiceController()
        {
            emailViewModel = new EmailViewModel(new EmailRepository());
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return emailViewModel.Get();
        }

        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            return emailViewModel.Get(id);
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            return emailViewModel.Delete(id);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] string value)
        {
            return emailViewModel.Create(value);
        }
    }
}
