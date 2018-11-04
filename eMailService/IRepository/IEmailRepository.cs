using System;
using System.Collections.Generic;
using System.Web.Http;
using eMailService.Models;

namespace eMailService.IRepository
{
    public interface IEmailRepository
    {
        List<EmailRecord> Get();

        EmailRecord Get(Guid Id);

        void Create(EmailRecord emailRecord);

        void Delete(Guid Id);
    }
}